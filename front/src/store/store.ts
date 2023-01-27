import {configureStore, combineReducers} from "@reduxjs/toolkit";
import {heroFilterOptionsReducer} from "./reducers/heroFilterOptionsReducer";
import {matchFilterOptionsReducer} from "./reducers/matchFilterOptionsReducer";
import {userReducer} from "./reducers/userReducer";

export const rootReducer = combineReducers({
    heroFilter: heroFilterOptionsReducer,
    matchFilter: matchFilterOptionsReducer,
    user: userReducer
});

export const store = configureStore({reducer: rootReducer});

export type IRootState = ReturnType<typeof rootReducer>;

export const ACCESS_TOKEN_KEY = 'accessToken';
export const REFRESH_TOKEN_KEY = 'refreshToken';
export const CURRENT_USER_EMAIL = 'email';