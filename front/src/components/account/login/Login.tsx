import React, {useContext, useState} from 'react';
import {Checkbox, FormControlLabel, TextField} from "@mui/material";
import Button from "@mui/material/Button";
import {AuthContext} from "../../../context/AuthContext";
import "./Login.scss";
import {AuthData} from "../../../models/temporaryModels/AuthData";
import {login} from "../../../api/accountApi";

const Login = () => {
    const {isAuth, setIsAuth} = useContext(AuthContext);
    const [authData, setAuthData] = useState<AuthData>({
        email: '',
        password: '',
        accessLevel: 'Default'
    });

    function signIn() {
        login(authData).then(data => {
            setIsAuth(true);
        });
    }

    return (
        <div className="login-container">
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
                    <div className="d-flex justify-content-end">
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