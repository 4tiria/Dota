import {HeroFilterOptions} from "../reducers/heroFilterOptionsReducer";
import {MatchFilterOptions} from "../reducers/matchFilterOptionsReducer";

export const updateDaysAgoFilter = (value: number) => {
    return {type: MatchFilterOptions.DAYS_AGO, payload: value};
}

export const updateDurationFilter = (value: number[]) => {
    return {type: MatchFilterOptions.DURATION, payload: value};
};

export const updateStartFilter = (value: number[]) => {
    return {type: MatchFilterOptions.START, payload: value};
};

