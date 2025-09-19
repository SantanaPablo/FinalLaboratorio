import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const API_URL = 'https://localhost:5001/api/Order';

const OrderList = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await fetch(API_URL);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setOrders(data);
            } catch (error) {
                setError(error);
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, []);

    if (loading) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-gray-700">Cargando...</div></div>;
    if (error) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-red-500">Error: {error.message}</div></div>;

    return (
        <div className="container mx-auto p-4 md:p-8 bg-gray-100 min-h-screen">
            <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8">
                <h1 className="text-4xl font-extrabold text-gray-800 mb-4 md:mb-0">Órdenes</h1>
                <Link
                    to="/orders/create"
                    className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 px-6 rounded-full shadow-lg transition duration-300 ease-in-out transform hover:scale-105"
                >
                    Crear Nueva Orden
                </Link>
            </div>
            <div className="bg-white rounded-xl shadow-2xl overflow-hidden">
                <table className="min-w-full leading-normal">
                    <thead>
                        <tr>
                           
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Fecha
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Cliente
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Total
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Acciones
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {orders.map((order) => (
                            <tr key={order.id} className="hover:bg-gray-50 transition duration-200">
                                
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{new Date(order.orderDate).toLocaleDateString()}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{order.customerName}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">${order.totalAmount.toFixed(2)}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <Link
                                        to={`/orders/${order.id}`}
                                        className="text-indigo-600 hover:text-indigo-900 font-medium"
                                    >
                                        Ver Detalles
                                    </Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default OrderList;
