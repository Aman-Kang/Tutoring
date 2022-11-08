import React, { Component } from "react";
import './style.css';
import { useAuth0 }
    from "@auth0/auth0-react";
export class Popup extends Component{
    render() {
        return (
            <div className="popup-box">
                <div className="box">
                    <span className="close-icon" onClick={this.props.handleClose}>x</span>
                    <div>
                        <p>Role: {this.props.role}</p>
                        <p>Name: {this.props.userId}</p>
                        <p>School: </p>
                        <p>Program: </p>
                        <p>Field of Study: </p>
                        <p>Semester: </p>
                        <button>Report User</button>
                    </div>
                </div>
            </div>
        );
    }
    
}

