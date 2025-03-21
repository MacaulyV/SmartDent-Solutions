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
  useWindowDimensions,
  Platform,
  PixelRatio,
} from 'react-native';
import { NavigationProp } from '@react-navigation/native';
import LinearGradient from 'react-native-linear-gradient';
import FastImage from 'react-native-fast-image';

/**
 * Paleta de cores para reutilizar no código
 */
const PALETTE = {
  gradientStart: '#310080',
  gradientEnd: '#000000',
  borderColor: '#E4AFFF',
  textDefault: '#FFFFFF',
  textSmart: '#009DFF',
  textDent: '#FF00FF',
};

/**
 * Tamanho de tela do dispositivo
 */
const { width: SCREEN_WIDTH, height: SCREEN_HEIGHT } = Dimensions.get('window');

/**
 * Escala baseada no iPhone de 375px de largura como referência.
 * Isso ajuda a manter o design proporcional em dispositivos diferentes.
 */
const scale = SCREEN_WIDTH / 375;

/**
 * Função para dimensionar fontes e elementos proporcionalmente ao tamanho de tela.
 * Usa PixelRatio para ficar coerente em telas de diferentes densidades.
 * O "-2" no Android é comum para evitar que os tamanhos fiquem levemente maiores
 * do que o desejado. Ajuste essa lógica conforme a necessidade do projeto.
 */
export function normalize(size: number): number {
  const newSize = size * scale;
  if (Platform.OS === 'ios') {
    return Math.round(PixelRatio.roundToNearestPixel(newSize));
  }
  return Math.round(PixelRatio.roundToNearestPixel(newSize)) - 2;
}

/**
 * Componente AnimatedFastImage que adiciona suporte a animações no FastImage
 */
const AnimatedFastImage = Animated.createAnimatedComponent(FastImage);

/**
 * Tipagens do botão animado
 */
interface BotaoAnimadoProps {
  onPress: () => void;
  children: React.ReactNode;
}

/**
 * Botão animado com feedback visual.
 * Ao pressionar, faz uma ligeira redução/incremento de escala,
 * conferindo "efeito de clique" suave.
 */
const BotaoAnimado: React.FC<BotaoAnimadoProps> = ({ onPress, children }) => {
  const scaleVal = useRef(new Animated.Value(0)).current;   // Animação de escala
  const opacityVal = useRef(new Animated.Value(0)).current; // Animação de opacidade

  const { width } = useWindowDimensions();

  useEffect(() => {
    // Ao montar o componente, realiza a animação de entrada (scale e fade in)
    Animated.parallel([
      Animated.timing(scaleVal, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
      Animated.timing(opacityVal, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
      }),
    ]).start();
  }, [opacityVal, scaleVal]);

  // Quando o botão é pressionado, reduz levemente a escala para dar sensação de clique
  const pressIn = () => {
    Animated.spring(scaleVal, {
      toValue: 0.95,
      friction: 3,
      useNativeDriver: true,
    }).start();
  };

  // Quando solta o botão, volta a escala normal
  const pressOut = () => {
    Animated.spring(scaleVal, {
      toValue: 1,
      friction: 3,
      useNativeDriver: true,
    }).start();
  };

  return (
    <Animated.View style={{ transform: [{ scale: scaleVal }], opacity: opacityVal }}>
      <LinearGradient
        colors={[PALETTE.gradientStart, PALETTE.gradientEnd]}
        start={{ x: 0, y: 3 }}
        end={{ x: 1, y: 1 }}
        style={[styles.botao, { width: width * 0.52 }]}
      >
        <TouchableOpacity
          onPressIn={pressIn}
          onPressOut={pressOut}
          onPress={onPress}
          activeOpacity={0.8}
        >
          <Text style={[styles.textoBotao, { fontSize: normalize(28) }]}>{children}</Text>
        </TouchableOpacity>
      </LinearGradient>
    </Animated.View>
  );
};

/**
 * Tipagem das props para a tela inicial (TelaCapa).
 * Recebe o objeto de navigation para seguir para próxima tela.
 */
interface TelaCapaProps {
  navigation: NavigationProp<any>;
}

/**
 * Componente principal da tela de capa,
 * exibe animações de introdução, título, subtítulo e botão para avançar.
 */
