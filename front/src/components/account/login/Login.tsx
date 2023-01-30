import React, {KeyboardEventHandler, useState} from 'react';
import {TextField} from "@mui/material";
import Button from "@mui/material/Button";
import "./Login.scss";
import {loginOnServer} from "../../../api/accountApi";
import {RegistrationRequest} from "../../../models/dto/requests/RegistrationRequest";
import {ACCESS_TOKEN_KEY} from "../../../store/store";
import {login} from "../../../store/actionCreators/user";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import {LoginRequest} from "../../../models/dto/requests/LoginRequest";

const Login = () => {
    const [authData, setAuthData] = useState<LoginRequest>({
        email: '',
        password: ''
    });

    const [message, setMessage] = useState<string>('');

    const dispatch = useDispatch();
    const navigate = useNavigate();

    function signIn() {
        loginOnServer(authData).then(data => {
            localStorage.setItem(ACCESS_TOKEN_KEY, data.accessToken);
            setMessage("");
            dispatch(login(data.account.id, data.account.accessLevel));
            navigate(`../heroes`);
        }).catch(error => {
            if (error.response.status == 401) {
                setMessage("Wrong email or password");
            } else {
                setMessage("Server error");
            }
        });
    }

    return (
        <div className="login-container" onKeyDown={event => {
            if (event.key == "Enter") {
                signIn();
            }
        }}>
            {authData ?
                <div>
                    <div>
                        <TextField
                            className="login-input"
                            label="Email"
                            type="email"
                            variant="outlined"
                            value={authData.email}
                            onChange={event => setAuthData(prevState => ({...prevState, email: event.target.value}))}
                        />
                    </div>
                    <div>
                        <TextField
                            className="login-input"
                            label="Password"
                            type="password"
                            variant="outlined"
                            value={authData.password}
                            onChange={event => setAuthData(prevState => ({...prevState, password: event.target.value}))}
                        />
                    </div>
                    <div className="d-flex justify-content-between align-items-center">
                        <div className="error mx-2">
                            {message}
                        </div>
                        <Button
                            className="confirm-button"
                            variant="outlined"
                            onClick={signIn}
                        >
                            Sign In
                        </Button>
                    </div>

                </div> : <></>}
        </div>
    );
};

export default Login;