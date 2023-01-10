import {Hero} from "./Hero";
import {Match} from "./Match";

export class HeroInMatch {
    heroId: number;
    hero: Hero;
    
    matchId: number;
    match: Match;
    
    side: string;
    kda: string;
    gold: number;
    xp: number;
}