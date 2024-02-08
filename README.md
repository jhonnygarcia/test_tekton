# Product Services

Esta aplicacion sirve para poder crear, modificar, eliminar y listar productos y así mismo se comunica con una API para poder aplicar un descuesto al producto. Esta aplicacion es un web API que expone sus servicios mediante Swagger.

## Características

- Utiliza .NET 8
- Implementa el patrón Mediator con CQRS
- Se conecta a SQL Server usando el ORM Entity Framework
- Para los unit test utiliza XUnit
- Se realizo un implementación de cache en la memoria con la propia tecnoligia de .NET ```IMemoryCache```
- Los servicios estan visibles con swagger y estan protegidos con token
- Para generar un token existen dos usuarios: un admin y un operator estos se encuentran en el appsettings:

  ```json
  "Security": {
    "BasicAuthentication": [
      {
        "UserName": "admin",
        "Password": "123"
      },
      {
        "UserName": "operator",
        "Password": "123"
      }
    ]
  },
  ```
- El controlador Management esta protegido para usuarios con el rol de admin
- Para proteger los endpoint se estan usando decoradores agregando un rol si se quiere limitar por rol o sin ninguno: 
  - ```[MainAuthorize(Roles = SystemRoles.Admin)]```
  - ```[MainAuthorize(Roles = SystemRoles.Operator)]```
  - ```[MainAuthorize]```
- Tambien estan protegido por Policy
  - ```[MainAuthorize(Policy = MainPolicies.PolicyAdmins)]```
  - ```[MainAuthorize(Policy = MainPolicies.PolicyOperators)]```
- La aplicación se conecta así misma para obtener los descuentos de un producto por su ID la api se encuentra en el controlador DiscountFakeController y este servicio se encuentra protegido el mismo es consumido como si fuera un servicio externo. (Esta implementación es meramente didáctica para proveerse así de un servicio web protegido que retorna el descuento para un producto)

## Requisitos Previos

Antes de comenzar, asegúrate de tener instalado lo siguiente:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/es/) (recomendado 2022)

## Configuración de la Base de Datos

Para configurar la base de datos que utiliza la aplicación, sigue estos pasos:

1. Abre el archivo `appsettings.json`.
2. Busca la sección de cadena de conexión y actualízala con los detalles de tu base de datos SQL Server.

   ```json
   "ConnectionStrings": {
     "MainConnection": "Server=tu_servidor;Database=tu_base_de_datos;User Id=tu_usuario;Password=tu_contraseña;Persist Security Info=False;Encrypt=False;TrustServerCertificate=False"
   }
   ```

## Creación de las Tablas

Hay dos formas de crear las tablas necesarias para la aplicación:

### Opción 1: Usando Migrations

1. Abre la solución en Visual Studio.
2. Establece el proyecto `DataAccess` como proyecto de inicio.
3. Abre la consola del administrador de paquetes (PMC) y ejecuta el siguiente comando:

   ```
   Update-Database
   ```

### Opción 2: Usando el Proyecto de Base de Datos

1. En la solución, localiza el proyecto `DataBase`.
2. Haz clic derecho sobre él y selecciona "Publicar".
3. Sigue las instrucciones para publicar las tablas en tu base de datos.

## Ejecutando la Aplicación

Para ejecutar la aplicación:

1. Presiona `F5` en Visual Studio.
2. Visual Studio compilará el proyecto y lanzará la aplicación.
