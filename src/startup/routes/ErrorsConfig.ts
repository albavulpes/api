import {Express, ErrorRequestHandler} from "express";

export class ErrorsConfig
{
    public static init(app: Express)
    {
        // Handle all application errors
        app.use(function (err, req, res, next)
        {
            console.error(err);

            return res.status(500).send(err.message);
        } as ErrorRequestHandler);

        process.on("unhandledRejection", function (reason, p)
        {
            console.error(reason);
        });
    }
}