import * as shortid from 'shortid';

import {MongoClient} from 'mongodb';
import {Configuration, AnnotationMappingProvider, IdentityGenerator} from 'hydrate-mongodb';

import {LOGGER} from '../../helpers/Logger';
import {CONFIG} from '../../config/Config';
import {initSessionManager} from '../../database/SessionManager';

import * as models from '../../database/models/Models';

export class DatabaseBootstrapper {
    public static async init(): Promise<void> {
        LOGGER.info('Initiating MongoDB...');

        // Initiate the configuration for Hydrate to understand our models
        const config = new Configuration();
        config.identityGenerator = new ShortIdGenerator();
        config.addMapping(new AnnotationMappingProvider(models));

        return await new Promise<void>((resolve, reject) => {
            // Connect to mongodb to set up the session factory
            MongoClient.connect(CONFIG.db.url, (err, db) => {
                if (err) {
                    reject(err);
                }

                config.createSessionFactory(db, (err, sessionFactory) => {
                    if (err) {
                        reject(err);
                    }

                    initSessionManager(sessionFactory);

                    resolve();
                });
            });
        });
    }
}

class ShortIdGenerator implements IdentityGenerator {
    generate(): string {
        return shortid.generate();
    }

    validate(value: string): boolean {
        return !!value;
    }

    fromString(text: string): any {
        return text;
    }

    areEqual(first: any, second: any): boolean {
        return first === second;
    }
}