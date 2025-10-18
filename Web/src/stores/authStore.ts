import { create } from "zustand";
import { persist } from "zustand/middleware";
import { api } from "@/lib/api";
import type { UserInfoDto, LoginResponseDto } from "@/api/src/models";

interface AuthState {
  user: UserInfoDto | null;
  accessToken: string | null;
  refreshToken: string | null;
  expiresAt: Date | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  
  // Actions
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  setUser: (user: UserInfoDto | null) => void;
  setTokens: (response: LoginResponseDto) => void;
  checkAuth: () => Promise<void>;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      expiresAt: null,
      isAuthenticated: false,
      isLoading: true,

      setTokens: (response: LoginResponseDto) => {
        const token = response.accessToken || null;
        
        set({
          accessToken: token,
          refreshToken: response.refreshToken || null,
          expiresAt: response.expiresAt ? new Date(response.expiresAt) : null,
          user: response.user || null,
          isAuthenticated: !!token,
        });
      },

      login: async (email: string, password: string) => {
        try {
          const response = await api.login(email, password);
          get().setTokens(response);
        } catch (error) {
          console.error("Login failed:", error);
          
          // Extrair mensagem de erro detalhada
          let errorMessage = "Erro ao fazer login";
          
          if (error instanceof Error) {
            errorMessage = error.message;
          }
          
          throw new Error(errorMessage);
        }
      },

      logout: () => {
        set({
          user: null,
          accessToken: null,
          refreshToken: null,
          expiresAt: null,
          isAuthenticated: false,
        });
      },

      setUser: (user: UserInfoDto | null) => {
        set({ user });
      },

      checkAuth: async () => {
        const { accessToken } = get();
        
        if (!accessToken) {
          set({ isLoading: false, isAuthenticated: false });
          return;
        }

        try {
          const user = await api.getCurrentUser(accessToken);
          
          set({
            user,
            isAuthenticated: true,
            isLoading: false,
          });
        } catch (error) {
          console.error("Auth check failed:", error);
          get().logout();
          set({ isLoading: false });
        }
      },
    }),
    {
      name: "auth-storage",
      partialize: (state) => ({
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        expiresAt: state.expiresAt,
        user: state.user,
      }),
    }
  )
);

