"use client";

import { useState, useEffect } from "react";
import type { UserResponseDto, UserCreateDto, UserUpdateDto, KnownSexoUsuario } from "@/api/src/models";

interface UserFormProps {
    user?: UserResponseDto | null;
    onSubmit: (user: UserCreateDto | UserUpdateDto) => Promise<void>;
    onCancel: () => void;
    isLoading?: boolean;
}

export function UserForm({ user, onSubmit, onCancel, isLoading }: UserFormProps) {
    const [formData, setFormData] = useState({
        nomeCompleto: "",
        email: "",
        cpf: "",
        dataNascimento: "",
        sexo: "Masculino" as string,
        nacionalidade: "",
        naturalidade: "",
        password: "",
    });

    const [errors, setErrors] = useState<string[]>([]);

    useEffect(() => {
        if (user) {
            setFormData({
                nomeCompleto: user.nomeCompleto || "",
                email: user.email || "",
                cpf: user.cpf || "",
                dataNascimento: user.dataNascimento
                    ? new Date(user.dataNascimento).toISOString().split("T")[0]
                    : "",
                sexo: user.sexo || "Masculino",
                nacionalidade: user.nacionalidade || "",
                naturalidade: user.naturalidade || "",
                password: "",
            });
        }
    }, [user]);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrors([]);

        try {
            const userData: any = {
                nomeCompleto: formData.nomeCompleto,
                email: formData.email,
                cpf: formData.cpf,
                dataNascimento: new Date(formData.dataNascimento),
                sexo: formData.sexo,
                nacionalidade: formData.nacionalidade,
                naturalidade: formData.naturalidade,
            };

            // Add password only for creation
            if (!user && formData.password) {
                userData.password = formData.password;
            }

            await onSubmit(userData);
        } catch (err: any) {
            const errorMessages = err.response?.data?.errors
                ? Object.values(err.response.data.errors).flat()
                : [err.message || "Erro ao salvar usuário"];
            setErrors(errorMessages as string[]);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            {errors.length > 0 && (
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
                    <div>
                        {errors.map((error, idx) => (
                            <p key={idx}>{error}</p>
                        ))}
                    </div>
                </div>
            )}

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="form-control md:col-span-2">
                    <label className="label">
                        <span className="label-text">Nome Completo *</span>
                    </label>
                    <input
                        type="text"
                        name="nomeCompleto"
                        placeholder="Nome completo"
                        className="input input-bordered w-full"
                        value={formData.nomeCompleto}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                    />
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">Email *</span>
                    </label>
                    <input
                        type="email"
                        name="email"
                        placeholder="email@exemplo.com"
                        className="input input-bordered w-full"
                        value={formData.email}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                    />
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">CPF *</span>
                    </label>
                    <input
                        type="text"
                        name="cpf"
                        placeholder="000.000.000-00"
                        className="input input-bordered w-full"
                        value={formData.cpf}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                        maxLength={14}
                    />
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">Data de Nascimento *</span>
                    </label>
                    <input
                        type="date"
                        name="dataNascimento"
                        className="input input-bordered w-full"
                        value={formData.dataNascimento}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                    />
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">Sexo</span>
                    </label>
                    <select
                        name="sexo"
                        className="select select-bordered w-full"
                        value={formData.sexo}
                        onChange={handleChange}
                        disabled={isLoading}
                    >
                        <option value="Masculino">Masculino</option>
                        <option value="Feminino">Feminino</option>
                        <option value="Outro">Outro</option>
                    </select>
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">Nacionalidade *</span>
                    </label>
                    <input
                        type="text"
                        name="nacionalidade"
                        placeholder="Brasileiro(a)"
                        className="input input-bordered w-full"
                        value={formData.nacionalidade}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                    />
                </div>

                <div className="form-control">
                    <label className="label">
                        <span className="label-text">Naturalidade *</span>
                    </label>
                    <input
                        type="text"
                        name="naturalidade"
                        placeholder="São Paulo"
                        className="input input-bordered w-full"
                        value={formData.naturalidade}
                        onChange={handleChange}
                        required
                        disabled={isLoading}
                    />
                </div>

                {!user && (
                    <div className="form-control md:col-span-2">
                        <label className="label">
                            <span className="label-text">Senha *</span>
                        </label>
                        <input
                            type="password"
                            name="password"
                            placeholder="Senha forte"
                            className="input input-bordered w-full"
                            value={formData.password}
                            onChange={handleChange}
                            required={!user}
                            disabled={isLoading}
                            minLength={8}
                        />
                        <label className="label">
                            <span className="label-text-alt">
                                Mínimo 8 caracteres, incluindo maiúsculas, minúsculas, números e
                                caracteres especiais
                            </span>
                        </label>
                    </div>
                )}
            </div>

            <div className="flex gap-2 justify-end mt-6">
                <button
                    type="button"
                    className="btn btn-ghost"
                    onClick={onCancel}
                    disabled={isLoading}
                >
                    Cancelar
                </button>
                <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={isLoading}
                >
                    {isLoading ? (
                        <>
                            <span className="loading loading-spinner"></span>
                            Salvando...
                        </>
                    ) : user ? (
                        "Atualizar"
                    ) : (
                        "Cadastrar"
                    )}
                </button>
            </div>
        </form>
    );
}

