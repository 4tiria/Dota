import { Input } from '@mui/material'
import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { heroFilterSlice } from 'store/reducers/heroFilterOptionsReducer'
import { IRootState } from '../../../store/store'

const NameFilter = () => {
    const heroNameFilter = useSelector<IRootState, string>(
        (state) => state.heroFilter.name
    )
    const dispatch = useDispatch()

    return (
        <Input
            className="name-filter"
            placeholder="hero name"
            value={heroNameFilter}
            onChange={(event) => {
                dispatch(heroFilterSlice.actions.setName(event.target.value))
            }}
        />
    )
}

export default NameFilter
