// Tentando entender melhor esses imports aqui: 
// React é óbvio que preciso pra construir a tela, 
// e uso esses Hooks (useEffect, useRef, useState) porque eles facilitam a vida com estado e animações.
import React, { useEffect, useRef, useState } from 'react'; 
import { AuthService } from '../../services/auth';

// Esses aqui são os componentes que vou usar do React Native. 
// É um monte, mas basicamente: View, Text, Image, etc. são coisas padrão pra layout e texto, 
// Dimensions pra medir a tela, Animated e Easing pra animações, e por aí vai.
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
  KeyboardAvoidingView,
  Platform,
  StatusBar,
  Modal,
  ScrollView,
  Alert,
} from 'react-native';

// Esse LinearGradient é tipo um degradê maneiro que vou colocar em botões e backgrounds, 
// dá um efeito legal.
import LinearGradient from 'react-native-linear-gradient';

// O BlurView serve pra criar aquele efeito de "embaçado" por cima de um fundo, 
// fica com cara de iOS estiloso.
import { BlurView } from '@react-native-community/blur';

// Aqui pego a width e height da tela do usuário, preciso disso pra responsividade e animações.
const { width, height } = Dimensions.get('window');

// Essa função aqui eu uso pra tornar um tamanho qualquer responsivo, 
// baseando o cálculo na largura da tela do dispositivo. 
// Descobri que a galera usa o 375 como referência porque é o tamanho do iPhone X.
const responsiveSize = (size: number, baseDimension = width) => {
  const scale = baseDimension / 375; 
  return Math.round(size * scale);
};

// Mesma ideia mas focada em tamanho de fonte, então uso a menor dimensão (width ou height) 
// pra manter a fonte proporcional.
const responsiveFontSize = (size: number) => {
  const scale = Math.min(width, height) / 375;
  return Math.round(size * scale);
};

// Aqui defino a interface do meu campo de formulário, pra eu garantir que recebo certinho 
// o placeholder, o valor e a função que atualiza esse valor, entre outras coisinhas.
interface CampoFormularioProps {
  placeholder: string;
  valor: string;
  setValor: (valor: string) => void;
  keyboardType?: 'default' | 'numeric' | 'email-address';
  secureTextEntry?: boolean;
  delay?: number;
  onBlur?: () => void;
  animationIndex?: number;
  style?: any;
}

// Componente específico de campo de formulário que criei, 
// com animações e tudo pra dar aquele estilo. 
const CampoFormulario: React.FC<CampoFormularioProps> = ({
  placeholder,
  valor,
  setValor,
  keyboardType = 'default',
  secureTextEntry = false,
  delay = 0,
  onBlur,
  animationIndex = 0,
  style,
}) => {
  // Usando refs e Animated.Value pra controlar animações de opacidade, posição, escala e rotação.
  const inputOpac = useRef(new Animated.Value(0)).current;
  const inputPos = useRef(new Animated.Value(responsiveSize(50))).current;
  const inputScale = useRef(new Animated.Value(0.9)).current;
  const inputRotate = useRef(new Animated.Value(-5)).current;

  // Criei uma interpolação pra transformar valores numéricos em graus (deg). 
  // Assim posso girar o componente de -5deg até 0deg.
  const spin = inputRotate.interpolate({
    inputRange: [-5, 0],
    outputRange: ['-5deg', '0deg']
  });

  // Quando o componente monta, faço uma sequência de animações:
  // - Delay pra esperar a hora certa
  // - Paralelo com fade in, subir o input, aumentar a escala e "desgirar".
  useEffect(() => {
    Animated.sequence([
      Animated.delay(delay),
      Animated.parallel([
        Animated.timing(inputOpac, {
          toValue: 1,
          duration: 400,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(inputPos, {
          toValue: 0,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.7)),
        }),
        Animated.timing(inputScale, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(inputRotate, {
          toValue: 0,
          duration: 500,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1.2)),
        }),
      ]),
    ]).start();
  }, []);

  // Retorno um Animated.View que envolve o TextInput, e aplico as transformações. 
  // Assim a caixinha do input faz aquele movimento legal.
  return (
    <View style={[styles.campoContainer, style]}>
      <Animated.View 
        style={{
          opacity: inputOpac,
          transform: [
            { translateY: inputPos },
            { scale: inputScale },
            { rotate: spin }
          ],
        }}
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
    </View>
  );
};

