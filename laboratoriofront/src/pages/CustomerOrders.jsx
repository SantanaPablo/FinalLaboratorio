import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';

// Use the correct API URL as shown in your cURL request
const API_URL = 'https://localhost:5001/api/Order/customer/';

const CustomerOrders = () => {
    const { customerId } = useParams();
    const [customerOrders, setCustomerOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchCustomerOrders = async () => {
            try {
                // Construct the full URL using the customerId from the URL params
                const response = await fetch(`${API_URL}${customerId}`);
                if (!response.ok) {
                    throw new Error(`Error: ${response.status} ${response.statusText}`);
                }
                const data = await response.json();
                
                // The cURL response body shows the orders as a direct array, not wrapped in a 'data' object.
                setCustomerOrders(data);

            } catch (error) {
                setError(error);
            } finally {
                setLoading(false);
            }
        };

        fetchCustomerOrders();
    }, [customerId]); // The useEffect dependency array ensures the fetch runs when customerId changes

    if (loading) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-gray-700">Cargando órdenes...</div></div>;
    if (error) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-red-500">Error: {error.message}</div></div>;

    return (
        <div className="container mx-auto p-4 md:p-8 bg-gray-100 min-h-screen">
            <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8">
                <h1 className="text-4xl font-extrabold text-gray-800 mb-4 md:mb-0">
                    Órdenes del Cliente #{customerId}
                </h1>
                <Link
                    to="/customers"
                    className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 px-6 rounded-full shadow-lg transition duration-300 ease-in-out transform hover:scale-105"
                >
                    Volver a Clientes
                </Link>
            </div>

            {customerOrders.length > 0 ? (
                <div className="space-y-8">
                    {customerOrders.map((order) => (
                        <div key={order.id} className="bg-white rounded-xl shadow-2xl overflow-hidden p-6">
                            <h2 className="text-2xl font-bold text-gray-800 mb-4">
                                Orden #{order.id} - Fecha: {new Date(order.orderDate).toLocaleDateString()}
                            </h2>
                            <p className="text-gray-600 mb-2">Total de la Orden: ${order.totalAmount.toFixed(2)}</p>
                            <p className="text-gray-600 mb-4">Cliente: {order.customerName}</p>

                            <h3 className="text-xl font-semibold text-gray-700 mb-2">Productos en la Orden:</h3>
                            <div className="overflow-x-auto">
                                <table className="min-w-full leading-normal">
                                    <thead>
                                        <tr>
                                            <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                                Producto
                                            </th>
                                            <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                                Cantidad
                                            </th>
                                            <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                                Precio Unitario
                                            </th>
                                            <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                                Subtotal
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {order.orderItems.map((item) => (
                                            <tr key={item.id} className="hover:bg-gray-50 transition duration-200">
                                                <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">
                                                    <div className="text-gray-900 whitespace-no-wrap">{item.productName}</div>
                                                </td>
                                                <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">
                                                    <div className="text-gray-900 whitespace-no-wrap">{item.quantity}</div>
                                                </td>
                                                <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">
                                                    <div className="text-gray-900 whitespace-no-wrap">${item.unitPrice.toFixed(2)}</div>
                                                </td>
                                                <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">
                                                    <div className="text-gray-900 whitespace-no-wrap">${item.subtotal.toFixed(2)}</div>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div className="flex items-center justify-center min-h-[40vh] bg-white rounded-xl shadow-2xl">
                    <div className="text-xl text-gray-700">No se encontraron órdenes para este cliente.</div>
                </div>
            )}
        </div>
    );
};

export default CustomerOrders;