import React, { Component } from 'react';

export class StudentLookForTutor extends Component {
    static displayName = StudentLookForTutor.name;

    render() {
        return (
            <div>
                <p>Enter the below mentioned details so that we can find you a prefect tutor!</p>
                <p>List of tutors based on your search:</p>
            </div>
        );
    }
}