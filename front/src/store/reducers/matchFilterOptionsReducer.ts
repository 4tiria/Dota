import {MatchFilterModel} from "../../models/filterModels/matchFilterModel";
import {Hero} from "../../models/Hero";
import {HeroFilterOptions} from "./heroFilterOptionsReducer";

const initialState: MatchFilterModel = {
    maxDuration: null,
    minDuration: null,
    maxEnd: null,
    minEnd: null,
    maxStart: null,
    minStart: null,
    selfTeam: [],
    otherTeam: [],
    take: 20!,
    skip: 0,
}

export enum MatchFilterOptions {
    DURATION = "DURATION",
    MIN_END = "MIN_END",
    MAX_END = "MAX_END",
    MIN_START = "MIN_START",
    MAX_START = "MAX_START",
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
            return {...state, minDuration: action.payload[0], maxDuration: action.payload[1]};
        case MatchFilterOptions.MIN_END:
            return {...state, minEnd: action.payload};
        case MatchFilterOptions.MAX_END:
            return {...state, maxEnd: action.payload};
        case MatchFilterOptions.MIN_START:
            return {...state, minStart: action.payload};
        case MatchFilterOptions.MAX_START:
            return {...state, maxStart: action.payload};
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