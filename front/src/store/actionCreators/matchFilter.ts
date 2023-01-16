import {HeroFilterOptions} from "../reducers/heroFilterOptionsReducer";
import {MatchFilterOptions} from "../reducers/matchFilterOptionsReducer";

export const updateMinDurationFilter = (value: number[]) => {
    return {type: MatchFilterOptions.DURATION, payload: value};
};

