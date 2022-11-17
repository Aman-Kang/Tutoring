import React from "react";
import { Container } from 'reactstrap';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { useState } from 'react';
import './NavMenu.css';
import LogoutButton from './LogoutButton';
import LoginButton from './LoginButton';
import RegisterButton from './RegisterButton';
import { StudentDataForm } from './StudentDataForm';

import { useAuth0 }
    from "@auth0/auth0-react";

export function NavBar(){
    const [collapsed, setCollapsed] = useState(true);
    const [role, setRole] = useState("");
    const [showDialog, setShowDialog] = useState(0);
    const { user, isAuthenticated} = useAuth0();
    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    }

    const findRole = () => {
        
        fetch('student/FindRole', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.email)
        }).then(res => res.text())
            .then(data => {
                if (data != "") setRole(data);
            });
    }

    const isUserCreated = () => {
        fetch('student/UserCreated', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user.sub.substring(6))
        }).then(res => res.text())
            .then(data => {
                if (data != "") setShowDialog(data);
            });
    }
    
    if (isAuthenticated) {
        findRole();
        if (role == "student") {
            isUserCreated();
            if (showDialog == 1) {
                return (
                    <div>
                        <header>
                            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                                <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                                <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                    <ul className="navbar-nav flex-grow">
                                        <NavItem>
                                            <NavLink><LogoutButton /></NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink>{user.email}</NavLink>
                                        </NavItem>
                                    </ul>
                                </Collapse>
                            </Navbar>
                        </header>
                        <Container>
                            <h4>Enter all details to complete your profile</h4>
                            <StudentDataForm role={role} />
                        </Container>
                   </div>
                );
            } else {
                return (
                    <header>
                        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                            <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/student-appointments">Appointments</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/look-for-tutor">Look For Tutor</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/student-help">Help</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/student-account">Account</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink><LogoutButton /></NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink>{user.email}</NavLink>
                                    </NavItem>
                                </ul>
                            </Collapse>
                        </Navbar>
                    </header>
                );
            }
            
        } else if (role == "tutor") {
            isUserCreated();
            if (showDialog == 1) {
                return (
                    <div>
                        <header>
                            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                                <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                                <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                    <ul className="navbar-nav flex-grow">
                                        <NavItem>
                                            <NavLink><LogoutButton /></NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink>{user.email}</NavLink>
                                        </NavItem>
                                    </ul>
                                </Collapse>
                            </Navbar>
                        </header>

                        <StudentDataForm role={role }/>
                    </div>
                );
            }
            else {
                return (
                    <header>
                        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                            <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                <ul className="navbar-nav flex-grow">
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/tutor-appointments">Appointments</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/tutor-message-requests">Tutoring Message Requests</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/student-help">Help</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/tutor-account">Account</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink><LogoutButton /></NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink>{user.email}</NavLink>
                                    </NavItem>
                                </ul>
                            </Collapse>
                        </Navbar>
                    </header>
                );
            }
        }else if(role == "admin") {
            return (
                <header>
                    <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                        <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/check-statistics">Check Statistics</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/user-messages">User messages</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/reported-accounts">Reported Accounts</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/admin-account">Account</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink><LogoutButton /></NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink>{user.email}</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Navbar>
                </header>
            );
        }
    } else {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                    <NavbarBrand tag={Link} to="/">Tutoriaa</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink><LoginButton /></NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink><RegisterButton /></NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
}

