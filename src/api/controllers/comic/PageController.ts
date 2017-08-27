import {Request, Response} from 'express-serve-static-core';
import {JsonController, Get, Post, Put, Patch, Delete, Req, Res, Param, UseBefore} from 'routing-controllers';
import {IRestController} from '../../../interfaces/IRestController';
import {authorize} from '../../middlewares/Authorize';
import {Roles} from '../../../database/data/admin/Roles';
import {PageWorker} from '../../workers/comic/PageWorker';

@JsonController('/page')
export class ChapterController implements IRestController {

    @Get('/read/:id')
    public async read(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await PageWorker.read(id);
    }

    @Put('/create/')
    @UseBefore(authorize(Roles.Editor))
    public async create(@Req() req: Request, @Res() res: Response) {
        return await PageWorker.create(req.body);
    }

    @Patch('/update/:id')
    @UseBefore(authorize(Roles.Editor))
    public async update(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await PageWorker.update(id, req.body);
    }

    @Delete('/delete/:id')
    @UseBefore(authorize(Roles.Editor))
    public async remove(@Req() req: Request, @Res() res: Response, @Param('id') id: string) {
        return await PageWorker.remove(id);
    }
}