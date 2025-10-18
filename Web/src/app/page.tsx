"use client";

import { useEffect } from "react";
import { useAuthStore } from "@/stores/authStore";
import { LoginForm } from "@/components/LoginForm";
import { UserDashboard } from "@/components/UserDashboard";

export default function Home() {
  const { isAuthenticated, isLoading, checkAuth } = useAuthStore();

  useEffect(() => {
    checkAuth();
  }, [checkAuth]);

  if (isLoading) {
    return (
      <div data-theme="synthwave" className="min-h-screen flex items-center justify-center">
        <span className="loading loading-spinner loading-lg"></span>
      </div>
    );
  }

  return (
    <div data-theme="synthwave" className="min-h-screen">
      {isAuthenticated ? <UserDashboard /> : <LoginForm />}
    </div>
  );
}
