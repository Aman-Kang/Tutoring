import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavBar } from './NavBar';
/**
 * Defines the high level layout of the site
 * */
export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <NavBar />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
