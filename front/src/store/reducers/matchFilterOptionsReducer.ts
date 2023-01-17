import {MatchFilterModel} from "../../models/filterModels/matchFilterModel";
import {Hero} from "../../models/Hero";
import {HeroFilterOptions} from "./heroFilterOptionsReducer";

const initialState: MatchFilterModel = {
    minDurationInMinutes: null,
    maxDurationInMinutes: null,
    minStartedMillisecondsBefore: null,
    maxStartedMillisecondsBefore: null,
    selfTeam: [],
    otherTeam: [],
    take: 20!,
    skip: 0,
}

export enum MatchFilterOptions {
    DURATION = "DURATION",
    START = "START",
    SELF_TEAM = "SELF_TEAM",
    OTHER_TEAM = "OTHER_TEAM",
    TAKE = "TAKE",
    SKIP = "SKIP",
    RESET = "RESET",
}

export interface IMatchFilterAction {
    type: MatchFilterOptions,
    payload: any,
}

export const matchFilterOptionsReducer = (state = initialState,
                                          action: IMatchFilterAction) => {
    switch (action.type) {
        case MatchFilterOptions.DURATION:
            return {...state, minDurationInMinutes: action.payload[0], maxDurationInMinutes: action.payload[1]};
        case MatchFilterOptions.START:
            return {...state, minStartedMillisecondsBefore: action.payload[0], maxStartedMillisecondsBefore: action.payload[1]};
        case MatchFilterOptions.SELF_TEAM:
            return {...state, selfTeam: action.payload};
        case MatchFilterOptions.OTHER_TEAM:
            return {...state, otherTeam: action.payload};
        case MatchFilterOptions.TAKE:
            return {...state, take: action.payload};
        case MatchFilterOptions.SKIP:
            return {...state, skip: action.payload};
        default:
        case MatchFilterOptions.RESET:
            return {...initialState};
    }
}