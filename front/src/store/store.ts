import {configureStore, combineReducers} from "@reduxjs/toolkit";
import {heroFilterOptionsReducer} from "./reducers/heroFilterOptionsReducer";
import {matchFilterOptionsReducer} from "./reducers/matchFilterOptionsReducer";
import {userReducer} from "./reducers/userReducer";
import {paletteReducer} from "./reducers/paletteReducer";

export const rootReducer = combineReducers({
    heroFilter: heroFilterOptionsReducer,
    matchFilter: matchFilterOptionsReducer,
    user: userReducer,
    palette: paletteReducer,
});

export const store = configureStore({reducer: rootReducer});

export type IRootState = ReturnType<typeof rootReducer>;

export const ACCESS_TOKEN_KEY = 'accessToken';
export const THEME = 'theme';