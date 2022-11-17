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
                setError(data);
                window.location.reload(false);
            });
    }

    return (
        <div className="row">
            <div className="col">
                <form onSubmit={submitForm}>
                    <div className="row">
                        <div className="col-6">
                            <p><strong>Name</strong>: <input type="text" value={name} onChange={nameChange} /></p>
                            <p><strong>Address</strong>: <input type="text" value={address} onChange={addressChange} /></p>
                            <p><strong>City</strong>: <input type="text" value={city} onChange={cityChange} /></p>
                            <p><strong>Postal Code</strong>: <input type="text" value={postal} onChange={postalChange} /></p>
                            <p><strong>Province</strong>: <select value={province} onChange={provinceChange}>
                                <option value="Alberta">Alberta</option>
                                <option value="British Columbia">British Columbia</option>
                                <option value="Manitoba">Manitoba</option>
                                <option value="New Brunswick">New Brunswick</option>
                                <option value="Newfoundland and Labrador" >Newfoundland and Labrador</option>
                                <option value="Northwest Territories">Northwest Territories</option>
                                <option value="Nova Scotia">Nova Scotia</option>
                                <option value="Nunavut">Nunavut</option>
                                <option value="Ontario" >Ontario</option>
                                <option value="Prince Edward Island">Prince Edward Island</option>
                                <option value="Quebec">Quebec</option>
                                <option value="Saskatchewan">Saskatchewan</option>
                                <option value="Yukon">Yukon</option>
                            </select></p>
                            <button className="btn btn-info">Submit</button>
                            <p className="text-primary">{errorMessage}</p>
                        </div>
                        <div className="col-6">
                            <p><strong>School</strong>: <input type="text" value={school} onChange={schoolChange} /></p>
                            <p><strong>Field of Study</strong>: <input type="text" value={field} onChange={fieldChange} /></p>
                            <p><strong>Program Name</strong>: <input type="text" value={program} onChange={programChange} /></p>
                            <p><strong>Semester</strong>: <input type="number" value={semester} onChange={semesterChange} /> Enter 0 if you have graduated</p>
                        </div>
                    </div>
                   
                </form>
            </div>
        </div>
    )
    
}

