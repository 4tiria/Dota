import {configureStore, combineReducers} from "@reduxjs/toolkit";
import {heroFilterOptionsReducer} from "./reducers/heroFilterOptionsReducer";
import {matchFilterOptionsReducer} from "./reducers/matchFilterOptionsReducer";

export const rootReducer = combineReducers({
    heroFilter: heroFilterOptionsReducer,
    matchFilter: matchFilterOptionsReducer,
});

export const store = configureStore({reducer: rootReducer});

export type IRootState = ReturnType<typeof rootReducer>;