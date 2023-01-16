import {Hero} from "./Hero";
import {Match} from "./Match";

export class HeroInMatch {
    heroId: number;
    hero: Hero;
    
    matchId: number;
    match: Match;
    
    side: string;
    kills: number;
    deaths: number;
    assists: number;
    gold: number;
    xp: number;
}