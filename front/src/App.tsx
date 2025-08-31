import {
    createTheme,
    CssBaseline,
    ThemeOptions,
    ThemeProvider,
} from '@mui/material'
import jwt from 'jwt-decode'
import React, { useEffect } from 'react'
import { Toaster } from 'react-hot-toast'
import { useDispatch, useSelector } from 'react-redux'
import { BrowserRouter } from 'react-router-dom'
import './App.scss'
import AppRouter from './components/AppRouter'
import Navbar from './components/navbar/navbar'
import { setPaletteFromLocalStorage } from './store/actionCreators/palette'
import { login, logout } from './store/actionCreators/user'
import { ACCESS_TOKEN_KEY, IRootState } from './store/store'
import './styles/App.scss'

export const App: React.FC = () => {
    const dispatch = useDispatch()
    const themeMode = useSelector<IRootState, Palette>((state) => state.palette)
    const themeOptions: ThemeOptions = {
        palette: {
            mode: themeMode,
            primary: {
                main: '#242424',
            },
            secondary: {
                main: '#369df3',
            },
        },
    }

    const theme = createTheme(themeOptions)

    useEffect(() => {
        dispatch(setPaletteFromLocalStorage())
        initAuthData()
    }, [])

    const initAuthData = (): void => {
        const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY)
        if (accessToken === null) {
            dispatch(logout())
        } else {
            const user = jwt<any>(accessToken)
            dispatch(login(null, user && user.role ? user.role : null))
        }
    }

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <BrowserRouter>
                <Navbar />
                <AppRouter />
                <Toaster />
            </BrowserRouter>
        </ThemeProvider>
    )
}

export default App
