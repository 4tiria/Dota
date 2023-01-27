import {ACCESS_TOKEN_KEY} from "../store/store";
import {baseUrl} from "./http";

export const heroListPath = baseUrl + "/hero/list";
export const heroFilteredListPath = baseUrl + "/hero/list/filter";
export const heroPath = baseUrl + "/hero";
export const addHeroPath = baseUrl + "/hero/empty";
export const deleteHeroPath = baseUrl + "/hero/delete";
export const tagPath = baseUrl + "/tag";
export const matchPath = baseUrl + "/match";
export const heroInMatchPath = baseUrl + "/match/hero";
export const imagePath = baseUrl + "/image";
export const assetPath = baseUrl + "/asset";
export const accountPath = baseUrl + "/account";

export const getAuthHeader = () => {
    return {'Authorization': `Bearer ${localStorage.getItem(ACCESS_TOKEN_KEY)}`};
}