const TelaCapa: React.FC<TelaCapaProps> = ({ navigation }) => {
  const { width, height } = useWindowDimensions();

  /**
   * Estado para armazenar o fator de escala de fonte, dependendo do tamanho do dispositivo
   */
  const [fontScale, setFontScale] = useState(1);

  // Animações do container principal
  const containerOpacity = useRef(new Animated.Value(0)).current;
  const containerScale = useRef(new Animated.Value(0.95)).current;

  // Animações do título
  const titleOpacity = useRef(new Animated.Value(0)).current;
  const titleY = useRef(new Animated.Value(-50)).current;
  const titleScale = useRef(new Animated.Value(0.8)).current;

  // Animações do subtítulo/descrição
  const descOpacity = useRef(new Animated.Value(0)).current;
  const descY = useRef(new Animated.Value(20)).current;

  // Animações da esfera (GIF)
  const sphereOpacity = useRef(new Animated.Value(0)).current;
  const sphereScale = useRef(new Animated.Value(0.5)).current;
  const sphereY = useRef(new Animated.Value(20)).current;

  // Animações do ícone (pequeno cérebro)
  const iconOpacity = useRef(new Animated.Value(0)).current;
  const iconY = useRef(new Animated.Value(-50)).current;
  const iconScale = useRef(new Animated.Value(0.8)).current;

  /**
   * Efeito para calcular e ajustar a escala de fonte,
   * com base em breakpoints para diferentes tamanhos de tela.
   */
  useEffect(() => {
    const calculateFontScale = () => {
      if (width >= 768) {
        // Tablets
        setFontScale(1.2);
      } else if (width >= 414) {
        // iPhone Plus/Pro Max etc.
        setFontScale(1.1);
      } else if (width <= 320) {
        // iPhone SE e dispositivos menores
        setFontScale(0.85);
      } else {
        // Padrão
        setFontScale(1);
      }
    };

    calculateFontScale();
  }, [width]);

  /**
   * Ao montar a tela, executa as animações em sequência/parallel:
   * - Container fadeIn + scale
   * - Título fadeIn + move de Y: -50 para 0 + scale
   * - Subtítulo fadeIn + move de Y: 20 para 0
   * - Esfera (GIF) fadeIn + scaleUp
   * - Ícone fadeIn + move Y
   */
  useEffect(() => {
    // Animação do container principal
    Animated.parallel([
      Animated.timing(containerOpacity, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
      Animated.timing(containerScale, {
        toValue: 1,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.ease),
      }),
    ]).start();

    // Animação do título (após pequeno delay)
    Animated.sequence([
      Animated.delay(300),
      Animated.parallel([
        Animated.timing(titleOpacity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
        Animated.timing(titleY, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.5)),
        }),
        Animated.timing(titleScale, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.ease),
        }),
      ]),
    ]).start();

    // Animação do subtítulo/descrição
    Animated.sequence([
      Animated.delay(600),
      Animated.parallel([
        Animated.timing(descOpacity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
        }),
        Animated.timing(descY, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
        }),
      ]),
    ]).start();

    // Animação da esfera (GIF) com scale e fade-in
    Animated.parallel([
      Animated.timing(sphereOpacity, {
        toValue: 1,
        duration: 2000,
        useNativeDriver: true,
      }),
      Animated.timing(sphereScale, {
        toValue: 1,
        duration: 2000,
        useNativeDriver: true,
      }),
      Animated.timing(sphereY, {
        toValue: 0,
        duration: 2000,
        useNativeDriver: true,
      }),
    ]).start();

    // Animação do ícone (cérebro)
    Animated.sequence([
      Animated.delay(600),
      Animated.parallel([
        Animated.timing(iconOpacity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
        }),
        Animated.timing(iconY, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
        }),
        Animated.timing(iconScale, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
        }),
      ]),
    ]).start();
  }, [
    containerOpacity,
    containerScale,
    titleOpacity,
    titleY,
    titleScale,
    descOpacity,
    descY,
    sphereOpacity,
    sphereScale,
    sphereY,
    iconOpacity,
    iconY,
    iconScale,
  ]);

  /**
   * Função para avançar para a próxima tela (Choice)
   */
  const avancar = () => {
    navigation.navigate('Choice');
  };

  /**
   * Estilos que dependem de dimensões e fontScale (responsividade).
   */
  const dynamicStyles = {
    titulo: {
      fontSize: normalize(58 * fontScale),
      marginBottom: height * 0.015,
    },
    subtitulo: {
      fontSize: normalize(22 * fontScale),
      lineHeight: normalize(29 * fontScale),
    },
    esfera: {
      width: width * 0.9,
      height: width * 0.9, // Mantém a proporção
      marginTop: height * 0.15,
    },
    containerConteudo: {
      paddingVertical: height * 0.05,
    },
    containerSuperior: {
      paddingHorizontal: width * 0.06,
    },
    iconCerebro: {
      width: normalize(50 * fontScale),
      height: normalize(40 * fontScale),
    },
    textoBotao: {
      fontSize: normalize(28 * fontScale),
    },
    containerBotao: {
      paddingHorizontal: width * 0.06,
      marginBottom: height * 0.02,
    },
  };

  return (
    <View style={styles.container}>
      {/* Imagem de fundo (capa) */}
      <Image
        source={require('../../assets/images/CapaWelcome.png')}
        style={styles.imagemFundo}
        resizeMode="cover"
      />
      
      {/* Container principal que engloba as seções */}
      <Animated.View
        style={[
          styles.containerConteudo,
          dynamicStyles.containerConteudo,
          {
            opacity: containerOpacity,
            transform: [{ scale: containerScale }],
          },
        ]}
      >
        {/* Seção superior (título, ícone e subtítulo) */}
        <View style={[styles.containerSuperior, dynamicStyles.containerSuperior]}>
          <Animated.Text
            style={[
              styles.titulo,
              dynamicStyles.titulo,
              {
                opacity: titleOpacity,
                transform: [{ translateY: titleY }, { scale: titleScale }],
              },
            ]}
          >
            Bem-vindo à{'\n'}
            <Text style={styles.textoSmart}>Smart</Text>
            <Text style={styles.textoDent}>Dent</Text>
            <View style={{ width: width * 0.025 }} />
            <Animated.Image
              source={require('../../assets/images/IconCapa.png')}
              style={[
                styles.iconCerebro,
                dynamicStyles.iconCerebro,
                {
                  opacity: iconOpacity,
                  transform: [{ translateY: iconY }, { scale: iconScale }],
                },
              ]}
              resizeMode="contain"
            />
          </Animated.Text>

          <Animated.Text
            style={[
              styles.subtitulo,
              dynamicStyles.subtitulo,
              {
                opacity: descOpacity,
                transform: [{ translateY: descY }],
              },
            ]}
            adjustsFontSizeToFit={width < 350}
            numberOfLines={4}
          >
            Cuide da sua saúde bucal com IA.{'\n'}
            Marque consultas, acompanhe seu{'\n'}
            histórico e receba suporte de forma{'\n'}
            prática e eficaz.
          </Animated.Text>
        </View>

        {/* Seção intermediária: Esfera (GIF) */}
        <View style={styles.containerEsfera}>
          <AnimatedFastImage
            source={require('../../assets/images/Esfera.gif')}
            style={[
              dynamicStyles.esfera,
              {
                opacity: sphereOpacity,
                transform: [{ scale: sphereScale }, { translateY: sphereY }],
              },
            ]}
            resizeMode={FastImage.resizeMode.contain}
          />
        </View>

        {/* Seção inferior (botão para avançar) */}
        <View style={[styles.containerBotao, dynamicStyles.containerBotao]}>
          <BotaoAnimado onPress={avancar}>Avançar</BotaoAnimado>
        </View>
      </Animated.View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  imagemFundo: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    width: '100%',
    height: '100%',
  },
  containerConteudo: {
    flex: 1,
    justifyContent: 'space-between',
  },
  containerSuperior: {
    flex: 0.3,
    justifyContent: 'flex-start',
  },
  titulo: {
    color: PALETTE.textDefault,
    fontFamily: 'Poppins-regular',
  },
  textoSmart: {
    color: PALETTE.textSmart,
  },
  textoDent: {
    color: PALETTE.textDent,
  },
  subtitulo: {
    color: PALETTE.textDefault,
  },
  containerEsfera: {
    flex: 0.5,
    alignItems: 'center',
    justifyContent: 'center',
  },
  containerBotao: {
    flex: 0.2,
    alignItems: 'center',
    justifyContent: 'flex-end',
  },
  botao: {
    borderWidth: 3,
    borderColor: PALETTE.borderColor,
    paddingVertical: '3.2%',
    borderRadius: 20,
    alignItems: 'center',
  },
  textoBotao: {
    color: PALETTE.textDefault,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  iconCerebro: {
    resizeMode: 'contain',
  },
});

export default TelaCapa;