// Props da tela de Login, basicamente só a navigation. 
// Talvez eu pudesse extrair ela e criar algo mais específico depois.
interface LoginScreenProps {
  navigation: any;
}

// Componente principal da tela de Login.
const LoginScreen: React.FC<LoginScreenProps> = ({ navigation }) => {
  // Estados para usuário, CPF, e afins. Uso useState pra cada campo e configuro alertas, etc.
  const [usuario, setUsuario] = useState('');
  const [nomeError, setNomeError] = useState('');
  const [cpf, setCpf] = useState('');
  const [cpfError, setCpfError] = useState('');
  const [termos, setTermos] = useState(false);
  const [alertVisible, setAlertVisible] = useState(false);
  const [alertMessage, setAlertMessage] = useState('');
  const [successAlertVisible, setSuccessAlertVisible] = useState(false);
  const [animationComplete, setAnimationComplete] = useState(false);
  const [loginType, setLoginType] = useState<'name' | 'email'>('name');
  const [touchedUsuario, setTouchedUsuario] = useState(false);

  // Variáveis de animação mais avançadas, tipo backgroundAnimation, particleOpacity, etc. 
  // Tô tentando criar um efeito de background se movendo, partícula aparecendo, etc.
  const backgroundAnimation = useRef(new Animated.Value(0)).current;
  const particleOpacity = useRef(new Animated.Value(0)).current;
  const masterScale = useRef(new Animated.Value(0.5)).current;
  const masterRotate = useRef(new Animated.Value(-5)).current;
  const masterFade = useRef(new Animated.Value(0)).current;
  const blurIntensity = useRef(new Animated.Value(0)).current;
  
  // Animações específicas pra logo.
  const logoOpac = useRef(new Animated.Value(0)).current;
  const logoSc = useRef(new Animated.Value(0.4)).current;
  const logoPos = useRef(new Animated.Value(responsiveSize(-120))).current;
  const logoRotate = useRef(new Animated.Value(-30)).current;
  
  // Animações pro título.
  const titleOpac = useRef(new Animated.Value(0)).current;
  const titlePos = useRef(new Animated.Value(responsiveSize(-60))).current;
  const titleScale = useRef(new Animated.Value(0.5)).current;
  
  // Animações pro botão.
  const btnOpac = useRef(new Animated.Value(0)).current;
  const btnPos = useRef(new Animated.Value(responsiveSize(50))).current;
  const btnScale = useRef(new Animated.Value(0.8)).current;
  const voltarOpac = useRef(new Animated.Value(0)).current;
  
  // Efeito de "brilho" que atravessa a tela, 
  // tipo uma faixa luminosa que passa por cima do layout.
  const glowOpacity = useRef(new Animated.Value(0)).current;
  const glowPosition = useRef(new Animated.Value(-width)).current;

  // Funções de validação do nome
  const contarPalavras = (nome: string) => {
    return nome.trim().split(/\s+/).filter(word => word !== '').length;
  };

  const handleNomeChange = (text: string) => {
    // Remove caracteres que não sejam letras ou acentos
    const filtered = text.replace(/[^a-zA-ZÀ-ÿ\s]/g, '');
    setUsuario(filtered);
    validarNome(filtered);
  };

  const validarNome = (nome: string = usuario) => {
    if (contarPalavras(nome) < 3) {
      setNomeError("Erro! Digite Seu Nome Completo.");
    } else {
      setNomeError('');
    }
  };

  // Formata o CPF praquele padrão 000.000.000-00 quando o cara digitar os números.
  const formatarCpf = (cpf: string) => {
    return cpf.replace(/(\d{3})(\d)/, '$1.$2')
              .replace(/(\d{3})(\d)/, '$1.$2')
              .replace(/(\d{3})(\d{2})$/, '$1-$2');
  };

  // Quando o usuário digita, eu limpo tudo que não for número e deixo no formato. 
  // Se passar de 11 dígitos, eu paro.
  const handleCpfChange = (text: string) => {
    const filtered = text.replace(/[^0-9]/g, '');
    if (filtered.length <= 11) {
      setCpf(formatarCpf(filtered));
      setCpfError('');
    }
  };

  // Checo se o CPF está completo (se tem 11 dígitos). Se não estiver, mostro erro.
  const validarCpf = () => {
    if (cpf.replace(/[^0-9]/g, '').length < 11) {
      setCpfError("Erro! Digite Todo o Seu CPF.");
    } else {
      setCpfError('');
    }
  };

  // Função principal de login, onde verifico se o usuário preencheu tudo. 
  // Se não, dou um alerta. Se sim, animo o botão e depois navego.
  const handleLogin = async () => {
    setTouchedUsuario(true);
    if (usuario.trim() === '' || cpf.replace(/[^0-9]/g, '').length < 11) {
      setAlertMessage("Por favor, preencha todos os campos obrigatórios corretamente.");
      setAlertVisible(true);
      return;
    }

    const authService = await AuthService.getInstance();
    const credentials = {
      identifier: usuario,
      cpf: cpf.replace(/[^0-9]/g, ''),
      loginType: loginType,
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

  // Togglezinho pra Lembrar-me. Verdadeiro/falso. 
  // Ele só muda a cor do checkbox e marca um "✓".
  const toggleTermos = () => {
    setTermos(!termos);
  };

  // Uso animação interpolada pra eu conseguir alterar, por exemplo, escala e rotação em graus.
  const backgroundScale = backgroundAnimation.interpolate({
    inputRange: [0, 1],
    outputRange: [1.2, 1]
  });
  
  const backgroundBlur = blurIntensity.interpolate({
    inputRange: [0, 1],
    outputRange: ['0px', '10px']
  });
  
  const masterRotation = masterRotate.interpolate({
    inputRange: [-5, 0],
    outputRange: ['-5deg', '0deg']
  });
  
  const logoSpin = logoRotate.interpolate({
    inputRange: [-30, 0],
    outputRange: ['-30deg', '0deg']
  });

  // Uso um useEffect pra rodar toda a sequência de animações logo que a tela monta.
  useEffect(() => {
    // masterAnimation é toda a coreografia da página: fundo, blur, logo, título, botão, etc.
    const masterAnimation = Animated.sequence([
      Animated.parallel([
        Animated.timing(backgroundAnimation, {
          toValue: 1,
          duration: 1000,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(blurIntensity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: false,
        }),
      ]),
      
      Animated.parallel([
        Animated.timing(masterFade, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(masterScale, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1)),
        }),
        Animated.timing(masterRotate, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1)),
        }),
        Animated.timing(particleOpacity, {
          toValue: 0.7,
          duration: 1000,
          useNativeDriver: true,
        }),
      ]),
      
      // Depois que o fundo e o blur entram, animo a logo.
      Animated.parallel([
        Animated.timing(logoOpac, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(logoPos, {
          toValue: 0,
          duration: 1100,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(2.5)),
        }),
        Animated.timing(logoSc, {
          toValue: 1,
          duration: 1100,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1.5)),
        }),
        Animated.timing(logoRotate, {
          toValue: 0,
          duration: 1100,
          useNativeDriver: true,
          easing: Easing.out(Easing.elastic(1.5)),
        }),
      ]),
      
      // Faço aquele "glow" ou brilho atravessar a tela, parece tipo um efeito de flash.
      Animated.timing(glowOpacity, {
        toValue: 0.7,
        duration: 300,
        useNativeDriver: true,
      }),
      Animated.timing(glowPosition, {
        toValue: width,
        duration: 1500,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
      Animated.timing(glowOpacity, {
        toValue: 0,
        duration: 300,
        useNativeDriver: true,
      }),
      
      // Depois que a logo já apareceu, animo o título.
      Animated.parallel([
        Animated.timing(titleOpac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(titlePos, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(2)),
        }),
        Animated.timing(titleScale, {
          toValue: 1,
          duration: 400,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(2)),
        }),
      ]),
      
      // E finalmente mostro o botão de Login, subindo ele e mudando opacidade.
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
      
      // E no final, animo a setinha de "Voltar".
      Animated.timing(voltarOpac, {
        toValue: 1,
        duration: 400,
        useNativeDriver: true,
      }),
    ]);
    
    // Executo a animação e, quando terminar, mudo um estado pra "animationComplete" ser true.
    masterAnimation.start(() => {
      setAnimationComplete(true);
    });

    // Caso eu precisasse limpar alguma coisa, poderia fazer aqui no return do useEffect.
    return () => {
      masterAnimation.stop();
    };
  }, []);

  // Retorno a estrutura principal, com KeyboardAvoidingView pra não ficar tudo bugado quando o teclado abrir.
  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      {/* Bar transparente pra um layout mais limpo. */}
      <StatusBar translucent backgroundColor="transparent" barStyle="light-content" />
      
      {/* Imagem de fundo, fica lá atrás. */}
      <Animated.Image
        source={require('../../assets/images/CapaChoice.png')}
        style={[styles.imagemFundo, { opacity: 1 }]}
        resizeMode="cover"
      />
      
      {/* Se for iOS, aplico o BlurView também, que dá aquele efeito de vidro fosco. */}
      {Platform.OS === 'ios' && (
        <BlurView
          style={StyleSheet.absoluteFill}
          blurType="dark"
          blurAmount={20}
          reducedTransparencyFallbackColor="black"
        />
      )}
      
      {/* ScrollView pra que a tela role se o teclado subir muito, evitando layout quebrado. */}
      <ScrollView contentContainerStyle={styles.scrollViewContent}>
        <Animated.View
          style={[
            styles.containerConteudo,
            {
              opacity: masterFade,
              transform: [
                { scale: masterScale },
                { rotate: masterRotation }
              ],
            },
          ]}
        >
          {/* Botão "Voltar" lá no topo, com animação de opacidade. */}
          <Animated.View style={{ opacity: voltarOpac }}>
            <TouchableOpacity 
              style={styles.botaoVoltar}
              onPress={() => navigation.goBack()}
            >
              <Text style={styles.textoVoltar}>⬅️ Voltar</Text>
            </TouchableOpacity>
          </Animated.View>
          
          {/* Container da logo, com animações de opacidade, posição, escala e rotação. */}
          <Animated.View
            style={[
              styles.logoContainer,
              {
                opacity: logoOpac,
                transform: [
                  { translateY: logoPos }, 
                  { scale: logoSc },
                  { rotate: logoSpin }
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
          
          {/* Texto "Começar! ✨", também animado ao entrar. */}
          <Animated.View 
            style={[
              styles.tituloContainer,
              {
                opacity: titleOpac,
                transform: [
                  { translateY: titlePos },
                  { scale: titleScale }
                ],
              }
            ]}
          >
            <Text style={styles.titulo}>Começar! ✨</Text>
          </Animated.View>
          
          {/* Container do formulário: campos de usuário e CPF, mais a caixinha de lembrar-me, etc. */}
          <View style={styles.formularioContainer}>
            <View style={styles.pickerContainer}>     
            </View>

            <CampoFormulario
              placeholder="Digite seu nome completo"
              valor={usuario}
              setValor={handleNomeChange}
              onBlur={() => validarNome()}
              delay={1700}
              animationIndex={0}
              style={{ marginBottom: 15 }}
            />
            {nomeError !== '' && (
              <Text style={styles.errorText}>{nomeError}</Text>
            )}
            
            <CampoFormulario
              placeholder="Digite seu CPF"
              valor={cpf}
              setValor={handleCpfChange}
              keyboardType="numeric"
              delay={1900}
              onBlur={validarCpf}
              animationIndex={1}
            />
            {cpfError !== '' && (
              <Text style={styles.errorText}>{cpfError}</Text>
            )}
            
            {/* Opções: Lembrar-me e "Esqueceu a senha?". */}
            <View style={styles.opcoesLoginContainer}>
              <View style={styles.lembrarContainer}>
                <TouchableOpacity 
                  style={styles.checkboxContainer}
                  onPress={toggleTermos}
                >
                  <View style={[styles.checkbox, termos ? styles.checkboxChecked : null]}>
                    {termos && <Text style={styles.checkMark}>✓</Text>}
                  </View>
                  <Text style={styles.lembrarTexto}>Lembrar-me</Text>
                </TouchableOpacity>
              </View>
              
              <TouchableOpacity>
                <Text style={styles.esqueceuSenha}>Esqueceu sua senha?</Text>
              </TouchableOpacity>
            </View>
            
            {/* Botão de Login, que fiz com animações. Ele faz um pulso quando clicado. */}
            <Animated.View
              style={[
                styles.botaoLoginContainer, 
                { 
                  opacity: btnOpac, 
                  transform: [
                    { translateY: btnPos },
                    { scale: btnScale }
                  ],
                  marginTop: responsiveSize(5),
                }
              ]}
            >
              <TouchableOpacity onPress={handleLogin} style={styles.botaoLogin}>
                <LinearGradient
                  colors={['#008CFF', '#0084FF', '#00AEFF']}
                  start={{ x: 0, y: 0 }}
                  end={{ x: 1, y: 0 }}
                  style={styles.gradient}
                >
                  <Text style={styles.textoBotaoLogin}>Entrar</Text>
                </LinearGradient>
              </TouchableOpacity>
            </Animated.View>
            
            {/* Caso o usuário não tenha conta, deixo um link pra criar. */}
            <View style={styles.criarContaContainer}>
              <View style={styles.criarContaRow}>
                <Text style={styles.naoTemContaTexto}>Ainda não possui uma conta?</Text>
                <TouchableOpacity onPress={() => navigation.navigate('Register')}>
                  <Text style={styles.criarContaTexto}> Criar uma</Text>
                </TouchableOpacity>
              </View>
            </View>
          </View>
        </Animated.View>
      </ScrollView>

      {/* Modal de alerta, se o usuário não preencher os campos ou algo der errado. */}
      <Modal
        transparent
        animationType="fade"
        visible={alertVisible}
        onRequestClose={() => setAlertVisible(false)}
      >
        <Animated.View style={[styles.modalBackground, { opacity: 1 }]}>
          <Animated.View style={styles.alertContainer}>
            <LinearGradient
              colors={['#0077FF', '#00AEFF']}
              start={{ x: 0, y: 0 }}
              end={{ x: 1, y: 0 }}
              style={styles.alertHeader}
            >
              <Text style={styles.alertHeaderText}>Atenção</Text>
            </LinearGradient>
            <View style={styles.alertContent}>
              <Text style={styles.alertText}>{alertMessage}</Text>
              <TouchableOpacity onPress={() => setAlertVisible(false)} style={styles.alertButtonGradient}>
                <Text style={styles.alertButtonText}>Entendi</Text>
              </TouchableOpacity>
            </View>
          </Animated.View>
        </Animated.View>
      </Modal>

      {/* Modal de sucesso, quando o login dá certo. */}
      <Modal
        transparent
        animationType="fade"
        visible={successAlertVisible}
        onRequestClose={() => setSuccessAlertVisible(false)}
      >
        <Animated.View style={[styles.modalBackground, { opacity: 1 }]}>
          <Animated.View style={styles.alertContainer}>
            <LinearGradient
              colors={['#0077FF', '#00AEFF']}
              start={{ x: 0, y: 0 }}
              end={{ x: 1, y: 0 }}
              style={styles.alertHeader}
            >
              <Text style={styles.alertHeaderText}>Sucesso</Text>
            </LinearGradient>
            <View style={styles.alertContent}>
              <Text style={styles.alertText}>Login realizado com sucesso!</Text>
            </View>
          </Animated.View>
        </Animated.View>
      </Modal>
    </KeyboardAvoidingView>
  );
};

// Aqui ficam todos os estilos, usando StyleSheet. 
// Eu procurei colocar responsivos e com sombras, bordas, etc. 
// Não tô 100% confiante se é a melhor forma, mas tá funcionando direitinho.
const styles = StyleSheet.create({
  container: { 
    flex: 1, 
    backgroundColor: 'rgba(0,0,0,0.9)' 
  },
  scrollViewContent: {
    flexGrow: 1,
    minHeight: '100%',
  },
  imagemFundo: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    width: '100%',
    height: '100%',
    resizeMode: 'cover',
  },
  particleOverlay: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    opacity: 0.5,
    zIndex: 1,
    backgroundColor: 'rgba(0,90,255,0.1)',
  },
  glowEffect: {
    position: 'absolute',
    top: 0,
    left: -width,
    width: width * 1.5,
    height: height,
    backgroundColor: 'rgba(0,150,255,0.2)',
    transform: [{ skewX: '-30deg' }],
    zIndex: 2,
  },
  containerConteudo: { 
    flex: 1, 
    paddingHorizontal: '6%',
    paddingTop: '7%',
    justifyContent: 'flex-start',
  },
  botaoVoltar: { 
    alignSelf: 'flex-start', 
    paddingVertical: responsiveSize(15), 
    paddingHorizontal: responsiveSize(5),
    marginTop: Platform.OS === 'ios' 
      ? responsiveSize(10) + (height > 800 ? 20 : 0) 
      : responsiveSize(10),
    zIndex: 10,
  },
  textoVoltar: { 
    color: '#FFFFFF', 
    fontSize: responsiveFontSize(18), 
    fontWeight: 'bold' 
  },
  logoContainer: { 
    alignItems: 'center', 
    marginTop: Platform.OS === 'ios' 
      ? responsiveSize(20) - (height < 700 ? 10 : 0) 
      : responsiveSize(20),
    marginBottom: responsiveSize(10),
    maxHeight: height * 0.25,
  },
  logo: { 
    width: width * 0.7,
    height: width * 0.4,
    maxWidth: 350,
    maxHeight: 200,
    aspectRatio: 1.75,
  },
  tituloContainer: { 
    alignItems: 'center', 
    marginBottom: responsiveSize(20),
    marginTop: responsiveSize(-15),
  },
  titulo: { 
    fontSize: responsiveFontSize(38), 
    color: '#FFFFFF', 
    fontWeight: 'bold', 
    paddingVertical: responsiveSize(15),
    textAlign: 'center',
    textShadowColor: 'rgba(0, 150, 255, 0.8)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 10,
  },
  formularioContainer: { 
    width: '100%',
    alignItems: 'center',
    paddingHorizontal: width < 350 ? '4%' : 0,
  },
  campoContainer: { 
    marginBottom: responsiveSize(15),
    width: '100%',
    maxWidth: 500,
  },
  input: {
    backgroundColor: 'rgba(17, 17, 17, 0.9)',
    borderRadius: responsiveSize(20),
    borderWidth: 1.5,
    borderColor: '#FFFFFF',
    fontStyle: 'italic',
    paddingHorizontal: responsiveSize(15),
    paddingVertical: Platform.OS === 'ios' 
      ? responsiveSize(15) 
      : responsiveSize(12),
    fontSize: responsiveFontSize(17),
    color: '#FFFFFF',
    width: '100%',
    shadowColor: '#0077FF',
    shadowOffset: { width: 0, height: 5 },
    shadowOpacity: 0.5,
    shadowRadius: 10,
    elevation: 5,
  },
  opcoesLoginContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: '100%',
    marginBottom: responsiveSize(30),
    marginTop: responsiveSize(10),
    paddingHorizontal: responsiveSize(10),
    maxWidth: 500,
  },
  lembrarContainer: {
    flexDirection: 'row',
    alignItems: 'center'
  },
  checkboxContainer: { 
    flexDirection: 'row', 
    alignItems: 'center' 
  },
  checkbox: {
    width: responsiveSize(25),
    height: responsiveSize(25),
    borderRadius: responsiveSize(20),
    borderWidth: 2,
    borderColor: '#0077FF',
    marginRight: responsiveSize(10),
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'transparent',
    shadowColor: '#0077FF',
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0.5,
    shadowRadius: 5,
    elevation: 3,
  },
  checkboxChecked: { 
    backgroundColor: '#2A8EFF' 
  },
  checkMark: { 
    color: '#FFFFFF', 
    fontSize: responsiveFontSize(14), 
    fontWeight: 'bold' 
  },
  lembrarTexto: { 
    color: '#FFFFFF', 
    fontSize: responsiveFontSize(16)
  },
  esqueceuSenha: {
    color: '#FFFFFF',
    fontSize: responsiveFontSize(16),
  },
  botaoLoginContainer: {
    marginBottom: responsiveSize(10),
    marginTop: responsiveSize(5),
    borderWidth: 2,
    borderColor: '#FFFFFF',
    width: Math.min(width * 0.5, 220),
    maxWidth: '80%',
    minWidth: 150,
    borderRadius: responsiveSize(20),
    overflow: 'hidden',
    shadowColor: '#00AEFF',
    shadowOffset: { width: 0, height: 10 },
    shadowOpacity: 0.8,
    shadowRadius: 15,
    elevation: 10,
  },
  botaoLogin: { 
    width: '100%',
    alignItems: 'center',
  },
  gradient: {
    paddingVertical: responsiveSize(15),
    alignItems: 'center',
    borderRadius: responsiveSize(20),
    width: '100%',
  },
  textoBotaoLogin: {
    color: '#FFFFFF',
    fontSize: responsiveFontSize(22),
    fontWeight: 'bold',
  },
  criarContaContainer: {
    marginTop: responsiveSize(20),
    alignItems: 'center',
    paddingBottom: responsiveSize(20),
  },
  criarContaRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    flexWrap: 'wrap',
  },
  naoTemContaTexto: {
    color: '#FFFFFF',
    fontSize: responsiveFontSize(16),
    marginBottom: 5,
    textAlign: 'center',
  },
  criarContaTexto: {
    color: '#009DFF',
    fontSize: responsiveFontSize(16),
    fontWeight: 'bold',
    marginBottom: 4,
    paddingLeft: 5,
    textShadowColor: 'rgba(0, 157, 255, 0.5)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 5,
  },
  errorText: {
    lineHeight: 20,
    color: '#ff5050',
    fontSize: 14,
    marginBottom: 20,
    marginLeft: -130,
    textAlign: 'left',
  },
  modalBackground: { 
    flex: 1, 
    justifyContent: 'center', 
    alignItems: 'center', 
    backgroundColor: 'rgba(0, 0, 0, 0.7)' 
  },
  alertContainer: { 
    width: Math.min(width * 0.85, 400), 
    backgroundColor: '#FFFFFF', 
    borderRadius: responsiveSize(20), 
    overflow: 'hidden',
    shadowColor: '#000', 
    shadowOffset: { width: 0, height: 4 }, 
    shadowOpacity: 0.3, 
    shadowRadius: 8, 
    elevation: 10,
  },
  alertHeader: {
    paddingVertical: responsiveSize(15),
    alignItems: 'center',
    justifyContent: 'center',
  },
  alertHeaderText: {
    color: '#FFFFFF',
    fontSize: responsiveFontSize(20),
    fontWeight: 'bold',
  },
  alertContent: {
    padding: responsiveSize(20),
    alignItems: 'center',
  },
  alertText: { 
    fontSize: responsiveFontSize(16), 
    marginBottom: responsiveSize(20), 
    textAlign: 'center',
    color: '#333',
    lineHeight: responsiveSize(22),
  },
  alertButtonGradient: {
    paddingVertical: responsiveSize(12),
    paddingHorizontal: responsiveSize(30),
    borderRadius: responsiveSize(25),
    backgroundColor: '#008CFF',
  },
  alertButtonText: { 
    color: '#FFFFFF', 
    fontSize: responsiveFontSize(16),
    fontWeight: 'bold',
  },
  pickerContainer: {
  },
});


export default LoginScreen;
