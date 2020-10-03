import React, { FC } from 'react';
import { Login } from '../../layouts';


export const LoginScreen: FC = () => {
    const onLogin = (username: string, password: string) => console.log(username + '-' + password);

    return <>
        <Login onLogin={onLogin}></Login>
    </>;
};

