import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { StudentNavMenu } from './StudentNavMenu';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <StudentNavMenu />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
