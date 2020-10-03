import React from 'react';
import { create } from 'react-test-renderer';
import { Register } from './Register';

describe(Register.name, () => {
    test('renders correctly', () => {
        const onRegister = jest.fn();
        const tree = create(<Register onRegister={onRegister} />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
