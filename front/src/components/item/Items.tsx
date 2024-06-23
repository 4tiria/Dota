import React, {useEffect, useState} from 'react';
import "./Items.scss";
import {Button, ButtonGroup, Paper, ToggleButton, ToggleButtonGroup} from "@mui/material";
import {ItemGroup} from "../../models/ItemGroup";
import ItemsShop from "./shop/ItemsShop";
import ItemsNeutral from "./neutral/ItemsNeutral";

class Page{
    name: string;
    jsx: JSX.Element;
}

const pages: Page[] = [
    {name: 'Лавка', jsx: <ItemsShop/>},
    {name: 'Нейтральные', jsx: <ItemsNeutral/>},
];

const Items = () => {

    const [page, setPage] = React.useState<Page>(pages[0]);

    const handleChange = (
        event: React.MouseEvent<HTMLElement>,
        newPage: Page
    ) => {
        if (newPage === null)
            return;
        
        setPage(newPage);
    };
    

    return (
        <div className="item-container">
            <Paper square={true}>
                <div className="d-flex justify-content-center p-2">
                    <ToggleButtonGroup
                        value={page}
                        exclusive
                        onChange={handleChange}
                    >
                        {pages.map(v => {
                            return (
                                <ToggleButton 
                                    sx={{
                                        width: 350,
                                    }}
                                    value={v}
                                    key={v.name}
                                >
                                    {v.name}
                                </ToggleButton>
                            )
                        })}
                    </ToggleButtonGroup>
                </div>
                <hr/>
                <div className="d-flex justify-content-center p-2">
                    {page.jsx}
                </div>
            </Paper>

        </div>
    );
};

export default Items;