-- Crear la base de datos
CREATE DATABASE UbyTec;
GO

-- Usar la base de datos
USE UbyTec;
GO

-- Tabla Clientes
CREATE TABLE Clientes (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCedula NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    DireccionProvincia NVARCHAR(100) NULL,
    DireccionCanton NVARCHAR(100) NULL,
    DireccionDistrito NVARCHAR(100) NULL,
    Telefono NVARCHAR(15) NULL,
    FechaNacimiento DATE NULL,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

-- Tabla Repartidores
CREATE TABLE Repartidores (
    RepartidorID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(200) NOT NULL,
    DireccionProvincia NVARCHAR(100) NULL,
    DireccionCanton NVARCHAR(100) NULL,
    DireccionDistrito NVARCHAR(100) NULL,
    CorreoElectronico NVARCHAR(100) UNIQUE NOT NULL,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

-- Tabla EstadoRepartidor
CREATE TABLE EstadoRepartidor (
    EstadoRepartidorID INT IDENTITY(1,1) PRIMARY KEY,
    RepartidorID INT NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    FOREIGN KEY (RepartidorID) REFERENCES Repartidores(RepartidorID)
);

-- Tabla Afiliados
CREATE TABLE Afiliados (
    AfiliadoID INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCedulaJuridica NVARCHAR(50) NOT NULL UNIQUE,
    NombreComercio NVARCHAR(100) NOT NULL,
    TipoComercioID INT NOT NULL,
    DireccionProvincia NVARCHAR(100) NULL,
    DireccionCanton NVARCHAR(100) NULL,
    DireccionDistrito NVARCHAR(100) NULL,
    CorreoElectronico NVARCHAR(100) UNIQUE NOT NULL,
    NumeroSNIPE NVARCHAR(50) NULL,
    Administrador NVARCHAR(100) NULL,
    Password NVARCHAR(255) NOT NULL,
    Estado NVARCHAR(50) NOT NULL
);

-- Tabla TiposComercio
CREATE TABLE TiposComercio (
    TipoComercioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreTipo NVARCHAR(100) NOT NULL
);

-- Tabla Administradores
CREATE TABLE Administradores (
    AdministradorID INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCedula NVARCHAR(50) NOT NULL UNIQUE,
    NombreCompleto NVARCHAR(200) NOT NULL,
    DireccionProvincia NVARCHAR(100) NULL,
    DireccionCanton NVARCHAR(100) NULL,
    DireccionDistrito NVARCHAR(100) NULL,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

-- Tabla DetallesPedido
CREATE TABLE DetallesPedido (
    DetallePedidoID INT IDENTITY(1,1) PRIMARY KEY,
    RepartidorID INT NOT NULL,
    ClienteID INT NOT NULL,
    AfiliadoID INT NOT NULL,
    FOREIGN KEY (RepartidorID) REFERENCES Repartidores(RepartidorID),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    FOREIGN KEY (AfiliadoID) REFERENCES Afiliados(AfiliadoID)
);

-- Tabla Pedidos
CREATE TABLE Pedidos (
    PedidoID INT IDENTITY(1,1) PRIMARY KEY,
    DetallePedidoID INT NOT NULL,
    ClienteID INT NOT NULL,
    AfiliadoID INT NOT NULL,
    RepartidorID INT NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    CantidadProducto INT NOT NULL,
    MontoProducto DECIMAL(10,2) NOT NULL,
    NombreRepartidor NVARCHAR(100) NOT NULL,
    NombreCliente NVARCHAR(100) NOT NULL,
    NombreComercio NVARCHAR(100) NOT NULL,
    NombreProducto NVARCHAR(100) NOT NULL,
    FOREIGN KEY (DetallePedidoID) REFERENCES DetallesPedido(DetallePedidoID),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    FOREIGN KEY (AfiliadoID) REFERENCES Afiliados(AfiliadoID),
    FOREIGN KEY (RepartidorID) REFERENCES Repartidores(RepartidorID)
);

-- Tabla Productos
CREATE TABLE Productos (
    ProductoID INT IDENTITY(1,1) PRIMARY KEY,
    AfiliadoID INT NOT NULL,
    NombreProducto NVARCHAR(100) NOT NULL,
    Categoria NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Foto NVARCHAR(MAX) NULL,
    FOREIGN KEY (AfiliadoID) REFERENCES Afiliados(AfiliadoID)
);

-- Tabla ProductosDetalle
CREATE TABLE ProductosDetalle (
    ProductoDetalleID INT IDENTITY(1,1) PRIMARY KEY,
    ProductoID INT NOT NULL,
    DetallePedidoID INT NOT NULL,
    Cantidad INT NOT NULL,
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID),
    FOREIGN KEY (DetallePedidoID) REFERENCES DetallesPedido(DetallePedidoID)
);

-- Tabla TelefonosRepartidores
CREATE TABLE TelefonosRepartidores (
    TelefonoRepartidorID INT IDENTITY(1,1) PRIMARY KEY,
    RepartidorID INT NOT NULL,
    Numero NVARCHAR(15) NOT NULL,
    FOREIGN KEY (RepartidorID) REFERENCES Repartidores(RepartidorID)
);

-- Tabla TelefonosAdministradores
CREATE TABLE TelefonosAdministradores (
    TelefonoAdministradorID INT IDENTITY(1,1) PRIMARY KEY,
    AdministradorID INT NOT NULL,
    Numero NVARCHAR(15) NOT NULL,
    FOREIGN KEY (AdministradorID) REFERENCES Administradores(AdministradorID)
);

-- Tabla TelefonosAfiliados
CREATE TABLE TelefonosAfiliados (
    TelefonoAfiliadoID INT IDENTITY(1,1) PRIMARY KEY,
    AfiliadoID INT NOT NULL,
    Numero NVARCHAR(15) NOT NULL,
    FOREIGN KEY (AfiliadoID) REFERENCES Afiliados(AfiliadoID)
);
