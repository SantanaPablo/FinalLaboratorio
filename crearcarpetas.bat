@echo off

rem Directorio raíz para la solución
set SOLUTION_ROOT=MS.Solution

rem Crear el directorio principal de la solución
mkdir "%SOLUTION_ROOT%"

rem Navegar a la carpeta de la solución para crear los proyectos dentro
cd "%SOLUTION_ROOT%"

rem Crear los directorios para cada microservicio
mkdir MS.Customer
mkdir MS.Product
mkdir MS.Order

rem Crear los subdirectorios para cada proyecto dentro de cada microservicio
mkdir MS.Customer\MS.Customer.API
mkdir MS.Customer\MS.Customer.Application
mkdir MS.Customer\MS.Customer.Domain
mkdir MS.Customer\MS.Customer.Infrastructure

mkdir MS.Product\MS.Product.API
mkdir MS.Product\MS.Product.Application
mkdir MS.Product\MS.Product.Domain
mkdir MS.Product\MS.Product.Infrastructure

mkdir MS.Order\MS.Order.API
mkdir MS.Order\MS.Order.Application
mkdir MS.Order\MS.Order.Domain
mkdir MS.Order\MS.Order.Infrastructure

echo Estructura de directorios creada correctamente.