import {ACCESS_TOKEN_KEY} from "../store/store";
import {baseApiUrl, baseAuthUrl} from "./http";

export const heroListPath = baseApiUrl + "/hero/list";
export const heroFilteredListPath = baseApiUrl + "/hero/list/filter";
export const heroPath = baseApiUrl + "/hero";
export const addHeroPath = baseApiUrl + "/hero/empty";
export const deleteHeroPath = baseApiUrl + "/hero/delete";
export const matchPath = baseApiUrl + "/match";
export const heroInMatchPath = baseApiUrl + "/match/hero";
export const accountPath = baseAuthUrl;

export const getAuthHeader = () => {
    return {'Authorization': `Bearer ${localStorage.getItem(ACCESS_TOKEN_KEY)}`};
}