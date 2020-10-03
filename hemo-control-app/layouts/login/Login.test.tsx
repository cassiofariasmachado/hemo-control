import React from 'react';
import { create } from 'react-test-renderer';
import { Login } from './Login';

describe(Login.name, () => {
    test('renders correctly', () => {
        const onLogin = jest.fn();
        const tree = create(<Login onLogin={onLogin} />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
