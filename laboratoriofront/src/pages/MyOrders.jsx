import React, { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";

const ORDER_API_URL = "https://localhost:5001/api";

const MyOrders = ({ token, userId, isAuthenticated, email }) => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate("/login");
      return;
    }

    const fetchOrders = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await fetch(
          `${ORDER_API_URL}/order/customer/${userId}`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (!response.ok) {
          if (response.status === 401 || response.status === 403) {
            throw new Error(
              "Su sesión ha expirado o no tiene permisos. Por favor, inicie sesión de nuevo."
            );
          }
          throw new Error(`Error al cargar órdenes (Estado: ${response.status})`);
        }

        const data = await response.json();
        setOrders(Array.isArray(data) ? data : []);
      } catch (err) {
        console.error("Fetch Orders Error:", err);
        setError(err.message || "No se pudieron cargar sus órdenes.");
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, [token, userId, isAuthenticated, navigate]);

  return (
    <div className="bg-white p-8 rounded-xl shadow-2xl border border-green-100 max-w-4xl mx-auto mt-10">
      <h2 className="text-3xl font-bold text-green-700 mb-6 flex items-center">
        Mis Órdenes
        <span className="ml-3 text-lg font-normal text-gray-500">
          ({email || "No disponible"})
        </span>
      </h2>

      {loading && (
        <div className="flex justify-center items-center py-10 text-blue-500">
          Cargando sus órdenes...
        </div>
      )}

      {error && (
        <div className="p-4 rounded-lg bg-red-100 border border-red-400 text-red-700 text-sm">
          {error}
        </div>
      )}

      {!loading && !error && orders.length === 0 && (
        <div className="p-4 text-center text-gray-500 bg-yellow-50 rounded-lg">
          No tienes órdenes registradas.
        </div>
      )}

      {!loading && orders.length > 0 && (
        <div className="space-y-4">
          {orders.map((order, index) => (
            <div
              key={order.id || index}
              className="p-4 border border-gray-200 rounded-lg hover:bg-gray-50 transition duration-150"
            >
              <div className="flex justify-between items-center font-medium">
                <span className="text-lg text-gray-800">
                  Pedido #{order.id || "N/A"}
                </span>
                <span
                  className={`px-3 py-1 text-xs font-semibold rounded-full ${
                    order.status === "Completed"
                      ? "bg-green-100 text-green-800"
                      : "bg-yellow-100 text-yellow-800"
                  }`}
                >
                  {order.status || "Pendiente"}
                </span>
              </div>
              <p className="text-gray-600 mt-1">
                Fecha:{" "}
                {new Date(order.orderDate || Date.now()).toLocaleDateString()}
              </p>
              <p className="text-xl font-bold text-blue-600 mt-2">
                Total: ${order.totalAmount ? order.totalAmount.toFixed(2) : "0.00"}
              </p>
              <Link
                to={`/orders/${order.id}`}
                className="text-sm text-blue-500 hover:text-blue-700 mt-1 block"
              >
                Ver Detalles →
              </Link>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MyOrders;
