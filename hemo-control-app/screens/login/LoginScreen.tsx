import React, { FC, useContext } from 'react';
import { Login } from '../../layouts';
import { NavigationProp, ParamListBase } from '@react-navigation/native';

type LoginScreenProps = {
    onLogin: (username: string, password: string) => void,
    navigation: NavigationProp<ParamListBase>
};

export const LoginScreen: FC<LoginScreenProps> = ({ navigation, onLogin }) => {
    return <>
        <Login onLogin={onLogin} navigation={navigation}></Login>
    </>;
};

