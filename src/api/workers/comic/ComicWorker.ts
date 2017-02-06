import {Comic} from "../../../database/models/comic/Comic";
import {IPayload} from "../../../interfaces/IPayload";
import {IRestWorker} from "../../../interfaces/IRestWorker";
import {SessionManager} from "../../../database/SessionManager";

class Worker implements IRestWorker<Comic>
{
    public async create(body): Promise<IPayload<Comic>>
    {
        // Validate request
        if (!body.title)
        {
            return {
                success: false,
                message: "Comic title is required to create a new comic."
            };
        }

        // Create the new comic
        var comic = new Comic();
        comic.title = body.title;

        var session = SessionManager.createSession();

        // Save the comic
        await new Promise((resolve, reject) =>
        {
            session.save(comic, () => resolve());
            session.flush();
        });

        session.close();

        return {
            success: true,
            data: comic
        };
    }

    public async read(id: string): Promise<IPayload<Comic>>
    {
        var session = SessionManager.createSession();
        var comic = await session.query(Comic)
            .findOne({_id: id})
            .asPromise();

        if (!comic)
        {
            return {
                success: false,
                message: `Comic with ID '${id}' does not exist.`
            }
        }

        session.close();

        return {
            success: true,
            data: comic
        }
    }

    public async update(id: string, body): Promise<IPayload<Comic>>
    {
        return undefined;
    }

    public async remove(id: string): Promise<IPayload<Comic>>
    {
        return undefined;
    }

    public async getAllComics(page = 1, show = 10): Promise<IPayload<Comic[]>>
    {
        var session = SessionManager.createSession();

        var comics = await session.query(Comic)
            .findAll()
            .sort("title", 1)
            .skip(show * (page - 1))
            .limit(show)
            .asPromise();

        session.close();

        return {
            success: true,
            data: comics
        }
    }
}

export const ComicWorker = new Worker();