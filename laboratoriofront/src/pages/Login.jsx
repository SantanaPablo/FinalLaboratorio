import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const CUSTOMER_API_URL = "https://localhost:5002/api";

const Login = ({ isAuthenticated, onLogin }) => {
  const [email, setEmail] = useState("ejemplo@test.com");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/myorders");
    }
  }, [isAuthenticated, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await fetch(`${CUSTOMER_API_URL}/Auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.Error || "Fallo en la autenticación");
      }

      const data = await response.json();

      onLogin(data.token, data.customerId, data.email);

      navigate("/myorders");
    } catch (err) {
      console.error("Login Error:", err);
      setError(err.message || "Error de conexión con el servidor.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-md mx-auto bg-white p-8 rounded-xl shadow-2xl border border-blue-100 mt-10">
      <h2 className="text-3xl font-bold text-center text-blue-700 mb-6">
        Iniciar Sesión
      </h2>
      <p className="text-center text-sm text-gray-500 mb-6">
        Ingresa tu email para generar un JWT y acceder a tus pedidos.
      </p>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div>
          <label
            htmlFor="email"
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            Email del Cliente
          </label>
          <input
            id="email"
            type="email"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 transition duration-150"
            placeholder="ejemplo@test.com"
            disabled={loading}
          />
        </div>

        <button
          type="submit"
          className={`w-full flex justify-center items-center py-2 px-4 border border-transparent rounded-lg shadow-sm text-lg font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition duration-200 ${
            loading ? "opacity-70 cursor-not-allowed" : ""
          }`}
          disabled={loading}
        >
          {loading ? "Autenticando..." : "Acceder"}
        </button>
      </form>

      {error && (
        <div className="mt-4 p-3 rounded-lg bg-red-100 border border-red-400 text-red-700 text-sm">
          Error: {error}
        </div>
      )}
    </div>
  );
};

export default Login;
