import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
const CUSTOMER_API_URL = 'https://localhost:5002/api/Customer';
const PRODUCT_API_URL = 'https://localhost:5000/api/Product';
const ORDER_API_URL = 'https://localhost:5001/api/Order';

const OrderForm = () => {
    const navigate = useNavigate();
    const [customers, setCustomers] = useState([]);
    const [products, setProducts] = useState([]);
    const [selectedCustomer, setSelectedCustomer] = useState('');
    const [orderItems, setOrderItems] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [customersRes, productsRes] = await Promise.all([
                    fetch(CUSTOMER_API_URL),
                    fetch(PRODUCT_API_URL),
                ]);
                if (!customersRes.ok) throw new Error('Error al cargar clientes');
                if (!productsRes.ok) throw new Error('Error al cargar productos');

                setCustomers(await customersRes.json());
                setProducts(await productsRes.json());
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    const handleAddItem = (e) => {
        const productId = parseInt(e.target.value, 10);
        if (!productId) return;

        const product = products.find(p => p.id === productId);
        if (product && !orderItems.some(item => item.productId === product.id)) {
            setOrderItems(prev => [
                ...prev,
                { productId: product.id, productName: product.name, price: product.price, quantity: 1, stock: product.stockQuantity || product.stock || 0 }
            ]);
        }

        e.target.value = '';
    };

    const handleQuantityChange = (productId, quantity) => {
        let q = parseInt(quantity, 10);
        if (isNaN(q) || q < 1) q = 1;

        setOrderItems(prev =>
            prev.map(item =>
                item.productId === productId ? { ...item, quantity: Math.min(q, item.stock) } : item
            )
        );
    };

    const handleRemoveItem = (productId) => {
        setOrderItems(prev => prev.filter(item => item.productId !== productId));
    };

    const totalAmount = orderItems.reduce((sum, item) => sum + item.price * item.quantity, 0);

    const handleSubmit = async () => {
        setLoading(true);
        setError(null);

        if (!selectedCustomer) {
            setError('Debe seleccionar un cliente.');
            setLoading(false);
            return;
        }

        if (orderItems.length === 0) {
            setError('Debe agregar al menos un producto.');
            setLoading(false);
            return;
        }

        try {
            const response = await fetch(ORDER_API_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    customerId: parseInt(selectedCustomer, 10),
                    orderItems: orderItems.map(item => ({ productId: item.productId, quantity: item.quantity })),
                }),
            });

            if (!response.ok) {
                const text = await response.text();
                throw new Error(text || 'Error al crear la orden');
            }

            const createdOrder = await response.json();

            navigate(`/orders/${createdOrder.id}`);

        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <div className="p-4 text-center">Cargando...</div>;

    return (
        <div className="max-w-3xl mx-auto p-4 bg-white rounded shadow">
            <h1 className="text-2xl font-bold mb-4">Crear Nueva Orden</h1>

            {error && <div className="bg-red-100 text-red-700 p-2 mb-4 rounded">{error}</div>}

            <div className="mb-4">
                <label className="block mb-2">Cliente</label>
                <select
                    value={selectedCustomer}
                    onChange={e => setSelectedCustomer(e.target.value)}
                    className="border rounded p-2 w-full"
                >
                    <option value="">-- Seleccione un cliente --</option>
                    {customers.map(c => (
                        <option key={c.id} value={c.id}>{c.name} ({c.email})</option>
                    ))}
                </select>
            </div>

            <div className="mb-4">
                <label className="block mb-2">Agregar Producto</label>
                <select onChange={handleAddItem} className="border rounded p-2 w-full">
                    <option value="">-- Seleccione un producto --</option>
                    {products.filter(p => !orderItems.some(item => item.productId === p.id))
                        .map(p => (
                            <option key={p.id} value={p.id}>
                                {p.name} - ${p.price} ({p.stockQuantity || p.stock || 0} en stock)
                            </option>
                        ))}
                </select>
            </div>

            {orderItems.length > 0 && (
                <div className="mb-4">
                    <table className="w-full border">
                        <thead>
                            <tr>
                                <th className="border px-2 py-1">Producto</th>
                                <th className="border px-2 py-1">Precio</th>
                                <th className="border px-2 py-1">Cantidad</th>
                                <th className="border px-2 py-1">Stock</th>
                                <th className="border px-2 py-1">Subtotal</th>
                                <th className="border px-2 py-1">Acción</th>
                            </tr>
                        </thead>
                        <tbody>
                            {orderItems.map(item => (
                                <tr key={item.productId}>
                                    <td className="border px-2 py-1">{item.productName}</td>
                                    <td className="border px-2 py-1">${item.price.toFixed(2)}</td>
                                    <td className="border px-2 py-1">
                                        <input
                                            type="number"
                                            value={item.quantity}
                                            onChange={e => handleQuantityChange(item.productId, e.target.value)}
                                            min="1"
                                            max={item.stock}
                                            className="w-16 border rounded px-1"
                                        />
                                    </td>
                                    <td className="border px-2 py-1">{item.stock}</td>
                                    <td className="border px-2 py-1">${(item.price * item.quantity).toFixed(2)}</td>
                                    <td className="border px-2 py-1">
                                        <button onClick={() => handleRemoveItem(item.productId)} className="text-red-600">Eliminar</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <div className="text-right mt-2 font-bold">Total: ${totalAmount.toFixed(2)}</div>
                </div>
            )}

            <button
                onClick={handleSubmit}
                disabled={loading || orderItems.length === 0 || !selectedCustomer || orderItems.some(item => item.quantity > item.stock)}
                className="bg-indigo-600 text-white px-4 py-2 rounded disabled:bg-indigo-300"
            >
                {loading ? 'Creando...' : 'Crear Orden'}
            </button>
        </div>
    );
};

export default OrderForm;
