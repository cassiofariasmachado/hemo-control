import React, { FC, useEffect } from 'react';
import styled from 'styled-components/native';
import { InfusionResponse } from '../../models';
import { FlatList, View, Text } from 'react-native';

type InfusionsProps = {
    infusions: InfusionResponse[]
}

const Container = styled.View`
    flex: 1;
    marginTop: 50px;
    alignItems: center;
    flexDirection: column;
    alignItems: stretch;
`

const ListItem = styled.View`
    padding: 20px;
    margin: 10px;
    borderRadius: 5px;
    backgroundColor: #fff;
    alignItems: stretch;
    alignSelf: stretch;
    justifyContent: center;
    marginHorizontal: 30px;
    height: 80px;
    shadowOpacity: 0.20;
    shadowRadius: 2.20px;
    elevation: 3;
`

const ListItemTitle = styled.Text`
    fontWeight: bold;
`;

const ListItemText = styled.Text`
    color: black;
    fontSize: 16px;
`

export const InfusionsList: FC<InfusionsProps> = ({ infusions }) => {
    const renderItem = (item: InfusionResponse) => (
        <ListItem>
            <ListItemText><ListItemTitle>{item.bleedingLocal}</ListItemTitle></ListItemText>
            <ListItemText>
                <ListItemTitle>Data e local: </ListItemTitle>
                {`${item.treatmentLocal}, ${new Date(item.date).toLocaleDateString()} ${new Date(item.date).toLocaleTimeString()}`}
            </ListItemText>
            <ListItemText>
                <ListItemTitle>Fator: </ListItemTitle>
                {item.factor ? ` ${item.factor.brand}, ${item.factor.unity} UI, Lote ${item.factor.lot}` : 'Não informado'}
            </ListItemText>
        </ListItem>
    );

    return <Container>
        <FlatList data={infusions} renderItem={({ item }) => renderItem(item)} keyExtractor={item => item.id.toString()} />
    </Container>;
};
