import {Asset} from "../models/Asset";
import axios from "axios";
import {accountPath, assetPath} from "./apiPaths";
import {Account} from "../models/Account";
import {AuthData} from "../models/temporaryModels/AuthData";

export async function register(authenticationData: AuthData): Promise<Account> {
    const response = await axios.post<Account>(`${accountPath}/register`, authenticationData);
    return response.data;
}

export async function login(authenticationData: AuthData): Promise<Account> {
    const response = await axios.post<Account>(`${accountPath}/login`, authenticationData);
    return response.data;
}