import React, { Component } from 'react';
import { CustomAccordion } from './CustomAccordion';
import { useEffect,useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";


export function AdminUserMsg(){
    const { user, isAuthenticated } = useAuth0();
    const [queries, setQueries] = useState([]);
    const [reply, setReply] = useState([]);

    useEffect(() => {
        getQueries();
    }, []);
    const getQueries = () => {
        fetch('admin/GetQueries', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                setQueries(data);
            });
    }
    const replyChange = (e) => {
        setReply(e.target.value);
    }
    function sendReply(queryId){
        fetch('admin/SendReply', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                queryId: queryId,
                userId: user.sub.substring(6),
                query:reply
            })
        }).then(res => res.text())
            .then(data => {
                console.log(data);
            });
    }
    return (
        <div>
            <h3>User Messages</h3>
            {queries.map((q, index) =>
                <div key={index}>
                    <CustomAccordion title={q.UserId}
                        content={
                            <div>
                                <p>{q.Query}</p>
                                <p>Enter you response: </p>
                                <p><textarea
                                    onChange={replyChange}
                                    rows={5}
                                    cols={50}
                                /></p>
                                <button onClick={(e) => sendReply(q.QueryId,e)}>Send a Reply</button>
                            </div>
                        } />
                    <br />
                </div>
            )
            }
        </div>
    );
    
}
