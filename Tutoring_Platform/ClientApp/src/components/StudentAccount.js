import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';
/**
 * Creates the student account page that shows profile data.
 * */
export function StudentAccount() {
    const { user, isAuthenticated } = useAuth0();
    const [errorMessage, setError] = useState("");
    const [name, setName] = useState("");
    const [address, setAddress] = useState("");
    const [city, setCity] = useState("");
    const [postal, setPostal] = useState("");
    const [school, setSchool] = useState("");
    const [province, setProvince] = useState("Alberta");
    const [field, setField] = useState("Arts and Humanities");
    const [program, setProgram] = useState("");
    const [semester, setSemester] = useState("");
    useEffect(() => {
        if (isAuthenticated) {
            getInfo();
        }
    }, []);
    const getInfo = () => {
        fetch('student/GetInfo', {
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
    const updateInfo = () => {
        if (address.trim() != "" && city.trim() != "" && postal.trim() != "" && province.trim() != "" && school.trim() != "" &&
            program.trim() != "" && field.trim() != "") {
            setError("");
            fetch('student/UpdateInfo', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    userId: user.sub.substring(6),
                    address: address.trim(),
                    city: city.trim(),
                    postal: postal.trim(),
                    province: province.trim(),
                    school: school.trim(),
                    program: program.trim(),
                    field: field.trim(),
                    semester: semester
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
                    <h2>Profile</h2><br />
                    < p ><strong>Name</strong> {name }</ p >
                    < p ><strong>Email</strong> {user.email}</ p >
                    <p><strong>Address</strong> <input type="text" value={address} onChange={addressChange} /></p>
                    <p><strong>City</strong> <input type="text" value={city} onChange={cityChange} /></p>
                    <p><strong>Postal Code</strong> <input type="text" value={postal} onChange={postalChange} /></p>
                    <p><strong>Province</strong> <select value={province} onChange={provinceChange}>
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
                    <button className="btn btn-info" onClick={updateInfo}>Update</button>
                    <p className="text-primary">{errorMessage}</p>
                </div>
                <div className="col">
                    < p ><strong>School</strong> <input type="text" value={school} onChange={schoolChange} /></ p >
                    <p><strong>Field of Study</strong> <select value={field} onChange={fieldChange}>
                        <option value="Arts and Humanities">Arts & Humanities</option>
                        <option value="Business and Management">Business & Management</option>
                        <option value="Computer Sciences">Computer Sciences</option>
                        <option value="Education">Education</option>
                        <option value="Fine Arts" >Fine Arts</option>
                        <option value="Engineering and Technology">Engineering & Technology</option>
                        <option value="Mathematics">Mathematics</option>
                        <option value="Medicine and Life Sciences">Medicine and Life Sciences</option>
                        <option value="Natural Sciences" >Natural Sciences</option>
                        <option value="Social Sciences">Social Sciences</option>
                    </select></p>
                    <p><strong>Program</strong> <input type="text" value={program} onChange={programChange} /></p>
                    <p><strong>Semester</strong> <input type="text" value={semester} onChange={semesterChange} /></p>
                </div>
            </ div >
        </div>
    );
   
}