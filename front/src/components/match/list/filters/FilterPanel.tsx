import React, {useEffect} from 'react';
import {useSelector} from "react-redux";
import {IRootState} from "../../../../store/store";
import {MatchFilterModel} from "../../../../models/filterModels/matchFilterModel";
import "./FilterPanel.scss";
import DurationFilter from "./durationFilter/DurationFilter";

const FilterPanel = () => {
    return (
        <div className="filter-container">
            <DurationFilter/>
        </div>
    );
};

export default FilterPanel;