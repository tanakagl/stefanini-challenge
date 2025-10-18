"use client";

import type { UserResponseDto } from "@/api/src/models";

interface DeleteConfirmModalProps {
    user: UserResponseDto | null;
    isOpen: boolean;
    onConfirm: () => void;
    onCancel: () => void;
    isDeleting?: boolean;
}

export function DeleteConfirmModal({
    user,
    isOpen,
    onConfirm,
    onCancel,
    isDeleting,
}: DeleteConfirmModalProps) {
    if (!isOpen || !user) return null;

    return (
        <div className="modal modal-open">
            <div className="modal-box">
                <h3 className="font-bold text-lg">Confirmar Exclusão</h3>
                <p className="py-4">
                    Tem certeza que deseja excluir o usuário{" "}
                    <span className="font-bold text-error">{user.nomeCompleto}</span>?
                </p>
                <p className="text-sm opacity-70">
                    Esta ação não pode ser desfeita.
                </p>
                <div className="modal-action">
                    <button
                        className="btn btn-ghost"
                        onClick={onCancel}
                        disabled={isDeleting}
                    >
                        Cancelar
                    </button>
                    <button
                        className="btn btn-error"
                        onClick={onConfirm}
                        disabled={isDeleting}
                    >
                        {isDeleting ? (
                            <>
                                <span className="loading loading-spinner"></span>
                                Excluindo...
                            </>
                        ) : (
                            "Excluir"
                        )}
                    </button>
                </div>
            </div>
            <div className="modal-backdrop" onClick={onCancel}></div>
        </div>
    );
}

