import React, {useEffect, useState} from 'react';
import './ItemsShop.scss'
import {ItemGroup} from "../../../models/ItemGroup";
import {IRootState, THEME} from "../../../store/store";
import {useSelector} from "react-redux";
import {Button} from "@mui/material";

const ItemsMain = () => {
    const [groups, setGroups] = useState<ItemGroup[]>([]);
    const themeMode = useSelector<IRootState, Palette>(state => state.palette);

    useEffect(() => {
        setGroups([
            {id: 1, name: '1'},
            {id: 2, name: '2'},
            {id: 3, name: '3'},
            {id: 4, name: '4'},
            {id: 5, name: '5'},
            {id: 6, name: '6'},
            {id: 7, name: '7'},
            {id: 8, name: '8'},
            {id: 9, name: '9'},
            {id: 10, name: '10'},
            {id: 11, name: '11'},
        ]);
    }, []);

    return (
        <div className="d-flex justify-content-center">
            {groups.map(g => {
                return (
                    <div className="item-group-column" key={g.id}>
                        <div className={`item-group ${themeMode === "dark"
                            ? "item-group-black"
                            : "item-group-white"}`}
                        >
                            {g.name}
                        </div>
                        <div className="my-2">
                            {['i', 't', 'e', 'm', 's'].map(i => {
                                return (
                                    <div 
                                        className="d-flex justify-content-center"
                                        key={i}
                                    >
                                        {i}
                                    </div>
                                )
                            })}
                        </div>

                    </div>
                );
            })}
        </div>
    );
};

export default ItemsMain;