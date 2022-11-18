import React from 'react';
import {useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';
/**
 * Creates the tutoring message requests page for tutor where they can accept the requests and send time slots to students.
 * */
export function TutorMessageRequests(){
    const { user, isAuthenticated } = useAuth0();
    const [requests, setRequests] = useState([]);
    const [errorMessage, setError] = useState("");
    const [slot1, setSlot1] = useState("");
    const [slot2, setSlot2] = useState("");
    const [slot3, setSlot3] = useState("");
    const [slot4, setSlot4] = useState("");
    const [slot5, setSlot5] = useState("");
    const [message, setMessage] = useState("");

    const displayRequests = () => {
        fetch('tutor/GetTutorRequests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                setRequests(data);
            });
    }
    const messageChange = (e) => {
        setMessage(e.target.value);
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
        setError("");
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
                    slot5: slot5,
                    message: message
                })
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    setSlot1("");
                    setSlot2("");
                    setSlot3("");
                    setSlot4("");
                    setSlot5("");
                    setMessage("");
                });
        } else {
            setError("All fields should be filled in to submit the request!")
        }
        
    }
    useEffect(() => {
        if (isAuthenticated) {
            displayRequests();
        }
        
    }, []);

    function reportUser(studId) {
        setError("");
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
                setError(data);
            });
    }
    return (
        <div>
            <h2>Tutoring Requests by students</h2><br />
            <p className="text-primary">{errorMessage}</p>
            <div>
                {(Object.keys(requests).length > 0) ?
                    requests.map((r, index) => 
                        <div key={index}>
                            <CustomAccordion title={r.Name}
                                content={
                                    <div>
                                        <p><strong>School</strong> - {r.School}</p>
                                        <p><strong>Program</strong> - {r.Program}</p>
                                        <p><strong>Semester</strong> - {r.Semester}</p>
                                        <p><strong>Course Name</strong> - {r.CourseName}</p>
                                        <p><strong>Days</strong> -
                                            {((r.Days[0] == 1) ? <a>[Sunday] </a> : <a></a>)}
                                            { ((r.Days[1] == 1) ? <a>[Monday] </a> : <a></a>)}
                                            { ((r.Days[2] == 1) ? <a>[Tuesday] </a> : <a></a>)}
                                            { ((r.Days[3] == 1) ? <a>[Wednsday] </a> : <a></a>)}
                                            { ((r.Days[4] == 1) ? <a>[Thursday] </a> : <a></a>)}
                                            { ((r.Days[5] == 1) ? <a>[Friday] </a> : <a></a>)}
                                            {((r.Days[6] == 1) ? <a>[Saturday] </a> : <a></a>)}
                                        </p>
                                        <p><strong>Message</strong> - { r.Message}</p>
                                        <p><stron>Enter five time slots for the student to pick from. The sessions will be an hour long.</stron></p>
                                        <p><input type="datetime-local" value={slot1} onChange={timeSlot1Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot2} onChange={timeSlot2Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot3} onChange={timeSlot3Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot4} onChange={timeSlot4Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><input type="datetime-local" value={slot5} onChange={timeSlot5Changed} min="2022-01-01T00:00" max="2023-12-31T00:00" /></p>
                                        <p><strong>Enter a message for student</strong> </p>
                                        <p><textarea
                                            value={message}
                                            onChange={messageChange}
                                            rows={2}
                                            cols={50}
                                        /></p>
                                        <button className="btn btn-info mr-10" onClick={(e) => sendTimeSlots(r.Id, slot1, slot2, slot3, slot4, slot5, e)}>Send Time Slots to Student</button><a> </a>
                                        <button className="btn btn-info" onClick={(e) => reportUser(r.StudId, e) }>Report User</button>
                                    </div>
                                } />
                            <br />
                            
                        </div>
                    ):<p>No Tutoring requests for now!</p>
                }
            </div>
        </div>
    );
    
}
