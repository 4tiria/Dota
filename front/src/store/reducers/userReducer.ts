import {User} from "../../models/dto/User";

const initialState: User = {
    accessLevel: null,
    accountId: null,
    isAuth: false,
}

export enum UserOptions {
    LOGIN = "LOGIN",
    LOGOUT = "LOGOUT",
    REFRESH = "REFRESH",
}


export interface IUserAction {
    type: UserOptions;
    payload: any;
}

export const userReducer = (state: User = initialState,
                            action: IUserAction): User => {
    switch (action.type) {
        case UserOptions.LOGIN:
            return {...action.payload, isAuth: true};
        case UserOptions.LOGOUT:
            return {...initialState};
        default:
            return {...state};
    }
}