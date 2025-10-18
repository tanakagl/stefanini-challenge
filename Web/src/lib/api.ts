// Simple API client using fetch
import type {
  LoginResponseDto,
  UserInfoDto,
  UserResponseDto,
  UserCreateDto,
  UserUpdateDto,
} from "@/api/src/models";
import { endpoints } from "./endpoints";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "https://localhost:7077";

class ApiError extends Error {
  constructor(
    message: string,
    public status: number,
    public body?: unknown
  ) {
    super(message);
    this.name = "ApiError";
  }
}

async function fetchApi<T>(
  endpoint: string,
  options: RequestInit = {}
): Promise<T> {
  const url = `${API_BASE_URL}${endpoint}`;
  
  const response = await fetch(url, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  if (!response.ok) {
    const errorText = await response.text();
    let errorBody;
    try {
      errorBody = JSON.parse(errorText);
    } catch {
      errorBody = errorText;
    }
    
    throw new ApiError(
      errorBody?.message || errorBody?.title || `Request failed with status ${response.status}`,
      response.status,
      errorBody
    );
  }

  // Check if response has content before parsing JSON
  const contentLength = response.headers.get("content-length");
  const contentType = response.headers.get("content-type");
  
  // If no content or content-length is 0, return empty object
  if (
    response.status === 204 || 
    contentLength === "0" || 
    !contentType?.includes("application/json")
  ) {
    return {} as T;
  }

  // Try to parse JSON, return empty object if it fails
  const text = await response.text();
  if (!text) {
    return {} as T;
  }
  
  return JSON.parse(text);
}

export const api = {
  // Auth
  login: (email: string, password: string): Promise<LoginResponseDto> =>
    fetchApi(endpoints.auth.login, {
      method: "POST",
      body: JSON.stringify({ email, password }),
    }),

  getCurrentUser: (token: string): Promise<UserInfoDto> =>
    fetchApi(endpoints.auth.me, {
      headers: { Authorization: `Bearer ${token}` },
    }),

  // Users
  getAllUsers: (token: string): Promise<UserResponseDto[]> =>
    fetchApi(endpoints.users.getAll, {
      headers: { Authorization: `Bearer ${token}` },
    }),

  getUsersByName: (token: string, nome: string): Promise<UserResponseDto[]> =>
    fetchApi(endpoints.users.search(nome), {
      headers: { Authorization: `Bearer ${token}` },
    }),

  // Register new user (no auth required - uses the same endpoint as createUser)
  register: (user: UserCreateDto): Promise<UserResponseDto> =>
    fetchApi(endpoints.users.create, {
      method: "POST",
      body: JSON.stringify(user),
    }),

  createUser: (token: string, user: UserCreateDto): Promise<UserResponseDto> =>
    fetchApi(endpoints.users.create, {
      method: "POST",
      headers: { Authorization: `Bearer ${token}` },
      body: JSON.stringify(user),
    }),

  updateUser: (token: string, id: string, user: UserUpdateDto): Promise<UserResponseDto> =>
    fetchApi(endpoints.users.update(id), {
      method: "PUT",
      headers: { Authorization: `Bearer ${token}` },
      body: JSON.stringify(user),
    }),

  deleteUser: (token: string, id: string): Promise<void> =>
    fetchApi(endpoints.users.delete(id), {
      method: "DELETE",
      headers: { Authorization: `Bearer ${token}` },
    }),
};

