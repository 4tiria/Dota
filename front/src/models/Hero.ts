import {Tag} from "./Tag";
import {HeroImage} from "./HeroImage";

export class Hero {
    id: number;
    name: string;
    attackType: string;
    mainAttribute: string;
    tags: Tag[];
    image: HeroImage | undefined;
}