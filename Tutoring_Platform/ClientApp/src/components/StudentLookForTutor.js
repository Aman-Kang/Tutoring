import React from 'react';
import { useState } from 'react';
import { CustomAccordion } from './CustomAccordion';
import { useAuth0 }
    from "@auth0/auth0-react";

/**
 * Student can search for tutors on this page by submitting the course name and the days available information
 * 
 * */
export function StudentLookForTutor() {
    const { user } = useAuth0();
    const [courseName, setCourseName] = useState("");
    const [errorMessage, setError] = useState("");
    const [response, setResponse] = useState("");
    const [sunday, setSunday] = useState(0);
    const [monday, setMonday] = useState(0);
    const [tuesday, setTuesday] = useState(0);
    const [wednesday, setWednesday] = useState(0);
    const [thursday, setThursday] = useState(0);
    const [friday, setFriday] = useState(0);
    const [saturday, setSaturday] = useState(0);
    const [tutors, setTutors] = useState([]);
    const [message, setMessage] = useState("");

    const courseNameChange = (e) => {
        setCourseName(e.target.value);
    }
    const sundayChange=()=> {
        if (sunday == 0) {
            setSunday(1);
            
        }
        else {
            setSunday(0);
        }
        
    }
    const mondayChange=()=>{
        if (monday == 0) {
            setMonday(1);
        }
        else {
            setMonday(0);
        }

    }
    const tuesdayChange=()=> {
        if (tuesday == 0) {
            setTuesday(1);
        }
        else {
            setTuesday(0);
        }

    }
    const wednesdayChange=()=>{
        if (wednesday == 0) {
            setWednesday(1);
        }
        else {
            setWednesday(0);
        }

    }
    const thursdayChange=()=>{
        if (thursday == 0) {
            setThursday(1);
        }
        else {
            setThursday(0);
        }

    }
    const fridayChange=()=>{
        if (friday == 0) {
            setFriday(1);
        }
        else {
            setFriday(0);
        }

    }
    const saturdayChange=()=>{
        if (saturday == 0) {
            setSaturday(1);
        }
        else {
            setSaturday(0);
        }

    }
    const onSubmit=(e)=>{
        e.preventDefault();
        if (courseName.trim() != "" && (sunday == 1 || monday == 1 || tuesday == 1 || wednesday == 1 ||
            thursday == 1 || friday == 1 || saturday == 1)) {
            searchForTutors();
        }
        else {
            setError("All fields should be filled in to submit the request!")
        }
    }

    const messageChange = (e) => {
        setMessage(e.target.value);
    }

    const searchForTutors = () => {
        setError("");
        setResponse("");
        setTutors([]);
        fetch('student/SearchTutors', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                courseName: courseName.trim(),
                days: [sunday, monday, tuesday, wednesday, thursday, friday, saturday],
                userId: user.sub.substring(6)
            })
        }).then(res => res.json())
            .then(data => {
                setTutors(data);
                setCourseName("");
                setSunday(0);
                setMonday(0);
                setTuesday(0);
                setWednesday(0);
                setThursday(0);
                setFriday(0);
                setSaturday(0);
            });
    }

    function sendTutorRequest(courseName, days, tutorId, studId) {
        setResponse("");
        setError("");
        fetch('student/SendTutorRequest', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                courseName: courseName,
                days: days,
                tutorId: tutorId,
                studId: studId,
                message:message
            })
        }).then(res => res.text())
            .then(data => {
                setResponse(data);
            });
    }
    function reportUser(tutorId) {
        setResponse("");
        setError("");
        fetch('student/ReportUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId: user.sub.substring(6),
                accountId: tutorId
            })
        }).then(res => res.text())
            .then(data => {
                setResponse(data);
            });
    }

    return (
        <div>
            <p>Enter the below mentioned details so that we can find you a perfect tutor!</p><br />
            <p className="text-primary">{errorMessage}</p>
            <form onSubmit={onSubmit}>
                <div className="row">
                    <div className="col-6">
                        <p>Program Course in which you need help: </p>
                        <p><input type="text" value={courseName} onChange={courseNameChange }/></p>
                        
                    </div>
                    <div className="col-6">
                        <p>What days of the week you want the tutoring sessions:</p>
                        <p><input type="checkbox" value="sunday" onChange={sundayChange} checked={sunday == 1 }/> Sunday</p>
                        <p><input type="checkbox" value="monday" onChange={mondayChange} checked={monday == 1} /> Monday</p>
                        <p><input type="checkbox" value="tuesday" onChange={tuesdayChange} checked={tuesday == 1} /> Tuesday</p>
                        <p><input type="checkbox" value="wednesday" onChange={wednesdayChange} checked={wednesday == 1} /> Wednesday</p>
                        <p><input type="checkbox" value="thursday" onChange={thursdayChange} checked={thursday == 1} /> Thursday</p>
                        <p><input type="checkbox" value="friday" onChange={fridayChange} checked={friday == 1} /> Friday</p>
                        <p><input type="checkbox" value="saturday" onChange={saturdayChange} checked={saturday == 1} /> Saturday</p>
                    </div>
                </div>
                <div classname="row">
                    <div className="col-2">
                        <button className="btn btn-info">Search</button>
                    </div>
                </div>
            </form>
            <br />
            <h4>List of tutors based on your search:</h4>
            <br/>
            <div className="row">
                <div className="col-12">
                    <p className="text-primary">{response}</p>
                    {(Object.keys(tutors).length > 0)?tutors.map((t, index) =>
                        <div key={index}>
                            <CustomAccordion title={t.Name}
                                content={
                                    <div>
                                        <p><strong>School</strong> - {t.School}</p>
                                        <p><strong>Status</strong> - {t.Status}</p>
                                        <p><strong>Wage</strong> - ${t.Wage}</p>
                                        <p><strong>Enter a message for tutor</strong>: </p>
                                        <p><textarea
                                            value={message}
                                            onChange={messageChange}
                                            rows={2}
                                            cols={50}
                                        /></p>
                                        <button className="btn btn-info mr-10" onClick={(e) => sendTutorRequest(t.CourseName, t.Days, t.tutorId, t.studId, e)}>Send Tutor Request</button><a> </a>
                                        <button className="btn btn-info" onClick={(e) => reportUser(t.tutorId, e)}>Report User</button>
                                    </div>
                                } />
                            <br />
                            
                        </div>
                    ):<p>No tutors can be found! Please change your search criteria or check back later!</p>
                    }
                 </div>
            </div>
        </div>  
    );
}