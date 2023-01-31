import {THEME} from "../store";

const initialState: Palette = 'dark';

export enum PaletteOptions {
    SET = "SET",
    SET_FROM_LOCAL_STORAGE = "SET_FROM_LOCAL_STORAGE",
}

interface IPaletteAction {
    type: PaletteOptions,
    payload: Palette
}

export const paletteReducer = (state = initialState, action: IPaletteAction): Palette => {
    switch (action.type) {
        case PaletteOptions.SET_FROM_LOCAL_STORAGE:
            return localStorage.getItem(THEME) == "dark" ? "dark" : "light";
        case PaletteOptions.SET:
            localStorage.setItem(THEME, action.payload);
            return action.payload;
        default:
            return state;
    }
}