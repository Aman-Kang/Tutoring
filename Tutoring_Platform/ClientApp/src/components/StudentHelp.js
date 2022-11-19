import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';

/**
 * Creates the Help page for student and tutor where they can submit a query and check out the replies submitted by the
 * admin
 * */
export function StudentHelp() {
    const { user, isAuthenticated } = useAuth0();
    const [query, setQuery] = useState("");
    const [replies, setReplies] = useState([]);
    const [errorMessage, setError] = useState("");
    const AskQuery = () => {
        if (query.trim() != "") {
            setError("");
            fetch('student/AskQuery', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    userId: user.sub.substring(6),
                    query: query.trim()
                })
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    setQuery("");
                });
        } else {
            setError("The question must be entered to submit this form!")
        }
        
    }

    const getReplies = () => {
        fetch('student/GetReplies', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                setReplies(data);
        });
    }


    const textChange = (e) => {
        setQuery(e.target.value);
    }
    useEffect(() => {
        if (isAuthenticated) {
            getReplies();
        }
    }, []);
    return (
        <div>
            <p>What do you need help with?</p>
            <p><textarea
                value={query }
                onChange={textChange}
                rows={5}
                cols={50}
            /></p>
            <button className="btn btn-info" onClick={AskQuery}>Send</button>
            <p className="text-primary">{errorMessage}</p>
            <br />

            <h3>Query Replies</h3><br />
            {(Object.keys(replies).length > 0) ? replies.map((a, index) =>
                <div key={index}>
                    <ul>
                        <li><strong>{a.question}</strong></li>
                    </ul>
                        <p> - {a.answer }</p>
                        <br />
                    </div>
                ) : <p>No queries have been replied for now!</p>
            }
        </div>
    );
    
}