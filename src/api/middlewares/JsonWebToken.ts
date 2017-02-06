import * as moment from "moment";
import * as jwt from "jsonwebtoken";

import {CONFIG} from "../../config/Config";
import {AuthHelper, IPassportUser} from "../../helpers/AuthHelper";
import {SessionManager} from "../../database/SessionManager";
import {User} from "../../database/models/auth/User";
import {IPayload} from "../../interfaces/IPayload";

// Middleware to hook up JWT
export function jsonWebToken()
{
    return function (request, response, next)
    {
        // check cookies or url parameters or post parameters for token
        var token = request.body.token || request.query.token || request.cookies[CONFIG.jwt.cookie.name];

        if (!token)
            return next();

        // Verifies secret and checks expiry
        jwt.verify(token, CONFIG.jwt.secret, function (err, decoded: IPassportUser)
        {
            if (err)
                return next(err);

            // Let's refresh the token if it has expired
            refreshTokenIfExpired(decoded)
                .then(resolved =>
                {
                    var data = resolved.data;

                    // If new token was provided, update cookie
                    if (data.token)
                    {
                        AuthHelper.setAuthCookie(data.token, request, response);
                    }

                    // If user was removed, clear cookie
                    if (!data.decoded)
                    {
                        AuthHelper.clearAuthCookie(response);
                    }

                    // Save to req.user whatever is the new decoded, refreshed or not
                    request.user = data.decoded;

                    // Carry on
                    return next();
                })
                .catch(function (err)
                {
                    return next(err);
                });
        });
    }
}

async function refreshTokenIfExpired(decoded: IPassportUser): Promise<IPayload<{decoded: IPassportUser, token?: string}>>
{
    var issuedAt = decoded.iat;
    var now = moment().unix().valueOf();

    var expiryDuration = CONFIG.jwt.expiryInMinutes * 60;

    // If not expired just return the previous decoded token
    if ((now - issuedAt) <= expiryDuration)
    {
        return {
            success: true,
            data: {decoded: decoded}
        };
    }

    var session = SessionManager.createSession();

    // Otherwise, let's recheck the database and make sure User claims the correct stuff
    var dbUser = await session.query(User).findOne({_id: decoded.id}).asPromise();

    session.close();

    // If user does not exist anymore, invalidate the session user
    if (!dbUser)
    {
        return {
            success: false,
            data: {decoded: null}
        };
    }

    // Update the session user with stuff from the DB
    decoded = AuthHelper.getUserForSession(dbUser);

    // Set the issued at time to NOW
    decoded.iat = now;

    // Generate new token
    return {
        success: true,
        data: {
            decoded: decoded,
            token: AuthHelper.generateJWToken(decoded)
        }
    }
}
