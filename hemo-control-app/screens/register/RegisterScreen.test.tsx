import React from 'react';
import { create } from 'react-test-renderer';
import { RegisterScreen } from './RegisterScreen';

describe(RegisterScreen.name, () => {
    test('renders correctly', () => {
        const tree = create(<RegisterScreen />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
