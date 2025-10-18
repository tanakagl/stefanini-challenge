"use client";

import { useEffect, useState } from "react";
import { useUserStore } from "@/stores/userStore";
import { useAuthStore } from "@/stores/authStore";
import type { UserResponseDto } from "@/api/src/models";

interface UserListProps {
    onEdit: (user: UserResponseDto) => void;
    onDelete: (user: UserResponseDto) => void;
}

export function UserList({ onEdit, onDelete }: UserListProps) {
    const { users, isLoading, error, searchTerm, setSearchTerm, fetchUsers, searchUsers } = useUserStore();
    const currentUser = useAuthStore((state) => state.user);
    const [localSearchTerm, setLocalSearchTerm] = useState("");

    useEffect(() => {
        fetchUsers();
    }, []);

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await searchUsers(localSearchTerm);
        } catch (err) {
            // Error is already handled in the store
        }
    };

    const formatDate = (date?: Date) => {
        if (!date) return "-";
        return new Date(date).toLocaleDateString("pt-BR");
    };

    const formatCpf = (cpf?: string) => {
        if (!cpf) return "-";
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    };

    return (
        <div className="space-y-4">
            {/* Search Bar */}
            <form onSubmit={handleSearch} className="flex gap-2">
                <input
                    type="text"
                    placeholder="Buscar por nome..."
                    className="input input-bordered flex-1"
                    value={localSearchTerm}
                    onChange={(e) => setLocalSearchTerm(e.target.value)}
                />
                <button type="submit" className="btn btn-primary" disabled={isLoading}>
                    {isLoading ? (
                        <span className="loading loading-spinner"></span>
                    ) : (
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                            />
                        </svg>
                    )}
                </button>
                {localSearchTerm && (
                    <button
                        type="button"
                        className="btn btn-ghost"
                        onClick={async () => {
                            setLocalSearchTerm("");
                            await fetchUsers();
                        }}
                    >
                        Limpar
                    </button>
                )}
            </form>

            {error && (
                <div className="alert alert-error">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="stroke-current shrink-0 h-6 w-6"
                        fill="none"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
                        />
                    </svg>
                    <span>{error}</span>
                </div>
            )}

            {/* Table */}
            <div className="overflow-x-auto">
                <table className="table table-zebra w-full">
                    <thead>
                        <tr>
                            <th>Nome</th>
                            <th>Email</th>
                            <th>CPF</th>
                            <th>Nascimento</th>
                            <th>Sexo</th>
                            <th>Nacionalidade</th>
                            <th className="text-right">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {isLoading ? (
                            <tr>
                                <td colSpan={7} className="text-center py-8">
                                    <span className="loading loading-spinner loading-lg"></span>
                                </td>
                            </tr>
                        ) : users.length === 0 ? (
                            <tr>
                                <td colSpan={7} className="text-center py-8 opacity-70">
                                    {localSearchTerm
                                        ? "Nenhum usuário encontrado com esse nome"
                                        : "Nenhum usuário cadastrado"}
                                </td>
                            </tr>
                        ) : (
                            users.map((user) => {
                                const isCurrentUser = user.id === currentUser?.id;
                                return (
                                    <tr key={user.id} className={isCurrentUser ? "hover bg-primary/10" : "hover"}>
                                        <td className="font-medium">
                                            {user.nomeCompleto}
                                            {isCurrentUser && (
                                                <span className="ml-2 badge badge-primary badge-sm">Você</span>
                                            )}
                                        </td>
                                        <td>{user.email}</td>
                                        <td>{formatCpf(user.cpf)}</td>
                                        <td>{formatDate(user.dataNascimento)}</td>
                                        <td>{user.sexo}</td>
                                        <td>{user.nacionalidade}</td>
                                        <td className="text-right">
                                            <div className="flex gap-2 justify-end">
                                                <button
                                                    className="btn btn-sm btn-ghost"
                                                    onClick={() => onEdit(user)}
                                                    title="Editar"
                                                    disabled={isCurrentUser}
                                                >
                                                    <svg
                                                        xmlns="http://www.w3.org/2000/svg"
                                                        className="h-4 w-4"
                                                        fill="none"
                                                        viewBox="0 0 24 24"
                                                        stroke="currentColor"
                                                    >
                                                        <path
                                                            strokeLinecap="round"
                                                            strokeLinejoin="round"
                                                            strokeWidth={2}
                                                            d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
                                                        />
                                                    </svg>
                                                </button>
                                                <button
                                                    className="btn btn-sm btn-ghost text-error"
                                                    onClick={() => onDelete(user)}
                                                    title={isCurrentUser ? "Não é possível excluir seu próprio usuário" : "Excluir"}
                                                    disabled={isCurrentUser}
                                                >
                                                    <svg
                                                        xmlns="http://www.w3.org/2000/svg"
                                                        className="h-4 w-4"
                                                        fill="none"
                                                        viewBox="0 0 24 24"
                                                        stroke="currentColor"
                                                    >
                                                        <path
                                                            strokeLinecap="round"
                                                            strokeLinejoin="round"
                                                            strokeWidth={2}
                                                            d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                                                        />
                                                    </svg>
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                );
                            })
                        )}
                    </tbody>
                </table>
            </div>

            {/* Stats */}
            {!isLoading && users.length > 0 && (
                <div className="stats shadow w-full">
                    <div className="stat">
                        <div className="stat-title">Total de Usuários</div>
                        <div className="stat-value text-primary">{users.length}</div>
                        <div className="stat-desc">
                            {localSearchTerm ? `Filtrado por "${localSearchTerm}"` : "Todos os usuários"}
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

