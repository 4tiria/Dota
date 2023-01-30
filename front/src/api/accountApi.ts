import {accountPath} from "./apiPaths";
import {RegistrationRequest} from "../models/dto/requests/RegistrationRequest";
import {api} from "./http";
import {AuthResponse} from "../models/dto/responses/AuthResponse";
import {LogoutRequest} from "../models/dto/requests/LogoutRequest";
import {LoginRequest} from "../models/dto/requests/LoginRequest";

export async function registerOnServer(registrationRequest: RegistrationRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>(`${accountPath}/register`, registrationRequest);
    return response.data;
}

export async function loginOnServer(loginRequest: LoginRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>(`${accountPath}/login`, loginRequest);
    return response.data;
}

export async function logoutOnServer(logoutRequest: LogoutRequest): Promise<any> {
    const response = await api.post<any>(`${accountPath}/logout`, logoutRequest);
    return response.data;
}
