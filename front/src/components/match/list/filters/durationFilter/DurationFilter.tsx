import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../../../store/store";
import {MatchFilterModel} from "../../../../../models/filterModels/matchFilterModel";
import "./DurationFilter.scss";
import Slider from "@mui/material/Slider";
import {updateMinDurationFilter} from "../../../../../store/actionCreators/matchFilter";

const minDistance = 2;
const waitForInputMilliseconds = 500;

const DurationFilter = () => {
    const filters = useSelector<IRootState, MatchFilterModel>(state => state.matchFilter);
    const dispatch = useDispatch();
    const [value, setValue] = useState<number[]>([10, 80]);

    useEffect(() => {
        const timeOutId = setTimeout(
            () => dispatch(updateMinDurationFilter(value)), 
            waitForInputMilliseconds);
        return () => clearTimeout(timeOutId);
    }, [value]);
    
    const handleChange = (
        event: Event,
        newValue: number | number[],
        activeThumb: number,
    ) => {
        if (!Array.isArray(newValue))
            return;
        
        let updatedValue;
        if (newValue[1] - newValue[0] < minDistance) {
            if (activeThumb === 0) {
                const clamped = Math.min(newValue[0], 100 - minDistance);
                updatedValue = [clamped, clamped + minDistance];
            } else {
                const clamped = Math.max(newValue[1], minDistance);
                updatedValue = [clamped - minDistance, clamped];
            }
        } else {
            updatedValue = newValue as number[];
        }
       
        setValue(updatedValue);
    };

    return (
        <div className="duration-container">
            <div className="duration-text">
                Duration: {`${value[0]}:00 - ${value[1]}:00`}
            </div>
            <Slider
                getAriaLabel={() => 'Minimum distance shift'}
                value={value}
                onChange={handleChange}
                valueLabelDisplay="auto"
                getAriaValueText={() => `${value[0]}:00 - ${value[1]}:00`}
                disableSwap
            />
        </div>
    );
};

export default DurationFilter;