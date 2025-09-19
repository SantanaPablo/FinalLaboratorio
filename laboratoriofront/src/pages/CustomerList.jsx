import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const API_URL = 'https://localhost:5002/api/Customer';

const CustomerList = () => {
    const [customers, setCustomers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchCustomers = async () => {
            try {
                const response = await fetch(API_URL);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setCustomers(data);
            } catch (error) {
                setError(error);
            } finally {
                setLoading(false);
            }
        };

        fetchCustomers();
    }, []);

    // Se han agregado clases de Tailwind para centrar y estilizar los mensajes de estado.
    if (loading) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-gray-700">Cargando...</div></div>;
    if (error) return <div className="flex items-center justify-center min-h-screen bg-gray-100"><div className="text-xl text-red-500">Error: {error.message}</div></div>;

    return (
        // Se aplican estilos para el contenedor principal de la página.
        <div className="container mx-auto p-4 md:p-8 bg-gray-100 min-h-screen">
            {/* Se utiliza flexbox para alinear el título y el botón. */}
            <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8">
                <h1 className="text-4xl font-extrabold text-gray-800 mb-4 md:mb-0">Clientes</h1>
                <Link
                    to="/customers/create"
                    // Clases para un botón estilizado con efectos de hover.
                    className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 px-6 rounded-full shadow-lg transition duration-300 ease-in-out transform hover:scale-105"
                >
                    Agregar Nuevo Cliente
                </Link>
            </div>
            {/* Contenedor de la tabla con sombras y bordes redondeados. */}
            <div className="bg-white rounded-xl shadow-2xl overflow-hidden">
                <table className="min-w-full leading-normal">
                    <thead>
                        <tr>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                ID
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Nombre
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Email
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Dirección
                            </th>
                            <th className="px-6 py-4 border-b-2 border-gray-200 bg-gray-50 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                                Acciones
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {customers.map((customer) => (
                            // Efecto de hover en las filas de la tabla.
                            <tr key={customer.id} className="hover:bg-gray-50 transition duration-200">
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{customer.id}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{customer.name}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{customer.email}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <div className="text-gray-900 whitespace-no-wrap">{customer.address}</div>
                                </td>
                                <td className="px-6 py-5 border-b border-gray-200 bg-white text-sm">
                                    <Link
                                        to={`/customers/${customer.id}`}
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

export default CustomerList;
