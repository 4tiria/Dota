import React, {useContext, useState} from 'react';
import {
    AppBar,
    Box,
    Button,
    Container,
    IconButton,
    Menu,
    MenuItem,
    Toolbar,
} from "@mui/material";
import {redirect, useNavigate} from "react-router-dom";
import {MdAccountCircle} from "react-icons/md";
import {useDispatch, useSelector} from "react-redux";
import {logout} from "../../store/actionCreators/user";
import {User} from "../../models/dto/User";
import {ACCESS_TOKEN_KEY, IRootState} from "../../store/store";
import {logoutApi} from "../../api/accountApi";

const pages = [
    {title: 'Герои', redirectTo: '/heroes'},
    {title: 'Матчи', redirectTo: '/matches'},
    {title: 'Игроки', redirectTo: '/players'}
];

const Navbar = () => {
    const dispatch = useDispatch();
    const user = useSelector<IRootState, User>(state => state.user);

    const [settingsMenu, setSettingsMenu] = useState([
        {
            title: 'Login', action: () => {
                redirect('/login')
            }, ifLoggedIn: false
        },
        {
            title: 'Register', action: () => {
                redirect('/register')
            }, ifLoggedIn: false
        },
        {
            title: 'Account', action: () => {
                redirect('/profile')
            }, ifLoggedIn: true
        },
        {
            title: 'Logout', action: () => {
                logoutApi({email: user.email}).then();
                localStorage.removeItem(ACCESS_TOKEN_KEY);
                dispatch(logout());
                redirect('/login');
            }, ifLoggedIn: true
        },
        {
            title: 'Settings', action: () => {
                redirect('/settings')
            }, ifLoggedIn: true
        },
    ]);
    const navigate = useNavigate();

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
                    <div>
                        <span>{user.email}</span>
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
                            {settingsMenu.filter(x => x.ifLoggedIn == user.isAuth).map(x => {
                                return (<div key={x.title}>
                                    <MenuItem onClick={x.action}>{x.title}</MenuItem>
                                </div>)
                            })}
                        </Menu>
                    </div>

                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Navbar;