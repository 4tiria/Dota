import axios from "axios";
import {ACCESS_TOKEN_KEY} from "../store/store";
import {AuthResponse} from "../models/dto/responses/AuthResponse";

//todo: 1. Interceptors - на сервере или на клиенте? -------------------------- ГОТОВО
//todo: 2. Refresh Token - сделать тоже jwt, это важно ------------------------ ГОТОВО 
//todo: 3. Cookie для Refresh Token, причем httpOnly -------------------------- ГОТОВО 
//todo: 4. Почта и подтверждение
//todo: 5. Вынести аутентификацию и авторизацию в отдельный проект


export const baseApiUrl = "http://localhost:5000/api";
export const baseAuthUrl = "http://localhost:34072/auth";

export const api = axios.create({
    withCredentials: true,
});

api.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem(ACCESS_TOKEN_KEY)}`;
    return config;
});

api.interceptors.response.use((config) => {
    return config;
},async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && error.config && !error.config._isRetry) {
        originalRequest._isRetry = true;
        try {
            const response = await axios.get<AuthResponse>(`${baseAuthUrl}/refresh`, {withCredentials: true})
            localStorage.setItem('token', response.data.accessToken);
            return api.request(originalRequest);
        } catch (e) {
            console.log('НЕ АВТОРИЗОВАН');
        }
    }
    throw error;
});