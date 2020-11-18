import React, { FC, useEffect, useState } from 'react';
import { InfusionsList } from '../../layouts';
import { InfusionResponse } from '../../models';
import { getInfusions } from '../../services';

type InfusionsScreenProps = { accessToken: string };

export const InfusionsScreen: FC<InfusionsScreenProps> = ({ accessToken }) => {
    const [infusions, setInfusions] = useState<InfusionResponse[]>([]);

    useEffect(() => {
        async function loadInfusions() {
            const response = await getInfusions(accessToken);

            setInfusions(response.items);
        }

        loadInfusions();
    }, [accessToken]);

    return <>
        <InfusionsList infusions={infusions} ></InfusionsList>
    </>;
};

