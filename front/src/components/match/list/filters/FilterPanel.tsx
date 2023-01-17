import React, {useEffect} from 'react';
import {useSelector} from "react-redux";
import {IRootState} from "../../../../store/store";
import {MatchFilterModel} from "../../../../models/filterModels/matchFilterModel";
import "./FilterPanel.scss";
import DurationFilter from "./durationFilter/DurationFilter";
import StartFilter from "./startFilter/StartFilter";

const FilterPanel = () => {
    return (
        <div className="filter-panel-container">
            <div className="filter-container">
                <DurationFilter/>
            </div>
            <div className="filter-container">
                <StartFilter/>
            </div>
        </div>
    );
};

export default FilterPanel;