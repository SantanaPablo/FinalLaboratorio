import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const ORDER_API_URL = 'https://localhost:5001/api/Order';
const CUSTOMER_API_URL = 'https://localhost:5002/api/Customer';
const PRODUCT_API_URL = 'https://localhost:5000/api/Product';

const OrderDetails = () => {
    const { id } = useParams(); // ID de la orden
    const navigate = useNavigate();
    const [order, setOrder] = useState(null);
    const [customers, setCustomers] = useState([]);
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);

                const [orderRes, customersRes, productsRes] = await Promise.all([
                    fetch(`${ORDER_API_URL}/${id}`),
                    fetch(CUSTOMER_API_URL),
                    fetch(PRODUCT_API_URL)
                ]);

                if (!orderRes.ok) throw new Error('Error al cargar la orden');
                if (!customersRes.ok) throw new Error('Error al cargar clientes');
                if (!productsRes.ok) throw new Error('Error al cargar productos');

                const [orderData, customersData, productsData] = await Promise.all([
                    orderRes.json(),
                    customersRes.json(),
                    productsRes.json()
                ]);

                setOrder(orderData);
                setCustomers(customersData);
                setProducts(productsData);
            } catch (err) {
                setError(err.message || 'Error al cargar los datos.');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [id]);

    const getCustomerName = (customerId) => {
        const customer = customers.find(c => c.id === customerId);
        return customer ? customer.name : 'Cliente desconocido';
    };

    const getProductName = (productId) => {
        const product = products.find(p => p.id === productId);
        return product ? product.name : 'Producto desconocido';
    };

    if (loading) return <div className="text-center p-8">Cargando orden...</div>;
    if (error) return <div className="text-center text-red-600 p-8">Error: {error}</div>;
    if (!order) return <div className="text-center p-8">Orden no encontrada.</div>;

    return (
        <div className="min-h-screen bg-gray-100 p-4 md:p-8">
            <div className="max-w-4xl mx-auto bg-white rounded-xl shadow-2xl p-8">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-3xl font-extrabold text-gray-800">
                        Detalles de la Orden #{order.id}
                    </h1>
                    <button
                        onClick={() => navigate('/orders')}
                        className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg"
                    >
                        Volver a �rdenes
                    </button>
                </div>

                <div className="mb-6">
                    <h2 className="text-xl font-semibold mb-2">Informaci�n de la Orden</h2>
                    <p><strong>Cliente:</strong> {getCustomerName(order.customerId)}</p>
                    <p><strong>Fecha:</strong> {new Date(order.orderDate).toLocaleDateString()}</p>
                    <p><strong>Total:</strong> ${order.totalAmount ? order.totalAmount.toFixed(2) : '0.00'}</p>
                </div>

                <div>
                    <h2 className="text-xl font-semibold mb-4">Productos</h2>
                    <div className="overflow-x-auto">
                        <table className="min-w-full leading-normal">
                            <thead>
                                <tr>
                                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Producto</th>
                                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Cantidad</th>
                                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Precio Unitario</th>
                                    <th className="px-5 py-3 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                {order.orderItems.map((item, index) => (
                                    <tr key={index}>
                                        <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">
                                            {getProductName(item.productId)}
                                        </td>
                                        <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">{item.quantity}</td>
                                        <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">${item.unitPrice ? item.unitPrice.toFixed(2) : '0.00'}</td>
                                        <td className="px-5 py-5 border-b border-gray-200 bg-white text-sm">${(item.quantity * (item.unitPrice || 0)).toFixed(2)}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default OrderDetails;
