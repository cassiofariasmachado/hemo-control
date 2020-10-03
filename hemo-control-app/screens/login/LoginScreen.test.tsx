import React from 'react';
import { create } from 'react-test-renderer';
import { LoginScreen } from './LoginScreen';

describe(LoginScreen.name, () => {
    test('renders correctly', () => {
        const tree = create(<LoginScreen />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
