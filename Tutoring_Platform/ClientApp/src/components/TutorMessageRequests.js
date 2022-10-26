import React from 'react';
import { useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';

export function TutorMessageRequests(){
    const { user, isAuthenticated } = useAuth0();
    const [requests, setRequests] = useState([]);
    const displayRequests = () => {
        console.log(user.sub.substring(6));
        fetch('tutor/GetTutorRequests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                console.log(data);
                setRequests(data);
            });
    }

    
    return (
        <div>
            <h3>List of Students</h3>
            {displayRequests()}
            <div>
                {
                    requests.map((r, index) => 
                        <div key={index}>
                            <CustomAccordion title={r.Name}
                                content={
                                    <div>
                                        <p>School {r.School}</p>
                                        <p>Program {r.Program}</p>
                                        <p>Semester {r.Semester}</p>
                                        <p>Course Name {r.CourseName}</p>
                                        <p>Days</p>
                                    </div>
                                } />
                            <br />
                            <p><input type="text" /></p>
                            <p><input type="text" /></p>
                            <p><input type="text" /></p>
                            <p><input type="text" /></p>
                            <p><input type="text" /></p>
                            <button onClick={}>Send Time Slots to Student</button>
                        </div>
                    )
                }
            </div>
        </div>
    );
    
}
