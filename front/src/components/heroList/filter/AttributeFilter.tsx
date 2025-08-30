import { Box, FormControl, InputLabel, MenuItem, Select } from '@mui/material'
import { MainAttribute } from 'models/Hero'
import React, { useEffect, useState } from 'react'
import { useDispatch } from 'react-redux'
import { heroFilterSlice } from 'store/reducers/heroFilterOptionsReducer'
import { attributes } from '../../../styles/attributes'
import './FilterStyles.scss'

const options = attributes
    .map((attribute) => {
        return {
            value: attribute.name,
            label: (
                <div style={{ color: attribute.color }}>{attribute.name}</div>
            ),
        }
    })
    .concat({ value: 'All', label: <div>Any</div> })

const AttributeFilter = () => {
    const dispatch = useDispatch()

    const [value, setValue] = useState<MainAttribute | 'All'>('All')

    useEffect(() => {
        dispatch(heroFilterSlice.actions.setAttribute(value))
    }, [dispatch, value])

    return (
        <Box className="box-margin">
            <FormControl fullWidth color="secondary">
                <InputLabel id="label-main-attribute">
                    Main Attribute
                </InputLabel>
                <Select
                    labelId="label-main-attribute"
                    id="select-main-attribute"
                    className="attribute-combobox"
                    value={value}
                    label="Main Attribute"
                    onChange={(event) =>
                        setValue(event.target.value as MainAttribute | 'All')
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

export default AttributeFilter
