import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import LogoutButton from './LogoutButton';
import LoginButton from './LoginButton';
import RegisterButton from './RegisterButton';

import { useAuth0 }
    from "@auth0/auth0-react";

export class StudentNavMenu extends Component {
    static displayName = StudentNavMenu.name;

  constructor (props) {
    super(props);

      this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

    render() {
        if (isAuthenticated) {
            return (
                <header>
                    <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>

                        <NavbarBrand tag={Link} to="/">Tutoring Platform</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
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
        } else {
            return (
                <header>
                    <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>

                        <NavbarBrand>Tutoring Platform</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
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
}
