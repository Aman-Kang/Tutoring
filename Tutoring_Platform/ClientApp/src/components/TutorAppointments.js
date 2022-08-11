import React, { Component } from 'react';

export class TutorAppointments extends Component {
    static displayName = TutorAppointments.name;

    render() {
        return (
            <div>
                <p>It is your responsibility to check that the student has transferred the money to your account an hour before every session. If the transfer is not made, you can choose to not go to the session.</p>
                <h3>Upcoming Appointments</h3>
                <h3>Confirmed Appointments By Students</h3>
            </div>
        );
    }
}
