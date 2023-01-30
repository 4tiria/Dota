import './styles/App.scss';
import React, {useContext, useEffect, useState} from "react";
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {downloadAllAssets} from "./assets/AssetManager";
import {ACCESS_TOKEN_KEY} from "./store/store";
import {login, logout} from "./store/actionCreators/user";
import jwt from 'jwt-decode';
import {useDispatch} from "react-redux";
import useLocalStorage from "use-local-storage";
import "./App.scss";
import {ThemeContext} from "./context/ThemeContext";

export const App: React.FC = () => {
    const dispatch = useDispatch();
    const [theme, setTheme] = useState<Theme>('Light');

    useEffect(() => {
        downloadAllAssets().then(_ => initAuthData());
    }, []);

    function initAuthData() {
        let accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
        if (accessToken == null) {
            dispatch(logout());
        } else {
            let user = jwt<any>(accessToken);
            dispatch(login(null, user && user.role ? user.role : null));
        }
    }

    return (
        <ThemeContext.Provider value={{
            theme: theme,
            setTheme: setTheme
        }}>
            <BrowserRouter>
                <Navbar/>
                <AppRouter/>
            </BrowserRouter>
        </ThemeContext.Provider>


    );
}

export default App;
