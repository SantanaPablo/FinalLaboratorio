import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import CustomerList from './pages/CustomerList';
import CustomerForm from './pages/CustomerForm';

function App() {
    return (
        <Router>
            <div className="min-h-screen bg-gray-100 p-4">
                <nav className="bg-white shadow-md p-4 mb-8 rounded-lg flex justify-between items-center">
                    <h1 className="text-2xl font-bold text-gray-800">Microservices App</h1>
                    <div className="space-x-4">
                        <Link to="/customers" className="text-blue-600 hover:text-blue-800 font-medium">Customers</Link>
                        {/* Add links for Products and Orders here once you create their components */}
                    </div>
                </nav>
                <Routes>
                    <Route path="/customers" element={<CustomerList />} />
                    <Route path="/customers/create" element={<CustomerForm />} />
                    <Route path="/customers/:id" element={<CustomerForm />} />
                    {/* Add routes for Products and Orders here */}
                </Routes>
            </div>
        </Router>
    );
}

export default App;