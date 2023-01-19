import './styles/App.scss';
import React, {useEffect, useState} from "react";
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./components/AppRouter";
import Navbar from "./components/navbar/navbar";
import {downloadAllAssets} from "./assets/AssetManager";
import {AuthContext} from "./context/AuthContext";

export const App: React.FC = () => {
    const [isAuth, setIsAuth] = useState<boolean>(false);
    
    useEffect(() => {
        downloadAllAssets().then();
    }, []);
    
    useEffect(() => {
        if (localStorage.getItem('auth')){
            setIsAuth(true);
        }
    }, [])

    return (
        <AuthContext.Provider value={{
            isAuth,
            setIsAuth
        }}>
            <BrowserRouter>
                <Navbar/>
                <AppRouter/>
            </BrowserRouter>
        </AuthContext.Provider>
    );
}

export default App;
