# Plaga Cero API

**Plaga Cero API** es el backend de la aplicación **Plaga Cero**, diseñada para la detección de plagas de maíz. Utiliza .NET Core y Entity Framework Core con una base de datos MySQL.

## Estructura del Proyecto

- **Proyecto Backend**: `PlagaCero.API`

## Requisitos Previos

- [.NET SDK](https://dotnet.microsoft.com/download) (versión 6.0 o superior)
- [MySQL](https://www.mysql.com/downloads/) (o MariaDB)

## Setup del Backend

### 1. Instalación de Entity Framework Core

Navega al directorio del proyecto backend:

```bash
cd PlagaCero.API
```

Ejecuta los siguientes comandos para instalar las herramientas y paquetes necesarios:

```bash
# Instalar el CLI de EF Core
dotnet tool install --global dotnet-ef --version 8.0.4

# Agregar el paquete de diseño de EF Core
dotnet add package Microsoft.EntityFrameworkCore.Design

# Restaurar paquetes
dotnet restore
```

### 2. Verificar la Instalación

Para confirmar que la instalación fue exitosa, ejecuta:

```bash
dotnet ef
```

Deberías ver una salida que indica que el CLI de EF Core está funcionando correctamente.

### 3. Instalación del Proveedor de MySQL

Agrega el paquete de MySQL para Entity Framework Core:

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.2
```

### 4. Crear Base de Datos en MySQL

Conéctate a tu servidor MySQL y crea la base de datos `PlagaCero`. Asegúrate de tener el cliente MySQL instalado en tu sistema. Ejecuta:

```bash
mysql -u root -h localhost
CREATE DATABASE PlagaCero;
```

### 5. Configuración de Variables de Entorno

Configura las variables de entorno necesarias para tu aplicación. Dependiendo de tu sistema operativo, usa:

### 6. Migraciones

Agrega y aplica las migraciones para crear las tablas en la base de datos:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 7. Paquete para Swagger

Para agregar documentación y pruebas a tu API, instala el paquete para Swagger:

```bash
dotnet add package Swashbuckle.AspNetCore.Filters
```

## Ejecución de la Aplicación

Para ejecutar la API, asegúrate de estar en el directorio del proyecto y utiliza el siguiente comando:

```bash
dotnet run
```
