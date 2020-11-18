import React from 'react';
import { create } from 'react-test-renderer';
import { InfusionsList } from './InfusionsList';

describe(InfusionsList.name, () => {
    test('renders correctly', () => {
        const infusions = [
            {
                id: 1,
                date: new Date('2020-10-03T21:58:26'),
                userWeigth: 82,
                factor: {
                    brand: 'Baxter',
                    unity: 1000,
                    lot: 'ABC123'
                },
                isHemarthrosis: true,
                isBleeding: true,
                isTreatmentContinuation: true,
                bleedingLocal: 'Cotovelo D',
                treatmentLocal: 'Casa'
            },
            {
                id: 2,
                date: new Date('2020-10-03T20:58:26'),
                userWeigth: 82,
                isHemarthrosis: true,
                isBleeding: true,
                isTreatmentContinuation: true,
                bleedingLocal: 'Joelho E',
                treatmentLocal: 'Casa',
            }
        ];

        const tree = create(<InfusionsList infusions={infusions} />).toJSON();

        expect(tree).toMatchSnapshot();
    });
});
