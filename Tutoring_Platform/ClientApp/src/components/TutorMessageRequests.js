import React from 'react';
import {useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';

export function TutorMessageRequests(){
    const { user, isAuthenticated } = useAuth0();
    const [requests, setRequests] = useState([]);
    const [errorMessage, setError] = useState("");
    const [slot1, setSlot1] = useState("");
    const [slot2, setSlot2] = useState("");
    const [slot3, setSlot3] = useState("");
    const [slot4, setSlot4] = useState("");
    const [slot5, setSlot5] = useState("");

    const displayRequests = () => {
        fetch('tutor/GetTutorRequests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") setRequests(data);
            });
    }
    const timeSlot1Changed = (e) => {
        setSlot1(e.target.value);
    }
    const timeSlot2Changed = (e) => {
        setSlot2(e.target.value);
    }
    const timeSlot3Changed = (e) => {
        setSlot3(e.target.value);
    }
    const timeSlot4Changed = (e) => {
        setSlot4(e.target.value);
    }
    const timeSlot5Changed = (e) => {
        setSlot5(e.target.value);
    }
    function sendTimeSlots(id, slot1, slot2, slot3, slot4, slot5) {
        if (slot1 != "" && slot2 != "" && slot3 != "" && slot4 != "" && slot5 != "") {
            setError("");
            fetch('tutor/SendAppointSlots', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    requestId: id,
                    slot1: slot1,
                    slot2: slot2,
                    slot3: slot3,
                    slot4: slot4,
                    slot5: slot5
                })
            }).then(res => res.text())
                .then(data => {
                    console.log(data);
                    setSlot1("");
                    setSlot2("");
                    setSlot3("");
                    setSlot4("");
                    setSlot5("");
                });
        } else {
            setError("All fields should be filled in to submit the request!")
        }
        
    }
    useEffect(() => {
        displayRequests();
    }, []);

    function reportUser(studId) {
        fetch('tutor/ReportUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId: user.sub.substring(6),
                accountId: studId
            })
        }).then(res => res.text())
            .then(data => {
                console.log(data);
            });
    }
    return (
        <div>
            <h3>List of Students</h3>
            
            <div>
                {
                    requests.map((r, index) => 
                        <div key={index}>
                            <CustomAccordion title={r.Name}
                                content={
                                    <div>
                                        <p>School - {r.School}</p>
                                        <p>Program - {r.Program}</p>
                                        <p>Semester - {r.Semester}</p>
                                        <p>Course Name - {r.CourseName}</p>
                                        <p>Days - {}</p>

                                        <p><input type="datetime-local" value={slot1} onChange={timeSlot1Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot2} onChange={timeSlot2Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot3} onChange={timeSlot3Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot4} onChange={timeSlot4Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot5} onChange={timeSlot5Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <button onClick={(e) => sendTimeSlots(r.Id, slot1, slot2, slot3, slot4, slot5, e)}>Send Time Slots to Student</button>
                                        <h5>{errorMessage}</h5>
                                        
                                        <button onClick={(e) => reportUser(r.StudId, e) }>Report User</button>
                                    </div>
                                } />
                            <br />
                            
                        </div>
                    )
                }
            </div>
        </div>
    );
    
}
