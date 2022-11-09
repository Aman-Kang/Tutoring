import React from 'react';
import {useEffect, useState } from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { CustomAccordion } from './CustomAccordion';

export function TutorAppointments(){
    const { user, isAuthenticated } = useAuth0();
    const [errorMessage, setError] = useState("");
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
        if (paypal != "" && zoom != "") {
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
                    console.log(data);
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
    useEffect(() => {
        getAppointments();
        displayAppoints();
    }, []);
    return (
        <div>
            <p>It is your responsibility to check that the student has transferred the money to your account an hour before every session. If the transfer is not made, you can choose to not go to the session.</p>
            <h3>Upcoming Appointments</h3>
            {appointmentsA.map((a, index) =>
                <div key={index}>
                    <CustomAccordion title={a.Date}
                        content={
                            <div>
                                <p>Course {a.Course}</p>
                                <p>Student: {a.TutorStud}</p>
                                <p>Meeting Link: {a.Zoom}</p>
                            </div>
                        } />
                    <br />
                </div>
            )
            }
            <h3>Confirmed Appointments By Students</h3>
            {
                appointments.map((a, index) =>
                    <div key={index}>
                        <CustomAccordion title={a.Name}
                            content={
                                <div>
                                    <p>Course: {a.Course}</p>
                                    <p>Date and Time: {a.Slot}</p>
                                    <p>Enter Paypal link: <input type="text" value={paypal} onChange={paypalChange} /></p>
                                    <p>Enter Zoom link: <input type="text" value={zoom} onChange={zoomChange} /></p>
                                    <button onClick={(e) => confirmAppoint(a.Id, e)}>Add to upcoming appointments</button>
                                    <h5>{errorMessage}</h5>
                                </div>

                            } />
                        <br />
                    </div>
                )
            }
        </div>
    );
    
}
