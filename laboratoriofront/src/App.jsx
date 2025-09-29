import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';

import CustomerList from './pages/CustomerList';
import CustomerForm from './pages/CustomerForm';
import ProductList from './pages/ProductList';
import ProductForm from './pages/ProductForm';
import OrderList from './pages/OrderList';
import OrderForm from './pages/OrderForm';
import OrderDetails from './pages/OrderDetails';
import CustomerOrders from './pages/CustomerOrders';

import Login from './pages/Login';
import MyOrders from './pages/MyOrders';

function App() {
    // Estado global de autenticación
    const [token, setToken] = useState(null);
    const [userId, setUserId] = useState(null);
    const [email, setEmail] = useState(null);

    const isAuthenticated = !!token;

    // Cargar datos guardados en localStorage cuando inicia la app
    useEffect(() => {
        const savedToken = localStorage.getItem('token');
        const savedUserId = localStorage.getItem('userId');
        const savedEmail = localStorage.getItem('email');

        if (savedToken && savedUserId && savedEmail) {
            setToken(savedToken);
            setUserId(savedUserId);
            setEmail(savedEmail);
        }
    }, []);

    // Manejar login exitoso
    const handleLogin = (jwt, customerId, customerEmail) => {
        setToken(jwt);
        setUserId(customerId);
        setEmail(customerEmail);

        localStorage.setItem('token', jwt);
        localStorage.setItem('userId', customerId);
        localStorage.setItem('email', customerEmail);
    };

    // Cerrar sesión
    const handleLogout = () => {
        setToken(null);
        setUserId(null);
        setEmail(null);

        localStorage.removeItem('token');
        localStorage.removeItem('userId');
        localStorage.removeItem('email');
    };

    return (
        <Router>
            <div className="min-h-screen bg-gray-100 p-4">
                <nav className="bg-white shadow-md p-4 mb-8 rounded-lg flex justify-between items-center">
                    <h1 className="text-2xl font-bold text-gray-800">Microservices App</h1>
                    <div className="space-x-4">
                        <Link to="/customers" className="text-blue-600 hover:text-blue-800 font-medium">Customers</Link>
                        <Link to="/products" className="text-blue-600 hover:text-blue-800 font-medium">Products</Link>
                        <Link to="/orders" className="text-blue-600 hover:text-blue-800 font-medium">Orders</Link>

                        {isAuthenticated && (
                            <Link to="/myorders" className="text-blue-600 hover:text-blue-800 font-medium">
                                My Orders
                            </Link>
                        )}

                        {!isAuthenticated ? (
                            <Link to="/login" className="text-green-600 hover:text-green-800 font-medium">Login</Link>
                        ) : (
                            <button
                                onClick={handleLogout}
                                className="text-red-600 hover:text-red-800 font-medium"
                            >
                                Logout ({email})
                            </button>
                        )}
                    </div>
                </nav>

                <Routes>
                    <Route path="/" element={<Navigate to="/customers" replace />} />

                    {/* Clientes */}
                    <Route path="/customers" element={<CustomerList />} />
                    <Route path="/customers/create" element={<CustomerForm />} />
                    <Route path="/customers/:id" element={<CustomerForm />} />
                    <Route path="/customers/:customerId/orders" element={<CustomerOrders />} />

                    {/* Productos */}
                    <Route path="/products" element={<ProductList />} />
                    <Route path="/products/create" element={<ProductForm />} />
                    <Route path="/products/:id" element={<ProductForm />} />

                    {/* Órdenes */}
                    <Route path="/orders" element={<OrderList />} />
                    <Route path="/orders/create" element={<OrderForm />} />
                    <Route path="/orders/:id" element={<OrderDetails />} />

                    {/* Login y mis órdenes */}
                    <Route
                        path="/login"
                        element={<Login isAuthenticated={isAuthenticated} onLogin={handleLogin} />}
                    />
                    <Route
                        path="/myorders"
                        element={
                            isAuthenticated ? (
                                <MyOrders
                                    token={token}
                                    userId={userId}
                                    email={email}
                                    isAuthenticated={isAuthenticated}
                                />
                            ) : (
                                <Navigate to="/login" replace />
                            )
                        }
                    />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
