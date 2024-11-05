# Plaga Cero API

Plaga Cero API es el backend de la aplicación Plaga Cero, diseñada para la detección de plagas de maíz. Utiliza .NET Core y Entity Framework Core con una base de datos MySQL.

## Estructura del Proyecto
- Proyecto Backend: `PlagaCero.API`

## Requisitos Previos
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (versión 6.0 o superior)
- [MySQL](https://www.mysql.com/downloads/) (o MariaDB)

## Setup del Backend

### 1. Instalación de Entity Framework Core
Navega al directorio del proyecto backend:

```bash
cd PlagaCero.API
```

Ejecuta los siguientes comandos para instalar las herramientas y paquetes necesarios:

```bash
dotnet tool install --global dotnet-ef --version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design
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
Configura las siguientes variables de entorno para almacenar el usuario y contraseña de la base de datos en tu sistema:

- `PLAGA_CERO_USER`: El nombre de usuario para la base de datos MySQL.
- `PLAGA_CERO_PASSWORD`: La contraseña del usuario de la base de datos MySQL.

#### macOS / Linux
Abre tu terminal y ejecuta los siguientes comandos para configurar las variables de entorno:

```bash
export PLAGA_CERO_USER="tu_usuario_mysql"
export PLAGA_CERO_PASSWORD="tu_contraseña_mysql"
```

Para que estas variables estén disponibles en cada nueva sesión de terminal, agrégalas al archivo `~/.bashrc`, `~/.zshrc` o al archivo de configuración de tu shell:

```bash
echo 'export PLAGA_CERO_USER="tu_usuario_mysql"' >> ~/.bashrc
echo 'export PLAGA_CERO_PASSWORD="tu_contraseña_mysql"' >> ~/.bashrc

# Si usas zsh
echo 'export PLAGA_CERO_USER="tu_usuario_mysql"' >> ~/.zshrc
echo 'export PLAGA_CERO_PASSWORD="tu_contraseña_mysql"' >> ~/.zshrc
```

Después, aplica los cambios:

```bash
source ~/.bashrc  # O source ~/.zshrc si usas zsh
```

#### Windows
En Windows, puedes configurar variables de entorno a través de la línea de comandos o la interfaz gráfica.

**Línea de comandos (PowerShell):**

```powershell
[System.Environment]::SetEnvironmentVariable('PLAGA_CERO_USER', 'tu_usuario_mysql', [System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('PLAGA_CERO_PASSWORD', 'tu_contraseña_mysql', [System.EnvironmentVariableTarget]::User)
```

**Interfaz gráfica:**

1. Haz clic derecho en "Este equipo" y selecciona "Propiedades".
2. Haz clic en "Configuración avanzada del sistema".
3. En la pestaña "Opciones avanzadas", haz clic en "Variables de entorno".
4. En la sección "Variables de usuario", haz clic en "Nueva" y agrega:
   - Nombre de la variable: `PLAGA_CERO_USER`
   - Valor de la variable: `tu_usuario_mysql`
5. Repite el proceso para `PLAGA_CERO_PASSWORD`.

### Conexión a MySQL

Es importante asegurarte de que el puerto de MySQL esté correctamente configurado en tu cadena de conexión. Si estás utilizando un entorno como MAMP, XAMPP, o similar, verifica el puerto de MySQL:

- **MAMP**: Por defecto, MAMP utiliza el puerto **8889** para MySQL.
- **XAMPP**: Generalmente, el puerto por defecto es **3306**.

**Actualización de la cadena de conexión:**

Asegúrate de que tu cadena de conexión en el archivo de configuración de tu contexto de datos (`AppDb`) incluya el puerto correcto:

```csharp
var connectionString = $"server=localhost;port=8889;user={DbUser};password={DbPassword};database={AppConfig.DatabaseName}";
```

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
