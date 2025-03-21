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
  Alert,
  Platform,
  PixelRatio,
  useWindowDimensions,
  ImageBackground,
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import RadialGradient from 'react-native-radial-gradient';

/**
 * Aqui eu achei bom criar uma espécie de "paleta" com as cores,
 * porque assim, se alguém precisar mudar alguma cor do app,
 * fica mais fácil de mexer só nesse objeto, em vez de alterar
 * um monte de linhas separadas pelo código.
 */
const PALETTE = {
  smartBlue: '#009DFF',
  dentPink: '#FF00FF',
  textDefault: '#FFFFFF',
  gradientStart: '#008CFF',
  gradientEnd: '#0084FF',
  borderColor: '#FFFFFF',
};

/**
 * Aqui estou pegando as dimensões iniciais da tela.
 * Sei que posso usar o hook useWindowDimensions() pra ter algo mais dinâmico depois,
 * mas gosto de ter também essas variáveis fixas como referência.
 */
const { width: SCREEN_WIDTH, height: SCREEN_HEIGHT } = Dimensions.get('window');

/**
 * Essa função serve pra normalizar o tamanho dos elementos
 * (principalmente as fontes) de acordo com o tamanho do aparelho.
 * Aprendi que usar valores fixos pode ficar esquisito em telas muito grandes ou muito pequenas,
 * então eu uso esse "scale" que é baseado numa largura de referência (iPhone 375px).
 *
 * Também notei que no Android às vezes o texto ficava um pouco maior do que eu queria,
 * então fiz essa gambiarra de subtrair 2 se for Android.
 */
export function normalize(size: number): number {
  const scale = SCREEN_WIDTH / 375;
  const newSize = size * scale;

  if (Platform.OS === 'ios') {
    return Math.round(PixelRatio.roundToNearestPixel(newSize));
  }

  return Math.round(PixelRatio.roundToNearestPixel(newSize)) - 2;
}

/**
 * Essas interfaces ajudam a tipar as props do botão animado.
 * Bem básico, mas me poupa de possíveis erros e me obriga a respeitar
 * a estrutura do componente.
 */
interface BotaoAnimadoProps {
  onPress: () => void;
  children: React.ReactNode;
  style?: object;
  textStyle?: object;
}

/**
 * Esse componente é um botão que "anima" quando clicado.
 * Tive a ideia de fazer isso pra dar um feedback visual mais legal,
 * tipo, ao pressionar, o botão diminui um pouquinho pra parecer um clique real.
 */
const BotaoAnimado: React.FC<BotaoAnimadoProps> = ({ onPress, children, style, textStyle }) => {
  // Essas refs são responsáveis por controlar a escala e a opacidade do botão.
  // Assim que a tela abre, faço um "fade in" e um "scale up" simultâneos.
  const scaleVal = useRef(new Animated.Value(0)).current;
  const opacVal = useRef(new Animated.Value(0)).current;

  // Quando o componente carrega (mount), quero que ele faça essa animação de entrar em cena.
  useEffect(() => {
    Animated.parallel([
      Animated.timing(scaleVal, {
        toValue: 1, // 1 é a escala normal do botão
        duration: 800, // 800ms = 0.8s, acho um tempinho legal
        useNativeDriver: true,
        easing: Easing.out(Easing.ease), // Esse easing deixa a animação mais suave no final
      }),
      Animated.timing(opacVal, {
        toValue: 1, // Fica visível
        duration: 800,
        useNativeDriver: true,
      }),
    ]).start();
  }, [scaleVal, opacVal]);

  // Essas funções servem para quando o usuário pressiona o botão,
  // eu querer dar aquele efeito de "afundar" o botão levemente.
  const pressIn = () => {
    Animated.spring(scaleVal, {
      toValue: 0.95, // Diminui um tiquinho
      friction: 3,   // Define quanto "rebote" a animação vai ter
      useNativeDriver: true,
    }).start();
  };

  // Quando solta o botão, volta pra escala 1, que é o tamanho normal dele.
  const pressOut = () => {
    Animated.spring(scaleVal, {
      toValue: 1,
      friction: 3,
      useNativeDriver: true,
    }).start();
  };

  // Aqui estou retornando o botão com a animação aplicada.
  // Usei Animated.View pra poder mexer em escala e opacidade de forma fácil.
  return (
    <Animated.View style={{ transform: [{ scale: scaleVal }], opacity: opacVal }}>
      <TouchableOpacity
        onPressIn={pressIn} // quando começa a pressionar
        onPressOut={pressOut} // quando solta o botão
        onPress={onPress}
        style={[styles.botao, style]}
        activeOpacity={0.8} // pra dar uma leve transparência quando pressionar
      >
        <Text style={[styles.textoBotao, textStyle]}>{children}</Text>
      </TouchableOpacity>
    </Animated.View>
  );
};

