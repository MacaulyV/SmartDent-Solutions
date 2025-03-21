import React, { useEffect, useRef, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  Dimensions,
  Image,
  TouchableOpacity,
  Animated,
  Easing,
  ScrollView,
  ImageSourcePropType,
  ImageBackground,
  StatusBar,
} from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import { NavigationProp } from '@react-navigation/native';
import AnimatedFastImage from 'react-native-fast-image';

const { width, height } = Dimensions.get('window');

// Interface para as props do OpcaoCard
interface OpcaoCardProps {
  icon: ImageSourcePropType;
  title: string;
  onPress: () => void;
  delay?: number;
  customIconStyle?: object;
  index: number;
}

// Componente para os cards de opções com animação aprimorada
const OpcaoCard: React.FC<OpcaoCardProps> = ({ 
  icon, 
  title, 
  onPress, 
  delay = 0, 
  customIconStyle = {},
  index
}) => {
  const opacity = useRef(new Animated.Value(0)).current;
  const scale = useRef(new Animated.Value(0.5)).current;
  const translateY = useRef(new Animated.Value(50)).current;
  const translateX = useRef(new Animated.Value(index % 2 === 0 ? -30 : 30)).current;
  const rotate = useRef(new Animated.Value(index % 2 === 0 ? -10 : 10)).current;

  useEffect(() => {
    Animated.sequence([
      Animated.delay(delay),
      Animated.parallel([
        Animated.timing(opacity, {
          toValue: 1,
          duration: 600,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(scale, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.7)),
        }),
        Animated.timing(translateY, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(translateX, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(rotate, {
          toValue: 0,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
      ]),
    ]).start();
  }, []);

  // Animação ao pressionar
  const pressAnim = useRef(new Animated.Value(1)).current;
  
  const handlePressIn = () => {
    Animated.spring(pressAnim, {
      toValue: 0.92,
      friction: 5,
      tension: 300,
      useNativeDriver: true,
    }).start();
  };
  
  const handlePressOut = () => {
    Animated.spring(pressAnim, {
      toValue: 1,
      friction: 3,
      tension: 400,
      useNativeDriver: true,
    }).start();
  };

  return (
    <Animated.View
      style={[
        styles.opcaoCardContainer,
        {
          opacity,
          transform: [
            { scale },
            { translateY },
            { translateX },
            { rotate: rotate.interpolate({
                inputRange: [-10, 0, 10],
                outputRange: ['-10deg', '0deg', '10deg']
              })
            }
          ],
        },
      ]}
    >
      <TouchableOpacity
        style={styles.opcaoCard}
        onPress={onPress}
        activeOpacity={1}
        onPressIn={handlePressIn}
        onPressOut={handlePressOut}
      >
        <Animated.View style={{
          width: '100%',
          height: '100%',
          alignItems: 'center',
          justifyContent: 'center',
          transform: [{ scale: pressAnim }]
        }}>
          <LinearGradient
            colors={['#FFFFFF', '#F5F9FF']}
            style={styles.cardGradient}
          />
          
          <Animated.Image
            source={icon}
            style={[
              styles.opcaoIcon, 
              customIconStyle,
              {
                transform: [
                  { scale: pressAnim }
                ]
              }
            ]}
            resizeMode="contain"
          />
          
          <Animated.Text 
            style={[
              styles.opcaoTitle,
              {
                transform: [
                  { scale: Animated.multiply(pressAnim, 0.96).interpolate({
                      inputRange: [0.88, 1],
                      outputRange: [0.96, 1]
                    })
                  }
                ]
              }
            ]}
          >
            {title}
          </Animated.Text>
        </Animated.View>
      </TouchableOpacity>
    </Animated.View>
  );
};

// Interface para as props do DestaqueTopo
interface DestaqueTopoProps {
  onPress: () => void;
  animationProgress: Animated.Value;
}

// Componente para o card destacado no topo com animação aprimorada
const DestaqueTopo: React.FC<DestaqueTopoProps> = ({ onPress, animationProgress }) => {
  const opacity = useRef(new Animated.Value(0)).current;
  const translateY = useRef(new Animated.Value(-50)).current;
  const scale = useRef(new Animated.Value(0.9)).current;
  const rotate = useRef(new Animated.Value(-3)).current;

  useEffect(() => {
    Animated.parallel([
      Animated.timing(opacity, {
        toValue: 1,
        duration: 1000,
        useNativeDriver: true,
        easing: Easing.out(Easing.exp),
      }),
      Animated.timing(translateY, {
        toValue: 0,
        duration: 1000,
        useNativeDriver: true,
        easing: Easing.out(Easing.back(1.7)),
      }),
      Animated.timing(scale, {
        toValue: 1,
        duration: 1000,
        useNativeDriver: true,
        easing: Easing.out(Easing.back(1.5)),
      }),
      Animated.timing(rotate, {
        toValue: 0,
        duration: 800,
        useNativeDriver: true,
        easing: Easing.out(Easing.exp),
      }),
    ]).start();
  }, []);

  // Efeito de parallax suave para elementos internos
  const titleTranslateY = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [5, 0]
  });
  
  const subtitleTranslateY = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [10, 0]
  });
  
  const buttonTranslateY = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [15, 0]
  });

  // Animação ao pressionar botão
  const buttonScale = useRef(new Animated.Value(1)).current;
  
  const handleButtonPressIn = () => {
    Animated.spring(buttonScale, {
      toValue: 0.95,
      friction: 5,
      tension: 300,
      useNativeDriver: true,
    }).start();
  };
  
  const handleButtonPressOut = () => {
    Animated.spring(buttonScale, {
      toValue: 1,
      friction: 3,
      tension: 400,
      useNativeDriver: true,
    }).start();
  };

  return (
    <Animated.View
      style={[
        styles.destaqueContainer,
        {
          opacity,
          transform: [
            { translateY },
            { scale },
            { rotate: rotate.interpolate({
                inputRange: [-3, 0],
                outputRange: ['-3deg', '0deg']
              })
            }
          ],
        },
      ]}
    >
      <View style={styles.destaqueContent}>
        <View style={styles.destaqueTextContainer}>
          <Animated.Text 
            style={[
              styles.destaqueTitulo,
              { transform: [{ translateY: titleTranslateY }] }
            ]}
          >
            Encontre o que você Precisa
          </Animated.Text>
          
          <Animated.Text 
            style={[
              styles.destaqueSubtitulo,
              { transform: [{ translateY: subtitleTranslateY }] }
            ]}
          >
            Agende suas consultas e veja seu histórico facilmente
          </Animated.Text>
          
          <Animated.View
            style={{
              transform: [
                { translateY: buttonTranslateY },
                { scale: buttonScale }
              ]
            }}
          >
            <TouchableOpacity 
              style={styles.agendarBotao} 
              onPress={onPress}
              activeOpacity={1}
              onPressIn={handleButtonPressIn}
              onPressOut={handleButtonPressOut}
            >
              <LinearGradient
                colors={['#0099FF', '#0070FF']}
                start={{ x: 0, y: 0 }}
                end={{ x: 1, y: 0 }}
                style={styles.buttonGradient}
              >
                <Text style={styles.agendarBotaoTexto}>Agendar</Text>
              </LinearGradient>
            </TouchableOpacity>
          </Animated.View>
        </View>
        
        <Animated.View
          style={{
            opacity,
            transform: [
              { scale: Animated.add(1, Animated.multiply(animationProgress, 0.05)) },
              { translateY: Animated.multiply(animationProgress, -5) }
            ],
          }}
        >
          <AnimatedFastImage
            source={require('../../assets/images/Clinica.gif')}
            style={styles.destaqueImagem}
            resizeMode="contain"
          />
        </Animated.View>
      </View>
    </Animated.View>
  );
};

