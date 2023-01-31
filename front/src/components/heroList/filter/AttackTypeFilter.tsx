import React, {useEffect, useState} from 'react';
import {attackTypes} from "../../../styles/attackTypes";
import "./FilterStyles.scss";
import {useDispatch, useSelector} from "react-redux";
import {updateHeroAttackTypeFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";
import {Box, FormControl, InputLabel, MenuItem, Select} from "@mui/material";


const options = attackTypes.map(at => {
    return {value: at.name, label: <div>{at.name} {at.icon}</div>}
}).concat({value: "All", label: <div>Any</div>});

const AttackTypeFilter = () => {
    const heroAttackTypeFilter = useSelector<IRootState, string>(state => state.heroFilter.attackType);
    const dispatch = useDispatch();
    
    const [value, setValue] = useState<string>('');

    useEffect(() => {
        dispatch(updateHeroAttackTypeFilter(value))
    }, [value]);
    
    return (
        <Box className="box-margin">
            <FormControl fullWidth color="secondary">
                <InputLabel id="label-attack-type">Attack Type</InputLabel>
                <Select
                    labelId="label-attack-type"
                    id="select-attack-type"
                    className="attribute-combobox"
                    value={value}
                    label="Attack Type"
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

export default AttackTypeFilter;