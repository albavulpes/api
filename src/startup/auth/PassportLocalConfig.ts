import {Strategy} from 'passport-local';
import {AuthWorker} from '../../api/workers/AuthWorker';

async function strategyHandler(identifier, password, next) {
    // Let's check if the user input was valid
    const loginResult = await AuthWorker.validateLogin(identifier, password);
    if (loginResult.error) {
        return next(null, null, {message: loginResult.error});
    }

    const passportResult = loginResult.data;

    // Login the user and give them a session
    return next(null, passportResult);
}

export const LOCAL_STRATEGY = new Strategy({usernameField: 'email', passwordField: 'password'}, strategyHandler);