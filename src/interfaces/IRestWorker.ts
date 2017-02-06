import {IPayload} from "./IPayload";
import {Model} from "../database/base/Model";

export interface IRestWorker<TModel extends Model>
{
    create(body: any, ...args): Promise<IPayload<TModel>>;

    read(id: string, ...args): Promise<IPayload<TModel>>;

    update(id: string, body: any, ...args): Promise<IPayload<TModel>>;

    remove(id: string, ...args): Promise<IPayload<TModel>>;
}
