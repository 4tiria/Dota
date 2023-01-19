import React, {useContext} from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import HeroList from "./heroList/HeroList";
import HeroInfo from "./hero/HeroInfo";
import MatchInfo from "./match/MatchInfo";
import MatchList from "./match/list/MatchList";
import Registration from "./account/registraion/Registration";
import {AuthContext} from "../context/AuthContext";
import Login from "./account/login/Login";

const AppRouter = () => {

    return (
        <>
            <Routes>
                <Route path='*' element={<Navigate to="/heroes" replace/>}/>
                <Route path='/heroes' element={<HeroList/>}/>
                <Route path='/hero/:name' element={<HeroInfo/>}/>
                <Route path='/match' element={<MatchInfo/>}/>
                <Route path='/matches' element={<MatchList/>}/>
                <Route path='/login' element={<Login/>}/>
                <Route path='/register' element={<Registration/>}/>
            </Routes>
        </>
    );
};

export default AppRouter;