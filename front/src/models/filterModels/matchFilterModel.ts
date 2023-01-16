import {Hero} from "../Hero";

export class MatchFilterModel {
    minDuration?: number | undefined = 10;
    maxDuration?: number | undefined = 80;
    minStart?: number | undefined;
    maxStart?: number | undefined;
    minEnd?: number | undefined;
    maxEnd?: number | undefined;
    
    selfTeam: Hero[] = [];
    otherTeam: Hero[] = [];

    skip?: number | undefined;
    take?: number | undefined;
}