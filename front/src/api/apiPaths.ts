﻿import {ACCESS_TOKEN_KEY} from "../store/store";

export const serverPath = "http://localhost:5000/";
export const heroListPath = serverPath + "api/hero/list";
export const heroFilteredListPath = serverPath + "api/hero/list/filter";
export const heroPath = serverPath + "api/hero";
export const addHeroPath = serverPath + "api/hero/empty";
export const deleteHeroPath = serverPath + "api/hero/delete";
export const tagPath = serverPath + "api/tag";
export const matchPath = serverPath + "api/match";
export const heroInMatchPath = serverPath + "api/match/hero";
export const imagePath = serverPath + "api/image";
export const assetPath = serverPath + "api/asset";
export const accountPath = serverPath + "api/account";

export const getAuthHeader = () => {
    return {'Authorization' : `Bearer ${localStorage.getItem(ACCESS_TOKEN_KEY)}`}
}