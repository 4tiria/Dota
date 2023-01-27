import './styles/App.scss';
import React, {useEffect, useState} from "react";
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {downloadAllAssets} from "./assets/AssetManager";
import {ACCESS_TOKEN_KEY} from "./store/store";
import {login, logout} from "./store/actionCreators/user";
import jwt from 'jwt-decode';
import {useDispatch} from "react-redux";

export const App: React.FC = () => {

    const dispatch = useDispatch();

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
        <BrowserRouter>
            <Navbar/>
            <AppRouter/>
        </BrowserRouter>
    );
}

export default App;
