import { AttackType, MainAttribute } from "models/Hero";
import { HeroFilterOptions, IHeroFilterAction } from "../reducers/heroFilterOptionsReducer";

export const updateHeroNameFilter = (name: string) => {
    return {type: HeroFilterOptions.NAME, payload: name};
};

export const updateHeroAttackTypeFilter = (attackType: AttackType | "All"): IHeroFilterAction => {
    return {type: HeroFilterOptions.ATTACK_TYPE, payload: attackType};
};

export const updateHeroMainAttributeFilter = (attribute: MainAttribute | "All"): IHeroFilterAction => {
    return {type: HeroFilterOptions.ATTRIBUTE, payload: attribute};
};

export const updateHeroRoleFilter = (roles: string[]) : IHeroFilterAction => {
    return {type: HeroFilterOptions.TAGS, payload: roles};
};

export const resetHeroFilters = (): IHeroFilterAction => {
    return {type: HeroFilterOptions.RESET, payload: null};
};


