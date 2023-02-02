import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../../../store/store";
import {MatchListFilterModel} from "../../../../../models/filterModels/matchListFilterModel";
import "./DurationFilter.scss";
import Slider from "@mui/material/Slider";
import {updateDurationFilter, updateStartFilter} from "../../../../../store/actionCreators/matchFilter";
import {Box, Checkbox} from "@mui/material";

const minDistance = 2;
const waitForInputMilliseconds = 500;

const marks = [0, 30, 60, 90, 120].map(x => {
    return {value: x, label: `${x}:00`}
})

const DurationFilter = () => {
    const dispatch = useDispatch();
    const [value, setValue] = useState<number[]>([10, 80]);
    const [enableFilter, setEnableFilter] = useState<boolean>(false);
    
    useEffect(() => {
        if (!enableFilter) return;
        const timeOutId = setTimeout(
            () => dispatch(updateDurationFilter(value)),
            waitForInputMilliseconds);
        return () => clearTimeout(timeOutId);
    }, [value]);

    useEffect(() => {
        dispatch(updateDurationFilter(enableFilter ? value : [null, null]));
    }, [enableFilter]);
    
    useEffect(() => {dispatch(updateDurationFilter([null, null]))},[]);

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
                const clamped = Math.min(newValue[0], 120 - minDistance);
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
        <Box>
            <div className="d-flex justify-content-between">
                <div>
                    Duration: {`${value[0]}:00 - ${value[1]}:00`}
                </div>
                <div>
                    <Checkbox
                        color="secondary"
                        checked={enableFilter}
                        onChange={event => setEnableFilter(event.target.checked)}/>
                </div>
            </div>
            
            <Slider
                disabled={!enableFilter}
                getAriaLabel={() => 'Minimum distance shift'}
                value={value}
                max={120}
                color="secondary"
                onChange={handleChange}
                valueLabelDisplay="auto"
                marks={marks}
                getAriaValueText={() => `${value[0]}:00 - ${value[1]}:00`}
                disableSwap
            />
        </Box>
    );
};

export default DurationFilter;