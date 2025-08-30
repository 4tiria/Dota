import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { AttackType, MainAttribute } from 'models/Hero'
import { HeroFilterModel } from '../../models/filterModels/heroFilter'

const initialState: HeroFilterModel = {
    name: '',
    attackType: 'All',
    mainAttribute: 'All',
    roles: [],
}

export const heroFilterSlice = createSlice({
    name: 'heroFilter',
    initialState,
    reducers: {
        setName: (state, action: PayloadAction<string>) => {
            state.name = action.payload
        },
        setAttackType: (state, action: PayloadAction<'All' | AttackType>) => {
            state.attackType = action.payload
        },
        setAttribute: (state, action: PayloadAction<'All' | MainAttribute>) => {
            state.mainAttribute = action.payload
        },
        setTags: (state, action: PayloadAction<string[]>) => {
            state.roles = action.payload
        },
        reset: () => initialState,
    },
})
