import {createContext} from 'react';

export const ThemeContext = createContext<CustomTheme>(null);

export class CustomTheme {
    theme: Theme;
    setTheme: React.Dispatch<React.SetStateAction<Theme>>
}
