import React, { Component } from 'react';

export class StudentLookForTutor extends Component {
    static displayName = StudentLookForTutor.name;

    render() {
        return (
            <div>
                <p>Enter the below mentioned details so that we can find you a prefect tutor!</p>
                <div className="row">
                    <div className="col-6">
                        <p>Program Course in which you need help (Write the full name of the course)</p>
                        <p><input type="text" /></p>
                        <button>Submit</button>
                    </div>
                    <div className="col-6">
                        <p>What days of the week you want the tutoring sessions:</p>
                        <p><input type="checkbox" /> Sunday</p>
                        <p><input type="checkbox" /> Monday</p>
                        <p><input type="checkbox" /> Tuesday</p>
                        <p><input type="checkbox" /> Wednesday</p>
                        <p><input type="checkbox" /> Thursday</p>
                        <p><input type="checkbox" /> Friday</p>
                        <p><input type="checkbox" /> Saturday</p>
                    </div>
                </div>
                <p>List of tutors based on your search:</p>
                <p>Filters</p>
                <div className="row">
                    <div className="col-3">
                        <p>Status</p>
                        <p></p>
                    </div>
                    <div className="col-3">
                        <p>Wage</p>
                        <p></p>
                    </div>
                    <div className="col-3">
                        <p>School</p>
                        <p></p>
                    </div>
                    <div className="col-3">
                        <p>Program</p>
                        <p></p>
                    </div>
                </div>
            </div>  
        );
    }
}