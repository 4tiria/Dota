import {Hero} from "../Hero";

export class MatchFilterModel {
    minDurationInMinutes?: number | undefined = 10;
    maxDurationInMinutes?: number | undefined = 80;
    minStartedMillisecondsBefore?: number | undefined;
    maxStartedMillisecondsBefore?: number | undefined;
    
    selfTeam: Hero[] = [];
    otherTeam: Hero[] = [];

    skip?: number | undefined;
    take?: number | undefined;
    daysAgo?: number | undefined;
}