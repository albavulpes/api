import path = require("path");
import {IAppConfig} from "../interfaces/IAppConfig";

const env = process.env.NODE_ENV || "development";
const isDevEnv = (env === "development");

const rootPath = path.join(__dirname, "..");

export const CONFIG: IAppConfig = {
    rootPath: rootPath,
    port: isDevEnv ? 7950 : 80,
    cors: {
        origin: process.env.CLIENT_URL || true,
        credentials: true
    },
    winston: {
        level: isDevEnv ? "debug" : "info"
    },
    jwt: {
        secret: process.env.JWT_SECRET || "asdfghjkl",
        expiryInMinutes: 30,
        cookie: {
            name: process.env.JWT_COOKIE || "project.elegantstrokes.presence",
            options: {
                httpOnly: true,
                secure: !isDevEnv,
                expires: new Date(2099, 1, 1)
            }
        }
    },
    db: {
        connectionString: "mongodb://localhost:27017/project-elegant-strokes"
    },
    settings: {
        fileStoragePath: process.env.SETTINGS_STORAGE_PATH || path.resolve("/data/project-strokes/")
    }
};