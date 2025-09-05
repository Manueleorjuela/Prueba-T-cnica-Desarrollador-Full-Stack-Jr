# Prueba Técnica Full-Stack Jr
Breve descripción: Proyecto de prueba técnica con backend .NET, frontend Angular y bases de datos SQL utilizando JWT para autenticación.

#Requisitos

##Requisitos previos

### Backend (ASP.NET Core Web API en C#)
- Microsoft Visual Studio 2022 (o superior) con .NET 7 SDK
- Plantilla: ASP.NET Core Web API
- Servidor SQL
- Navegador web para probar Swagger 

### Frontend (Angular)
- Node.js 18+
- npm
- Editor de código Visual Studio Code
- Bootstrap 

### Herramientas opcionales
- Postman (para probar la API si no se desea usar el Swagger)

## Configuración de la base de datos y variables de entorno

### Base de datos en SQL
1. Abrir SQL Server Management Studio.
2. Establecer conexón en base de datos local (Instancia localhost)
3. Ejecutar los siguientes comandos para crear la base de datos(Uno por uno):
	
	CREATE DATABASE MiPrueba;
	
	USE MiPrueba;
	GO
	
	CREATE TABLE Users ( 
    	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
   	Email NVARCHAR(256) UNIQUE NOT NULL, 
   	PasswordHash VARBINARY(MAX) NOT NULL, 
   	PasswordSalt VARBINARY(MAX) NOT NULL, 
   	Name NVARCHAR(150) NOT NULL, 
    	Role NVARCHAR(20) NOT NULL DEFAULT 'user', 
   	IsActive BIT NOT NULL DEFAULT 1, 
   	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
   	UpdatedAt DATETIME2 NULL 
	);
	GO

	INSERT INTO Users (Email, PasswordHash, PasswordSalt, Name)
	VALUES (
        'user@demo.com',
   	0x6E59258024A6FA00CEC8C256AC5AABB46FC5EB4CFC1B9F70138373D6A399E370,
   	0x7FDFD02F2F1B78B544A4E2A8910E95C3,
   	'user'
        );
        GO
	
	INSERT INTO Users (Email, PasswordHash, PasswordSalt, Name)
	VALUES (
        'admin@demo.com',
   	0x9D4652A7705BEE1E3013CD42A81923E222E5F847E008B1C7CAC5A73F71A70931,
   	0xF135DB921235BF3F6B0B6F4B8FBAB5F8,
   	'user'
        );
        GO
	

## Secretos e información sensible dentro del backend

Para proteger información sensible como JWT key y la cadena de conexión a la base de datos, usamos **User Secrets** en .NET. Esto permite guardar estos valores localmente sin incluirlos en el repositorio.

1. Ejecuta en la terminal dentro de la carpeta del proyecto (donde está tu archivo .csproj):
	
	dotnet user -secrets init

	**JWT Key:**
	dotnet user-secrets set "Jwt:Key" "clave_super_secreta_que_no_va_al-repo"
	
	**Connection String de la base de datos:**
	dotnet user-secrets set "ConnectionStrings:MiPrueba" "Server=localhost; Database=MiPrueba; Integrated 	Security=True; TrustServerCertificate=True;"

	**Verificar secretos guardados
	dotnet user-secrets list 

## Funcionamiento Backend y Frontend
1. Abrir la carpeta del backend en Visual Studio: 
2. 2. Verificar la configuración de **CORS** en `Program.cs`:
	csharp
	builder.Services.AddCors(options =>
	{
    	        options.AddPolicy("CorsPolicy", policy =>
        	policy.WithOrigins("http://localhost:(URL GENERADA)") // URL de tu frontend
               .AllowAnyMethod()
               .AllowAnyHeader());
	});

3. Ubicarse sobre la carpeta que contiene el frontend y ejecutar:
	npm install
	npm strart - Como alternativa si se ejecuta desde angular utlizar comando ng serve

##Peticiones postman de ejemplo
En formato JSON. Tener en cuenta la dirección del localhost del backend para realizar la prueba.

Registrarse
Método: POST
Ruta: https://localhost:7076/API/Register
{
	"email" : "ejemplo@correo.com",
	"password": "Ejemplo123!",
	"name": "Manuel"
}

Logearse
Método: POST
Ruta: https://localhost:7076/API/Login
{
	"email": "admin@correo.com",
	"password":  "Admin123*"
}

Listar Usuarios
Método: GET
Ruta: https://localhost:7076/API/Admin/ListarUsuarios
-Tener en cuenta que se hace una validación de token por inicio de sesión recibido por el body del JSON de respuesta, configurar en Auth Barear Token e ingresar.

Eliminar usuario
Método: DELETE
Ruta: https://localhost:7076/API/Admin/Eliminar_Usuario/{id}
-Tener en cuenta que se hace una validación de token por inicio de sesión recibido por el body del JSON de respuesta, configurar en Auth Barear Token e ingresar.

Añadir usuario por admin
Método: POST
Ruta: https://localhost:7076/API/Admin/AñadirUsuario
-Tener en cuenta que se hace una validación de token por inicio de sesión recibido por el body del JSON de respuesta, configurar en Auth Barear Token e ingresar.
{	
    "email" : "ejemplo2@correo.com",
    "role": "user",
    "name": "Manuel2",
    "password": "Ejemplo123!"	
}

Ver usuario
Método: GET
Ruta: https://localhost:7076/API/Loged/VerUsuario/{id}
-Tener en cuenta que se hace una validación de token por inicio de sesión recibido por el body del JSON de respuesta, configurar en Auth Barear Token e ingresar.

Editar usuario
Método: PUT
Ruta: https://localhost:7076/API/Loged/EditarUsuario/{id}
-Tener en cuenta que se hace una validación de token por inicio de sesión recibido por el body del JSON de respuesta, configurar en Auth Barear Token e ingresar.
{	
    "email" : "ejemplo3@correo.com",
    "role": "admin",
    "name": "Manuel3",
    "password": "Ejemplo123!"
}

## Credenciales de prueba para ingreso

### Usuario administrador: 
	admin@demo.com 
	Admin123! 

## Usuario:
	user@demo.com
	User123!

## LIMITACIONES
Esta API es una versión básica desarrollada como prueba de concepto y aprendizaje. Aún existen varias áreas de mejora:

###Validaciones insuficientes: Falta validación robusta de campos como email, contraseña y nombres.

###Roles y permisos: Actualmente no hay control completo de lo que puede hacer un usuario vs un administrador.

###Login y expiración: El sistema de login aún no maneja expiración de sesión o tokens JWT.

###Pruebas unitarias: Faltan pruebas unitarias completas para todos los endpoints y la lógica de negocio.

###Código repetido y encapsulación: Hay partes con repetición de código y falta de encapsulación de lógica en servicios backend y frontend.

###Diseño del PUT: La actualización de usuarios (PUT) podría estar mejor estructurada y segura.

###Nomenclatura: Algunos nombres de variables y convenciones de idioma podrían mejorarse para mantener consistencia.

###General: Es un proyecto sencillo, hecho en 4 días como reto, por lo que su robustez y escalabilidad son limitadas.

A pesar de estas limitaciones, el proyecto se centró en la práctica y el aprendizaje, incluyendo la creación de endpoints funcionales, el manejo seguro de contraseñas mediante hashes y salts, la integración con SQL Server y el consumo de la API a través de Postman. Además, se trabajó con un frontend básico, utilizando HTML para establecer conexiones y probar la interacción con la API.