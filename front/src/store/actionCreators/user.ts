import {UserOptions} from "../reducers/userReducer";
import {User} from "../../models/dto/User";
import {Account} from "../../models/Account";

export const login = (userId: string, accessLevel: string) => {
    return {type: UserOptions.LOGIN, payload: {userId: userId, accessLevel: accessLevel}};
};

export const logout = () => {
    return {type: UserOptions.LOGOUT};
};