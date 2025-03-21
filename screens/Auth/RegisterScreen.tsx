import React, { useEffect, useRef, useState } from 'react';
import {
  View,
  Text,
  Image,
  StyleSheet,
  Dimensions,
  Animated,
  Easing,
  TouchableOpacity,
  TextInput,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
  Modal,
  StatusBar,
  ModalProps,
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import RadialGradient from 'react-native-radial-gradient';
import MainScreen from '../Main/MainScreen';
import { IPacienteData } from '../../services/api';
import { AuthService } from '../../services/auth';

const { width, height } = Dimensions.get('window');

interface BotaoAnimadoProps {
  onPress: () => void;
  children: React.ReactNode;
  style?: object;
  textStyle?: object;
}

const BotaoAnimado: React.FC<BotaoAnimadoProps> = ({ onPress, children, style, textStyle }) => {
  const scaleVal = useRef(new Animated.Value(0)).current;
  const opacVal = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    Animated.parallel([
      Animated.timing(scaleVal, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.elastic(1.2)),
      }),
      Animated.timing(opacVal, {
        toValue: 1,
        duration: 600,
        useNativeDriver: true,
      }),
    ]).start();
  }, []);

  const pressIn = () => {
    Animated.spring(scaleVal, {
      toValue: 0.92,
      friction: 3,
      useNativeDriver: true,
    }).start();
  };

  const pressOut = () => {
    Animated.spring(scaleVal, {
      toValue: 1,
      friction: 3,
      tension: 40,
      useNativeDriver: true,
    }).start();
  };

  return (
    <Animated.View style={{ transform: [{ scale: scaleVal }], opacity: opacVal }}>
      <TouchableOpacity
        onPressIn={pressIn}
        onPressOut={pressOut}
        onPress={onPress}
        style={[styles.botao, style]}
        activeOpacity={0.8}
      >
        <Text style={[styles.textoBotao, textStyle]}>{children}</Text>
      </TouchableOpacity>
    </Animated.View>
  );
};

interface CampoFormularioProps {
  placeholder: string;
  valor: string;
  setValor: (valor: string) => void;
  keyboardType?: 'default' | 'numeric' | 'email-address';
  secureTextEntry?: boolean;
  onBlur?: () => void;
  delay?: number;
}

const CampoFormulario: React.FC<CampoFormularioProps> = ({
  placeholder,
  valor,
  setValor,
  keyboardType = 'default',
  secureTextEntry = false,
  onBlur,
  delay = 0,
}) => {
  const inputOpac = useRef(new Animated.Value(0)).current;
  const inputPos = useRef(new Animated.Value(50)).current;
  const inputScale = useRef(new Animated.Value(0.9)).current;

  useEffect(() => {
    Animated.sequence([
      Animated.delay(delay),
      Animated.parallel([
        Animated.timing(inputOpac, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(inputPos, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.7)),
        }),
        Animated.timing(inputScale, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
      ]),
    ]).start();
  }, []);

  return (
    <Animated.View 
      style={[
        styles.campoContainer,
        {
          opacity: inputOpac,
          transform: [
            { translateY: inputPos },
            { scale: inputScale }
          ],
        },
      ]}
    >
      <TextInput
        style={styles.input}
        placeholder={placeholder}
        placeholderTextColor="#999"
        value={valor}
        onChangeText={setValor}
        keyboardType={keyboardType}
        secureTextEntry={secureTextEntry}
        onBlur={onBlur}
      />
    </Animated.View>
  );
};

interface RegisterScreenProps {
  navigation: any;
}

