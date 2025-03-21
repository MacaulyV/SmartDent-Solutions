// navigation/AppNavigator.tsx

import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

// Importo as telas do app
import WelcomeScreen from '../screens/Onboarding/WelcomeScreen';
import ChoiceScreen from '../screens/Onboarding/ChoiceScreen';
import LoginScreen from '../screens/Auth/LoginScreen';
import RegisterScreen from '../screens/Auth/RegisterScreen';
import MainScreen from '../screens/Main/MainScreen';
import ProfileScreen from '../screens/Main/ProfileScreen';

// Crio o Stack Navigator para gerenciar as telas
const Stack = createNativeStackNavigator();

export default function AppNavigator() {
  // NavigationContainer envolve toda a navegação e mantém o estado da navegação.
  // Aqui, configurei as telas e desativei o cabeçalho para cada uma.
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="Welcome">
        {/* Tela de boas-vindas */}
        <Stack.Screen
          name="Welcome"
          component={WelcomeScreen}
          options={{ headerShown: false }}
        />

        {/* Tela para escolha de login ou cadastro */}
        <Stack.Screen
          name="Choice"
          component={ChoiceScreen}
          options={{ headerShown: false }}
        />

        {/* Tela de login */}
        <Stack.Screen
          name="Login"
          component={LoginScreen}
          options={{ headerShown: false }}
        />

        {/* Tela de cadastro */}
        <Stack.Screen
          name="Register"
          component={RegisterScreen}
          options={{ headerShown: false }}
        />

        {/* Tela principal do app */}
        <Stack.Screen
          name="Main"
          component={MainScreen}
          options={{ headerShown: false }}
        />

        {/* Adicione a tela de Profile */}
        <Stack.Screen
          name="Profile"
          component={ProfileScreen}
          options={{ headerShown: false }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
