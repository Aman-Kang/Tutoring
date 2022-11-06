import React, { Component } from 'react';
import { CustomAccordion } from './CustomAccordion';
import { useAuth0 }
    from "@auth0/auth0-react";
export function StudentAppointments(){
    const { user, isAuthenticated } = useAuth0();
    const displayRequests = () => {
        console.log(user.sub.substring(6));
        fetch('student/DisplayRequests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                console.log(data);
            });
    }
    
    return (
        <div>
            <p>You cannot attend any calss without making the payment to the tutor. Pay the tutor an hour before the session otherwie the tutor will not be coming to the session.</p>
            <h3>Upcoming Appointments</h3>
            <h3>Appointment Request By Tutor</h3>
            {displayRequests()}
        </div>
    );
    
}