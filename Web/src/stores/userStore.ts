import { create } from "zustand";
import { api } from "@/lib/api";
import { useAuthStore } from "./authStore";
import type {
  UserResponseDto,
  UserCreateDto,
  UserUpdateDto,
} from "@/api/src/models";

interface UserState {
  users: UserResponseDto[];
  isLoading: boolean;
  error: string | null;
  searchTerm: string;

  // Actions
  setSearchTerm: (term: string) => void;
  fetchUsers: () => Promise<void>;
  searchUsers: (name: string) => Promise<void>;
  createUser: (user: UserCreateDto) => Promise<UserResponseDto>;
  updateUser: (id: string, user: UserUpdateDto) => Promise<UserResponseDto>;
  deleteUser: (id: string) => Promise<void>;
  clearError: () => void;
}

export const useUserStore = create<UserState>((set, get) => ({
  users: [],
  isLoading: false,
  error: null,
  searchTerm: "",

  setSearchTerm: (term: string) => {
    set({ searchTerm: term });
  },

  fetchUsers: async () => {
    set({ isLoading: true, error: null });
    try {
      const token = useAuthStore.getState().accessToken;
      if (!token) throw new Error("Not authenticated");
      
      const users = await api.getAllUsers(token);
      set({ users, isLoading: false });
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : "Erro ao carregar usuários",
        isLoading: false,
        users: [],
      });
      throw error;
    }
  },

  searchUsers: async (name: string) => {
    set({ isLoading: true, error: null, searchTerm: name });
    try {
      const token = useAuthStore.getState().accessToken;
      if (!token) throw new Error("Not authenticated");
      
      const users = name
        ? await api.getUsersByName(token, name)
        : await api.getAllUsers(token);
      set({ users, isLoading: false });
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : "Erro ao buscar usuários",
        isLoading: false,
        users: [],
      });
      throw error;
    }
  },

  createUser: async (user: UserCreateDto) => {
    set({ isLoading: true, error: null });
    try {
      const token = useAuthStore.getState().accessToken;
      if (!token) throw new Error("Not authenticated");
      
      const newUser = await api.createUser(token, user);
      
      // Refresh the list
      await get().fetchUsers();
      
      set({ isLoading: false });
      return newUser;
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : "Erro ao criar usuário",
        isLoading: false,
      });
      throw error;
    }
  },

  updateUser: async (id: string, user: UserUpdateDto) => {
    set({ isLoading: true, error: null });
    try {
      const token = useAuthStore.getState().accessToken;
      if (!token) throw new Error("Not authenticated");
      
      const updatedUser = await api.updateUser(token, id, user);
      
      // Refresh the list
      await get().fetchUsers();
      
      set({ isLoading: false });
      return updatedUser;
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : "Erro ao atualizar usuário",
        isLoading: false,
      });
      throw error;
    }
  },

  deleteUser: async (id: string) => {
    set({ isLoading: true, error: null });
    try {
      const token = useAuthStore.getState().accessToken;
      if (!token) throw new Error("Not authenticated");
      
      await api.deleteUser(token, id);
      
      // Refresh the list
      await get().fetchUsers();
      
      set({ isLoading: false });
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : "Erro ao excluir usuário",
        isLoading: false,
      });
      throw error;
    }
  },

  clearError: () => {
    set({ error: null });
  },
}));

