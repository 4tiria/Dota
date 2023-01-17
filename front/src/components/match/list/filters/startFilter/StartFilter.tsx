import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {updateStartFilter} from "../../../../../store/actionCreators/matchFilter";
import "./StartFilter.scss";
import Slider from "@mui/material/Slider";

const minDistance = 0;
const waitForInputMilliseconds = 500;

const nonLinearLabels = [
    {
        label: "30 days",
        ms: 30 * 24 * 60 * 60 * 1000
    },
    {
        label: "20 days",
        ms: 20 * 24 * 60 * 60 * 1000
    },
    {
        label: "10 days",
        ms: 10 * 24 * 60 * 60 * 1000
    },
    {
        label: "5 days",
        ms: 5 * 24 * 60 * 60 * 1000
    },
    {
        label: "3 days",
        ms: 3 * 24 * 60 * 60 * 1000
    },
    {
        label: "2 days",
        ms: 2 * 24 * 60 * 60 * 1000
    },
    {
        label: "24 hours",
        ms: 24 * 60 * 60 * 1000
    },
    {
        label: "18 hours",
        ms: 18 * 60 * 60 * 1000
    },
    {
        label: "12 hours" ,
        ms: 12 * 60 * 60 * 1000
    },
    {
        label: "8 hours",
        ms: 8 * 60 * 60 * 1000
    },
    {
        label: "4 hours",
        ms: 4 * 60 * 60 * 1000
    },
    {
        label: "2 hours",
        ms: 2 * 60 * 60 * 1000
    },
    {
        label: "1 hour",
        ms: 60 * 60 * 1000
    },
]

const valueArray: ValueMs[] = nonLinearLabels.map((x, index) => {
    return {index: index, label: x.label, ms: x.ms}
});

const marks = valueArray.filter(x => x.index % 3 == 0).map(x => {
    return {value: x.index, label: x.label}
});


class ValueMs{
    index: number;
    label: string;
    ms: number;
}

const StartFilter = () => {
    const dispatch = useDispatch();
    const [value, setValue] = useState<ValueMs[]>([valueArray[0], valueArray[5]]); //Days ago

    useEffect(() => {
        const timeOutId = setTimeout(
            () => dispatch(updateStartFilter(value.map(x => x.ms))),
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
                const clamped = Math.min(newValue[0], 12 - minDistance);
                updatedValue = [clamped, clamped + minDistance];
            } else {
                const clamped = Math.max(newValue[1], minDistance);
                updatedValue = [clamped - minDistance, clamped];
            }
        } else {
            updatedValue = newValue as number[];
        }

        setValue(updatedValue.map(x => valueArray[x]));
    };

    return (
        <div>
            <div>
                Match started: 
            </div>
            <div>
                {`${value[0].label} - ${value[1].label} ago`}
            </div>
            <Slider
                getAriaLabel={() => 'Minimum distance shift'}
                value={value.map(x => x.index)}
                min={0}
                max={12}
                onChange={handleChange}
                valueLabelDisplay="off"
                marks={marks}
                getAriaValueText={() => `${value[0].label} - ${value[1].label} ago`}
                disableSwap
            />
        </div>
    );
};

export default StartFilter;