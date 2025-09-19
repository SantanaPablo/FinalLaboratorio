import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const API_URL = 'https://localhost:5002/api/Customer';

const CustomerForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [customer, setCustomer] = useState({ name: '', email: '', address: '' });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (id) {
            setLoading(true);
            fetch(`${API_URL}/${id}`)
                .then(res => res.json())
                .then(data => {
                    setCustomer(data);
                    setLoading(false);
                })
                .catch(err => {
                    setError(err);
                    setLoading(false);
                });
        }
    }, [id]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCustomer(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        const method = id ? 'PUT' : 'POST';
        const url = id ? `${API_URL}/${id}` : API_URL;

        try {
            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(customer),
            });

            if (!response.ok) {
                throw new Error('Failed to save customer');
            }

            navigate('/customers');
        } catch (err) {
            setError(err);
            setLoading(false);
        }
    };

    if (loading) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-gray-700">Cargando...</div></div>;
    if (error) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-red-500">Error: {error.message}</div></div>;

    return (
        <div className="container mx-auto p-4 md:p-8 max-w-2xl bg-gray-100 min-h-screen flex items-center justify-center">
            <div className="bg-white rounded-xl shadow-2xl p-8 w-full">
                <h1 className="text-3xl font-extrabold text-gray-800 mb-6 text-center">{id ? 'Editar Cliente' : 'Crear Cliente'}</h1>
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="name">
                            Nombre
                        </label>
                        <input
                            type="text"
                            name="name"
                            id="name"
                            value={customer.name}
                            onChange={handleChange}
                            className="shadow appearance-none border rounded-lg w-full py-3 px-4 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all duration-300"
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="email">
                            Email
                        </label>
                        <input
                            type="email"
                            name="email"
                            id="email"
                            value={customer.email}
                            onChange={handleChange}
                            className="shadow appearance-none border rounded-lg w-full py-3 px-4 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all duration-300"
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="address">
                            Dirección
                        </label>
                        <input
                            type="text"
                            name="address"
                            id="address"
                            value={customer.address}
                            onChange={handleChange}
                            className="shadow appearance-none border rounded-lg w-full py-3 px-4 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all duration-300"
                        />
                    </div>
                    <div className="flex flex-col sm:flex-row items-center justify-between space-y-4 sm:space-y-0 sm:space-x-4">
                        <button
                            type="submit"
                            disabled={loading}
                            className="w-full sm:w-auto bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-6 rounded-full focus:outline-none focus:shadow-outline transition duration-300 ease-in-out transform hover:scale-105"
                        >
                            {loading ? 'Guardando...' : 'Guardar Cliente'}
                        </button>
                        <button
                            type="button"
                            onClick={() => navigate('/customers')}
                            className="w-full sm:w-auto bg-gray-500 hover:bg-gray-700 text-white font-bold py-3 px-6 rounded-full focus:outline-none focus:shadow-outline transition duration-300 ease-in-out transform hover:scale-105"
                        >
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CustomerForm;
