import {Request, Response} from "express-serve-static-core";
import {JsonController, Get, Post, Put, Patch, Delete, Req, Res, Param, QueryParam, UseBefore} from "routing-controllers";
import {IRestController} from "../../../interfaces/IRestController";
import {authorize} from "../../middlewares/Authorize";
import {Roles} from "../../../database/data/admin/Roles";
import {ComicWorker} from "../../workers/comic/ComicWorker";

@JsonController("/comic")
export class ComicController implements IRestController
{
    @Get("/read/:id")
    public async read(@Req() req: Request, @Res() res: Response, @Param("id") id: string)
    {
        return await ComicWorker.read(id);
    }

    @Put("/create/")
    @UseBefore(authorize(Roles.Editor))
    public async create(@Req() req: Request, @Res() res: Response)
    {
        return await ComicWorker.create(req.body);
    }

    @Patch("/update/:id")
    @UseBefore(authorize(Roles.Editor))
    public async update(@Req() req: Request, @Res() res: Response, @Param("id") id: string)
    {
        return await ComicWorker.update(id, req.body);
    }

    @Delete("/delete/:id")
    @UseBefore(authorize(Roles.Editor))
    public async remove(@Req() req: Request, @Res() res: Response, @Param("id") id: string)
    {
        return await ComicWorker.remove(id);
    }


    @Get("/getAllComics")
    public async getAllComics(@QueryParam("page") page: number, @QueryParam("show") show: number)
    {
        return await ComicWorker.getAllComics(page, show);
    }
}