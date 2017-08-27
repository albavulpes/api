import 'reflect-metadata'; // this shim is required
import path = require('path');

import {Express} from 'express';
import {useExpressServer} from 'routing-controllers';

import {CONFIG} from '../../config/Config';
import {ErrorsConfig} from './ErrorsConfig';
import {LOGGER} from '../../helpers/Logger';

export class RoutingBootstrapper {
    public static async init(app: Express) {
        LOGGER.info('Setting up Routes...');

        // Setup routing-controllers
        useExpressServer(app, {
            controllers: [path.join(CONFIG.rootPath, 'api/controllers/**/*.js')]
        });

        // Setup error handlers for all routes
        ErrorsConfig.init(app);
    }
}