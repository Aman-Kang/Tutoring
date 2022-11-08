import React, { Component } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';
export function StudentHelp() {
    const { user, isAuthenticated } = useAuth0();
    const [query, setQuery] = useState("");
    const AskQuery = () => {
        fetch('student/AskQuery', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId: user.sub.substring(6),
                query: query
            })
        }).then(res => res.text())
            .then(data => {
                console.log(data);
            });
    }
    const textChange = (e) => {
        setQuery(e.target.value);
    }
    return (
        <div>
            <p>What do you need help with?</p>
            <p><textarea
                onChange={textChange}
                rows={5}
                cols={50}
            /></p>
            <button onClick={AskQuery}>Send</button>
        </div>
    );
    
}