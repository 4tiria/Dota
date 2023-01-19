import {HeroInMatch} from "./HeroInMatch";

export class Match{
    id: Guid;
    heroes: HeroInMatch[];
    start: number;
    end: number;
    score: string;
    winnerSide: string;
    daysAgo: number;
}