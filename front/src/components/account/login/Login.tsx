import React, {useState} from 'react';
import {TextField} from "@mui/material";
import Button from "@mui/material/Button";
import "./Login.scss";
import {loginApi} from "../../../api/accountApi";
import {AuthRequest} from "../../../models/dto/requests/AuthRequest";
import {ACCESS_TOKEN_KEY} from "../../../store/store";
import {login} from "../../../store/actionCreators/user";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";

const Login = () => {
    const [authData, setAuthData] = useState<AuthRequest>({
        email: '',
        password: '',
        accessLevel: 'Default'
    });
    
    const [message, setMessage] = useState<string>('');
    
    const dispatch = useDispatch();
    const navigate = useNavigate();

    function signIn() {
        loginApi(authData).then(data => {
            localStorage.setItem(ACCESS_TOKEN_KEY, data.accessToken);
            setMessage("");
            dispatch(login(authData.email, authData.accessLevel));
            navigate(`../heroes`);
        }).catch(error => {
            if (error.response.status == 401){
                setMessage("Wrong email or password");
            }
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