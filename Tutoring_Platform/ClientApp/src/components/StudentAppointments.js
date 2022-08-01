import React, { Component } from 'react';

export class StudentAppointments extends Component {
    static displayName = StudentAppointments.name;

    render() {
        return (
            <div>
                <p>You cannot attend any calss without making the payment to the tutor. Pay the tutor an hour before the session otherwie the tutor will not be coming to the session.</p>
                <h3>Upcoming Appointments</h3>
                <h3>Appointment Request By Tutor</h3>
            </div>
        );
    }
}