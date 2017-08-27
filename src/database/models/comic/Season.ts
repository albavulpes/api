import {Field, Entity, ElementType} from 'hydrate-mongodb';
import {Model} from '../../base/Model';
import {Arc} from './Arc';

@Entity()
export class Season extends Model {
    @Field()
    public comicId: string;

    @Field()
    public number: number;

    @Field()
    public title: string;
}
