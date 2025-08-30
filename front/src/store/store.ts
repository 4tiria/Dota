import { combineReducers, configureStore } from '@reduxjs/toolkit'
import { heroFilterSlice } from './reducers/heroFilterOptionsReducer'
import { matchFilterSlice } from './reducers/matchFilterOptionsReducer'
import { paletteReducer } from './reducers/paletteReducer'
import { userReducer } from './reducers/userReducer'

const reducer = (state = { data: null, loading: false }, action) => {
    switch (action.type) {
        case 'FETCH_START':
            return { ...state, loading: true }
        case 'FETCH_SUCCESS':
            return { data: action.payload, loading: false }
        default:
            return state
    }
}
const fetchData = () => async (dispatch) => {
    dispatch({ type: 'FETCH_START' })

    const response = await fetch('https://jsonplaceholder.typicode.com/todos/1')
    const data = await response.json()

    dispatch({ type: 'FETCH_SUCCESS', payload: data })
}

export const rootReducer = combineReducers({
    heroFilter: heroFilterSlice.reducer,
    matchFilter: matchFilterSlice.reducer,
    user: userReducer,
    palette: paletteReducer,
    reducer: reducer,
})

export const store = configureStore({
    reducer: rootReducer,
})
store.dispatch(fetchData())

export type IRootState = ReturnType<typeof rootReducer>

export const ACCESS_TOKEN_KEY = 'accessToken'
export const THEME = 'theme'
