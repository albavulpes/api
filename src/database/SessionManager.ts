import _ = require('lodash');
import {SessionFactory} from 'hydrate-mongodb';

export const SessionManager = {} as SessionFactory;

export function initSessionManager(sessionFactory: SessionFactory) {
    _.assignIn(SessionManager, sessionFactory);
}