// Interface para as props do MainScreen
interface MainScreenProps {
  navigation: NavigationProp<any>;
}

const MainScreen: React.FC<MainScreenProps> = ({ navigation }) => {
  // Estado para controlar se a animação de entrada está concluída
  const [animationComplete, setAnimationComplete] = useState(false);
  
  // Valor para animação do background
  const bgAnimValue = useRef(new Animated.Value(0)).current;
  
  // Valores para animações dos elementos principais
  const headerOpacity = useRef(new Animated.Value(0)).current;
  const headerTranslateY = useRef(new Animated.Value(-30)).current;
  const headerScale = useRef(new Animated.Value(0.9)).current;
  
  const subtitleOpacity = useRef(new Animated.Value(0)).current;
  const subtitleTranslateY = useRef(new Animated.Value(-20)).current;
  
  // Valor de progresso para animações paralaxe
  const scrollY = useRef(new Animated.Value(0)).current;
  
  // Valor para efeito de revelação inicial
  const revealAnim = useRef(new Animated.Value(0)).current;
  
  // Indicador de scroll para animações paralaxe
  const animationProgress = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    // Animação de revelação inicial
    Animated.timing(revealAnim, {
      toValue: 1,
      duration: 1200,
      useNativeDriver: true,
      easing: Easing.out(Easing.exp),
    }).start();
    
    // Animação de entrada do background
    Animated.timing(bgAnimValue, {
      toValue: 1,
      duration: 2000,
      useNativeDriver: false,
      easing: Easing.out(Easing.exp),
    }).start();

    // Sequência de animações para os elementos da interface
    Animated.sequence([
      // Animação do título
      Animated.parallel([
        Animated.timing(headerOpacity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(headerTranslateY, {
          toValue: 0,
          duration: 1000,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.7)),
        }),
        Animated.timing(headerScale, {
          toValue: 1,
          duration: 1000,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.2)),
        }),
      ]),

      // Animação do subtítulo
      Animated.parallel([
        Animated.timing(subtitleOpacity, {
          toValue: 1,
          duration: 800,
          useNativeDriver: true,
          easing: Easing.out(Easing.exp),
        }),
        Animated.timing(subtitleTranslateY, {
          toValue: 0,
          duration: 1000,
          useNativeDriver: true,
          easing: Easing.out(Easing.back(1.7)),
        }),
      ]),
    ]).start(() => {
      // Marca a animação como concluída
      setAnimationComplete(true);
      
      // Inicia um loop de animação para efeitos sutis contínuos
      startContinuousAnimations();
    });
  }, []);
  
  // Função para iniciar animações sutis contínuas após a entrada
  const startContinuousAnimations = () => {
    // Cria uma animação sutil e contínua para o efeito de parallax
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
  };

  // Funções para lidar com navegação ou ações dos botões
  const handleMarcarConsulta = () => {
    // navigation.navigate('MarcarConsulta');
    console.log('Navegar para Marcar Consulta');
  };

  const handleHistoricoConsultas = () => {
    // navigation.navigate('HistoricoConsultas');
    console.log('Navegar para Histórico de Consultas');
  };

  const handleChatSuporte = () => {
    // navigation.navigate('ChatSuporte');
    console.log('Navegar para Chat de Suporte');
  };

  const handlePerfil = () => {
    navigation.navigate('Profile');
  };

  const handleBack = () => {
    navigation.goBack();
  };
  
  // Interpolações para efeitos visuais avançados
  const backgroundScale = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [1, 1.05]
  });
  
  const backgroundTranslateY = animationProgress.interpolate({
    inputRange: [0, 1],
    outputRange: [0, -10]
  });

  return (
    <View style={styles.container}>
      <StatusBar translucent backgroundColor="transparent" barStyle="light-content" />
      
      {/* Camada de revelação para animação inicial */}
      <Animated.View style={[
        styles.revealLayer,
        {
          opacity: revealAnim.interpolate({
            inputRange: [0, 0.4, 1],
            outputRange: [1, 0.7, 0]
          }),
          transform: [
            { 
              translateY: revealAnim.interpolate({
                inputRange: [0, 1],
                outputRange: [0, -height]
              })
            }
          ]
        }
      ]}>
        <LinearGradient
          colors={['#0099FF', '#0070FF']}
          style={styles.revealGradient}
        />
      </Animated.View>
      
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
            colors={['rgba(0,0,0,0.4)', 'rgba(0,60,120,0.6)']}
            style={styles.backgroundOverlay}
          />
        </ImageBackground>
      </Animated.View>
      
      {/* Partículas animadas (efeito visual sutíl) */}
      <Animated.View style={styles.particlesContainer} pointerEvents="none">
        {Array(6).fill(0).map((_, index) => {
          const particleAnim = useRef(new Animated.Value(0)).current;
          
          // Inicia animação para cada partícula com delay diferente
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

      <Animated.ScrollView 
        showsVerticalScrollIndicator={false}
        onScroll={Animated.event(
          [{ nativeEvent: { contentOffset: { y: scrollY } } }],
          { useNativeDriver: false }
        )}
        scrollEventThrottle={16}
        style={{ flex: 1 }}
      >
        <View style={styles.contentContainer}>
          {/* Título da tela */}
          <Animated.Text
            style={[
              styles.headerTitle,
              {
                opacity: headerOpacity,
                transform: [
                  { translateY: headerTranslateY },
                  { scale: headerScale }
                ],
                textShadowColor: 'rgba(0,0,0,0.4)',
                textShadowOffset: { width: 0, height: 2 },
                textShadowRadius: 3
              },
            ]}
          >
            Explore Opções
          </Animated.Text>

          {/* Card destacado no topo */}
          <DestaqueTopo 
            onPress={handleMarcarConsulta} 
            animationProgress={animationProgress}
          />

          {/* Título da seção de opções */}
          <Animated.Text
            style={[
              styles.opcoesTitulo,
              {
                opacity: subtitleOpacity,
                transform: [
                  { translateY: subtitleTranslateY },
                  { 
                    scale: Animated.add(1, Animated.multiply(animationProgress, 0.03))
                  }
                ],
                textShadowColor: 'rgba(0,0,0,0.4)',
                textShadowOffset: { width: 0, height: 2 },
                textShadowRadius: 3
              },
            ]}
          >
            Opções
          </Animated.Text>

          {/* Grid de opções */}
          <View style={styles.opcoesGrid}>
            <OpcaoCard
              title="Marcar Consulta"
              icon={require('../../assets/images/Calendario.png')}
              onPress={handleMarcarConsulta}
              delay={800}
              customIconStyle={{ width: 50, height: 50 }}
              index={0}
            />
            <OpcaoCard
              title="Histórico de Consultas"
              icon={require('../../assets/images/Historico.png')}
              onPress={handleHistoricoConsultas}
              delay={950}
              customIconStyle={{ width: 50, height: 50 }}
              index={1}
            />
            <OpcaoCard
              title="Chat de Suporte"
              icon={require('../../assets/images/IA.png')}
              onPress={handleChatSuporte}
              delay={1100}
              customIconStyle={{ width: 50, height: 50 }}
              index={2}
            />
            <OpcaoCard
              title="Tela de Perfil"
              icon={require('../../assets/images/Perfil.png')}
              onPress={handlePerfil}
              delay={1250}
              customIconStyle={{ width: 50, height: 50 }}
              index={3}
            />
          </View>
        </View>
      </Animated.ScrollView>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  revealLayer: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    zIndex: 100,
  },
  revealGradient: {
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
  contentContainer: {
    flex: 1,
    paddingHorizontal: 30,
    paddingTop: 50,
    paddingBottom: 40,
  },
  headerTitle: {
    fontSize: 34,
    fontWeight: 'bold',
    color: '#FFFFFF',
    marginTop: 30,
    marginBottom: 25,
    paddingLeft: 15,
  },
  destaqueContainer: {
    borderWidth: 2,
    borderColor: '#00BFFF',
    backgroundColor: '#FFFFFF',
    borderRadius: 30,
    marginBottom: 30,
    shadowColor: '#0099FF',
    shadowOffset: { width: 0, height: 10 },
    shadowOpacity: 0.3,
    shadowRadius: 15,
    elevation: 15,
  },
  destaqueContent: {
    flexDirection: 'row',
    padding: 20,
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  destaqueTextContainer: {
    flex: 1,
  },
  destaqueTitulo: {
    fontSize: 22,
    width: 500,
    lineHeight: 26,
    fontWeight: 'bold',
    color: '#0084FF',
    marginBottom: 8,
  },
  destaqueSubtitulo: {
    lineHeight: 20,
    width: 230,
    fontSize: 15,
    color: '#222222',
    marginBottom: 18,
  },
  agendarBotao: {
    marginLeft: 5,
    borderRadius: 15,
    overflow: 'hidden',
    alignSelf: 'flex-start',
    shadowColor: '#0070FF',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 8,
    elevation: 8,
  },
  buttonGradient: {
    paddingVertical: 12,
    paddingHorizontal: 35,
  },
  agendarBotaoTexto: {
    color: '#FFFFFF',
    fontWeight: 'bold',
    fontSize: 16,
    textShadowColor: 'rgba(0,0,0,0.2)',
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 2,
  },
  destaqueImagem: {
    marginTop: 65,
    marginRight: 25,
    width: 80,
    height: 80,
  },
  opcoesTitulo: {
    fontSize: 26,
    fontWeight: 'bold',
    color: '#FFFFFF',
    marginBottom: 25,
    paddingLeft: 15,
  },
  opcoesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  opcaoCardContainer: {
    width: '47%',
    marginBottom: 25,
  },
  opcaoCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 30,
    padding: 15,
    alignItems: 'center',
    justifyContent: 'center',
    height: 150,
    borderWidth: 2,
    borderColor: '#00BFFF',
    overflow: 'hidden',
    shadowColor: '#0099FF',
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.25,
    shadowRadius: 12,
    elevation: 12,
  },
  cardGradient: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
  },
  opcaoIcon: {
    width: 60,
    height: 60,
    marginBottom: 15,
  },
  opcaoTitle: {
    width: 90,
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333333',
    textAlign: 'center',
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
});

export default MainScreen;