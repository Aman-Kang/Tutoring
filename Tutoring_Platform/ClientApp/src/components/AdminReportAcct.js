import React from 'react';
import { useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';

export function AdminReportAcct(){
    const { user, isAuthenticated } = useAuth0();
    const [accounts, setAccounts] = useState([]);
    useEffect(() => {
        getReportedAcc();
    }, []);

    const getReportedAcc = () => {
        fetch('student/GetReportedAcc', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") setAccounts(data);
            });
    }
    function deleteUser(accountId) {
        fetch('student/DeleteUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ accountId:accountId })
        }).then(res => res.text())
            .then(data => {
                console.log(data);
            });
    }
    return (
        <div>
            <h3>Reported Accounts</h3>
            {accounts.map((a, index) =>
                <div key={index}>
                    <CustomAccordion title={a.Name}
                        content={
                            <div>
                                <p>Reported By - {a.By}</p>
                                <button onClick={(e) => deleteUser(a.AccountId,e)}>Delete Reported User</button>
                            </div>
                        } />
                    <br />
                </div>
            )
            }
        </div>
    );
    
}
