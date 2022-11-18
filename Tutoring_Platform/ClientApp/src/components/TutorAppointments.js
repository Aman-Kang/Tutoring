import React from 'react';
import {useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';
/**
 * Displays the upcoming appointments and any appointments for which the studnet has selected a slot.
 * */
export function TutorAppointments(){
    const { user, isAuthenticated } = useAuth0();
    const [errorMessage, setError] = useState("");
    const [response, setResponse] = useState("");
    const [appointments, setAppointments] = useState([]);
    const [paypal, setPaypal] = useState("");
    const [zoom, setZoom] = useState("");
    const [appointmentsA, setAppointmentsA] = useState([]);

    const displayAppoints = () => {
        fetch('tutor/GetConfirmedAppointments', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") setAppointments(data);
            });
    }

    const paypalChange = (e) => {
        setPaypal(e.target.value);
    }
    const zoomChange = (e) => {
        setZoom(e.target.value);
    }

    function confirmAppoint(slotId) {
        setResponse("");
        if (paypal.trim() != "" && zoom.trim() != "") {
            setError("");
            fetch('tutor/AddToAppoints', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    slotId: slotId,
                    paypal: paypal,
                    zoom: zoom
                })
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    setPaypal("");
                    setZoom("");
                });
        } else {
            setError("All fields should be filled in to submit the request!")
        }
        
    }
    const getAppointments = () => {
        fetch('tutor/GetAppointments', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") setAppointmentsA(data);
            });
    }
    function markAsDone(confirmId, appointmentDate) {
        setError("");
        setResponse("");
        var today = new Date();
        var year = parseInt(appointmentDate.substring(0, 4));
        var month = parseInt(appointmentDate.substring(5, 7));
        var day = parseInt(appointmentDate.substring(8, 10));
        var hour = parseInt(appointmentDate.substring(11, 13));
        var min = parseInt(appointmentDate.substring(14, 16));

        const date = new Date(year, month, day, hour, min);
        console.log(today.getTime() + ", " + date.getTime());
        console.log(confirmId);
        if (today.getTime() <= date.getTime()) {
            setError("");
            fetch('student/MarkAsDone', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(String(confirmId))
            }).then(res => res.text())
                .then(data => {
                    setResponse(data);
                    window.location.reload(false);
                    getAppointments();
                    displayAppoints();
                });
        } else {
            setResponse("The appointment can only be marked as done if the appointment date and time has passed!")
        }
    }
    useEffect(() => {
        if (isAuthenticated) {
            getAppointments();
            displayAppoints();
        }
        
    }, []);
    return (
        <div>
            <p>It is your responsibility to check that the student has transferred the money to your account an hour before every session. If the transfer is not made, you can choose to not go to the session.</p>
            <h3>Upcoming Appointments</h3>
            <p className="text-primary">{response}</p>
            {(Object.keys(appointmentsA).length > 0) ? appointmentsA.map((a, index) =>
                <div key={index}>
                    <CustomAccordion title={a.Date.substring(0, 10)} 
                        content={
                            <div>
                                <p><strong>Time</strong> - {a.Date.substring(11)}</p>
                                <p><strong>Course</strong> - {a.Course}</p>
                                <p><strong>Student</strong> - {a.TutorStud}</p>
                                <p><strong>Meeting Link</strong> - {a.Zoom}</p>
                                <button className="btn btn-info" onClick={(e) => markAsDone(a.ConfirmId, a.Date, e)}>Mark this appointment as Done</button>
                            </div>
                        } />
                    <br />
                </div>
            ):<p>No booked appointments for now!</p>
            }
            <h3>Confirmed Appointments By Students</h3>
            <p className="text-primary">{errorMessage}</p>
            {(Object.keys(appointments).length > 0) ?
                appointments.map((a, index) =>
                    <div key={index}>
                        <CustomAccordion title={a.Name}
                            content={
                                <div>
                                    <p><strong>Course</strong> - {a.Course}</p>
                                    <p><strong>Date and Time</strong> - <a>{a.Slot.substring(0, 10)}</a> <a>{a.Slot.substring(11)}</a></p>
                                    <p><strong>Enter Paypal link</strong> - <input type="text" value={paypal} onChange={paypalChange} /></p>
                                    <p><strong>Enter Meeting link</strong> - <input type="text" value={zoom} onChange={zoomChange} /></p>
                                    <button className="btn btn-info" onClick={(e) => confirmAppoint(a.Id, e)}>Add to upcoming appointments</button>
                                </div>
                            } />
                        <br />
                    </div>
                ):<p>No confirmed appointments by the students!</p>
            }
        </div>
    );
    
}
