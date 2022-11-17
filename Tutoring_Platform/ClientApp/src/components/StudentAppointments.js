import React from 'react';
import { CustomAccordion } from './CustomAccordion';
import { useEffect,useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";

export function StudentAppointments(){
    const { user } = useAuth0();
    const [requests, setRequests] = useState([]);
    const [errorMessage, setError] = useState("");
    const [response, setResponse] = useState("");
    const [slot, setSlot] = useState("");
    const [appointments, setAppointments] = useState([]);
    const displayRequests = () => {
        fetch('student/DisplayRequests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "")setRequests(data);
            });
    }

    const slotChanged = (e) => {
        setSlot(e.target.value);

    }
    const getAppointments = () => {
        fetch('student/GetAppointments', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => 
            res.json()
        )
            .then(data => {
                setAppointments(data);
            });
    }
    const sendConfirmedSlot = () => {
        if (slot != "") {
            setError("");
            fetch('student/SendConfirmedSlot', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(slot)
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    setSlot(0);
                });
        } else {
            setError("A slot must be selected in order to submit it!")
        }
        
    }

    function markAsDone(confirmId, appointmentDate) {
        var today = new Date();
        var year = parseInt(appointmentDate.substring(0, 4));
        var month = parseInt(appointmentDate.substring(5, 7));
        var day = parseInt(appointmentDate.substring(8, 10));
        var hour = parseInt(appointmentDate.substring(11, 13));
        var min = parseInt(appointmentDate.substring(14, 16));

        const date = new Date(year, month, day, hour, min);
        console.log(today.getTime() + ", " + date.getTime());
        console.log(confirmId);
        if (today.getTime() <= date.getTime())
        {
            setError("");
            fetch('student/MarkAsDone', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(String(confirmId))
            }).then(res => res.text())
                .then(data => { 
                    window.location.reload(false);
                    setResponse(data);
                });
        } else {
            setResponse("The appointment can only be marked as done if the appointment date and time has passed!")
        }
    }
    useEffect(() => {
        getAppointments();
        displayRequests();
    }, []);
    return (
        <div>
            <p>You cannot attend any class without making the payment to the tutor. Pay the tutor an hour before the session otherwise the tutor will not be coming to the session.</p>
            <h3>Upcoming Appointments</h3>
            <p className="text-primary">{response}</p>
            {(Object.keys(appointments).length > 0) ? 
                appointments.map((a, index) =>
                    <div key={index}>
                        <CustomAccordion title={a.Date.substring(0, 10)} 
                            content={
                                <div>
                                    <p><strong>Time</strong> - {a.Date.substring(11)}</p>
                                    <p><strong>Course</strong> - {a.Course}</p>
                                    <p><strong>Tutor</strong> - {a.TutorStud}</p>
                                    <p><strong>Paypal link</strong> - {a.Paypal}</p>
                                    <p><strong>Meeting Link</strong> - {a.Zoom}</p>
                                    <button className="btn btn-info" onClick={(e) => markAsDone(a.ConfirmId,a.Date,e)}>Mark this appointment as Done</button>
                                </div>

                            } />
                        <br />
                    </div>
                ) : <p>No appointments booked for now!</p>
            
            }
            <h3>Appointment Request By Tutor</h3>
            <p className="text-primary">{errorMessage}</p>
            {(Object.keys(requests).length > 0) ? requests.map((r, index) =>
                <div key={index}>
                    <CustomAccordion title={r.Name}
                        content={
                            <div>
                                <p><strong>Course</strong> - {r.CourseName}</p>
                                <p><strong>Message</strong> - {r.Message }</p>
                                <p><strong>Please pick a time slot from below</strong>:</p>

                                <p><input type="radio" value={r.Id1} onChange={slotChanged} checked={slot == r.Id1} /> {r.Slot1 }</p>
                                <p><input type="radio" value={r.Id2} onChange={slotChanged} checked={slot == r.Id2} /> {r.Slot2}</p>
                                <p><input type="radio" value={r.Id3} onChange={slotChanged} checked={slot == r.Id3} /> {r.Slot3}</p>
                                <p><input type="radio" value={r.Id4} onChange={slotChanged} checked={slot == r.Id4} /> {r.Slot4}</p>
                                <p><input type="radio" value={r.Id5} onChange={slotChanged} checked={slot == r.Id5} /> {r.Slot5}</p>
                                <button className="btn btn-info" onClick={sendConfirmedSlot}>Send Time Slot to Tutor</button>
                                
                            </div>
                        } />
                    <br />
                </div>
            ):<p>No appointment requests by tutor!</p>
            }
        </div>
    );
    
}