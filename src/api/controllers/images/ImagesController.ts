import path = require('path');
import fs = require('fs');
import {Request, Response} from 'express-serve-static-core';
import {Controller, Get, Res, Req, Param, ContentType, OnNull} from 'routing-controllers';
import {IMAGES_STORAGE_PATH} from '../../../config/Constants';

@Controller('/images')
export class ImagesController {

    @Get('/:file')
    @ContentType('jpeg')
    @OnNull(404)
    public async getImage(@Req() req: Request, @Res() res: Response, @Param('file') file: string) {
        const imageFile = `${file}`;
        const imagePath = path.join(IMAGES_STORAGE_PATH, imageFile);

        try {
            const buffer = fs.readFileSync(imagePath);

            return res.end(buffer); // will send 200
        }
        catch (err) {
            return null; // will send 404
        }
    }
}