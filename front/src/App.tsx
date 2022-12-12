import './styles/App.scss';
import React from "react";
import HeroInfo from "./components/hero/heroInfo";
import {BrowserRouter, Navigate, Route, Routes} from "react-router-dom";
import HeroList from "./components/heroList/HeroList";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";

export const App: React.FC = () => {
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
