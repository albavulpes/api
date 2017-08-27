import {Field, Entity, Embeddable, Type} from 'hydrate-mongodb';
import {Model} from '../../base/Model';

@Embeddable()
export class Passport extends Model {
    @Field()
    public protocol: string;

    @Field()
    public password: string;

    @Field()
    public accessToken: string;

    @Field()
    public provider: string;

    @Field()
    public identifier: string;

    // TODO: Figure out how to store 'any' types in mongo
    public tokens: any;
}