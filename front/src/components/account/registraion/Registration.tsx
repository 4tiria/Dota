import React, {useContext, useEffect, useState} from 'react';
import Button from "@mui/material/Button";
import {register} from "../../../api/accountApi";
import {AuthData} from "../../../models/temporaryModels/AuthData";
import "./Registration.scss";
import {Checkbox, FormControlLabel, TextField} from "@mui/material";
import {AuthContext} from "../../../context/AuthContext";

const Registration = () => {
    const {isAuth, setIsAuth} = useContext(AuthContext);

    const [isValid, setIsValid] = useState<boolean>(false);
    const [error, setError] = useState<string>(null);
    const [message, setMessage] = useState<string>('');
    const [repeatedPassword, setRepeatedPassword] = useState<string>('');
    const [authData, setAuthData] = useState<AuthData>({
        email: '',
        password: '',
        accessLevel: 'Default'
    });

    useEffect(() => {
        setValidation();
    }, [authData, repeatedPassword])

    function isValidEmail(email): boolean {
        return /\S+@\S+\.\S+/.test(email);
    }

    function isValidPassword(password): boolean {
        return password.length > 0;
    }

    function handleEmailChange(event): void {
        setAuthData(prevState => ({...prevState, email: event.target.value}));
    }

    function handlePasswordChange(event): void {
        setAuthData(prevState => ({
            ...prevState,
            password: event.target.value
        }))
    }

    function handleRepeatPasswordChange(event): void {
        setRepeatedPassword(event.target.value);
    }

    function setValidation(): void {
        if (!isValidEmail(authData.email)) {
            setIsValid(false);
            setError('Email is invalid');
            return;
        }

        if (!isValidPassword(authData.password)) {
            setIsValid(false);
            setError('Password is invalid');
            return;
        }
        
        if (authData.password != repeatedPassword){
            setIsValid(false);
            setError('Passwords do not match');
            return;
        }

        setIsValid(true);
        setError(null);
    }


    function signUp() {
        register(authData).then(data => {
            setMessage(JSON.stringify(data));
            setIsAuth(true);
            //todo: redirect somewhere
        }).catch(error =>
            setMessage(JSON.stringify(error)));
    }

    return (
        <div className="registration-container">
            {authData ?
                <div className="registration-form">
                    <div>
                        <TextField
                            className="registration-input"
                            label="Email"
                            type="email"
                            variant="outlined"
                            value={authData.email}
                            onChange={handleEmailChange}
                        />
                    </div>
                    <div>
                        <TextField
                            className="registration-input"
                            label="Password"
                            type="password"
                            variant="outlined"
                            value={authData.password}
                            onChange={handlePasswordChange}
                        />
                    </div>
                    <div>
                        <TextField
                            className="registration-input"
                            label="Repeat password"
                            type="password"
                            variant="outlined"
                            value={authData.password}
                            onChange={handleRepeatPasswordChange}
                        />
                    </div>
                    <div className="d-flex justify-content-between">
                        <div>
                            <FormControlLabel
                                control={
                                    <Checkbox
                                        className="checkbox"
                                        checked={authData.accessLevel == "Admin"}
                                        onChange={event => {
                                            setAuthData(prevState => {
                                                return ({
                                                    ...prevState,
                                                    accessLevel: event.target.checked ? "Admin" : "Default"
                                                });
                                            })
                                        }}
                                        defaultChecked/>
                                }
                                label="Make admin"
                            />
                        </div>
                        <div>
                            <Button
                                className="confirm-button"
                                variant="outlined"
                                disabled={!isValid}
                                onClick={signUp}
                            >
                                Sign Up
                            </Button>
                        </div>
                    </div>
                    <div className="error">{error}</div>
                    <div className="message">{message}</div>
                </div> : <></>}
        </div>
    );
};

export default Registration;