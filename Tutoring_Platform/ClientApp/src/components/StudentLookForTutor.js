import React, { Component } from 'react';
import { useState } from 'react';
import { CustomAccordion } from './CustomAccordion';

export function StudentLookForTutor(){
    const [courseName, setCourseName] = useState("");
    const [sunday, setSunday] = useState(0);
    const [monday, setMonday] = useState(0);
    const [tuesday, setTuesday] = useState(0);
    const [wednesday, setWednesday] = useState(0);
    const [thursday, setThursday] = useState(0);
    const [friday, setFriday] = useState(0);
    const [saturday, setSaturday] = useState(0);
    const [tutors, setTutors] = useState([]);

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
        searchForTutors();
    }

    const searchForTutors=()=>{
        fetch('student/SearchTutors', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                courseName: courseName,
                days: [sunday, monday, tuesday, wednesday, thursday, friday, saturday]
            })
        }).then(res => res.json())
            .then(data => {
                setTutors(data);
        });
    }

    return (
        <div>
            <p>Enter the below mentioned details so that we can find you a prefect tutor!</p>
            <form onSubmit={onSubmit}>
                <div className="row">
                    <div className="col-6">
                        <p>Program Course in which you need help (Write the full name of the course)</p>
                        <p><input type="text" value={courseName} onChange={courseNameChange }/></p>
                        <button>Submit</button>
                    </div>
                    <div className="col-6">
                        <p>What days of the week you want the tutoring sessions:</p>
                        <p><input type="checkbox" value="sunday" onChange={sundayChange }/> Sunday</p>
                        <p><input type="checkbox" value="monday" onChange={mondayChange} /> Monday</p>
                        <p><input type="checkbox" value="tuesday" onChange={tuesdayChange}/> Tuesday</p>
                        <p><input type="checkbox" value="wednesday" onChange={wednesdayChange} /> Wednesday</p>
                        <p><input type="checkbox" value="thursday" onChange={thursdayChange} /> Thursday</p>
                        <p><input type="checkbox" value="friday" onChange={fridayChange} /> Friday</p>
                        <p><input type="checkbox" value="saturday" onChange={saturdayChange} /> Saturday</p>
                    </div>
                </div>
            </form>
            
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
                <div>
                    {tutors.map((t, index) =>
                        <div key={index}>
                            <CustomAccordion title={t.Name}
                                content={
                                    <div>
                                        <p>School {t.School}</p>
                                        <p>Status {t.Status}</p>
                                        <p>Wage {t.Wage}</p>
                                    </div>
                                } />
                            <br />
                        </div>
                    )}
                {
                    tutors.map((t, index) => {
                        console.log(t);
                        console.log(t.Name);
                        console.log(t.School);
                        <div key={index}>
                            <p>{t.Status}</p>
                            <CustomAccordion
                                title={t.Name}
                                content={t.Program + "<br/>" + t.School + "<br/>" + t.Status + "<br/>" + t.Wage}
                            />
                        </div>
                    })
                    }
                 </div>
            </div>
        </div>  
    );
}