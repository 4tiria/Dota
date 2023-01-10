import React, {useEffect, useState} from 'react';
import {ICallBack} from "../../interfaces/ICallBack";
import {Input} from "@mui/material";
import {IReset} from "../../interfaces/IReset";
import {HeroFilterModel} from "../../../models/filterModels/heroFilter";
import {useDispatch, useSelector} from 'react-redux';
import HeroFilterOptionsReducer, {IHeroFilterState} from "../../../store/reducers/heroFilterOptionsReducer";
import {updateHeroNameFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";


const NameFilter = () => {
    const heroNameFilter = useSelector<IRootState, string>(state => state.heroFilter.name);
    const dispatch = useDispatch();
    
    return (
        <div>
            <Input className="name-filter"
                   placeholder="hero name"
                   value={heroNameFilter}
                   onChange={(event) => {
                       let newValue = event.target.value;
                       dispatch(updateHeroNameFilter(newValue));
                   }}
            />
        </div>
    );
};

export default NameFilter;