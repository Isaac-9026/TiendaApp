# TiendaApp - Gestión de Productos

Aplicación web para la administración y control de inventario de productos construida con **ASP.NET Core Blazor (.NET 10)** y **Entity Framework Core**, conectada a **Microsoft SQL Server**.

---

## Características

- **Gestión Integral de Productos (CRUD):**
  - **Listar:** Visualización de productos con su marca asociada, precio, stock y acciones.
  - **Crear:** Formulario con selección dinámica de marcas y validaciones en tiempo real.
  - **Editar:** Carga del producto seleccionado con actualización automática.
  - **Eliminar:** Confirmación interactiva en el navegador mediante JavaScript Interop (`IJSRuntime`).
- **Componentes Reutilizables:**
  - Componente de formulario `ProductForm.razor` desacoplado y reutilizable tanto para operaciones de alta como de modificación.
- **Acceso a Datos Seguro y Concurrente:**
  - Implementación de `IDbContextFactory<TiendaContext>` para gestionar instancias de contexto de EF Core de forma segura en Blazor Server.
  - Consultas optimizadas con LinQ (`AsNoTracking()`, `Include()`, `ExecuteDeleteAsync()`).
- **Validaciones de Modelo:**
  - Reglas de validación declarativas usando `System.ComponentModel.DataAnnotations` (`[Required]`, `[Range]`, `[StringLength]`).
- **Diseño Responsivo:**
  - Estilizado con Bootstrap 5 para una interfaz limpia, adaptable a dispositivos móviles y ordenadores.

---

## Tecnologías Utilizadas

