import {User} from "../../models/temporaryModels/User";

const initialState: User = {
    accessLevel: null,
    email: null,
}

export enum UserOptions{
    LOGIN = "LOGIN",
    LOGOUT = "LOGOUT",
    REGISTER = "REGISTER",
    REFRESH = "REFRESH",
}


export interface IUserAction {
    type: UserOptions;
    payload: any;    
}

export const userReducer = (state: User  = initialState, 
                            action: IUserAction): User => {
    switch (action.type){
        case UserOptions.LOGIN:
            return { ...action.payload};
        case UserOptions.LOGOUT:
            return {...initialState};
        case UserOptions.REGISTER:
            return { ...action.payload};
        case UserOptions.REFRESH:
            return { ...action.payload};
    }
}