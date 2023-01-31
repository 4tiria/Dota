import React, {useContext, useEffect, useState} from 'react';
import {FormControlLabel, FormGroup, Paper, Switch} from "@mui/material";
import "./Settings.scss";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";
import {setPalette} from "../../../store/actionCreators/palette";

const Settings = () => {
    const theme = useSelector<IRootState, Palette>(state => state.palette);
    const dispatch = useDispatch();

    function handleChange(event): void {
        dispatch(setPalette(event.target.checked ? 'dark' : 'light'));
    }

    return (
        <div className="settings-container" id={theme}>
            <FormGroup>
                <FormControlLabel
                    control={
                        <Switch
                            onChange={handleChange}
                            checked={theme == 'dark'}/>
                    }
                    label={`${theme} theme`}/>
            </FormGroup>
        </div>
    );
};

export default Settings;