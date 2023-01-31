import React, {useEffect, useState} from 'react';
import {attributes} from "../../../styles/attributes";
import "./FilterStyles.scss";
import {updateHeroMainAttributeFilter} from "../../../store/actionCreators/heroFilter";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";
import {Box, FormControl, InputLabel, MenuItem, Select} from "@mui/material";

const options = attributes.map(a => {
    return {value: a.name, label: <div style={{'color': a.color}}>{a.name}</div>}
}).concat({value: 'All', label: <div>Any</div>});

const AttributeFilter = () => {
    const heroAttributeFilter = useSelector<IRootState, string>(state => state.heroFilter.mainAttribute);
    const dispatch = useDispatch();

    const [value, setValue] = useState<string>('');

    useEffect(() => {
        dispatch(updateHeroMainAttributeFilter(value))
    }, [value]);

    return (
        <Box className="box-margin">
            <FormControl fullWidth color="secondary">
                <InputLabel id="label-main-attribute">Main Attribute</InputLabel>
                <Select
                    labelId="label-main-attribute"
                    id="select-main-attribute"
                    className="attribute-combobox"
                    value={value}
                    label="Main Attribute"
                    onChange={event =>
                        setValue(event.target.value as string)
                    }
                >
                    {options.map(x =>
                        <MenuItem 
                            value={x.value}
                            key={x.value}
                        >
                            {x.label}
                        </MenuItem>
                    )}
                </Select>
            </FormControl>
        </Box>
    );
};

export default AttributeFilter;