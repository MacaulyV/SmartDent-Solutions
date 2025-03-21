import React, { useState } from 'react';
import { View, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { AuthService } from '../services/auth';
import { CampoFormulario } from '../components/CampoFormulario';
import { styles } from '../styles/styles';

const LoginScreen = () => {
  const navigation = useNavigation();
  const [usuario, setUsuario] = useState('');
  const [cpf, setCpf] = useState('');
  const [alertMessage, setAlertMessage] = useState('');
  const [alertVisible, setAlertVisible] = useState(false);
  const [successAlertVisible, setSuccessAlertVisible] = useState(false);

  const handleLogin = async () => {
    if (usuario.trim() === '' || cpf.replace(/[^0-9]/g, '').length < 11) {
      Alert.alert("Erro", "Por favor, preencha todos os campos obrigatórios corretamente.");
      return;
    }

    const authService = await AuthService.getInstance();
    const credentials = {
      identifier: usuario,
      cpf: cpf.replace(/[^0-9]/g, ''),
      loginType: 'name',
    };

    const response = await authService.login(credentials);
    if (response.success) {
      setAlertMessage("Login realizado com sucesso!");
      setSuccessAlertVisible(true);
      
      setTimeout(() => {
        setSuccessAlertVisible(false);
        navigation.navigate('Main');
      }, 1500);
    } else {
      setAlertMessage(response.message);
      setAlertVisible(true);
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.formularioContainer}>
        <CampoFormulario
          placeholder="Digite seu nome completo"
          valor={usuario}
          setValor={setUsuario}
          delay={1700}
          animationIndex={0}
        />
        <CampoFormulario
          placeholder="Digite seu CPF"
          valor={cpf}
          setValor={setCpf}
          delay={1700}
          animationIndex={1}
        />
      </View>
    </View>
  );
};

export default LoginScreen; 