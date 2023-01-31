import {PaletteOptions} from "../reducers/paletteReducer";

export const setPalette = (theme: Palette) => {
    return {type: PaletteOptions.SET, payload: theme};
};

export const setPaletteFromLocalStorage = () => {
    return {type: PaletteOptions.SET_FROM_LOCAL_STORAGE};
};

