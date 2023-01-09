import {configureStore, combineReducers} from "@reduxjs/toolkit";
import {heroFilterOptionsReducer} from "./reducers/heroFilterOptionsReducer";

export const rootReducer = combineReducers({
    heroFilter: heroFilterOptionsReducer
});

export const store = configureStore({reducer: rootReducer});

export type IRootState = ReturnType<typeof rootReducer>;