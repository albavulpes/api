import {IPayload} from "../../../interfaces/IPayload";
import {IRestWorker} from "../../../interfaces/IRestWorker";
import {SessionManager} from "../../../database/SessionManager";
import {Arc} from "../../../database/models/comic/Arc";
import {Chapter} from "../../../database/models/comic/Chapter";

class Worker implements IRestWorker<Chapter>
{
    public async create(body: Chapter): Promise<IPayload<Chapter>>
    {
        // Validate request
        if (!body.arcId)
        {
            return {
                success: false,
                message: "Arc information given is invalid. Cannot create Chapter."
            };
        }

        var session = SessionManager.createSession();

        // Get the proper parent
        var arc = await session.query(Arc).findOne({_id: body.arcId}).asPromise();

        if (!arc)
        {
            return {
                success: false,
                message: "Requested Arc does not exist. Cannot create Chapter."
            };
        }

        // Get the number of seasons available for this comic to set the season number
        var count = await session.query(Chapter).count({arcId: body.arcId}).asPromise();

        // Create a new season
        var chapter = new Chapter();
        chapter.arcId = body.arcId;
        chapter.title = body.title;
        chapter.number = count + 1;

        // Save the season
        await new Promise((resolve, reject) =>
        {
            session.save(chapter, () => resolve());
            session.flush();
        });

        session.close();

        return {
            success: true,
            data: chapter
        };
    }

    public async read(id: string): Promise<IPayload<Chapter>>
    {
        var session = SessionManager.createSession();
        var chapter = await session.query(Chapter).findOne({_id: id}).asPromise();

        if (!chapter)
        {
            return {
                success: false,
                message: `Chapter with ID '${id}' does not exist.`
            }
        }

        session.close();

        return {
            success: true,
            data: chapter
        }
    }

    public async update(id: string, body): Promise<IPayload<Chapter>>
    {
        return undefined;
    }

    public async remove(id: string): Promise<IPayload<Chapter>>
    {
        return undefined;
    }

    public async getChaptersForArc(arcId: string, page = 1, show = 10): Promise<IPayload<Chapter[]>>
    {
        var session = SessionManager.createSession();

        var chapters = await session.query(Chapter)
            .findAll({arcId})
            .sort("number", 1)
            .skip(show * (page - 1))
            .limit(show)
            .asPromise();

        session.close();

        return {
            success: true,
            data: chapters
        }
    }
}

export const ChapterWorker = new Worker();