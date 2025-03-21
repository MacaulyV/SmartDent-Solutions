import { StorageService } from './storage';
import { LoginCredentials, UserData, AuthResponse } from '../types/auth';
import { createUser, IPacienteData } from './api';

export class AuthService {
  private static instance: AuthService;
  private storageService!: StorageService;

  private constructor() {
    // O construtor pode permanecer vazio
  }

  public static async getInstance(): Promise<AuthService> {
    if (!AuthService.instance) {
      AuthService.instance = new AuthService();
      AuthService.instance.storageService = await StorageService.getInstance(); // Aguarde a instância do StorageService
    }
    return AuthService.instance;
  }

  public async register(userData: IPacienteData): Promise<AuthResponse> {
    try {
      console.log('AuthService: Iniciando registro do usuário');
      
      // Primeiro, tenta registrar na API
      const response = await createUser(userData);
      console.log('AuthService: Resposta da API:', response);

      if (response.status === 201 || response.status === 200) {
        // Se o registro na API foi bem sucedido, salva localmente
        const localUserData: UserData = {
          fullName: userData.nomeCompleto,
          email: userData.email,
          cpf: userData.cpf,
          lastLogin: new Date().toISOString(),
          deviceId: '', // Será preenchido pelo StorageService
        };

        console.log('AuthService: Tentando salvar dados localmente:', localUserData);
        const saved = await this.storageService.saveUserData(localUserData);
        console.log('AuthService: Resultado do salvamento local:', saved);

        if (saved) {
          return {
            success: true,
            message: 'Cadastro realizado com sucesso!',
            data: localUserData,
          };
        } else {
          console.error('AuthService: Erro ao salvar dados localmente');
          return {
            success: false,
            message: 'Erro ao salvar dados localmente',
            error: 'storage_error',
          };
        }
      } else {
        console.error('AuthService: Erro na resposta da API:', response);
        return {
          success: false,
          message: 'Erro ao cadastrar na API',
          error: 'api_error',
        };
      }
    } catch (error: any) {
      console.error('AuthService: Erro durante o registro:', error);
      return {
        success: false,
        message: error.message || 'Erro ao realizar cadastro',
        error: 'unknown_error',
      };
    }
  }

  public async login(credentials: LoginCredentials): Promise<AuthResponse> {
    try {
      // Primeiro, tenta validar com os dados locais
      const storedData = await this.storageService.getUserData();
      
      if (!storedData) {
        return {
          success: false,
          message: 'Dados de login não encontrados',
          error: 'no_stored_data',
        };
      }

      console.log('Dados armazenados:', storedData);
      console.log('Credenciais recebidas:', credentials);

      // Valida o CPF
      if (storedData.cpf !== credentials.cpf) {
        return {
          success: false,
          message: 'CPF incorreto',
          error: 'invalid_cpf',
        };
      }

      // Valida o identificador (nome ou email)
      if (credentials.loginType === 'email' && storedData.email !== credentials.identifier) {
        return {
          success: false,
          message: 'Email incorreto',
          error: 'invalid_email',
        };
      }

      if (credentials.loginType === 'name' && storedData.fullName !== credentials.identifier) {
        return {
          success: false,
          message: 'Nome incorreto',
          error: 'invalid_name',
        };
      }

      // Se chegou aqui, os dados estão corretos
      return {
        success: true,
        message: 'Login realizado com sucesso!',
        data: storedData,
      };
    } catch (error: any) {
      return {
        success: false,
        message: error.message || 'Erro ao realizar login',
        error: 'unknown_error',
      };
    }
  }

  public async validateLocalLogin(credentials: LoginCredentials): Promise<boolean> {
    try {
      const storedData = await this.storageService.getUserData();
      
      if (!storedData) return false;

      // Valida o CPF
      if (storedData.cpf !== credentials.cpf) return false;

      // Valida o identificador (nome ou email)
      if (credentials.loginType === 'email' && storedData.email !== credentials.identifier) {
        return false;
      }

      if (credentials.loginType === 'name' && storedData.fullName !== credentials.identifier) {
        return false;
      }

      return true;
    } catch (error) {
      console.error('Erro ao validar login local:', error);
      return false;
    }
  }

  public async logout(): Promise<boolean> {
    try {
      return await this.storageService.clearUserData();
    } catch (error) {
      console.error('Erro ao realizar logout:', error);
      return false;
    }
  }
} 