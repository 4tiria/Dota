import {HeroFilterAction, HeroFilterOptions} from "../reducers/heroFilterOptionsReducer";
import {Tag} from "../../models/Tag";

export const updateHeroNameFilter = (name: string) => {
    return {type: HeroFilterOptions.NAME, payload: name};
};

export const updateHeroAttackTypeFilter = (attackType: string): HeroFilterAction => {
    return {type: HeroFilterOptions.ATTACK_TYPE, payload: attackType};
};

export const updateHeroMainAttributeFilter = (attribute: string): HeroFilterAction => {
    return {type: HeroFilterOptions.ATTRIBUTE, payload: attribute};
};

export const updateHeroTagsFilter = (tags: Tag[]) : HeroFilterAction => {
    return {type: HeroFilterOptions.TAGS, payload: tags};
};

export const resetHeroFilters = (): HeroFilterAction => {
    return {type: HeroFilterOptions.RESET, payload: null};
};


