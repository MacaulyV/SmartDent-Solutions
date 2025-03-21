export interface UserData {
  fullName: string;
  email: string;
  cpf: string;
  lastLogin: string;
  deviceId: string;
}

export interface LoginCredentials {
  identifier: string; // Pode ser nome ou email
  cpf: string;
  loginType: 'name' | 'email';
}

export interface AuthResponse {
  success: boolean;
  message: string;
  data?: UserData;
  error?: string;
}

export interface StorageData {
  userData: UserData;
  encryptionKey: string;
  lastUpdate: string;
} 