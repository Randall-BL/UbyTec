USE UbyTec;
GO

-- Insertar Tipos de Comercio
INSERT INTO TiposComercio (NombreTipo) VALUES
('Restaurante'),
('Tienda de Ropa'),
('Electrónica');

-- Insertar Administradores
INSERT INTO Administradores (NumeroCedula, NombreCompleto, DireccionProvincia, DireccionCanton, DireccionDistrito, Usuario, PasswordHash) VALUES
('001-1234-5678', 'Carlos Pérez', 'San José', 'Central', 'Carmen', 'admin1', 'passwordhash1'),
('002-8765-4321', 'Ana Rodríguez', 'Alajuela', 'Central', 'San José', 'admin2', 'passwordhash2');

-- Insertar Afiliados
INSERT INTO Afiliados (NumeroCedulaJuridica, NombreComercio, TipoComercioID, DireccionProvincia, DireccionCanton, DireccionDistrito, CorreoElectronico, NumeroSNIPE, Administrador, Password, Estado) VALUES
('3101234567', 'Burger King', 1, 'San José', 'Central', 'Carmen', 'contacto@burgerking.com', 'SNIPE12345', 'Carlos Pérez', 'password1', 'Activo'),
('3107654321', 'Tiendas Mundo', 2, 'Alajuela', 'Central', 'San Rafael', 'contacto@tiendasmundo.com', 'SNIPE67890', 'Ana Rodríguez', 'password2', 'Activo');

-- Insertar Clientes
INSERT INTO Clientes (NumeroCedula, Nombre, Apellidos, DireccionProvincia, DireccionCanton, DireccionDistrito, Telefono, FechaNacimiento, Usuario, PasswordHash) VALUES
('114-5678-9123', 'Juan', 'Sánchez', 'San José', 'Central', 'Hospital', '88888888', '1985-05-15', 'cliente1', 'passwordhashcliente1'),
('115-4321-8765', 'María', 'González', 'Alajuela', 'Central', 'San Rafael', '87777777', '1990-10-20', 'cliente2', 'passwordhashcliente2');

-- Insertar Repartidores
INSERT INTO Repartidores (NombreCompleto, DireccionProvincia, DireccionCanton, DireccionDistrito, CorreoElectronico, Usuario, PasswordHash) VALUES
('Pedro Jiménez', 'San José', 'Central', 'Zapote', 'pedro@correo.com', 'repartidor1', 'passwordhashrepartidor1'),
('Laura Pérez', 'Heredia', 'Central', 'San Pablo', 'laura@correo.com', 'repartidor2', 'passwordhashrepartidor2');

-- Insertar EstadoRepartidor
INSERT INTO EstadoRepartidor (RepartidorID, Estado) VALUES
(1, 'Disponible'),
(2, 'Ocupado');

-- Insertar Productos
INSERT INTO Productos (AfiliadoID, NombreProducto, Categoria, Precio, Foto) VALUES
(1, 'Hamburguesa Clásica', 'Comida', 3000, NULL),
(1, 'Papas Fritas Grandes', 'Comida', 1500, NULL),
(2, 'Camiseta Básica', 'Ropa', 7000, NULL),
(2, 'Jeans Ajustados', 'Ropa', 20000, NULL);

-- Insertar DetallesPedido
INSERT INTO DetallesPedido (RepartidorID, ClienteID, AfiliadoID) VALUES
(1, 1, 1),
(2, 2, 2);

-- Insertar Pedidos
INSERT INTO Pedidos (DetallePedidoID, ClienteID, AfiliadoID, RepartidorID, Total, CantidadProducto, MontoProducto, NombreRepartidor, NombreCliente, NombreComercio, NombreProducto) VALUES
(1, 1, 1, 1, 4500, 2, 1500, 'Pedro Jiménez', 'Juan Sánchez', 'Burger King', 'Hamburguesa Clásica'),
(2, 2, 2, 2, 27000, 1, 20000, 'Laura Pérez', 'María González', 'Tiendas Mundo', 'Jeans Ajustados');

-- Insertar ProductosDetalle
INSERT INTO ProductosDetalle (ProductoID, DetallePedidoID, Cantidad) VALUES
(1, 1, 1),
(2, 1, 1),
(3, 2, 1);

-- Insertar TelefonosRepartidores
INSERT INTO TelefonosRepartidores (RepartidorID, Numero) VALUES
(1, '88888888'),
(2, '87777777');

-- Insertar TelefonosAdministradores
INSERT INTO TelefonosAdministradores (AdministradorID, Numero) VALUES
(1, '12345678'),
(2, '87654321');

-- Insertar TelefonosAfiliados
INSERT INTO TelefonosAfiliados (AfiliadoID, Numero) VALUES
(1, '22222222'),
(2, '33333333');
