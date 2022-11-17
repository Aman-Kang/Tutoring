import React from 'react';
import { useAuth0 }
    from "@auth0/auth0-react";
import { useEffect, useState } from 'react';

export function StudentAccount() {
    const { user } = useAuth0();
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
    useEffect(() => {
        getInfo();
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
        if (address != "" && city != "" && postal != "" && province != "" && school != "" &&
            program != "" && field != "") {
            setError("");
            fetch('student/UpdateInfo', {
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
                    < p ><strong>Name</strong> {name }</ p >
                    < p ><strong>Email</strong> {user.email}</ p >
                    <p><strong>Address</strong> <input type="text" value={address} onChange={addressChange} /></p>
                    <p><strong>City</strong> <input type="text" value={city} onChange={cityChange} /></p>
                    <p><strong>Postal Code</strong> <input type="text" value={postal} onChange={postalChange} /></p>
                    <p><strong>Province</strong> <input type="text" value={province} onChange={provinceChange} /></p>
                    <button className="btn btn-info" onClick={updateInfo}>Update</button>
                    <p className="text-primary">{errorMessage}</p>
                </div>
                <div className="col">
                    < p ><strong>School</strong> <input type="text" value={school} onChange={schoolChange} /></ p >
                    < p ><strong>Field of Study</strong> <input type="text" value={field} onChange={fieldChange} /></ p >
                    <p><strong>Program</strong> <input type="text" value={program} onChange={programChange} /></p>
                    <p><strong>Semester</strong> <input type="text" value={semester} onChange={semesterChange} /></p>
                </div>
            </ div >
        </div>
    );
   
}