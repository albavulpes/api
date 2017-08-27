import {IPayload} from '../../../interfaces/IPayload';
import {IRestWorker} from '../../../interfaces/IRestWorker';
import {SessionManager} from '../../../database/SessionManager';
import {Comic} from '../../../database/models/comic/Comic';
import {Season} from '../../../database/models/comic/Season';

class Worker implements IRestWorker<Season> {

    public async create(body: Season): Promise<IPayload<Season>> {
        // Validate request
        if (!body.comicId) {
            return {
                success: false,
                message: 'Comic information given is invalid. Cannot create Season.'
            };
        }

        var session = SessionManager.createSession();

        // Get the proper comic
        var comic = await session.query(Comic).findOne({_id: body.comicId}).asPromise();

        if (!comic) {
            return {
                success: false,
                message: 'Requested Comic does not exist. Cannot create Season.'
            };
        }

        // Get the number of seasons available for this comic to set the season number
        var count = await session.query(Season).count({comicId: body.comicId}).asPromise();

        // Create a new season
        var season = new Season();
        season.comicId = body.comicId;
        season.title = body.title;
        season.number = count + 1;

        // Save the season
        await new Promise((resolve, reject) => {
            session.save(season, () => resolve());
            session.flush();
        });

        session.close();

        return {
            success: true,
            data: season
        };
    }

    public async read(id: string): Promise<IPayload<Season>> {
        var session = SessionManager.createSession();
        var season = await session.query(Season).findOne({_id: id}).asPromise();

        if (!season) {
            return {
                success: false,
                message: `Season with ID '${id}' does not exist.`
            };
        }

        session.close();

        return {
            success: true,
            data: season
        };
    }

    public async update(id: string, body): Promise<IPayload<Season>> {
        return undefined;
    }

    public async remove(id: string): Promise<IPayload<Season>> {
        return undefined;
    }

    public async getSeasonsForComic(comicId: string, page = 1, show = 10): Promise<IPayload<Season[]>> {
        var session = SessionManager.createSession();

        var seasons = await session.query(Season)
            .findAll({comicId})
            .sort('title', 1)
            .skip(show * (page - 1))
            .limit(show)
            .asPromise();

        session.close();

        return {
            success: true,
            data: seasons
        };
    }
}

export const SeasonWorker = new Worker();