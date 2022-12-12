import React from 'react';
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
import {IoMenuOutline} from "react-icons/io5";
import {redirect, useNavigate} from "react-router-dom";
import {red} from "@mui/material/colors";

const pages = [
    {title:'Герои', redirectTo:'/heroes'},
    {title:'Матчи', redirectTo:'/matches'},
    {title:'Игроки', redirectTo:'/players'}
];

const settings = ['Профиль', 'Аккаунт', 'Выйти'];


const Navbar = () => {
    const navigate = useNavigate();
    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);

    return (
        <AppBar style={{background: '#2E3B55'}} position="relative">
            <Container maxWidth="xl">
                <Toolbar disableGutters>
                    <Box sx={{flexGrow: 1, display: {xs: 'none', md: 'flex'}}}>
                        {pages.map((page) => (
                            <Button
                                key={page.title}
                                onClick={()=> navigate(page.redirectTo)}
                                sx={{py: 2, px:4, color: 'white', display: 'block'}}
                            >
                                {page.title}
                            </Button>
                        ))}
                    </Box>
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Navbar;