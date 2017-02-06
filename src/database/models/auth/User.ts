import {Field, Entity, ElementType, Fetch, FetchType} from "hydrate-mongodb";
import {Model} from "../../base/Model";
import {Passport} from "./Passport";

@Entity()
export class User extends Model
{
    @Field()
    public username: string;

    @Field()
    public email: string;

    @Field()
    public createdAt: Date;

    @Field()
    @ElementType(String)
    public roles: string[];

    @Field()
    @ElementType(Passport)
    @Fetch(FetchType.Lazy)
    public passports:Passport[];
}
