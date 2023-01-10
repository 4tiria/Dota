import './styles/App.scss';
import React, {useEffect} from "react";
import {BrowserRouter, Navigate, Route, Routes} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {downloadHeroImages} from "./assets/HeroImages";
import {downloadAllAssets} from "./assets/AssetManager";

export const App: React.FC = () => {
    useEffect(() => {
        downloadAllAssets().then();
    }, []);

    return (
        <div>
            <BrowserRouter>
                <Navbar/>
                <AppRouter/>
            </BrowserRouter>
        </div>
    );
}

export default App;
