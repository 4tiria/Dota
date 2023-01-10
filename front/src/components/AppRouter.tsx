import React from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import HeroList from "./heroList/HeroList";
import HeroInfo from "./hero/HeroInfo";
import MatchInfo from "./match/MatchInfo";
import MatchList from "./match/list/MatchList";

const AppRouter = () => {
    return (
        <>
            <Routes>
                <Route path='*' element={<Navigate to="/heroes" replace/>}/>
                <Route path='/heroes' element={<HeroList/>}/>
                <Route path='/hero/:name' element={<HeroInfo/>}/>
                <Route path='/match' element={<MatchInfo/>}/>
                <Route path='/matches' element={<MatchList/>}/>
            </Routes>
        </>
    );
};

export default AppRouter;