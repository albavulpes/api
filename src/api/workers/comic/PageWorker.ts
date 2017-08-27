import {IPayload} from '../../../interfaces/IPayload';
import {IRestWorker} from '../../../interfaces/IRestWorker';
import {SessionManager} from '../../../database/SessionManager';
import {Chapter} from '../../../database/models/comic/Chapter';
import {Page} from '../../../database/models/comic/Page';

class Worker implements IRestWorker<Page> {

    public async create(body: Page): Promise<IPayload<Page>> {
        // Validate request
        if (!body.chapterId) {
            return {
                success: false,
                message: 'Chapter information given is invalid. Cannot create Page.'
            };
        }

        const session = SessionManager.createSession();

        // Get the proper parent
        let arc = await session.query(Chapter).findOne({_id: body.chapterId}).asPromise();

        if (!arc) {
            return {
                success: false,
                message: 'Requested Chapter does not exist. Cannot create Page.'
            };
        }

        // Get the number of seasons available for this comic to set the season number
        const count = await session.query(Page).count({chapterId: body.chapterId}).asPromise();

        // Create a new season
        const page = new Page();
        page.chapterId = body.chapterId;
        page.number = count + 1;

        // Save the season
        await new Promise((resolve, reject) => {
            session.save(page, () => resolve());
            session.flush();
        });

        session.close();

        return {
            success: true,
            data: page
        };
    }

    public async read(id: string): Promise<IPayload<Page>> {
        const session = SessionManager.createSession();
        let chapter = await session.query(Page).findOne({_id: id}).asPromise();

        if (!chapter) {
            return {
                success: false,
                message: `Page with ID '${id}' does not exist.`
            };
        }

        session.close();

        return {
            success: true,
            data: chapter
        };
    }

    public async update(id: string, body): Promise<IPayload<Page>> {
        return undefined;
    }

    public async remove(id: string): Promise<IPayload<Page>> {
        return undefined;
    }

    public async getPagesForChapter(chapterId: string, page = 1, show = 10): Promise<IPayload<Page[]>> {
        const session = SessionManager.createSession();

        const comics = await session.query(Page)
            .findAll({chapterId})
            .sort('title', 1)
            .skip(show * (page - 1))
            .limit(show)
            .asPromise();

        session.close();

        return {
            success: true,
            data: comics
        };
    }
}

export const PageWorker = new Worker();