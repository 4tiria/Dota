import {MatchFilterOptions} from "../reducers/matchFilterOptionsReducer";

export const updateDurationFilter = (value: number[]) => {
    return {type: MatchFilterOptions.DURATION, payload: value};
};

export const updateStartFilter = (value: number[]) => {
    return {type: MatchFilterOptions.START, payload: value};
};

