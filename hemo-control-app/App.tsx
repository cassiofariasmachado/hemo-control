import React, { ReactElement, useEffect, useState, createContext, useCallback } from 'react';
import { LoginScreen, RegisterScreen } from './screens';
import { createStackNavigator } from '@react-navigation/stack';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer, RouteProp } from '@react-navigation/native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { InfusionsScreen } from './screens/infusions/InfusionsScreen';
import { Ionicons } from '@expo/vector-icons';
import { login } from './services';

const Stack = createStackNavigator();
const Tabs = createBottomTabNavigator();

const tabIcons: { [key: string]: { defaultIcon: string, focusedIcon: string } } = {
  'Infusions': {
    defaultIcon: 'ios-information-circle',
    focusedIcon: 'ios-information-circle-outline'
  },
  'Settings': {
    defaultIcon: 'ios-list-box',
    focusedIcon: 'ios-list'
  },
};

export default function App(): ReactElement {
  const [accessToken, setAccessToken] = useState('');

  useEffect(() => {
    async function loadAccessToken() {
      const accessToken = await AsyncStorage.getItem('accessToken');
      setAccessToken(accessToken || '');
    }

    loadAccessToken();
  }, []);

  const onLogin = useCallback(async (username: string, password: string) => {
    const loginResponse = await login(username, password);

    setAccessToken(loginResponse.accessToken);
  }, [])

  const onLogout = useCallback(() => {
    setAccessToken('');
  }, [])

  const notAuthenticatedHomeScreen = () => (
    <Stack.Navigator>
      <Stack.Screen name="Login" options={{ title: 'Login' }} children={(props) => <LoginScreen {...props} onLogin={onLogin} ></LoginScreen>} />
      <Stack.Screen name="Register" options={{ title: 'Cadastro' }} component={RegisterScreen} />
    </Stack.Navigator>
  );

  const getTabScreenOptions = useCallback(({ route }: any) => ({
    tabBarIcon: ({ focused, color, size }: any) => {
      const icon = tabIcons[route.name];
      const iconName = focused ? icon?.focusedIcon : icon?.defaultIcon;

      return <Ionicons name={iconName || ''} size={size} color={color} />;
    },
  }), []);

  const tabBarOptions = {
    activeTintColor: 'black',
    inactiveTintColor: 'gray',
    style: { minHeight: 60 }
  };

  const authenticatedHomeScreen = () => (
    <Tabs.Navigator screenOptions={getTabScreenOptions} tabBarOptions={tabBarOptions}>
      <Tabs.Screen name="Infusions" children={(props) => <InfusionsScreen {...props} accessToken={accessToken}></InfusionsScreen>} />
      <Tabs.Screen name="Settings" children={(props) => <InfusionsScreen {...props} accessToken={accessToken}></InfusionsScreen>} />
    </ Tabs.Navigator >
  );

  return (
    <NavigationContainer>
      {
        !accessToken
          ? notAuthenticatedHomeScreen()
          : authenticatedHomeScreen()
      }
    </NavigationContainer>
  );
}
