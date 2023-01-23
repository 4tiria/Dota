import {AuthData} from "../../models/temporaryModels/AuthData";
import {UserOptions} from "../reducers/userReducer";

export const login = (authData: AuthData) => {
    return {type: UserOptions.LOGIN, payload: authData};
};

export const logout = () => {
    return {type:  UserOptions.LOGOUT};
};

export const register = (authData: AuthData) => {
    return {type:  UserOptions.REGISTER,  payload: authData};
};

export const refresh = () => {
    return {type:  UserOptions.REFRESH, };
};