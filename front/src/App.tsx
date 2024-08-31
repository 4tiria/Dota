import './styles/App.scss';
import React, {useEffect} from "react";
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {ACCESS_TOKEN_KEY, IRootState} from "./store/store";
import {login, logout} from "./store/actionCreators/user";
import jwt from 'jwt-decode';
import {useDispatch, useSelector} from "react-redux";
import "./App.scss";
import {createTheme, CssBaseline, ThemeOptions, ThemeProvider} from "@mui/material";
import {setPaletteFromLocalStorage} from "./store/actionCreators/palette";

export const App: React.FC = () => {
    const dispatch = useDispatch();
    const themeMode = useSelector<IRootState, Palette>(state => state.palette);
    const themeOptions: ThemeOptions = {
        palette: {
            mode: themeMode,
            primary: {
                main: '#242424',
            },
            secondary: {
                main: '#369df3',
            },
        },
    };

    const theme = createTheme(themeOptions);

    useEffect(() => {
        dispatch(setPaletteFromLocalStorage());
        initAuthData();
    }, []);

    const initAuthData = (): void => {
        const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
        if (accessToken === null) {
            dispatch(logout());
        } else {
            const user = jwt<any>(accessToken);
            dispatch(login(null, user && user.role ? user.role : null));
        }
    }

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <BrowserRouter>
                <Navbar/>
                <AppRouter/>
            </BrowserRouter>
        </ThemeProvider>
    );
}

export default App;
