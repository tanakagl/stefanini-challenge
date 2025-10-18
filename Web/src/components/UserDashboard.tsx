"use client";

import { useState } from "react";
import { useAuthStore } from "@/stores/authStore";
import { useUserStore } from "@/stores/userStore";
import { UserList } from "./UserList";
import { UserForm } from "./UserForm";
import { DeleteConfirmModal } from "./DeleteConfirmModal";
import type { UserResponseDto, UserCreateDto, UserUpdateDto } from "@/api/src/models";

type ModalMode = "create" | "edit" | null;

export function UserDashboard() {
    const user = useAuthStore((state) => state.user);
    const logout = useAuthStore((state) => state.logout);
    const { createUser, updateUser, deleteUser: deleteUserFromStore } = useUserStore();

    const [modalMode, setModalMode] = useState<ModalMode>(null);
    const [selectedUser, setSelectedUser] = useState<UserResponseDto | null>(null);
    const [deleteUser, setDeleteUser] = useState<UserResponseDto | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);
    const [notification, setNotification] = useState<{
        type: "success" | "error";
        message: string;
    } | null>(null);

    const showNotification = (type: "success" | "error", message: string) => {
        setNotification({ type, message });
        setTimeout(() => setNotification(null), 5000);
    };

    const handleCreateUser = async (userData: UserCreateDto | UserUpdateDto) => {
        setIsSubmitting(true);
        try {
            await createUser(userData as UserCreateDto);
            showNotification("success", "Usuário criado com sucesso!");
            setModalMode(null);
        } catch (error: any) {
            showNotification("error", error.message || "Erro ao criar usuário");
            throw error;
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleUpdateUser = async (userData: UserCreateDto | UserUpdateDto) => {
        if (!selectedUser?.id) return;
        setIsSubmitting(true);
        try {
            await updateUser(selectedUser.id, userData as UserUpdateDto);
            showNotification("success", "Usuário atualizado com sucesso!");
            setModalMode(null);
            setSelectedUser(null);
        } catch (error: any) {
            showNotification("error", error.message || "Erro ao atualizar usuário");
            throw error;
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleDeleteUser = async () => {
        if (!deleteUser?.id) return;
        setIsDeleting(true);
        try {
            await deleteUserFromStore(deleteUser.id);
            showNotification("success", "Usuário excluído com sucesso!");
            setDeleteUser(null);
        } catch (error: any) {
            showNotification("error", error.message || "Erro ao excluir usuário");
        } finally {
            setIsDeleting(false);
        }
    };

    const handleEdit = (user: UserResponseDto) => {
        setSelectedUser(user);
        setModalMode("edit");
    };

    const handleDelete = (user: UserResponseDto) => {
        setDeleteUser(user);
    };

    const handleCloseModal = () => {
        setModalMode(null);
        setSelectedUser(null);
    };

    return (
        <div className="min-h-screen p-4 md:p-8">
            {/* Header */}
            <div className="max-w-7xl mx-auto mb-8">
                <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 mb-6">
                    <div>
                        <h1 className="text-4xl font-bold mb-2"> Sistema de Usuários</h1>
                        <p className="text-lg opacity-70">
                            Bem-vindo, <span className="font-semibold">{user?.nomeCompleto}</span>!
                        </p>
                    </div>
                    <button className="btn btn-ghost" onClick={logout}>
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
                                d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"
                            />
                        </svg>
                        Sair
                    </button>
                </div>

                {/* Notification */}
                {notification && (
                    <div className={`alert ${notification.type === "success" ? "alert-success" : "alert-error"} mb-4`}>
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="stroke-current shrink-0 h-6 w-6"
                            fill="none"
                            viewBox="0 0 24 24"
                        >
                            {notification.type === "success" ? (
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth="2"
                                    d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                                />
                            ) : (
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth="2"
                                    d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
                                />
                            )}
                        </svg>
                        <span>{notification.message}</span>
                    </div>
                )}

                {/* Action Button */}
                <button
                    className="btn btn-primary btn-lg"
                    onClick={() => setModalMode("create")}
                >
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="h-6 w-6"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M12 4v16m8-8H4"
                        />
                    </svg>
                    Novo Usuário
                </button>
            </div>

            {/* User List */}
            <div className="max-w-7xl mx-auto">
                <div className="card bg-base-100 shadow-xl">
                    <div className="card-body">
                        <h2 className="card-title text-2xl mb-4">Usuários Cadastrados</h2>
                        <UserList
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                        />
                    </div>
                </div>
            </div>

            {/* Create/Edit Modal */}
            {modalMode && (
                <div className="modal modal-open">
                    <div className="modal-box max-w-3xl">
                        <h3 className="font-bold text-2xl mb-4">
                            {modalMode === "create" ? "Novo Usuário" : "Editar Usuário"}
                        </h3>
                        <UserForm
                            user={modalMode === "edit" ? selectedUser : null}
                            onSubmit={modalMode === "create" ? handleCreateUser : handleUpdateUser}
                            onCancel={handleCloseModal}
                            isLoading={isSubmitting}
                        />
                    </div>
                    <div className="modal-backdrop" onClick={handleCloseModal}></div>
                </div>
            )}

            {/* Delete Confirmation Modal */}
            <DeleteConfirmModal
                user={deleteUser}
                isOpen={!!deleteUser}
                onConfirm={handleDeleteUser}
                onCancel={() => setDeleteUser(null)}
                isDeleting={isDeleting}
            />
        </div>
    );
}

