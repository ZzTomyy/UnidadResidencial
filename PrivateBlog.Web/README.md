# Sistema de Gestión de Unidad Residencial

Proyecto desarrollado en ASP.NET Core MVC con Entity Framework y SQL Server.

## Requisitos

- Visual Studio
- SQL Server (Express o cualquier instancia local)

## Configuración del proyecto

Antes de ejecutar el proyecto debes configurar la conexión a la base de datos en:

appsettings.json

Ejemplo de connection string para SQL Server Express:

Server=.\SQLEXPRESS;Database=UnidadResidencialDb;Trusted_Connection=True;TrustServerCertificate=True

Si usas otra instancia de SQL Server debes modificar el servidor según tu configuración.

## Crear la base de datos

Abrir la consola del administrador de paquetes y ejecutar:

Update-Database

Esto creará automáticamente la base de datos y las tablas necesarias.

## Ejecutar el proyecto

Después de configurar la base de datos, ejecutar el proyecto normalmente desde Visual Studio.