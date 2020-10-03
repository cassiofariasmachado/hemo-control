import React, { useState, FC } from 'react';
import styled from 'styled-components/native';

type LoginProps = {
    onLogin: (username: string, password: string) => void
}

const Container = styled.View`
    flex: 1;
    alignItems: center;
    justifyContent: center;
    backgroundColor: #fff;
`

const Input = styled.TextInput`
    paddingHorizontal: 10px;
    paddingVertical: 10px;
    backgroundColor: #fff;
    alignSelf: stretch;
    marginBottom: 20px;
    marginHorizontal: 30px;
    fontSize: 16px;
    borderBottomWidth: 1px;
    borderColor: black;
`

const LoginButton = styled.TouchableOpacity`
    paddingHorizontal: 20px;
    paddingVertical: 15px;
    borderRadius: 5px;
    backgroundColor: black;
    alignSelf: stretch;
    alignItems: center;
    justifyContent: center;
    marginBottom: 15px;
    marginHorizontal: 30px;
`

const LoginText = styled.Text`
    color: #fff;
    fontWeight: bold;
    fontSize: 16px;
`

export const Login: FC<LoginProps> = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    return <>
        <Container>
            <Input placeholder="Usuário" autoCompleteType="username" onChangeText={setUsername} />

            <Input placeholder="Senha" autoCompleteType="password" secureTextEntry={true} onChangeText={setPassword} />

            <LoginButton onPress={() => onLogin(username, password)}>
                <LoginText>Login</LoginText>
            </LoginButton>
        </Container>
    </>;
};
