import React, {useContext, useState} from 'react';
import {
    AppBar,
    Box,
    Button,
    Container, FormControlLabel, FormGroup,
    IconButton,
    Menu,
    MenuItem, Switch,
    Toolbar,
} from "@mui/material";
import {redirect, useNavigate} from "react-router-dom";
import {MdAccountCircle} from "react-icons/md";
import {useDispatch, useSelector} from "react-redux";
import {logout} from "../../store/actionCreators/user";
import {User} from "../../models/dto/User";
import {ACCESS_TOKEN_KEY, IRootState} from "../../store/store";
import {logoutOnServer} from "../../api/accountApi";
import {setPalette} from "../../store/actionCreators/palette";

const pages = [
    {title: 'Heroes', redirectTo: '/heroes'},
    {title: 'Matches', redirectTo: '/matches'},
    {title: 'Players', redirectTo: '/players'}
];

const Navbar = () => {
    const dispatch = useDispatch();
    const user = useSelector<IRootState, User>(state => state.user);
    const theme = useSelector<IRootState, Palette>(state => state.palette);

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
                logoutOnServer({accountId: user.accountId}).then();
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

    const handleThemeChange = (event): void => {
        dispatch(setPalette(event.target.checked ? 'dark' : 'light'));
    }

    return (
        <AppBar color="primary" position="relative">
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
                        <span>{user?.accountId}</span>
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
                                return (
                                    <div key={x.title}>
                                        <MenuItem
                                            onClick={() => {
                                                x.action();
                                                resetAnchor();
                                            }}
                                        >
                                            {x.title}
                                        </MenuItem>
                                    </div>)
                            })}
                        </Menu>
                    </div>
                    <FormGroup>
                        <FormControlLabel
                            control={
                                <Switch
                                    onChange={handleThemeChange}
                                    checked={theme == 'dark'}
                                />
                            }
                            label={`${theme}`}/>
                    </FormGroup>
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Navbar;