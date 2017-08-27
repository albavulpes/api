import {Field, Entity, ElementType} from 'hydrate-mongodb';
import {Model} from '../../base/Model';

@Entity()
export class Page extends Model {
    @Field()
    public chapterId: string;

    @Field()
    public number: number;

    @Field()
    public imageId: string;
}
