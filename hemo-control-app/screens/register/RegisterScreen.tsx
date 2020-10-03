import React, { FC } from 'react';
import { Register, User } from '../../layouts';


export const RegisterScreen: FC = () => {
    const onRegister = (user: User) => console.log(JSON.stringify(user));

    return <>
        <Register onRegister={onRegister}></Register>
    </>;
};

