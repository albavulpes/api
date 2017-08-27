import {Request, Response} from 'express-serve-static-core';
import {JsonController, Get, Post, Put, Patch, Delete, Req, Res, Param, UseBefore} from 'routing-controllers';
import {IRestController} from '../../../interfaces/IRestController';
import {authorize} from '../../middlewares/Authorize';
import {Roles} from '../../../database/data/admin/Roles';
import {ArcWorker} from '../../workers/comic/ArcWorker';

@JsonController('/arc')
export class ArcController implements IRestController {

    @Get('/read/:id')
    public async read(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await ArcWorker.read(id);
    }

    @Put('/create/')
    @UseBefore(authorize(Roles.Editor))
    public async create(@Req() req: Request, @Res() res: Response) {
        return await ArcWorker.create(req.body);
    }

    @Patch('/update/:id')
    @UseBefore(authorize(Roles.Editor))
    public async update(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await ArcWorker.update(id, req.body);
    }

    @Delete('/delete/:id')
    @UseBefore(authorize(Roles.Editor))
    public async remove(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await ArcWorker.remove(id);
    }
}