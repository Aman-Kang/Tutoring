import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';

/**
 * This function is responsible for Admin account page display and is imported in NavBar.js
 * */
export function AdminAccount() {
    const { user, isAuthenticated } = useAuth0();
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    useEffect(() => {
        if (isAuthenticated) {
            getInfo();
        }
    }, []);
    const getInfo = () => {
        fetch('admin/GetInfo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") {
                    setName(data[0].Name);
                    setEmail(data[0].Address);
                }

            });
    }
    
    return (
        <div>
            < div className="row" >
                <div className="col">
                    <p><strong>Name</strong> - {name}</p>
                    <p><strong>Email</strong> - {email}</p>

                </div>
            </ div >
        </div>
    );
}
