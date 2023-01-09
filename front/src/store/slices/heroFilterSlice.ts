
import {createSlice} from "@reduxjs/toolkit";
import {HeroFilterModel} from "../../models/filterModels/heroFilter";

export const heroFilterSlice = createSlice({
    name: 'heroFilter',
    initialState: new HeroFilterModel(),
    reducers: {
        
    }
})