import React from 'react';
import { useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";

export function AdminCheckStats() {
    const { user} = useAuth0();
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
        getStats();
    }, []);
    return (
        <div>
            <h2>Statistics</h2>
            {stats.map((a, index) =>
                <div key={index}>
                    <strong>{a.Name} - </strong>{a.Data }
                    <br />
                </div>
            )
            }
        </div>
    );
    
}
