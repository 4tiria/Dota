import {HeroFilterModel} from "../../models/filterModels/heroFilter";

const initialState: HeroFilterModel = {name: '', attackType: 'All', mainAttribute: 'All', tags: []};

export enum HeroFilterOptions {
    NAME = "NAME",
    ATTACK_TYPE = "ATTACK_TYPE",
    ATTRIBUTE = "ATTRIBUTE",
    TAGS = "TAGS",
    RESET = "RESET",
}

export interface IHeroFilterAction {
    type: HeroFilterOptions;
    payload: any;
}

export const heroFilterOptionsReducer = (state = initialState,
                                         action: IHeroFilterAction): HeroFilterModel => {
    switch (action.type) {
        case HeroFilterOptions.NAME:
            return {...state, name: action.payload};
        case HeroFilterOptions.ATTACK_TYPE:
            return {...state, attackType: action.payload};
        case HeroFilterOptions.ATTRIBUTE:
            return {...state, mainAttribute: action.payload};
        case HeroFilterOptions.TAGS:
            return {...state, tags: action.payload};
        default:
        case HeroFilterOptions.RESET:
            return {...initialState};

    }
  
}

export default heroFilterOptionsReducer;