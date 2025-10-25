import axios from 'axios'
import { AuthResponse } from '../models/dto/responses/AuthResponse'
import { ACCESS_TOKEN_KEY } from '../store/store'

//TODO: перейти на jira lolxddxdxdxdxdxdxdxdxdxdxd https://www.youtube.com/watch?v=p4XKVgOnRbc
//todo: 1. Interceptors - на сервере или на клиенте? -------------------------- ГОТОВО
//todo: 2. Refresh Token - сделать тоже jwt, это важно ------------------------ ГОТОВО
//todo: 3. Cookie для Refresh Token, причем httpOnly -------------------------- ГОТОВО
//todo: 4. Почта и подтверждение  --------------------------------------------- ГОТОВО
//todo: 5. Вынести аутентификацию и авторизацию в отдельный проект

export const baseUrl = process.env.REACT_APP_BASE_URL!
export const baseApiUrl = process.env.REACT_APP_BASE_API_URL!
export const baseAuthUrl = process.env.REACT_APP_BASE_AUTH_URL!

const dateReviver = (key: string, value: any) => {
    if (typeof value === 'number' && value > 1e11) {
        return new Date(value) // ms timestamp
    }

    if (typeof value === 'string' && /^\d{13}$/.test(value)) {
        return new Date(Number(value))
    }

    return value
}

export const api = axios.create({
    withCredentials: true,
    transformResponse: [
        (data: any) => {
            if (typeof data === 'string') {
                try {
                    return JSON.parse(data, dateReviver)
                } catch {
                    return data
                }
            }
            return data
        },
    ],
})

api.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem(
        ACCESS_TOKEN_KEY
    )}`
    return config
})

api.interceptors.response.use(
    (config) => {
        return config
    },
    async (error) => {
        const originalRequest = error.config
        if (
            error.response?.status === 401 &&
            error.config &&
            !error.config._isRetry
        ) {
            originalRequest._isRetry = true
            try {
                const response = await axios.get<AuthResponse>(
                    `${baseAuthUrl}/refresh`,
                    { withCredentials: true }
                )
                localStorage.setItem('token', response.data.accessToken)
                return api.request(originalRequest)
            } catch (e) {
                console.log('НЕ АВТОРИЗОВАН')
            }
        }
        throw error
    }
)
