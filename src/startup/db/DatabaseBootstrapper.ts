import Q = require("q");
import shortid = require("shortid");
import {MongoClient} from "mongodb";
import {Configuration, AnnotationMappingProvider, IdentityGenerator} from "hydrate-mongodb";

import {LOGGER} from "../../helpers/Logger";
import {CONFIG} from "../../config/Config";
import {initSessionManager} from "../../database/SessionManager";

import * as models from "../../database/models/Models";

export class DatabaseBootstrapper
{
    public static async init()
    {
        LOGGER.info("Initiating MongoDB...");

        // Initiate the configuration for Hydrate to understand our models
        var config = new Configuration();
        config.identityGenerator = new ShortIdGenerator();
        config.addMapping(new AnnotationMappingProvider(models));

        var def = Q.defer();

        // Connect to mongodb to set up the session factory
        MongoClient.connect(CONFIG.db.url, (err, db) =>
        {
            if (err)
            {
                def.reject(err);
            }

            config.createSessionFactory(db, (err, sessionFactory) =>
            {
                if (err)
                {
                    def.reject(err);
                }

                initSessionManager(sessionFactory);

                def.resolve(true);
            });
        });

        return await def.promise;
    }
}

class ShortIdGenerator implements IdentityGenerator
{
    generate(): string
    {
        return shortid.generate();
    }
    validate(value: string): boolean
    {
        return !!value;
    }
    fromString(text: string): any
    {
        return text;
    }
    areEqual(first: any, second: any): boolean
    {
        return first === second;
    }
}