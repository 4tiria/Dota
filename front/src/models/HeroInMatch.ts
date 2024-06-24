import {Hero} from "./Hero";
import {Match} from "./Match";

export class HeroInMatch {
    hero: Hero;
    
    match: Match;
    
    side: string;
    kills: number;
    deaths: number;
    assists: number;
    gold: number;
    xp: number;
}