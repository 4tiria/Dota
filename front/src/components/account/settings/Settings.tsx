import React, {useContext, useEffect, useState} from 'react';
import {FormControlLabel, FormGroup, Switch} from "@mui/material";
import "./Settings.scss";
import {ThemeContext} from "../../../context/ThemeContext";

const Settings = () => {
    const {theme, setTheme} = useContext(ThemeContext);

    function handleChange(event): void {
        setTheme(event.target.checked ? 'Dark' : 'Light');
    }

    return (
        <div className="settings-container" id={theme}>
            <FormGroup>
                <FormControlLabel control={
                    <Switch onChange={handleChange}/>
                } label={`${theme} Theme`}/>
            </FormGroup>
        </div>
    );
};

export default Settings;