import { Box, FormControl, InputLabel, MenuItem, Select } from '@mui/material'
import { AttackType } from 'models/Hero'
import React, { useEffect, useState } from 'react'
import { useDispatch } from 'react-redux'
import { heroFilterSlice } from 'store/reducers/heroFilterOptionsReducer'
import { attackTypes } from '../../../styles/attackTypes'
import './FilterStyles.scss'

const options = attackTypes
    .map((at) => {
        return {
            value: at.name,
            label: (
                <div>
                    {at.name} {at.icon}
                </div>
            ),
        }
    })
    .concat({ value: 'All', label: <div>Any</div> })

const AttackTypeFilter = () => {
    const dispatch = useDispatch()

    const [value, setValue] = useState<AttackType | 'All'>('All')

    useEffect(() => {
        dispatch(heroFilterSlice.actions.setAttackType(value))
    }, [value])

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
                    onChange={(event) =>
                        setValue(event.target.value as AttackType | 'All')
                    }
                >
                    {options.map((x) => (
                        <MenuItem value={x.value} key={x.value}>
                            {x.label}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </Box>
    )
}

export default AttackTypeFilter