const AlertaPersonalizado: React.FC<{ visible: boolean; mensagem: string; onClose: () => void; alertButtonText: string }> = ({ visible, mensagem, onClose, alertButtonText }) => {
  const fadeAnim = useRef(new Animated.Value(0)).current;
  const scaleAnim = useRef(new Animated.Value(0.5)).current;
  const slideAnim = useRef(new Animated.Value(20)).current;
  
  useEffect(() => {
    if (visible) {
      Animated.parallel([
        Animated.timing(fadeAnim, {
          toValue: 1,
          duration: 300,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(scaleAnim, {
          toValue: 1,
          duration: 350,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
        Animated.timing(slideAnim, {
          toValue: 0,
          duration: 350,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
      ]).start();
    } else {
      Animated.parallel([
        Animated.timing(fadeAnim, {
          toValue: 0,
          duration: 250,
          useNativeDriver: true,
        }),
        Animated.timing(scaleAnim, {
          toValue: 0.8,
          duration: 250,
          useNativeDriver: true,
        }),
      ]).start();
    }
  }, [visible]);

  return (
    <Modal
      transparent={true}
      animationType="none"
      visible={visible}
      onRequestClose={onClose}
    >
      <Animated.View style={[styles.modalBackground, { opacity: fadeAnim }]}>
        <Animated.View 
          style={[
            styles.alertContainer, 
            { 
              opacity: fadeAnim,
              transform: [
                { scale: scaleAnim },
                { translateY: slideAnim }
              ] 
            }
          ]}
        >
          <LinearGradient
            colors={['#0077FF', '#00AEFF']}
            start={{ x: 0, y: 0 }}
            end={{ x: 1, y: 0 }}
            style={styles.alertHeader}
          >
            <Text style={styles.alertHeaderText}>Atenção</Text>
          </LinearGradient>
          <View style={styles.alertContent}>
            <Text style={styles.alertText}>{mensagem}</Text>
            <TouchableOpacity 
              onPress={onClose} 
              style={styles.alertButton}
            >
              <LinearGradient
                colors={['#0084FF', '#00C3FF']}
                start={{ x: 0, y: 0 }}
                end={{ x: 1, y: 0 }}
                style={styles.alertButtonGradient}
              >
                <Text style={styles.alertButtonText}>{alertButtonText}</Text>
              </LinearGradient>
            </TouchableOpacity>
          </View>
        </Animated.View>
      </Animated.View>
    </Modal>
  );
};

const RegisterScreen: React.FC<RegisterScreenProps> = ({ navigation }) => {
  const [nomeCompleto, setNomeCompleto] = useState('');
  const [nomeError, setNomeError] = useState('');
  const [cpf, setCpf] = useState('');
  const [cpfError, setCpfError] = useState('');
  const [email, setEmail] = useState('');
  const [emailError, setEmailError] = useState('');
  const [empresa, setEmpresa] = useState('');
  const [termos, setTermos] = useState(false);
  const [alertVisible, setAlertVisible] = useState(false);
  const [alertMessage, setAlertMessage] = useState('');
  const [successAlertVisible, setSuccessAlertVisible] = useState(false);

  // Animação da entrada principal
  const backgroundOpacity = useRef(new Animated.Value(0)).current;
  const masterScale = useRef(new Animated.Value(1.05)).current;
  const masterFade = useRef(new Animated.Value(0)).current;
  
  // Animações de elementos específicos
  const contOpac = useRef(new Animated.Value(0)).current;
  const contScale = useRef(new Animated.Value(0.95)).current;
  const logoOpac = useRef(new Animated.Value(0)).current;
  const logoSc = useRef(new Animated.Value(0.6)).current;
  const logoPos = useRef(new Animated.Value(-60)).current;
  const logoRotate = useRef(new Animated.Value(-15)).current;
  const titleOpac = useRef(new Animated.Value(0)).current;
  const titlePos = useRef(new Animated.Value(-40)).current;
  const titleScale = useRef(new Animated.Value(0.7)).current;
  const btnOpac = useRef(new Animated.Value(0)).current;
  const btnPos = useRef(new Animated.Value(30)).current;
  const termosOpac = useRef(new Animated.Value(0)).current;
  const termosPos = useRef(new Animated.Value(40)).current;
  const voltarOpac = useRef(new Animated.Value(0)).current;

  // Lista de todos os planos odontológicos
  const todosOsPlanos: string[] = [
    'Dental Júnior',
    'Bem Estar',
    'Bem Estar White',
    'Bem Estar Pró',
    'Bem Estar Orto',
    'Bem Estar Orto White',
    'Convencional',
    'Integral',
    'Integral Plus',
    'Integral Doc',
    'Integral Doc Plus',
    'Premium',
    'Superior',
    'Classical',
    'Classical Doc',
    'Ômega',
    'Master',
    'Maximum White'
  ];

  /**
   * Função para gerar um plano odontológico aleatório.
   * Retorna um plano aleatório da lista de todos os planos.
   */
  const gerarPlanoOdonto = (): string => {
    const indice = Math.floor(Math.random() * todosOsPlanos.length);
    const planoSelecionado = todosOsPlanos[indice].trim();
    console.log('Plano odontológico selecionado:', planoSelecionado);
    return planoSelecionado; // Retorna apenas o nome do plano
  };

  /*****************************************************
   * Funções de Validação e Formatação dos Campos
   *****************************************************/

  // Filtra o nome para aceitar apenas letras e espaços e valida se contém pelo menos 3 palavras.
  const handleNomeChange = (text: string) => {
    // Remove caracteres que não sejam letras ou acentos
    const filtered = text.replace(/[^a-zA-ZÀ-ÿ\s]/g, '');
    setNomeCompleto(filtered);
    // Se já havia um erro e agora o nome possui pelo menos 3 palavras, limpa o erro
    if (nomeError && contarPalavras(filtered) >= 3) {
      setNomeError('');
    }
  };

  // Filtra o CPF para aceitar apenas dígitos e formata-o.
  const handleCpfChange = (text: string) => {
    const filtered = text.replace(/[^0-9]/g, '');
    if (filtered.length <= 11) {
      setCpf(formatarCpf(filtered));
    }
    validarCpf();
  };

  // Formata o CPF no padrão "123.456.789-00"
  const formatarCpf = (cpf: string) => {
    return cpf.replace(/(\d{3})(\d)/, '$1.$2')
              .replace(/(\d{3})(\d)/, '$1.$2')
              .replace(/(\d{3})(\d{2})$/, '$1-$2');
  };

  // Valida se o CPF possui exatamente 11 dígitos (apenas números)
  const validarCpf = () => {
    if (cpf.replace(/[^0-9]/g, '').length < 11) {
      setCpfError("Erro! Digite Todo o Seu CPF.");
    } else {
      setCpfError('');
    }
  };

  // Armazena o email e, se houver erro, tenta validá-lo novamente
  const handleEmailChange = (text: string) => {
    setEmail(text);
    if (emailError) {
      validarEmail(text);
    }
  };

  // Valida o email para garantir que está no formato "exemplo@gmail.com"
  const validarEmail = (email: string) => {
    const emailRegex = /^[^\s@]+@gmail\.com$/;
    if (!emailRegex.test(email)) {
      setEmailError("Erro! Insira um email válido no formato: exemplo@gmail.com");
    } else {
      setEmailError('');
    }
  };

  // Filtra o campo empresa para remover caracteres especiais
  const handleEmpresaChange = (text: string) => {
    const filtered = text.replace(/[^a-zA-Z0-9\s]/g, '');
    setEmpresa(filtered);
  };

  // Conta quantas palavras foram digitadas no nome (usado para validar o nome completo)
  const contarPalavras = (nome: string) => {
    return nome.trim().split(/\s+/).filter(word => word !== '').length;
  };

  // Valida se o nome contém pelo menos 3 palavras
  const validarNome = () => {
    if (contarPalavras(nomeCompleto) < 3) {
      setNomeError("Erro! Digite Seu Nome Completo.");
    } else {
      setNomeError('');
    }
  };

  /*****************************************************
   * Funções para Gerar Dados Fictícios Automáticos
   * (Campos não preenchidos pelo usuário)
   *****************************************************/

  // Gera uma data de nascimento fictícia no formato "yyyyMMdd" (entre 1960 e 2000)
  const gerarDataNascimentoFicticia = (): string => {
    const ano = Math.floor(Math.random() * (2000 - 1960 + 1)) + 1960;
    const mes = String(Math.floor(Math.random() * 12) + 1).padStart(2, '0');
    const dia = String(Math.floor(Math.random() * 28) + 1).padStart(2, '0');
    return `${ano}${mes}${dia}`;
  };

  // Gera um número de telefone fictício no formato "11XXXXXXXXX" (9 dígitos após o DDD "11")
  const gerarTelefoneFicticio = (): string => {
    const numero = Math.floor(Math.random() * 900000000 + 100000000);
    return '11' + numero.toString();
  };

  // Gera um endereço fictício simples
  const gerarEnderecoFicticio = (): string => {
    const numero = Math.floor(Math.random() * 900 + 100);
    return `Rua Fictícia, ${numero}, São Paulo`;
  };

  /*****************************************************
   * Função Principal de Cadastro (handleCadastrar)
   *****************************************************/
  const handleCadastrar = async () => {
    if (
      nomeCompleto.trim() === '' ||
      contarPalavras(nomeCompleto) < 3 ||
      cpf.replace(/[^0-9]/g, '').length !== 11 ||
      !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
    ) {
      setAlertMessage("Por favor, preencha todos os campos obrigatórios corretamente.");
      setAlertVisible(true);
      return;
    }
    if (!termos) {
      setAlertMessage("Você deve aceitar os termos para continuar.");
      setAlertVisible(true);
      return;
    }
    if (empresa && empresa.length > 100) {
      setAlertMessage("O nome da empresa deve ter até 100 caracteres.");
      setAlertVisible(true);
      return;
    }

    try {
      const payload: IPacienteData = {
        nomeCompleto: nomeCompleto.trim(),
        cpf: cpf.replace(/[^0-9]/g, ''),
        dataNascimento: gerarDataNascimentoFicticia(),
        email: email.trim(),
        telefone: gerarTelefoneFicticio(),
        endereco: gerarEnderecoFicticio(),
        planoOdontologico: gerarPlanoOdonto(),
        empresa: empresa.trim() || 'Individual'
      };

      console.log('Payload final:', payload);

      const authService = await AuthService.getInstance();
      const response = await authService.register(payload);

      if (response.success) {
        setAlertMessage("Cadastro realizado com sucesso!");
        setSuccessAlertVisible(true);
      } else {
        setAlertMessage(response.message);
        setAlertVisible(true);
      }
    } catch (error: any) {
      let errorMessage = "Erro ao cadastrar:\n";
      
      if (error.response) {
        errorMessage += `Status: ${error.response.status}\n`;
        errorMessage += `Dados: ${JSON.stringify(error.response.data, null, 2)}`;
      } else if (error.request) {
        errorMessage += "Não foi possível conectar ao servidor.\nVerifique sua conexão com a internet.";
      } else {
        errorMessage += error.message || "Erro desconhecido";
      }
      
      setAlertMessage(errorMessage);
      setAlertVisible(true);
    }
  };

  // Função para alternar a aceitação dos termos
  const toggleTermos = () => {
    setTermos(!termos);
  };

  // Ao fechar o alerta de sucesso, navega para a tela "Main"
  const handleSuccessAlertClose = () => {
    setSuccessAlertVisible(false);
    navigation.navigate('Main');
  };

  useEffect(() => {
    // Animação de entrada principal com sequência mais impactante
    
    // Primeiro anima o fundo
    Animated.timing(backgroundOpacity, {
      toValue: 1,
      duration: 600,
      useNativeDriver: true,
    }).start();
    
    // Anima o contêiner principal
    Animated.sequence([
      Animated.delay(100),
      Animated.parallel([
        Animated.timing(masterFade, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
        }),
        Animated.timing(masterScale, {
          toValue: 1,
          duration: 1000,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1)),
        }),
      ]),
    ]).start();
    
    // Animação específica do botão voltar
    Animated.sequence([
      Animated.delay(300),
      Animated.timing(voltarOpac, {
        toValue: 1,
        duration: 400,
        useNativeDriver: true,
      }),
    ]).start();

    // Animação da logo com rotação e escala
    Animated.sequence([
      Animated.delay(400),
      Animated.parallel([
        Animated.timing(logoOpac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(logoPos, {
          toValue: 0,
          duration: 900,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(2)),
        }),
        Animated.timing(logoSc, {
          toValue: 1,
          duration: 900,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1.3)),
        }),
        Animated.timing(logoRotate, {
          toValue: 0,
          duration: 900,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1.3)),
        }),
      ]),
    ]).start();

    // Animação do título com escala
    Animated.sequence([
      Animated.delay(700),
      Animated.parallel([
        Animated.timing(titleOpac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(titlePos, {
          toValue: 0,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.8)),
        }),
        Animated.timing(titleScale, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.8)),
        }),
      ]),
    ]).start();

    // Animação dos termos
    Animated.sequence([
      Animated.delay(1800),
      Animated.parallel([
        Animated.timing(termosOpac, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
        }),
        Animated.timing(termosPos, {
          toValue: 0,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
      ]),
    ]).start();

    // Animação do botão
    Animated.sequence([
      Animated.delay(2000),
      Animated.parallel([
        Animated.timing(btnOpac, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
        }),
        Animated.timing(btnPos, {
          toValue: 0,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.8)),
        }),
      ]),
    ]).start();
  }, []);
  
  // Rotação para animação da logo
  const spin = logoRotate.interpolate({
    inputRange: [-15, 0],
    outputRange: ['-15deg', '0deg']
  });

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <StatusBar translucent backgroundColor="transparent" barStyle="light-content" />
      
      <Animated.Image
        source={require('../../assets/images/CapaChoice.png')}
        style={[styles.imagemFundo, { opacity: backgroundOpacity }]}
      />
      
      <Animated.View
        style={[
          styles.containerConteudo,
          {
            opacity: masterFade,
            transform: [{ scale: masterScale }],
          },
        ]}
      >
        <ScrollView 
          contentContainerStyle={styles.scrollContent}
          showsVerticalScrollIndicator={false}
        >
          <Animated.View style={{ opacity: voltarOpac }}>
            <TouchableOpacity 
              style={[styles.botaoVoltar, { marginTop: 20 }]}
              onPress={() => navigation.goBack()}
            >
              <Text style={styles.textoVoltar}> ⬅️ Voltar</Text>
            </TouchableOpacity>
          </Animated.View>
          
          <Animated.View
            style={[
              styles.logoContainer,
              {
                opacity: logoOpac,
                transform: [
                  { translateY: logoPos }, 
                  { scale: logoSc },
                  { rotate: spin }
                ],
              },
            ]}
          >
            <Image
              source={require('../../assets/images/Logo.png')}
              style={styles.logo}
              resizeMode="contain"
            />
          </Animated.View>
          
          <Animated.View
            style={[
              styles.tituloContainer,
              {
                opacity: titleOpac,
                transform: [
                  { translateY: titlePos },
                  { scale: titleScale }
                ],
              },
            ]}
          >
            <Text style={styles.titulo}>Crie sua conta!</Text>
          </Animated.View>
          
          <View style={styles.formularioContainer}>
            <CampoFormulario
              placeholder="Digite seu nome completo"
              valor={nomeCompleto}
              setValor={handleNomeChange}
              onBlur={validarNome}
              keyboardType="default"
              delay={900}
            />
            {nomeError !== '' && (
              <Text style={styles.errorText}>{nomeError}</Text>
            )}
            
            <CampoFormulario
              placeholder="Digite seu CPF"
              valor={cpf}
              setValor={handleCpfChange}
              onBlur={validarCpf}
              keyboardType="numeric"
              delay={1100}
            />
            {cpfError !== '' && (
              <Text style={styles.errorText}>{cpfError}</Text>
            )}
            
            <CampoFormulario
              placeholder="Digite seu e-mail"
              valor={email}
              setValor={handleEmailChange}
              onBlur={() => validarEmail(email)}
              keyboardType="email-address"
              delay={1300}
            />
            {emailError !== '' && (
              <Text style={styles.errorText}>{emailError}</Text>
            )}
            
            <CampoFormulario
              placeholder="Digite sua empresa (Opcional)"
              valor={empresa}
              setValor={handleEmpresaChange}
              delay={1500}
            />
            
            <Animated.View
              style={[
                styles.termosContainer, 
                { 
                  opacity: termosOpac, 
                  transform: [{ translateY: termosPos }] 
                }
              ]}
            >
              <TouchableOpacity 
                style={styles.checkboxContainer}
                onPress={toggleTermos}
              >
                <View style={[
                  styles.checkbox, 
                  termos ? styles.checkboxChecked : null, 
                  { borderColor: termos ? '#00C3FF' : '#0077FF' }
                ]}>
                  {termos && <Text style={styles.checkMark}>✓</Text>}
                </View>
                <Text style={styles.termosTexto}>
                  Estou de acordo com os <Text style={styles.termosLink}>Termos de condições.</Text>
                </Text>
              </TouchableOpacity>
            </Animated.View>
            
            <Animated.View
              style={[
                styles.botaoCadastrarContainer, 
                { 
                  opacity: btnOpac, 
                  transform: [{ translateY: btnPos }] 
                }
              ]}
            >
              <View style={styles.borda}>
                <LinearGradient
                  colors={['#008CFF', '#0084FF', '#00AEFF']}
                  start={{ x: 0, y: 0 }}
                  end={{ x: 1, y: 0 }}
                  style={styles.botaoCadastrar}
                >
                  <BotaoAnimado
                    onPress={handleCadastrar}
                    textStyle={styles.textoBotaoCadastrar}
                  >
                    Cadastrar
                  </BotaoAnimado>
                </LinearGradient>
              </View>
            </Animated.View>
          </View>
        </ScrollView>
      </Animated.View>
      
      <AlertaPersonalizado 
        visible={alertVisible} 
        mensagem={alertMessage} 
        onClose={() => setAlertVisible(false)} 
        alertButtonText="Entendi"
      />
      
      <AlertaPersonalizado 
        visible={successAlertVisible} 
        mensagem="Cadastro realizado com sucesso!" 
        onClose={handleSuccessAlertClose} 
        alertButtonText="Avançar"
      />
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: { 
    flex: 1, 
    backgroundColor: 'rgba(0,0,0,0.8)' 
  },
  imagemFundo: {
    position: 'absolute',
    top: 0, left: 0, right: 0, bottom: 0,
    width: width,
    height: height,
    resizeMode: 'cover',
  },
  containerConteudo: { 
    flex: 1, 
    paddingHorizontal: 24,
    paddingTop: StatusBar.currentHeight || 0,
  },
  scrollContent: { 
    flexGrow: 1, 
    paddingBottom: 40 
  },
  botaoVoltar: { 
    alignSelf: 'flex-start', 
    paddingVertical: 15, 
    paddingHorizontal: 5 
  },
  textoVoltar: { 
    color: '#FFFFFF', 
    fontSize: 18, 
    fontWeight: 'bold' 
  },
  logoContainer: { 
    alignItems: 'center', 
    marginBottom: 10 
  },
  logo: { 
    width: width * 0.7, 
    height: width * 0.4 
  },
  tituloContainer: { 
    alignItems: 'center', 
    marginBottom: 20 
  },
  titulo: { 
    fontSize: 38, 
    color: '#FFFFFF', 
    fontWeight: 'bold', 
    paddingVertical: 5,
    marginTop: -10,
    textShadowColor: 'rgba(0, 132, 255, 0.5)',
    textShadowOffset: { width: 1, height: 1 },
    textShadowRadius: 10,
  },
  formularioContainer: { 
    width: '100%' 
  },
  campoContainer: { 
    marginBottom: 15 
  },
  input: {
    backgroundColor: 'rgba(17, 17, 17, 0.9)',
    borderRadius: 20,
    borderWidth: 1.5,
    borderColor: '#FFFFFF',
    fontStyle: 'italic',
    paddingHorizontal: 15,
    paddingVertical: 15,
    fontSize: 17,
    color: '#FFFFFF',
    marginBottom: 5,
    shadowColor: '#0084FF',
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0.3,
    shadowRadius: 10,
    elevation: 4,
  },
  errorText: {
    lineHeight: 20,
    color: '#ff5050',
    fontSize: 14,
    marginBottom: 20,
    marginLeft: 5,
  },
  termosContainer: { 
    marginTop: 10, 
    marginBottom: 20 
  },
  checkboxContainer: { 
    flexDirection: 'row', 
    alignItems: 'center' 
  },
  checkbox: {
    width: 25,
    height: 25,
    borderRadius: 20,
    borderWidth: 2,
    borderColor: '#0077FF',
    marginRight: 10,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'transparent',
  },
  checkboxChecked: { 
    backgroundColor: '#2A8EFF' 
  },
  checkMark: { 
    color: '#FFFFFF', 
    fontSize: 14, 
    fontWeight: 'bold' 
  },
  termosTexto: { 
    color: '#FFFFFF', 
    fontSize: 18, 
    lineHeight: 24 
  },
  termosLink: { 
    color: '#009DFF', 
    textDecorationLine: 'none' 
  },
  botaoCadastrarContainer: { 
    width: '100%', 
    alignItems: 'center', 
    marginTop: 5, 
  },
  botaoCadastrar: { 
    paddingVertical: 8, 
    width: width * 0.55, 
    alignItems: 'center',
  },
  botao: { 
    paddingVertical: 8, 
    width: width * 0.55, 
    alignItems: 'center' 
  },
  textoBotao: { 
    fontSize: 22, 
    fontWeight: 'bold' 
  },
  textoBotaoCadastrar: { 
    color: '#FFFFFF', 
    fontSize: 22, 
    fontWeight: 'bold',
    textShadowColor: 'rgba(0, 0, 0, 0.3)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 5,
  },
  borda: { 
    borderWidth: 2, 
    borderColor: '#FFFFFF', 
    borderRadius: 25, 
    overflow: 'hidden', 
    width: width * 0.55,
    shadowColor: '#0084FF',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.5,
    shadowRadius: 8,
    elevation: 8,
  },
  modalBackground: { 
    flex: 1, 
    justifyContent: 'center', 
    alignItems: 'center', 
    backgroundColor: 'rgba(0, 0, 0, 0.6)' 
  },
  alertContainer: { 
    width: '85%', 
    backgroundColor: '#FFFFFF', 
    borderRadius: 20, 
    overflow: 'hidden',
    shadowColor: '#000', 
    shadowOffset: { width: 0, height: 4 }, 
    shadowOpacity: 0.3, 
    shadowRadius: 8, 
    elevation: 10,
  },
  alertHeader: {
    paddingVertical: 15,
    alignItems: 'center',
    justifyContent: 'center',
  },
  alertHeaderText: {
    color: '#FFFFFF',
    fontSize: 20,
    fontWeight: 'bold',
  },
  alertContent: {
    padding: 20,
    alignItems: 'center',
  },
  alertText: { 
    fontSize: 16, 
    marginBottom: 20, 
    textAlign: 'center',
    color: '#333',
    lineHeight: 22,
  },
  alertButton: { 
    borderRadius: 25, 
    overflow: 'hidden',
    shadowColor: '#0077FF', 
    shadowOffset: { width: 0, height: 2 }, 
    shadowOpacity: 0.3, 
    shadowRadius: 5, 
    elevation: 5,
  },
  alertButtonGradient: {
    paddingVertical: 12,
    paddingHorizontal: 30,
    borderRadius: 25,
  },
  alertButtonText: { 
    color: '#FFFFFF', 
    fontSize: 16,
    fontWeight: 'bold',
  },
});

export default RegisterScreen;