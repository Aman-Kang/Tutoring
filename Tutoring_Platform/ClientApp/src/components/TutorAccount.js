import React, { Component } from 'react';
import { Profile } from './Profile';

export class TutorAccount extends Component {
    static displayName = TutorAccount.name;
    render() {
        return (
            <div>
                <Profile />
            </div>
        );
    }
}
