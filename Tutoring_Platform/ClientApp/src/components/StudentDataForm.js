import React from "react";
import { useAuth0 }
    from "@auth0/auth0-react";
import { useState } from 'react';


export function StudentDataForm(props) {
    const { user, isAuthenticated } = useAuth0();
    const [name, setName] = useState("");
    const [errorMessage, setError] = useState("");
    const [address, setAddress] = useState("");
    const [city, setCity] = useState("");
    const [postal, setPostal] = useState("");
    const [province, setProvince] = useState("");
    const [school, setSchool] = useState("");
    const [field, setField] = useState("");
    const [program, setProgram] = useState("");
    const [semester, setSemester] = useState(0);

    const nameChange = (e) => {
        setName(e.target.value);
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

    const submitForm = (e) => {
        e.preventDefault();
        if (name != "" && address != "" && city != "" && province != "" && school != "" &&
            field != "" && program != "") {
            createStudent();
        }
        else {
            setError("All fields should be filled in to submit the profile data!")
        }
    }

    const createStudent = () => {
        setError("");
        fetch('student/CreateStudent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: name,
                address: address,
                city: city,
                postal: postal,
                province: province,
                school: school,
                field: field,
                program: program,
                semester: semester.toString(),
                userId: user.sub.substring(6),
                role: props.role
            })
        }).then(res => res.text())
            .then(data => {
                console.log(data);
                window.location.reload(false);
            });
    }

    return (
        <div>
            <form onSubmit={submitForm}>
                <div className="row">
                    <div className="col-6">
                        <p>Name: <input type="text" value={name} onChange={nameChange} /></p>
                        <p>Address: <input type="text" value={address} onChange={addressChange} /></p>
                        <p>City: <input type="text" value={city} onChange={cityChange} /></p>
                        <p>Postal Code: <input type="text" value={postal} onChange={postalChange} /></p>
                        <p>Province: <input type="text" value={province} onChange={provinceChange} /></p>
                        
                    </div>
                    <div className="col-6">
                        <p>School: <input type="text" value={school} onChange={schoolChange} /></p>
                        <p>Field of Study: <input type="text" value={field} onChange={fieldChange} /></p>
                        <p>Program Name: <input type="text" value={program} onChange={programChange} /></p>
                        <p>Semester: <input type="number" value={semester} onChange={ semesterChange} /> Enter 0 if you have graduated</p>
                    </div>
                    
                </div>
                <div className="row">
                    <button>Submit</button>
                </div>
                <div className="row">
                    <h5>{ errorMessage }</h5>
                </div>
            </form>
        </div>
    )
    
}

