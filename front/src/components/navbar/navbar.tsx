import React, {useContext, useState} from 'react';
import {
    AppBar,
    Avatar,
    Box,
    Button,
    Container,
    IconButton,
    Menu,
    MenuItem,
    Toolbar,
    Tooltip,
    Typography
} from "@mui/material";
import {redirect, useNavigate} from "react-router-dom";
import {MdAccountCircle} from "react-icons/md";
import {AuthContext} from "../../context/AuthContext";

const pages = [
    {title: 'Герои', redirectTo: '/heroes'},
    {title: 'Матчи', redirectTo: '/matches'},
    {title: 'Игроки', redirectTo: '/players'}
];

const Navbar = () => {
    const {isAuth, setIsAuth} = useContext(AuthContext);
    const [settingsMenu, setSettingsMenu] = useState([
        {title: 'Login', action: () => {redirect('/login')}, ifLoggedIn: false},
        {title: 'Register', action: () => {redirect('/register')}, ifLoggedIn: false},
        {title: 'Account', action: () => {redirect('/profile')}, ifLoggedIn: true},
        {title: 'Quit', action: ()=> {setIsAuth(false)}, ifLoggedIn: true},
        {title: 'Settings', action: () => {redirect('/settings')}, ifLoggedIn: true},
    ]);
    const navigate = useNavigate();

    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);

    const resetAnchor = () => {
        setAnchorElNav(null);
    };

    const redirect = (path) => {
        navigate(path);
    };

    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };

    return (
        <AppBar style={{background: '#2E3B55'}} position="relative">
            <Container maxWidth="xl">
                <Toolbar disableGutters>
                    <Box sx={{flexGrow: 1, display: {xs: 'none', md: 'flex'}}}>
                        {pages.map((page) => (
                            <Button
                                key={page.title}
                                onClick={() => redirect(page.redirectTo)}
                                sx={{py: 2, px: 4, color: 'white', display: 'block'}}
                            >
                                {page.title}
                            </Button>
                        ))}
                    </Box>
                    {!isLoggedIn && (
                        <div>
                            <IconButton
                                size="large"
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                onClick={handleMenu}
                                color="inherit"
                            >
                                <MdAccountCircle/>
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorElNav}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                open={Boolean(anchorElNav)}
                                onClose={resetAnchor}
                            >
                                {settingsMenu.filter(x => x.ifLoggedIn == isLoggedIn).map(x => {
                                    return (<div key={x.title}>
                                        <MenuItem onClick={x.action}>{x.title}</MenuItem>
                                    </div>)
                                })}
                            </Menu>
                        </div>
                    )}
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Navbar;