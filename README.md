# 🖥️ Proyecto Microservicios ASP.NET Core + React

Este proyecto corresponde al **Trabajo de Laboratorio de Aplicaciones Servidor (2025)**.  
La solución implementa una arquitectura de **microservicios** con **ASP.NET**, **Entity Framework Core** y un **frontend en React (Vite + TailwindCSS)**.

---

## 🎯 Objetivo

- Desarrollar una solución completa con **Backend (microservicios)** y **Frontend (SPA)**.  
- Aplicar patrones y buenas prácticas: **DDD, Clean Architecture, CQRS, Repository Pattern**.  
- Utilizar **SQL Server** con **Entity Framework Core (Code First)**.  
- Exponer y consumir **REST APIs** para la gestión de productos, clientes y órdenes.  

---

## ⚙️ Backend – Microservicios

Cada microservicio cuenta con su propio `DbContext`, base de datos y API independiente.  
Para crear y aplicar las **migraciones** de cada uno, ubicarse en la raíz de la solución y ejecutar con powershell:

### 🔹 Product Microservice
dotnet ef migrations add InitialCreate `
    --project MS.Product.Infrastructure `
    --startup-project MS.Product.API `
    --output-dir Migrations

dotnet ef database update `
    --project MS.Product.Infrastructure `
    --startup-project MS.Product.API

### 🔹 Costumer Microservice
dotnet ef migrations add InitialCreate `
    --project MS.Customer.Infrastructure `
    --startup-project MS.Customer.API `
    --output-dir Migrations

dotnet ef database update `
    --project MS.Customer.Infrastructure `
    --startup-project MS.Customer.API

### 🔹 Infrastructure Microservice
dotnet ef migrations add InitialCreate `
    --project MS.Order.Infrastructure `
    --startup-project MS.Order.API `
    --output-dir Migrations

dotnet ef database update `
    --project MS.Order.Infrastructure `
    --startup-project MS.Order.API

📌 Notas:
La carpeta Migrations se genera automáticamente dentro de cada proyecto Infrastructure.

⚙️ Backend
Abrir la solución con Visual Studio y darle Run al perfil "3 Microservicios" (levanta 3 microservicios, 3 APIs en este caso en puertos 5000,5001 y 5002)

🌐 Frontend – React + Vite

1. Ejecutar el frontend React:
   cd laboratoriofront
   npm install
   npm run dev

2. Abrir el navegador en:
   http://localhost:5173 (o puerto que levante)

