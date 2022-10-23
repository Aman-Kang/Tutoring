import React, { Component } from 'react';
import { Profile } from './Profile';

export class StudentAccount extends Component {
    static displayName = StudentAccount.name;

    render() {
        return (
            <div>
                <Profile />
            </div>
        );
    }
}