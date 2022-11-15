import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';

export function AdminAccount() {
    const { user} = useAuth0();
    const [errorMessage, setError] = useState("");
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    useEffect(() => {
        getInfo();
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
                    <p>Name: {name }</p>
                    <p>Email: { email}</p>
                    
                </div>
                
            </ div >
        </div>
    );
}
