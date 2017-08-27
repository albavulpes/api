import moment = require('moment');
import * as jwt from 'jsonwebtoken';

import {CONFIG} from '../config/Config';
import {SignOptions} from 'jsonwebtoken';

import {User} from '../database/models/auth/User';

export interface IPassportUser {
    id: string,
    email: string,
    roles: string[],
    iat?: number;
}

export class AuthHelper {
    // Helper that logs in a user and sets them to the session
    public static registerUserToSession(user: User, request, response, next) {
        // Form the user to set to session (we dont wanna set all the user data to session)
        const passportUser = AuthHelper.getUserForSession(user);

        // Set the iat for the logged in user
        passportUser.iat = moment().unix().valueOf();

        // At this point, we are authenticated, so lets generate a JWT for the user
        const jwtToken = AuthHelper.generateJWToken(passportUser);

        // Set user to session
        request.login(passportUser, function (err) {
            if (err)
                return next(err);

            AuthHelper.setAuthCookie(jwtToken, request, response);

            return next();
        });
    }

    // Helper that sets a user to session
    public static getUserForSession(user: User): IPassportUser {
        return {
            id: user.id,
            email: user.email,
            roles: user.roles
        };
    }

    public static generateJWToken(passportUser: IPassportUser, options?: SignOptions) {
        options = options || {};

        return jwt.sign(passportUser, CONFIG.jwt.secret, options);
    }

    public static setAuthCookie(token, request, response) {
        const cookieConfig = CONFIG.jwt.cookie;

        response.cookie(cookieConfig.name, token, cookieConfig.options);

        request.token = token;
    }

    public static clearAuthCookie(response) {
        return response.clearCookie(CONFIG.jwt.cookie.name);
    }
}