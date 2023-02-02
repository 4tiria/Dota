import React, {useEffect} from 'react';
import {useSelector} from "react-redux";
import {IRootState} from "../../../../store/store";
import {MatchListFilterModel} from "../../../../models/filterModels/matchListFilterModel";
import "./FilterPanel.scss";
import DurationFilter from "./durationFilter/DurationFilter";
import StartFilter from "./startFilter/StartFilter";
import {Paper} from "@mui/material";

const FilterPanel = () => {
    return (
        <Paper className="filter-panel-container">
            <div className="filter-container">
                <DurationFilter/>
            </div>
            <div className="filter-container">
                <StartFilter/>
            </div>
        </Paper>
    );
};

export default FilterPanel;