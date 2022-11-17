import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import {useEffect, useState } from 'react';

export function TutorAccount() {
    const { user, isAuthenticated } = useAuth0();
    const [errorMessage, setError] = useState("");
    const [name, setName] = useState("");
    const [address, setAddress] = useState("");
    const [city, setCity] = useState("");
    const [postal, setPostal] = useState("");
    const [province, setProvince] = useState("");
    const [school, setSchool] = useState("");
    const [field, setField] = useState("");
    const [program, setProgram] = useState("");
    const [semester, setSemester] = useState("");
    const [wage, setWage] = useState(0);
    const [subject1, setSubject1] = useState("");
    const [subject2, setSubject2] = useState("");
    const [subject3, setSubject3] = useState("");

    const [sunday, setSunday] = useState(0);
    const [monday, setMonday] = useState(0);
    const [tuesday, setTuesday] = useState(0);
    const [wednesday, setWednesday] = useState(0);
    const [thursday, setThursday] = useState(0);
    const [friday, setFriday] = useState(0);
    const [saturday, setSaturday] = useState(0);


    useEffect(() => {
        getInfo();
    }, []);

    const getInfo = () => {
        fetch('tutor/GetInfo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.json())
            .then(data => {
                if (data != "") {
                    setName(data[0].Name);
                    setAddress(data[0].Address);
                    setCity(data[0].City);
                    setPostal(data[0].Postal);
                    setProvince(data[0].Province);
                    setSchool(data[0].School);
                    setField(data[0].Field);
                    setProgram(data[0].Program);
                    setSemester(data[0].Semester);
                    setWage(data[0].Wage);
                    setSubject1(data[0].Subjects[0]);
                    setSubject2(data[0].Subjects[1]);
                    setSubject3(data[0].Subjects[2]);
                    setSunday(data[0].Days[0]);
                    setMonday(data[0].Days[1]);
                    setTuesday(data[0].Days[2]);
                    setWednesday(data[0].Days[3]);
                    setThursday(data[0].Days[4]);
                    setFriday(data[0].Days[5]);
                    setSaturday(data[0].Days[6]);
                }
            });
    }
    const addressChange = (e) => {
        setAddress(e.target.value);
    }
    const cityChange = (e) => {
        setCity(e.target.value);
    }
    const postalChange = (e) => {
        setPostal(e.target.value);
    }
    const provinceChange = (e) => {
        setProvince(e.target.value);
    }
    const schoolChange = (e) => {
        setSchool(e.target.value);
    }
    const fieldChange = (e) => {
        setField(e.target.value);
    }
    const programChange = (e) => {
        setProgram(e.target.value);
    }
    const semesterChange = (e) => {
        setSemester(e.target.value);
    }
    const wageChange = (e) => {
        setWage(e.target.value);
    }
    const subjectChange1 = (e) => {
        setSubject1(e.target.value);
    }
    const subjectChange2 = (e) => {
        setSubject2(e.target.value);
    }
    const subjectChange3 = (e) => {
        setSubject3(e.target.value);
    }

    const sundayChange = () => {
        if (sunday == 0) {
            setSunday(1);

        }
        else {
            setSunday(0);
        }

    }
    const mondayChange = () => {
        if (monday == 0) {
            setMonday(1);
        }
        else {
            setMonday(0);
        }

    }
    const tuesdayChange = () => {
        if (tuesday == 0) {
            setTuesday(1);
        }
        else {
            setTuesday(0);
        }

    }
    const wednesdayChange = () => {
        if (wednesday == 0) {
            setWednesday(1);
        }
        else {
            setWednesday(0);
        }

    }
    const thursdayChange = () => {
        if (thursday == 0) {
            setThursday(1);
        }
        else {
            setThursday(0);
        }

    }
    const fridayChange = () => {
        if (friday == 0) {
            setFriday(1);
        }
        else {
            setFriday(0);
        }

    }
    const saturdayChange = () => {
        if (saturday == 0) {
            setSaturday(1);
        }
        else {
            setSaturday(0);
        }
    }

    const updateInfo = () => {
        if (address != "" && city != "" && postal != "" && province != "" && school != "" &&
            program != "" && field != "" && wage != 0 && (subject1 != "" || subject2 != "" || subject3 != "")) {
            setError("");
            fetch('tutor/UpdateInfo', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    userId: user.sub.substring(6),
                    address: address,
                    city: city,
                    postal: postal,
                    province: province,
                    school: school,
                    program: program,
                    field: field,
                    semester: semester,
                    wage: wage,
                    subjects: [subject1, subject2, subject3],
                    days: [sunday, monday, tuesday, wednesday, thursday, friday, saturday]
                })
            }).then(res => res.text())
                .then(data => {
                    setError(data);
                    
                });
        } else {
            setError("All fields should be filled in to submit the request!")
        }
        
    }
    return (
        <div>
            < div className="row" >
                <div className="col">
                    < p ><strong>Name</strong> {name}</ p >
                    < p ><strong>Email</strong> {user.email}</ p >
                    <p><strong>Address</strong> <input type="text" value={address} onChange={addressChange} /></p>
                    <p><strong>City</strong> <input type="text" value={city} onChange={cityChange} /></p>
                    <p><strong>Postal Code</strong> <input type="text" value={postal} onChange={postalChange} /></p>
                    <p><strong>Province</strong> <input type="text" value={province} onChange={provinceChange} /></p>
                    <p><strong>Workdays</strong> </p>
                    <p><input type="checkbox" value="sunday" onChange={sundayChange} checked={sunday == 1} /> Sunday</p>
                    <p><input type="checkbox" value="monday" onChange={mondayChange} checked={monday == 1} /> Monday</p>
                    <p><input type="checkbox" value="tuesday" onChange={tuesdayChange} checked={tuesday == 1} /> Tuesday</p>
                    <p><input type="checkbox" value="wednesday" onChange={wednesdayChange} checked={wednesday == 1} /> Wednesday</p>
                    <p><input type="checkbox" value="thursday" onChange={thursdayChange} checked={thursday == 1} /> Thursday</p>
                    <p><input type="checkbox" value="friday" onChange={fridayChange} checked={friday == 1} /> Friday</p>
                    <p><input type="checkbox" value="saturday" onChange={saturdayChange} checked={saturday == 1} /> Saturday</p>
                    <button className="btn btn-info" onClick={updateInfo}>Update</button>
                    <p className="text-primary">{errorMessage}</p>
                </div>
                <div className="col">
                    < p ><strong>School</strong> <input type="text" value={school} onChange={schoolChange} /></ p >
                    < p ><strong>Field of Study</strong> <input type="text" value={field} onChange={fieldChange} /></ p >
                    <p><strong>Program</strong> <input type="text" value={program} onChange={programChange} /></p>
                    <p><strong>Semester</strong> <input type="text" value={semester} onChange={semesterChange} /></p>
                    <p><strong>Subjects</strong> </p>
                    <p><input type="text" value={subject1} onChange={subjectChange1} /></p>
                    <p><input type="text" value={subject2} onChange={subjectChange2} /></p>
                    <p><input type="text" value={subject3} onChange={subjectChange3} /></p>
                    <p><strong>Hourly wage</strong> <input type="text" value={wage} onChange={wageChange} /></p>
                </div>
            </ div >
        </div>
    );
    
}
