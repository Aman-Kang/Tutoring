import React from "react";
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
        /**fetch('https://dev-lajrziuc.us.auth0.com/oauth/token',{
            method: 'POST',
            headers: { 'content-type': 'application/x-www-form-urlencoded' },
            data: new URLSearchParams({
                grant_type: 'client_credentials',
                client_id: 'g2tDObZINz3vJXu5w2aTBTQfPIoIDgwy',
                client_secret: 'eiNtsTMo0Z9ruPo-Ms0zLyKiu1a11h0OL0sarq776XUx172mZbpTa7SQmED166mr',
                audience: 'https://dev-lajrziuc.us.auth0.com/api/v2/'
            })
        });

        axios.request(options).then(function (response) {
            console.log(response.data);
        }).catch(function (error) {
            console.error(error);
        });*/

        fetch('student/FindRole', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
                //authorization:'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkJvMFVNVFU3QV9LYTRvWkE4am1IayJ9.eyJpc3MiOiJodHRwczovL2Rldi1sYWpyeml1Yy51cy5hdXRoMC5jb20vIiwic3ViIjoiYUEzZmxlUHFFdVN1UWVsYkZ3enNVZGpldEV0c09RWE1AY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZGV2LWxhanJ6aXVjLnVzLmF1dGgwLmNvbS9hcGkvdjIvIiwiaWF0IjoxNjY2NTQ4ODQxLCJleHAiOjE2NjY2MzUyNDEsImF6cCI6ImFBM2ZsZVBxRXVTdVFlbGJGd3pzVWRqZXRFdHNPUVhNIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSByZWFkOnVzZXJfY3VzdG9tX2Jsb2NrcyBjcmVhdGU6dXNlcl9jdXN0b21fYmxvY2tzIGRlbGV0ZTp1c2VyX2N1c3RvbV9ibG9ja3MgY3JlYXRlOnVzZXJfdGlja2V0cyByZWFkOmNsaWVudHMgdXBkYXRlOmNsaWVudHMgZGVsZXRlOmNsaWVudHMgY3JlYXRlOmNsaWVudHMgcmVhZDpjbGllbnRfa2V5cyB1cGRhdGU6Y2xpZW50X2tleXMgZGVsZXRlOmNsaWVudF9rZXlzIGNyZWF0ZTpjbGllbnRfa2V5cyByZWFkOmNvbm5lY3Rpb25zIHVwZGF0ZTpjb25uZWN0aW9ucyBkZWxldGU6Y29ubmVjdGlvbnMgY3JlYXRlOmNvbm5lY3Rpb25zIHJlYWQ6cmVzb3VyY2Vfc2VydmVycyB1cGRhdGU6cmVzb3VyY2Vfc2VydmVycyBkZWxldGU6cmVzb3VyY2Vfc2VydmVycyBjcmVhdGU6cmVzb3VyY2Vfc2VydmVycyByZWFkOmRldmljZV9jcmVkZW50aWFscyB1cGRhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGRlbGV0ZTpkZXZpY2VfY3JlZGVudGlhbHMgY3JlYXRlOmRldmljZV9jcmVkZW50aWFscyByZWFkOnJ1bGVzIHVwZGF0ZTpydWxlcyBkZWxldGU6cnVsZXMgY3JlYXRlOnJ1bGVzIHJlYWQ6cnVsZXNfY29uZmlncyB1cGRhdGU6cnVsZXNfY29uZmlncyBkZWxldGU6cnVsZXNfY29uZmlncyByZWFkOmhvb2tzIHVwZGF0ZTpob29rcyBkZWxldGU6aG9va3MgY3JlYXRlOmhvb2tzIHJlYWQ6YWN0aW9ucyB1cGRhdGU6YWN0aW9ucyBkZWxldGU6YWN0aW9ucyBjcmVhdGU6YWN0aW9ucyByZWFkOmVtYWlsX3Byb3ZpZGVyIHVwZGF0ZTplbWFpbF9wcm92aWRlciBkZWxldGU6ZW1haWxfcHJvdmlkZXIgY3JlYXRlOmVtYWlsX3Byb3ZpZGVyIGJsYWNrbGlzdDp0b2tlbnMgcmVhZDpzdGF0cyByZWFkOmluc2lnaHRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6bG9nc191c2VycyByZWFkOnNoaWVsZHMgY3JlYXRlOnNoaWVsZHMgdXBkYXRlOnNoaWVsZHMgZGVsZXRlOnNoaWVsZHMgcmVhZDphbm9tYWx5X2Jsb2NrcyBkZWxldGU6YW5vbWFseV9ibG9ja3MgdXBkYXRlOnRyaWdnZXJzIHJlYWQ6dHJpZ2dlcnMgcmVhZDpncmFudHMgZGVsZXRlOmdyYW50cyByZWFkOmd1YXJkaWFuX2ZhY3RvcnMgdXBkYXRlOmd1YXJkaWFuX2ZhY3RvcnMgcmVhZDpndWFyZGlhbl9lbnJvbGxtZW50cyBkZWxldGU6Z3VhcmRpYW5fZW5yb2xsbWVudHMgY3JlYXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRfdGlja2V0cyByZWFkOnVzZXJfaWRwX3Rva2VucyBjcmVhdGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiBkZWxldGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiByZWFkOmN1c3RvbV9kb21haW5zIGRlbGV0ZTpjdXN0b21fZG9tYWlucyBjcmVhdGU6Y3VzdG9tX2RvbWFpbnMgdXBkYXRlOmN1c3RvbV9kb21haW5zIHJlYWQ6ZW1haWxfdGVtcGxhdGVzIGNyZWF0ZTplbWFpbF90ZW1wbGF0ZXMgdXBkYXRlOmVtYWlsX3RlbXBsYXRlcyByZWFkOm1mYV9wb2xpY2llcyB1cGRhdGU6bWZhX3BvbGljaWVzIHJlYWQ6cm9sZXMgY3JlYXRlOnJvbGVzIGRlbGV0ZTpyb2xlcyB1cGRhdGU6cm9sZXMgcmVhZDpwcm9tcHRzIHVwZGF0ZTpwcm9tcHRzIHJlYWQ6YnJhbmRpbmcgdXBkYXRlOmJyYW5kaW5nIGRlbGV0ZTpicmFuZGluZyByZWFkOmxvZ19zdHJlYW1zIGNyZWF0ZTpsb2dfc3RyZWFtcyBkZWxldGU6bG9nX3N0cmVhbXMgdXBkYXRlOmxvZ19zdHJlYW1zIGNyZWF0ZTpzaWduaW5nX2tleXMgcmVhZDpzaWduaW5nX2tleXMgdXBkYXRlOnNpZ25pbmdfa2V5cyByZWFkOmxpbWl0cyB1cGRhdGU6bGltaXRzIGNyZWF0ZTpyb2xlX21lbWJlcnMgcmVhZDpyb2xlX21lbWJlcnMgZGVsZXRlOnJvbGVfbWVtYmVycyByZWFkOmVudGl0bGVtZW50cyByZWFkOmF0dGFja19wcm90ZWN0aW9uIHVwZGF0ZTphdHRhY2tfcHJvdGVjdGlvbiByZWFkOm9yZ2FuaXphdGlvbnNfc3VtbWFyeSBjcmVhdGU6YWN0aW9uc19sb2dfc2Vzc2lvbnMgcmVhZDpvcmdhbml6YXRpb25zIHVwZGF0ZTpvcmdhbml6YXRpb25zIGNyZWF0ZTpvcmdhbml6YXRpb25zIGRlbGV0ZTpvcmdhbml6YXRpb25zIGNyZWF0ZTpvcmdhbml6YXRpb25fbWVtYmVycyByZWFkOm9yZ2FuaXphdGlvbl9tZW1iZXJzIGRlbGV0ZTpvcmdhbml6YXRpb25fbWVtYmVycyBjcmVhdGU6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIHJlYWQ6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIHVwZGF0ZTpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgZGVsZXRlOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyBjcmVhdGU6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyByZWFkOm9yZ2FuaXphdGlvbl9tZW1iZXJfcm9sZXMgZGVsZXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJfcm9sZXMgY3JlYXRlOm9yZ2FuaXphdGlvbl9pbnZpdGF0aW9ucyByZWFkOm9yZ2FuaXphdGlvbl9pbnZpdGF0aW9ucyBkZWxldGU6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.caEuK9c42MbatmW3CZBDdyVE6zPo-isI-f0LJsma2t2lOoM8DEjqd4HM-SmD6hjVKHghkd2ZO9nFIiHxzer7kazE2HOFDM7L04iwZneYIuLdk779DDbrJYzENA0ucUKc04G7CrlScZNGelKQmLpB5m45P6QNecDWOGgldXFPH9Vgbyh_dcTsgIiR9rLsbCA2JZFMO2EIsFufkjrdxKkR_dnjCydNDEtxXLhKg1FrfffKgrVpQGxJcoEmJz3odchC_qb9V-L9gwWsWaf7GID8NUihOf5xN6W32x4xiGJnyJrTbeuCDd7sGLY0V8a1xjKtQ4XHO2xXHgxttUI_UszraA'
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
                                <NavbarBrand>Tutoring Platform</NavbarBrand>
                                <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                    <ul className="navbar-nav flex-grow">
                                        <NavItem>
                                            <LogoutButton />
                                        </NavItem>
                                        <NavItem>
                                            {user.email}
                                        </NavItem>
                                    </ul>
                                </Collapse>
                            </Navbar>
                            
                        </header>
                        
                        <StudentDataForm role={role }/>
                   </div>
                );
            } else {
                return (
                    <header>
                        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                            <NavbarBrand tag={Link} to="/">Tutoring Platform</NavbarBrand>
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
                                        <LogoutButton />
                                    </NavItem>
                                    <NavItem>
                                        {user.email}
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
                                <NavbarBrand tag={Link} to="/">Tutoring Platform</NavbarBrand>
                                <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                    <ul className="navbar-nav flex-grow">
                                        <NavItem>
                                            <LogoutButton />
                                        </NavItem>
                                        <NavItem>
                                            {user.email}
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
                            <NavbarBrand tag={Link} to="/">Tutoring Platform</NavbarBrand>
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
                                        <LogoutButton />
                                    </NavItem>
                                    <NavItem>
                                        {user.email}
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
                        <NavbarBrand tag={Link} to="/">Tutoring Platform</NavbarBrand>
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
                                    <LogoutButton />
                                </NavItem>
                                <NavItem>
                                    {user.email}
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
                    <NavbarBrand>Tutoring Platform</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <LoginButton />
                            </NavItem>
                            <NavItem>
                                <RegisterButton />
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
}

