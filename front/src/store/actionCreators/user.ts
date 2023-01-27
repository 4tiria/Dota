import {UserOptions} from "../reducers/userReducer";
import {User} from "../../models/dto/User";

export const login = (email: string, accessLevel: AccessLevel) => {
    return {type: UserOptions.LOGIN, payload: {email: email, accessLevel: accessLevel}};
};

export const logout = () => {
    return {type: UserOptions.LOGOUT};
};