import {Hero} from "../Hero";

export class MatchListFilterModel {
    minDurationInMinutes?: number | undefined = null;
    maxDurationInMinutes?: number | undefined = null;
    minStartedMillisecondsBefore?: number | undefined;
    maxStartedMillisecondsBefore?: number | undefined;
    
    selfTeam: Hero[] = [];
    otherTeam: Hero[] = [];

    skip?: number | undefined;
    take?: number | undefined;
}

export interface IMatchListFilterFactory{
    matchListFilterModel: MatchListFilterModel
}