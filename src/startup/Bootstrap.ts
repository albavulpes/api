import * as bodyParser from "body-parser";

import {Express} from "express";
import {DatabaseBootstrapper} from "./db/DatabaseBootstrapper";
import {AuthBootstrapper} from "./auth/AuthBootstrapper";
import {RoutingBootstrapper} from "./routes/RoutingBootstrapper";
import {LOGGER} from "../helpers/Logger";

export class Bootstrap
{
    public static async init(app: Express)
    {
        LOGGER.info("Bootstrapping application...");

        // Configure express middlewares
        app.use(bodyParser.json());
        app.use(bodyParser.urlencoded({extended: true}));

        // Connect to MongoDB
        await DatabaseBootstrapper.init();

        // Setup authentication
        await AuthBootstrapper.init(app);

        // Setup routes
        await RoutingBootstrapper.init(app);
    }
}