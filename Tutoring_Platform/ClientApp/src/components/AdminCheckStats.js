import React from 'react';
import { useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
/**
 * This function is created for Admin Check Statistics page and is imported in NavBar.js
 * */
export function AdminCheckStats() {
    const { user, isAuthenticated } = useAuth0();
    const [stats, setStats] = useState([]);

    const getStats = () => {
        fetch('admin/GetStats', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") {
                    setStats(data);
                }
            });
    }
    useEffect(() => {
        if (isAuthenticated) {
            getStats();
        }
        
    }, []);
    return (
        <div>
            <h2>Statistics</h2><br />
            {stats.map((a, index) =>
                <div key={index}>
                    <p><strong>{a.Name} - </strong>{a.Data}</p>
                    <br />
                </div>
            )
            }
        </div>
    );
    
}
