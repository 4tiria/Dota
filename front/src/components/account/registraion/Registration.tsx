import React, {useContext, useEffect, useState} from 'react';
import Button from "@mui/material/Button";
import {registerApi} from "../../../api/accountApi";
import {AuthRequest} from "../../../models/dto/requests/AuthRequest";
import "./Registration.scss";
import {Checkbox, FormControlLabel, TextField} from "@mui/material";
import {ACCESS_TOKEN_KEY} from "../../../store/store";
import {useDispatch} from "react-redux";
import {login} from "../../../store/actionCreators/user";
import {useNavigate} from "react-router-dom";

const Registration = () => {
    const [isValid, setIsValid] = useState<boolean>(false);
    const [message, setMessage] = useState<string>(null);
    const [repeatedPassword, setRepeatedPassword] = useState<string>('');
    const [authData, setAuthData] = useState<AuthRequest>({
        email: '',
        password: '',
        accessLevel: 'Default'
    });

    const dispatch = useDispatch();
    const navigate = useNavigate();
    
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
            setMessage('Email is invalid');
            return;
        }

        if (!isValidPassword(authData.password)) {
            setIsValid(false);
            setMessage('Password is invalid');
            return;
        }

        if (authData.password != repeatedPassword) {
            setIsValid(false);
            setMessage('Passwords do not match');
            return;
        }

        setIsValid(true);
        setMessage(null);
    }


    function signUp() {
        registerApi(authData).then(data => {
            localStorage.setItem(ACCESS_TOKEN_KEY, data.accessToken);
            dispatch(login(authData.email, authData.accessLevel));
            setMessage("");
            navigate(`../heroes`);
        }).catch(error => {
            setMessage("Something's wrong");
        });
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
                            value={repeatedPassword}
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
                    <div className="error">{message}</div>
                </div> : <></>}
        </div>
    );
};

export default Registration;