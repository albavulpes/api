import {Request, Response} from "express-serve-static-core";

export interface IRestController
{
    create(req: Request, res: Response, ...args);

    read(req: Request, res: Response, id: string, ...args);

    update(req: Request, res: Response, id: string, ...args);

    remove(req: Request, res: Response, id: string, ...args);
}
