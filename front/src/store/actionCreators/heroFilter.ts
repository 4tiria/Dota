import { Tag } from "../../models/Tag";
import {IHeroFilterAction, HeroFilterOptions} from "../reducers/heroFilterOptionsReducer";

export const updateHeroNameFilter = (name: string) => {
    return {type: HeroFilterOptions.NAME, payload: name};
};

export const updateHeroAttackTypeFilter = (attackType: string): IHeroFilterAction => {
    return {type: HeroFilterOptions.ATTACK_TYPE, payload: attackType};
};

export const updateHeroMainAttributeFilter = (attribute: string): IHeroFilterAction => {
    return {type: HeroFilterOptions.ATTRIBUTE, payload: attribute};
};

export const updateHeroTagsFilter = (tags: Tag[]) : IHeroFilterAction => {
    return {type: HeroFilterOptions.TAGS, payload: tags};
};

export const resetHeroFilters = (): IHeroFilterAction => {
    return {type: HeroFilterOptions.RESET, payload: null};
};


