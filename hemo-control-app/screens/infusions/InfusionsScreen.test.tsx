import React from 'react';
import { create } from 'react-test-renderer';
import { InfusionsScreen } from './InfusionsScreen';

describe(InfusionsScreen.name, () => {
    test('renders correctly', () => {
        const tree = create(<InfusionsScreen accessToken={''} />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
