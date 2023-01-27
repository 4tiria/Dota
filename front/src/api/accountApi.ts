import {accountPath} from "./apiPaths";
import {AuthRequest} from "../models/dto/requests/AuthRequest";
import {api} from "./http";
import {AuthResponse} from "../models/dto/responses/AuthResponse";
import {LogoutRequest} from "../models/dto/requests/LogoutRequest";

export async function registerApi(authenticationData: AuthRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>(`${accountPath}/register`, authenticationData);
    return response.data;
}

export async function loginApi(authenticationData: AuthRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>(`${accountPath}/login`, authenticationData);
    return response.data;
}

export async function logoutApi(logoutRequest: LogoutRequest): Promise<any> {
    const response = await api.post<any>(`${accountPath}/logout`, logoutRequest);
    return response.data;
}
