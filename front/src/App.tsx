import './styles/App.scss';
import React from "react";
import {BrowserRouter, Navigate, Route, Routes} from "react-router-dom";
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
