"use client";

import { useState } from "react";
import { useAuthStore } from "@/stores/authStore";
import { api } from "@/lib/api";

export function LoginForm() {
    const [isRegisterMode, setIsRegisterMode] = useState(false);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [nome, setNome] = useState("");
    const [cpf, setCpf] = useState("");
    const [dataNascimento, setDataNascimento] = useState("");
    const [nacionalidade, setNacionalidade] = useState("Brasileiro");
    const [naturalidade, setNaturalidade] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const login = useAuthStore((state) => state.login);

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setSuccess("");
        setIsLoading(true);

        try {
            await login(email, password);
        } catch (err) {
            setError(err instanceof Error ? err.message : "Falha ao fazer login. Verifique suas credenciais.");
        } finally {
            setIsLoading(false);
        }
    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setSuccess("");

        if (password !== confirmPassword) {
            setError("As senhas não coincidem");
            return;
        }

        if (password.length < 8) {
            setError("A senha deve ter no mínimo 8 caracteres");
            return;
        }

        setIsLoading(true);

        try {
            await api.register({
                nomeCompleto: nome,
                email: email,
                password: password,
                cpf: cpf,
                dataNascimento: new Date(dataNascimento),
                nacionalidade: nacionalidade,
                naturalidade: naturalidade,
            });
            setSuccess("Conta criada com sucesso! Faça login para continuar.");
            setIsRegisterMode(false);
            setNome("");
            setEmail("");
            setPassword("");
            setConfirmPassword("");
            setCpf("");
            setDataNascimento("");
            setNacionalidade("Brasileiro");
            setNaturalidade("");
        } catch (err) {
            setError(err instanceof Error ? err.message : "Falha ao criar conta.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center p-4">
            <div className="card w-full max-w-md bg-base-100 shadow-2xl">
                <div className="card-body">
                    <h2 className="card-title text-3xl font-bold text-center justify-center mb-2">
                        Sistema de Usuários
                    </h2>
                    <p className="text-center text-sm opacity-70 mb-4">
                        {isRegisterMode ? "Crie sua nova conta" : "Entre com suas credenciais"}
                    </p>

                    {/* Mensagem de sucesso */}
                    {success && (
                        <div className="alert alert-success">
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
                                    d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                                />
                            </svg>
                            <span>{success}</span>
                        </div>
                    )}

                    {/* Formulário de Login */}
                    {!isRegisterMode && (
                        <form onSubmit={handleLogin} className="space-y-4">
                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Email</span>
                                </label>
                                <input
                                    type="email"
                                    placeholder="seu@email.com"
                                    className="input input-bordered w-full"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Senha</span>
                                </label>
                                <input
                                    type="password"
                                    placeholder="••••••••"
                                    className="input input-bordered w-full"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

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

                            <div className="form-control mt-6">
                                <button
                                    type="submit"
                                    className="btn btn-primary w-full"
                                    disabled={isLoading}
                                >
                                    {isLoading ? (
                                        <>
                                            <span className="loading loading-spinner"></span>
                                            Entrando...
                                        </>
                                    ) : (
                                        "Entrar"
                                    )}
                                </button>
                            </div>
                        </form>
                    )}

                    {/* Formulário de Registro */}
                    {isRegisterMode && (
                        <form onSubmit={handleRegister} className="space-y-3 max-h-[70vh] overflow-y-auto pr-2">
                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Nome Completo</span>
                                </label>
                                <input
                                    type="text"
                                    placeholder="Seu nome completo"
                                    className="input input-bordered input-sm w-full"
                                    value={nome}
                                    onChange={(e) => setNome(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Email</span>
                                </label>
                                <input
                                    type="email"
                                    placeholder="seu@email.com"
                                    className="input input-bordered input-sm w-full"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">CPF</span>
                                </label>
                                <input
                                    type="text"
                                    placeholder="000.000.000-00"
                                    className="input input-bordered input-sm w-full"
                                    value={cpf}
                                    onChange={(e) => setCpf(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Data de Nascimento</span>
                                </label>
                                <input
                                    type="date"
                                    className="input input-bordered input-sm w-full"
                                    value={dataNascimento}
                                    onChange={(e) => setDataNascimento(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Nacionalidade</span>
                                </label>
                                <input
                                    type="text"
                                    placeholder="Ex: Brasileiro"
                                    className="input input-bordered input-sm w-full"
                                    value={nacionalidade}
                                    onChange={(e) => setNacionalidade(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Naturalidade</span>
                                </label>
                                <input
                                    type="text"
                                    placeholder="Ex: São Paulo/SP"
                                    className="input input-bordered input-sm w-full"
                                    value={naturalidade}
                                    onChange={(e) => setNaturalidade(e.target.value)}
                                    required
                                    disabled={isLoading}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Senha</span>
                                </label>
                                <input
                                    type="password"
                                    placeholder="Mínimo 8 caracteres"
                                    className="input input-bordered input-sm w-full"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    disabled={isLoading}
                                    minLength={8}
                                />
                            </div>

                            <div className="form-control">
                                <label className="label">
                                    <span className="label-text">Confirmar Senha</span>
                                </label>
                                <input
                                    type="password"
                                    placeholder="Digite a senha novamente"
                                    className="input input-bordered input-sm w-full"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    required
                                    disabled={isLoading}
                                    minLength={8}
                                />
                            </div>

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

                            <div className="form-control mt-6">
                                <button
                                    type="submit"
                                    className="btn btn-primary w-full"
                                    disabled={isLoading}
                                >
                                    {isLoading ? (
                                        <>
                                            <span className="loading loading-spinner"></span>
                                            Criando conta...
                                        </>
                                    ) : (
                                        "Criar Conta"
                                    )}
                                </button>
                            </div>
                        </form>
                    )}

                    {/* Toggle entre Login e Registro */}
                    <div className="divider">OU</div>
                    <button
                        type="button"
                        className="btn btn-ghost btn-sm w-full"
                        onClick={() => {
                            setIsRegisterMode(!isRegisterMode);
                            setError("");
                            setSuccess("");
                        }}
                        disabled={isLoading}
                    >
                        {isRegisterMode ? "Já tem uma conta? Faça login" : "Não tem conta? Crie uma agora"}
                    </button>
                </div>
            </div>
        </div>
    );
}