- **Plataforma:** [.NET 10.0](https://dotnet.microsoft.com/)
- **Framework Web:** ASP.NET Core Blazor (Interactive Server Mode)
- **ORM:** Entity Framework Core 10 (`Microsoft.EntityFrameworkCore.SqlServer`)
- **Base de Datos:** Microsoft SQL Server
- **Estilos e Interfaz:** Bootstrap 5 & Bootstrap Icons
- **Lenguaje:** C# 13 / 14

---

## Arquitectura del Proyecto

El proyecto sigue una arquitectura limpia basada en separación de responsabilidades:

```
[ Vista (Blazor Components) ]
          │  Inyección de Dependencias
          ▼
[ Capa de Servicios (IProductoService / ProductoService) ]
          │  IDbContextFactory<TiendaContext>
          ▼
[ Capa de Datos (TiendaContext / Entity Framework Core) ]
          │  T-SQL
          ▼
[ Base de Datos (SQL Server) ]
```

---

## Estructura del Proyecto

```plaintext
TiendaApp/
├── TiendaApp.slnx                     # Solución de Visual Studio
└── TiendaApp/
    ├── Components/
    │   ├── Layout/                    # Plantillas y menús
    │   │   ├── MainLayout.razor
    │   │   ├── NavMenu.razor          # Menú principal con enlace a productos
    │   │   └── ReconnectModal.razor
    │   ├── Pages/                     # Páginas y vistas Blazor
    │   │   ├── Productos/
    │   │   │   ├── Crear.razor        # Página para crear producto
    │   │   │   ├── Editar.razor       # Página para editar producto
    │   │   │   ├── Index.razor        # Listado principal de productos
    │   │   │   └── ProductForm.razor  # Componente formulario reutilizable
    │   │   ├── Counter.razor
    │   │   ├── Error.razor
    │   │   ├── Home.razor
    │   │   ├── NotFound.razor
    │   │   └── Weather.razor
    │   ├── App.razor                  # Componente raíz
    │   ├── Routes.razor               # Router principal
    │   └── _Imports.razor             # Usings globales de Razor
    ├── Data/
    │   └── TiendaContext.cs           # DbContext de Entity Framework
    ├── Models/                        # Modelos y entidades
    │   ├── Marca.cs                   # Entidad Marca
    │   └── Producto.cs                # Entidad Producto
    ├── Properties/
    │   └── launchSettings.json        # Configuración de ejecución local
    ├── Services/                      # Lógica de negocio y acceso a datos
    │   └── ProductoService.cs         # Interfaz e implementación de IProductoService
    ├── wwwroot/                       # Archivos estáticos (CSS, JS, Bootstrap)
    ├── appsettings.json               # Configuración y cadena de conexión
    ├── Program.cs                     # Configuración de servicios y pipeline HTTP
    └── TiendaApp.csproj               # Archivo de proyecto y dependencias NuGet
```

---

## Base de Datos (SQL Server)

Para inicializar la base de datos necesaria para la aplicación, ejecute el siguiente script en **SQL Server Management Studio (SSMS)**, **Azure Data Studio** o mediante **sqlcmd**:

```sql
-- 1. Crear la base de datos
CREATE DATABASE TIENDA;
GO

USE TIENDA;
GO

-- 2. Crear tabla de marcas
CREATE TABLE marcas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    marca VARCHAR(100) NOT NULL
);
GO

-- 3. Crear tabla de productos
CREATE TABLE Productos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idmarca INT NOT NULL,
    Descripcion VARCHAR(300) NOT NULL,
    Precio DECIMAL(8,2) NOT NULL,
    stock INT NOT NULL,
    CONSTRAINT FK_Productos_Marcas FOREIGN KEY (idmarca) REFERENCES marcas(id)
);
GO

-- 4. Insertar datos de prueba iniciales
INSERT INTO marcas (marca) VALUES 
('Apple'),
('Samsung'),
('Sony'),
('LG'),
('Dell'),
('Logitech');
GO

INSERT INTO Productos (idmarca, Descripcion, Precio, stock) VALUES 
(1, 'iPhone 15 Pro Max 256GB', 4899.00, 15),
(2, 'Samsung Galaxy S24 Ultra', 4699.00, 20),
(3, 'Sony WH-1000XM5 Auriculares Cancelación Ruido', 1250.00, 30),
(4, 'Smart TV LG OLED 55" 4K', 3499.00, 10),
(5, 'Laptop Dell XPS 15 Intel i7 16GB RAM', 6200.00, 8),
(6, 'Mouse Inalámbrico Logitech MX Master 3S', 380.00, 45);
GO
```

---

## Instalación y Configuración

### 1. Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (o versión compatible configurada)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server) (o SQL Server Express / LocalDB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [Visual Studio Code](https://code.visualstudio.com/) / [JetBrains Rider](https://www.jetbrains.com/rider/)

### 2. Clonar el Repositorio

```bash
git clone https://github.com/Isaac-9026/TiendaApp.git
cd TiendaApp
```

### 3. Configurar la Cadena de Conexión

Abra el archivo `TiendaApp/appsettings.json` y ajuste la cadena de conexión según su servidor local:

```json
{
  "ConnectionStrings": {
    "Tienda": "Server=localhost;Database=TIENDA;User Id=sa;Password=tu_password;TrustServerCertificate=True"
  }
}
```

> **Nota:** Si utiliza autenticación de Windows, puede usar:
> ```json
> "Tienda": "Server=localhost;Database=TIENDA;Trusted_Connection=True;TrustServerCertificate=True"
> ```

### 4. Restaurar Paquetes y Ejecutar

Desde la terminal en la raíz del proyecto:

```bash
cd TiendaApp
dotnet restore
dotnet build
dotnet run
```

O abra `TiendaApp.slnx` en Visual Studio y presione `F5` / `Ctrl+F5`.

La aplicación estará disponible por defecto en:
`https://localhost:7157` o `http://localhost:5247`.

---

## Rutas y Vistas Disponibles

| Ruta | Descripción |
|---|---|
| `/` | Página principal de bienvenida |
| `/productos` | Lista general de productos registrados con opciones de edición y eliminación |
| `/productos/crear` | Formulario para registrar un nuevo producto |
| `/productos/editar/{Id}` | Formulario de actualización de producto existente |
| `/counter` | Componente de ejemplo interactivo Blazor |
| `/weather` | Componente de ejemplo de datos del clima |

---

## Autor

- **Isaac** - [@Isaac-9026](https://github.com/Isaac-9026)
