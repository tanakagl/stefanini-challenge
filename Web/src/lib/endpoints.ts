// API Endpoints
export const endpoints = {
  auth: {
    login: "/api/Auth/login",
    me: "/api/Auth/me",
  },
  users: {
    getAll: "/api/v1/Users",
    search: (nome: string) => `/api/v1/Users/search?nome=${encodeURIComponent(nome)}`,
    create: "/api/v1/Users",
    update: (id: string) => `/api/v1/Users/${id}`,
    delete: (id: string) => `/api/v1/Users/${id}`,
  },
} as const;

