import React from "react";
import
{ useAuth0 }
from "@auth0/auth0-react";

export function Profile() {
    const { user, isAuthenticated} = useAuth0();
    
    if (isAuthenticated) {
        return (
            <div>
                < div >

                    < h2 >{user.name}</ h2 >
                    < p >{user.email}</ p >
                    <p>{user.sub}</p>

                </ div >
            </div>
        )
    }
}
