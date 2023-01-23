import './styles/App.scss';
import React, {useEffect, useState} from "react";
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {downloadAllAssets} from "./assets/AssetManager";

export const App: React.FC = () => {

    useEffect(() => {
        downloadAllAssets().then();
        //todo: get AT and RT from localstorage
    }, []);

    return (
        <BrowserRouter>
            <Navbar/>
            <AppRouter/>
        </BrowserRouter>
    );
}

export default App;
