import React, { Component } from 'react';
import { Profile } from './Profile';

export class AdminAccount extends Component {
    static displayName = AdminAccount.name;

    render() {
        return (
            <div>
                <Profile />
            </div>
        );
    }
}
