import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
/**
 * Creates Login Button that logs the user in with Redirect
 * */
const LoginButton = () => {
    const { loginWithRedirect } = useAuth0();

    return < button className="btn btn-info" onClick ={ () => loginWithRedirect()}> Log In </ button >;
};

export default LoginButton;
