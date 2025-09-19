import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import CustomerList from './pages/CustomerList';
import CustomerForm from './pages/CustomerForm';
import ProductList from './pages/ProductList';
import ProductForm from './pages/ProductForm';
import OrderList from './pages/OrderList';
import OrderForm from './pages/OrderForm';
import OrderDetails from './pages/OrderDetails';

function App() {
    return (
        <Router>
            <div className="min-h-screen bg-gray-100 p-4">
                <nav className="bg-white shadow-md p-4 mb-8 rounded-lg flex justify-between items-center">
                    <h1 className="text-2xl font-bold text-gray-800">Microservices App</h1>
                    <div className="space-x-4">
                        <Link to="/customers" className="text-blue-600 hover:text-blue-800 font-medium">Customers</Link>
                        <Link to="/products" className="text-blue-600 hover:text-blue-800 font-medium">Products</Link>
                        <Link to="/orders" className="text-blue-600 hover:text-blue-800 font-medium">Orders</Link>
                    </div>
                </nav>
                <Routes>
                    <Route path="/customers" element={<CustomerList />} />
                    <Route path="/customers/create" element={<CustomerForm />} />
                    <Route path="/customers/:id" element={<CustomerForm />} />
                    <Route path="/products" element={<ProductList />} />
                    <Route path="/products/create" element={<ProductForm />} />
                    <Route path="/products/:id" element={<ProductForm />} />
                    <Route path="/orders" element={<OrderList />} />
                    <Route path="/orders/create" element={<OrderForm />} />
                    <Route path="/orders" element={<OrderList />} />
                    <Route path="/orders/create" element={<OrderForm />} />
                    <Route path="/orders/:id" element={<OrderDetails />} />
                </Routes>

            </div>
        </Router>
    );
}

export default App;
