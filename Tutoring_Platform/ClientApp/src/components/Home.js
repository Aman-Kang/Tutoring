import React, { Component } from 'react';
import LoginButton from './LoginButton';
import LogoutButton from './LogoutButton';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
            <h1>Hello</h1>
            <LoginButton />
            <LogoutButton />
      </div>
    );
  }
}
