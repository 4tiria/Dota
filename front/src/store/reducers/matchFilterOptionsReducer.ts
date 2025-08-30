import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { Hero } from 'models/Hero'
import { MatchListFilterModel } from '../../models/filterModels/matchListFilterModel'

const initialState: MatchListFilterModel = {
    minDurationInMinutes: null,
    maxDurationInMinutes: null,
    minStartedMillisecondsBefore: null,
    maxStartedMillisecondsBefore: null,
    selfTeam: [],
    otherTeam: [],
    take: null,
    skip: null,
}

export const matchFilterSlice = createSlice({
    name: 'matchFilter',
    initialState,
    reducers: {
        setDuration: (state, action: PayloadAction<number[]>) => {
            state.minDurationInMinutes = action.payload[0]
            state.maxDurationInMinutes = action.payload[1]
        },
        setStart: (state, action: PayloadAction<number[]>) => {
            state.minStartedMillisecondsBefore = action.payload[0]
            state.maxStartedMillisecondsBefore = action.payload[1]
        },
        setSelfTeam: (state, action: PayloadAction<Hero[]>) => {
            state.selfTeam = action.payload
        },
        setOtherTeam: (state, action: PayloadAction<Hero[]>) => {
            state.otherTeam = action.payload
        },
        setTake: (state, action: PayloadAction<number>) => {
            state.take = action.payload
        },
        setSkip: (state, action: PayloadAction<number>) => {
            state.skip = action.payload
        },
        reset: () => initialState,
    },
})
