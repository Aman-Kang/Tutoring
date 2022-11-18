import React from 'react';
import { useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';
/**
 * Admin Report Account page is created to view all the report account requests and is imported in NavBar.js
 * */
export function AdminReportAcct(){
    const { user, isAuthenticated } = useAuth0();
    const [error, setError] = useState("");
    const [accounts, setAccounts] = useState([]);
   

    const getReportedAcc = () => {
        fetch('admin/GetReportedAcc', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                setAccounts(data);
            });
    }
    function deleteUser(accountId) {
        fetch('admin/DeleteUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ accountId:accountId })
        }).then(res => res.text())
            .then(data => {
                setError(data);
                window.location.reload(false);
                getReportedAcc();
            });
    }
    useEffect(() => {
        if (isAuthenticated) {
            getReportedAcc();
        }
        
    }, []);
    return (
        <div>
            <h2>Reported Accounts</h2><br />
            <p className="text-primary">{error}</p>
            {(Object.keys(accounts).length > 0) ? accounts.map((a, index) =>
                <div key={index}>
                    <CustomAccordion title={a.Name}
                        content={
                            <div>
                                <p><strong>Reported By</strong> - {a.By}</p>
                                <button className="btn btn-info" onClick={(e) => deleteUser(a.AccountId, e)}>Delete Reported User</button>
                            </div>
                        } />
                    <br />
                </div>
            ):<p>No account report requests at this moment!</p>
            }
        </div>
    );
    
}
