import axios from 'axios';

export interface IPacienteData {
  nomeCompleto: string;
  cpf: string;
  dataNascimento: string;  // Ex.: "19850615"
  email: string;
  telefone: string;        // Ex.: "11999999999"
  endereco: string;        // Ex.: "Rua Fictícia, 123, São Paulo"
  planoOdontologico: string;
  empresa?: string;        // Se vazio, a API salva como "Individual"
}

const api = axios.create({
  baseURL: 'https://smartdent-api.onrender.com', // Ajuste conforme necessário
  timeout: 10000,
});

// Interceptor para logar os dados enviados em cada requisição
api.interceptors.request.use(
  (config) => {
    console.log('Enviando requisição para:', config.url);
    console.log('Payload:', config.data);
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor para logar respostas (opcional)
api.interceptors.response.use(
  (response) => {
    console.log('Resposta recebida:', response.status, response.data);
    return response;
  },
  (error) => {
    console.error('Erro na resposta:', error.response || error.message);
    return Promise.reject(error);
  }
);

export const createUser = async (data: IPacienteData) => {
  // Verifique se o endpoint está correto: '/api/A_Pacientes' ou '/A_Pacientes'
  return api.post('/api/A_Pacientes', data);
};

export const getUser = (id: string) => api.get(`/api/A_Pacientes/${id}`);

export default api;
