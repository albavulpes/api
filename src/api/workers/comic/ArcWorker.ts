import {IPayload} from '../../../interfaces/IPayload';
import {IRestWorker} from '../../../interfaces/IRestWorker';
import {SessionManager} from '../../../database/SessionManager';
import {Season} from '../../../database/models/comic/Season';
import {Arc} from '../../../database/models/comic/Arc';

class Worker implements IRestWorker<Arc> {

    public async create(body: Arc): Promise<IPayload<Arc>> {
        // Validate request
        if (!body.seasonId) {
            return {
                success: false,
                message: 'Season information given is invalid. Cannot create Arc.'
            };
        }

        const session = SessionManager.createSession();

        // Get the proper parent
        let season = await session.query(Season).findOne({_id: body.seasonId}).asPromise();

        if (!season) {
            return {
                success: false,
                message: 'Requested Season does not exist. Cannot create Arc.'
            };
        }

        // Get the number of seasons available for this comic to set the season number
        const count = await session.query(Arc).count({seasonId: body.seasonId}).asPromise();

        // Create a new season
        const arc = new Arc();
        arc.seasonId = body.seasonId;
        arc.title = body.title;
        arc.number = count + 1;

        // Save the season
        await new Promise((resolve, reject) => {
            session.save(arc, () => resolve());
            session.flush();
        });

        session.close();

        return {
            success: true,
            data: arc
        };
    }

    public async read(id: string): Promise<IPayload<Arc>> {
        const session = SessionManager.createSession();
        let arc = await session.query(Arc)
            .findOne({_id: id})
            .asPromise();

        if (!arc) {
            return {
                success: false,
                message: `Arc with ID '${id}' does not exist.`
            };
        }

        session.close();

        return {
            success: true,
            data: arc
        };
    }

    public async update(id: string, body): Promise<IPayload<Arc>> {
        return undefined;
    }

    public async remove(id: string): Promise<IPayload<Arc>> {
        return undefined;
    }

    public async getArcsForSeason(seasonId: string, page = 1, show = 10): Promise<IPayload<Arc[]>> {
        const session = SessionManager.createSession();

        const arcs = await session.query(Arc)
            .findAll({seasonId})
            .sort('number', 1)
            .skip(show * (page - 1))
            .limit(show)
            .asPromise();

        session.close();

        return {
            success: true,
            data: arcs
        };
    }
}

export const ArcWorker = new Worker();