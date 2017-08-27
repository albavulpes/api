import * as validator from 'validator';
import * as passport from 'passport';

import {AuthHelper, IPassportUser} from '../../helpers/AuthHelper';
import {IPayload} from '../../interfaces/IPayload';
import {CryptoHelper} from '../../helpers/CryptoHelper';
import {SessionManager} from '../../database/SessionManager';
import {User} from '../../database/models/auth/User';
import {Passport} from '../../database/models/auth/Passport';

export class AuthWorker {

    public static async doLogin(request, response, next) {
        // Do authentication against passport
        passport.authenticate('local', function (err, passportUser, info): any {
            // If internal error occured
            if (err)
                return next(err);

            // If authentication error occured
            if (!passportUser) {
                // TODO: Maybe make this agnostic to response type, let the controller handle it?
                return response.json({
                    success: false,
                    message: info.message
                });
            }

            // Set user to session
            AuthHelper.registerUserToSession(passportUser, request, response, next);

        })(request, response, next);
    };

    public static async doRegister(request, response, next) {
        const input = request.body;

        // Let's check if the user input was valid
        const validateUser = await AuthWorker.validateNewUser(input);
        if (validateUser.error) {
            // TODO: Maybe make this agnostic to response type, let the controller handle it?
            return response.json({
                success: false,
                message: validateUser.error
            });
        }

        const session = SessionManager.createSession();

        // User input was valid, so let's create an account for them
        const newUser = new User();
        newUser.username = input.username || input.email;
        newUser.email = input.email;
        newUser.createdAt = new Date();
        newUser.roles = ['User'];
        newUser.passports = [];

        // Create the passport for the user
        const newPassport = new Passport();
        newPassport.protocol = 'local';
        newPassport.password = CryptoHelper.hashPassword(input.password);
        newPassport.accessToken = CryptoHelper.generateAccessToken();

        // Add the passport to the user
        newUser.passports.push(newPassport);

        session.save(newUser);

        session.close();

        // Auto login and set user to session
        AuthHelper.registerUserToSession(newUser, request, response, next);
    }

    // Login Validator
    public static async validateLogin(email, password): Promise<IPayload<IPassportUser>> {
        // Check for empty email and password
        if (!email || !password)
            return {error: 'Email Address and Password must be provided.'};

        const session = SessionManager.createSession();

        // Check if user exists
        let dbUser = await session.query(User).findOne({email: email}).fetch('passports').asPromise();

        session.close();

        if (!dbUser)
            return {error: 'Email and Password combination provided is not valid.'};

        // Now check password if it matches the user's associated Passport
        let passport = dbUser.passports.find(x => x.protocol === 'local');

        if (!passport)
            return {error: 'Password has not been set for this account.'};

        let isPasswordValid = CryptoHelper.validatePassword(password, passport.password);

        if (!isPasswordValid)
            return {error: 'Email and Password combination provided is not valid.'};

        // All checks passed
        return {
            data: AuthHelper.getUserForSession(dbUser)
        };
    };

    // New User validation on Register
    public static async validateNewUser(input): Promise<IPayload<null>> {
        // Check for empty email and password
        if (!input.email || !input.password)
            return {error: 'Email and Password must be specified.'};

        // Validate email address
        if (!validator.isEmail(input.email))
            return {error: 'Entered email is not a valid email address.'};

        // Validate password constraints
        const passwordMinLength = 8;

        if (input.password.length < passwordMinLength)
            return {error: 'Password must be at least ' + passwordMinLength + ' characters long.'};

        const session = SessionManager.createSession();

        // Check if user already exists
        const user = await session.query(User).findOne({email: input.email}).asPromise();

        session.close();

        if (user)
            return {error: 'A user with the same email already exists.'};

        // All checks passed
        return {};
    }
}