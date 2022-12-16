import React from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import HeroList from "./heroList/HeroList";
import HeroInfo from "./hero/HeroInfo";

const AppRouter = () => {
    return (
        <>
            <Routes>
                <Route path='*' element={<Navigate to="/heroes" replace/>}/>
                <Route path='/heroes' element={<HeroList/>}/>
                <Route path='/hero/:name' element={<HeroInfo/>}/>
            </Routes>
        </>
    );
};

export default AppRouter;