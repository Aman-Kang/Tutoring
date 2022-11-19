import React from 'react';
import { CustomAccordion } from './CustomAccordion';
import { useEffect,useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";

/**
 * Admin can check user messages and reply to those messages on this page. This component is imported in NavBar.js
 * */
export function AdminUserMsg(){
    const { user, isAuthenticated } = useAuth0();
    const [errorMessage, setError] = useState("");
    const [queries, setQueries] = useState([]);
    const [reply, setReply] = useState("");

    useEffect(() => {
        if (isAuthenticated) {
            getQueries();
        }
        
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
    function sendReply(queryId) {
        if (reply.trim() != "") {
            setError("");
            fetch('admin/SendReply', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    queryId: queryId,
                    userId: user.sub.substring(6),
                    query: reply.trim()
                })
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    setReply("");
                });
        } else {
            setError("The reply must be entered before submitting!")
        }
        
    }
    return (
        <div>
            <h2>User Messages</h2><br />
            <p className="text-primary">{errorMessage}</p>
            {(Object.keys(queries).length > 0) ? queries.map((q, index) =>
                <div key={index}>
                    <CustomAccordion title={q.UserId}
                        content={
                            <div>
                                <p>{q.Query}</p>
                                <p><strong>Enter you response</strong>: </p>
                                <p><textarea
                                    value={reply}
                                    onChange={replyChange}
                                    rows={5}
                                    cols={50}
                                /></p>
                                <button className="btn btn-info" onClick={(e) => sendReply(q.QueryId, e)}>Send a Reply</button>
                                
                            </div>
                        } />
                    <br />
                </div>
            ):<p>No User messages at this moment!</p>
            }
        </div>
    );
    
}
