import {HeroInMatch} from "./HeroInMatch";

export class Match{
    id: Guid;
    heroes: HeroInMatch[];
    start: Date;
    end: Date;
    score: string;    
}