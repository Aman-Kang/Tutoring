import React, { Component } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";

export function AdminAccount() {
    const { user, isAuthenticated } = useAuth0();
    return (
        <div>
            < div className="row" >
                <div className="col">
                    <p>Name: { }</p>
                    <p>Email: {user.email}</p>
                    
                </div>
                
            </ div >
        </div>
    );
}
