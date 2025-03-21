import EncryptedStorage from 'react-native-encrypted-storage';
import { UserData, StorageData } from '../types/auth';
import DeviceInfo from 'react-native-device-info';

const STORAGE_KEY = '@SmartDent:userData';

export class StorageService {
  private static instance: StorageService;
  private deviceId!: string;

  private constructor() {
    this.initializeDeviceId();
  }

  private async initializeDeviceId() {
    this.deviceId = await DeviceInfo.getUniqueId();
  }

  public static async getInstance(): Promise<StorageService> {
    if (!StorageService.instance) {
      StorageService.instance = new StorageService();
      await StorageService.instance.initializeDeviceId();
    }
    return StorageService.instance;
  }

  private generateEncryptionKey(): string {
    return DeviceInfo.getUniqueId() + Date.now().toString();
  }

  public async saveUserData(userData: UserData): Promise<boolean> {
    try {
      console.log('StorageService: Iniciando salvamento de dados');
      
      const storageData: StorageData = {
        userData: {
          ...userData,
          deviceId: this.deviceId,
          lastLogin: new Date().toISOString(),
        },
        encryptionKey: this.generateEncryptionKey(),
        lastUpdate: new Date().toISOString(),
      };

      console.log('StorageService: Dados preparados para salvamento:', storageData);

      await EncryptedStorage.setItem(
        STORAGE_KEY,
        JSON.stringify(storageData)
      );

      console.log('StorageService: Dados salvos com sucesso');
      
      // Verifica se os dados foram realmente salvos
      const savedData = await this.getUserData();
      console.log('StorageService: Verificação de dados salvos:', savedData);

      return true;
    } catch (error) {
      console.error('StorageService: Erro ao salvar dados do usuário:', error);
      return false;
    }
  }

  public async getUserData(): Promise<UserData | null> {
    try {
      const data = await EncryptedStorage.getItem(STORAGE_KEY);
      if (!data) return null;

      const storageData: StorageData = JSON.parse(data);

      // Verifica se os dados são do mesmo dispositivo
      if (storageData.userData.deviceId !== this.deviceId) {
        await this.clearUserData();
        return null;
      }

      // Verifica se os dados não estão muito antigos (30 dias)
      const lastUpdate = new Date(storageData.lastUpdate);
      const thirtyDaysAgo = new Date();
      thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);

      if (lastUpdate < thirtyDaysAgo) {
        await this.clearUserData();
        return null;
      }

      return storageData.userData;
    } catch (error) {
      console.error('Erro ao recuperar dados do usuário:', error);
      return null;
    }
  }

  public async clearUserData(): Promise<boolean> {
    try {
      await EncryptedStorage.removeItem(STORAGE_KEY);
      return true;
    } catch (error) {
      console.error('Erro ao limpar dados do usuário:', error);
      return false;
    }
  }

  public async validateStoredData(): Promise<boolean> {
    try {
      const data = await this.getUserData();
      return data !== null;
    } catch (error) {
      console.error('Erro ao validar dados armazenados:', error);
      return false;
    }
  }
} 