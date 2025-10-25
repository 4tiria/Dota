import { UserOptions } from '../reducers/userReducer'

export const login = (userId: string, accessLevel: string) => {
    return {
        type: UserOptions.LOGIN,
        payload: { userId: userId, accessLevel: accessLevel },
    }
}

export const logout = () => {
    return { type: UserOptions.LOGOUT }
}