/**
 * Aqui eu tiparia melhor o navigation, mas, por enquanto, deixei any.
 */
interface ChoiceScreenProps {
  navigation: any;
}

/**
 * Essa tela "ChoiceScreen" é onde o usuário pode escolher criar conta ou fazer login.
 * Eu decidi fazer umas animações parecidas com a tela anterior (outra que eu tinha)
 * pra ficar coerente no fluxo do app.
 */
const ChoiceScreen: React.FC<ChoiceScreenProps> = ({ navigation }) => {
  // Gosto de usar esse hook pra pegar o width e height da tela em tempo real.
  // É bacana se o usuário mudar a orientação do celular, por exemplo.
  const { width, height } = useWindowDimensions();
  
  // Esse estado eu inventei pra poder escalar as fontes dependendo do aparelho.
  // Fiz uns breakpoints tipo "se for tablet, aumenta mais a fonte".
  const [fontScale, setFontScale] = useState(1);
  
  // Uso esse effect pra monitorar mudanças de width,
  // aí recalculo o fontScale conforme uns ifs que decidi.
  useEffect(() => {
    const calculateFontScale = () => {
      if (width >= 768) {
        // imagino que seja um tablet
        setFontScale(1.2);
      } else if (width >= 414) {
        // iPhone Plus ou algo assim
        setFontScale(1.1);
      } else if (width <= 320) {
        // tipo iPhone SE ou celular pequeno
        setFontScale(0.85);
      } else {
        // meio que default
        setFontScale(1);
      }
    };

    calculateFontScale();
  }, [width]);

  // Agora, vou criar várias "refs" pra controlar as animações:
  // container, logo, título, descrição, botões...
  // Eu tenho que lembrar de chamá-las com "useRef(...).current"
  // e depois animar cada uma usando Animated.
  const contOpac = useRef(new Animated.Value(0)).current;
  const contScale = useRef(new Animated.Value(0.95)).current;

  const logoOpac = useRef(new Animated.Value(0)).current;
  const logoSc = useRef(new Animated.Value(0.8)).current;
  const logoPos = useRef(new Animated.Value(-30)).current;

  const titleOpac = useRef(new Animated.Value(0)).current;
  const titlePos = useRef(new Animated.Value(-20)).current;

  const descOpac = useRef(new Animated.Value(0)).current;
  const descPos = useRef(new Animated.Value(20)).current;

  const btn1Opac = useRef(new Animated.Value(0)).current;
  const btn1Pos = useRef(new Animated.Value(20)).current;
  const btn2Opac = useRef(new Animated.Value(0)).current;
  const btn2Pos = useRef(new Animated.Value(20)).current;

  // Adicione a declaração de animationProgress antes de usar
  const animationProgress = useRef(new Animated.Value(0)).current;

  // Adicione essas refs junto com as outras
  const backgroundScale = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [1, 1.05]
  });

  const backgroundTranslateY = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [0, -10]
  });

  // Esse effect é responsável por orquestrar todas as animações quando a tela carrega.
  // É meio grande, mas basicamente faz:
  // 1) container fadeIn + scale
  // 2) logo fadeIn + move + scale
  // 3) título fadeIn + move
  // 4) descrição fadeIn + move
  // 5) primeiro botão
  // 6) segundo botão
  useEffect(() => {
    // Container (fade e scale)
    Animated.parallel([
      Animated.timing(contOpac, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
      Animated.timing(contScale, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
    ]).start();

    // Logo (delay de 300ms, aí fade + move + scale)
    Animated.sequence([
      Animated.delay(300),
      Animated.parallel([
        Animated.timing(logoOpac, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(logoPos, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)), 
          // Easing.back dá esse efeito de "elástico" quando chega ao final
        }),
        Animated.timing(logoSc, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
      ]),
    ]).start();

    // Título (mais 300ms de delay, total 600)
    Animated.sequence([
      Animated.delay(600),
      Animated.parallel([
        Animated.timing(titleOpac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(titlePos, {
          toValue: 0,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
      ]),
    ]).start();

    // Descrição
    Animated.sequence([
      Animated.delay(900),
      Animated.parallel([
        Animated.timing(descOpac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(descPos, {
          toValue: 0,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
      ]),
    ]).start();

    // Botão 1 (criar conta)
    Animated.sequence([
      Animated.delay(1200),
      Animated.parallel([
        Animated.timing(btn1Opac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(btn1Pos, {
          toValue: 0,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
      ]),
    ]).start();

    // Botão 2 (fazer login)
    Animated.sequence([
      Animated.delay(1400),
      Animated.parallel([
        Animated.timing(btn2Opac, {
          toValue: 1,
          duration: 700,
          useNativeDriver: true,
        }),
        Animated.timing(btn2Pos, {
          toValue: 0,
          duration: 700,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
      ]),
    ]).start();

    // Adicione a animação contínua para o efeito parallax
    Animated.loop(
      Animated.sequence([
        Animated.timing(animationProgress, {
          toValue: 1,
          duration: 3000,
          useNativeDriver: true,
          easing: Easing.inOut(Easing.sin),
        }),
        Animated.timing(animationProgress, {
          toValue: 0,
          duration: 3000,
          useNativeDriver: true,
          easing: Easing.inOut(Easing.sin),
        }),
      ])
    ).start();
  }, [
    contOpac, contScale, logoOpac, logoPos, logoSc,
    titleOpac, titlePos, descOpac, descPos,
    btn1Opac, btn1Pos, btn2Opac, btn2Pos,
    animationProgress
  ]);

  // Aqui vou definir as funções que chamam outras telas.
  // A ideia é que o usuário clique pra registrar ou pra logar,
  // e a gente faz a navegação com o navigation.navigate().
  const handleRegister = () => {
    navigation.navigate('Register');
  };

  const handleLogin = () => {
    navigation.navigate('Login');
  };
  
  /**
   * Esses estilos dependem do tamanho da tela e do fontScale
   * que eu calculei com base em width. Acho isso útil pra evitar
   * problemas de layout em celulares pequenos ou tablets.
   */
  const dynamicStyles = {
    logo: {
      width: width * (width >= 768 ? 0.8 : 1.0),
      height: width * (width >= 768 ? 0.3 : 0.4),
    },
    logoContainer: {
      marginBottom: height * 0.05,
    },
    logoText: {
      fontSize: normalize(38 * fontScale),
      marginTop: -normalize(10 * fontScale),
    },
    textoSmart: {
      fontSize: normalize(46 * fontScale),
    },
    textoDent: {
      fontSize: normalize(46 * fontScale),
    },
    titulo: {
      fontSize: normalize(38 * fontScale),
    },
    emojiDente: {
      fontSize: normalize(28 * fontScale),
    },
    tituloContainer: {
      marginBottom: height * 0.02,
    },
    subtitulo: {
      fontSize: normalize(21 * fontScale),
      lineHeight: normalize(29 * fontScale),
      marginBottom: height * 0.06,
    },
    botao: {
      width: width * (width >= 768 ? 0.4 : 0.55),
      paddingVertical: height * 0.018,
    },
    textoBotao: {
      fontSize: normalize(22 * fontScale),
    },
    borda: {
      width: width * (width >= 768 ? 0.4 : 0.55),
    },
    botaoEspaco: {
      marginTop: height * 0.025,
    },
    containerConteudo: {
      paddingHorizontal: width * 0.06,
    }
  };

  return (
    <View style={styles.container}>
      {/* Imagem de fundo com animação */}
      <Animated.View style={{
        position: 'absolute',
        width: '100%',
        height: '100%',
        transform: [
          { scale: backgroundScale },
          { translateY: backgroundTranslateY }
        ]
      }}>
        <ImageBackground
          source={require('../../assets/images/CapaMain.png')}
          style={styles.imagemFundo}
          resizeMode="cover"
        >
          {/* Gradiente sobre a imagem de fundo */}
          <LinearGradient
            colors={['rgba(0,0,0,0.4)', 'rgba(12, 12, 12, 0.6)']}
            style={styles.backgroundOverlay}
          />
        </ImageBackground>
      </Animated.View>

      {/* Partículas animadas */}
      <Animated.View style={styles.particlesContainer} pointerEvents="none">
        {Array(6).fill(0).map((_, index) => {
          const particleAnim = useRef(new Animated.Value(0)).current;
          
          useEffect(() => {
            Animated.loop(
              Animated.timing(particleAnim, {
                toValue: 1,
                duration: 10000 + (index * 2000),
                delay: index * 500,
                useNativeDriver: true,
                easing: Easing.linear,
              })
            ).start();
          }, []);
          
          const particleSize = 6 + (index % 3) * 4;
          
          return (
            <Animated.View
              key={index}
              style={[
                styles.particle,
                {
                  width: particleSize,
                  height: particleSize,
                  borderRadius: particleSize / 2,
                  left: `${20 + (index * 15)}%`,
                  opacity: particleAnim.interpolate({
                    inputRange: [0, 0.2, 0.8, 1],
                    outputRange: [0, 0.6, 0.3, 0]
                  }),
                  transform: [
                    {
                      translateY: particleAnim.interpolate({
                        inputRange: [0, 1],
                        outputRange: [height * 0.3, -100]
                      })
                    },
                    {
                      translateX: particleAnim.interpolate({
                        inputRange: [0, 0.5, 1],
                        outputRange: [0, (index % 2 === 0 ? 30 : -30), 0]
                      })
                    }
                  ]
                }
              ]}
            />
          );
        })}
      </Animated.View>

      {/* Aqui é onde entra a animação do container principal:
          fadeIn + scale. */}
      <Animated.View
        style={[
          styles.containerConteudo,
          dynamicStyles.containerConteudo,
          {
            opacity: contOpac, // Animando opacidade
            transform: [{ scale: contScale }], // e escala
          },
        ]}
      >
        {/* Aqui coloco o logo do app, com a animação de opacidade, translateY e scale */}
        <Animated.View
          style={[
            styles.logoContainer,
            dynamicStyles.logoContainer,
            {
              opacity: logoOpac,
              transform: [{ translateY: logoPos }, { scale: logoSc }],
            },
          ]}
        >
          <Image
            source={require('../../assets/images/Logo.png')}
            style={[styles.logo, dynamicStyles.logo]}
            resizeMode="contain"
          />
          <Text style={[styles.logoText, dynamicStyles.logoText]}>
            <Text style={[styles.textoSmart, dynamicStyles.textoSmart]}>Smart</Text>
            <Text style={[styles.textoDent, dynamicStyles.textoDent]}>Dent</Text>
          </Text>
        </Animated.View>

        {/* Aqui é o título principal da tela, tipo "Comece Agora!" */}
        <Animated.View
          style={[
            styles.tituloContainer,
            dynamicStyles.tituloContainer,
            {
              opacity: titleOpac,
              transform: [{ translateY: titlePos }],
            },
          ]}
        >
          <Text style={[styles.titulo, dynamicStyles.titulo]}>
            Comece Agora! <Text style={[styles.emojiDente, dynamicStyles.emojiDente]}>🦷</Text>
          </Text>
        </Animated.View>

        {/* E aqui a descrição, que fala pro usuário se cadastrar ou logar */}
        <Animated.Text
          style={[
            styles.subtitulo,
            dynamicStyles.subtitulo,
            {
              opacity: descOpac,
              transform: [{ translateY: descPos }],
            },
          ]}
          adjustsFontSizeToFit={width < 350} // se for tela bem pequena, ajusta a fonte
          numberOfLines={3}
        >
          Cadastre-se ou faça login e tenha{'\n'}
          suas consultas e tratamentos na{'\n'}
          palma da mão!
        </Animated.Text>

        {/* Container dos botões: "Criar conta" e "Fazer login" */}
        <View style={styles.botoesContainer}>
          {/* Primeiro botão entra com uma animação: fade + translateY */}
          <Animated.View
            style={[
              styles.botaoWrapper,
              {
                opacity: btn1Opac,
                transform: [{ translateY: btn1Pos }],
              },
            ]}
          >
            <View style={[styles.borda, dynamicStyles.borda]}>
              {/*
                Usei RadialGradient porque achei divertido,
                mas poderia ser LinearGradient também.
              */}
              <RadialGradient
                colors={[PALETTE.gradientStart, PALETTE.gradientEnd]}
                style={[styles.botao, dynamicStyles.botao]}
              >
                <BotaoAnimado
                  onPress={handleRegister}
                  textStyle={[styles.textoBotaoCriar, dynamicStyles.textoBotao]}
                >
                  Criar conta
                </BotaoAnimado>
              </RadialGradient>
            </View>
          </Animated.View>

          {/* Segundo botão (login), com um espaço em cima pra não ficar colado no primeiro */}
          <Animated.View
            style={[
              styles.botaoWrapper,
              dynamicStyles.botaoEspaco,
              {
                opacity: btn2Opac,
                transform: [{ translateY: btn2Pos }],
              },
            ]}
          >
            <View style={[styles.borda, dynamicStyles.borda]}>
              <RadialGradient
                colors={[PALETTE.gradientStart, PALETTE.gradientEnd]}
                style={[styles.botao, dynamicStyles.botao]}
              >
                <BotaoAnimado
                  onPress={handleLogin}
                  textStyle={[styles.textoBotaoLogin, dynamicStyles.textoBotao]}
                >
                  Fazer login
                </BotaoAnimado>
              </RadialGradient>
            </View>
          </Animated.View>
        </View>
      </Animated.View>
    </View>
  );
};

/**
 * Aqui são estilos mais "fixos", mas misturo com alguns valores
 * que aparecem em dynamicStyles pra manter a responsividade.
 */
const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  imagemFundo: {
    flex: 1,
    justifyContent: 'center',
  },
  backgroundOverlay: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
  },
  particlesContainer: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    zIndex: 1,
  },
  particle: {
    position: 'absolute',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
  },
  containerConteudo: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  logoContainer: {
    alignItems: 'center',
  },
  logo: {
    marginBottom: 10,
    // Tô deixando width e height lá no dynamicStyles
  },
  logoText: {
    // idém, tô configurando tamanho lá no dynamicStyles
  },
  textoSmart: {
    color: PALETTE.smartBlue,
  },
  textoDent: {
    color: PALETTE.dentPink,
  },
  tituloContainer: {
    alignItems: 'center',
  },
  titulo: {
    color: PALETTE.textDefault,
    fontWeight: 'bold',
    // Tamanho da fonte definido dinamicamente
  },
  emojiDente: {
    // Tamanho da fonte definido dinamicamente
  },
  subtitulo: {
    color: PALETTE.textDefault,
    textAlign: 'justify',
    opacity: 0.9,
  },
  botoesContainer: {
    width: '100%',
    alignItems: 'center',
  },
  botaoWrapper: {
    width: '100%',
    alignItems: 'center',
  },
  botao: {
    borderRadius: 25,
    alignItems: 'center',
  },
  textoBotao: {
    fontWeight: 'bold',
  },
  textoBotaoCriar: {
    color: PALETTE.textDefault,
  },
  textoBotaoLogin: {
    color: PALETTE.textDefault,
  },
  borda: {
    borderWidth: 2,
    borderColor: PALETTE.borderColor,
    borderRadius: 22,
    overflow: 'hidden',
  },
});

export default ChoiceScreen;
