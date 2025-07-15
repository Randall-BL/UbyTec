-- Eliminar el procedimiento existente si ya existe
IF OBJECT_ID('MoverVentasCompletadas', 'P') IS NOT NULL
    DROP PROCEDURE MoverVentasCompletadas;
GO

-- Crear el procedimiento almacenado modificado
CREATE PROCEDURE MoverVentasCompletadas
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar en la tabla Pedidos las ventas con estado "En proceso" desde DetallesPedido
        INSERT INTO Pedidos (ClienteID, AfiliadoID, RepartidorID, CantidadProducto, NombreProducto, MontoProducto, Total, NombreCliente, NombreComercio, NombreRepartidor)
        SELECT 
            dp.ClienteID,
            dp.AfiliadoID,
            dp.RepartidorID,
            SUM(pd.Cantidad) AS CantidadProducto,
            MAX(p.NombreProducto) AS NombreProducto,
            MAX(pd.Precio) AS MontoProducto,
            SUM(pd.Cantidad * pd.Precio) AS Total,
            CONCAT(c.Nombre, ' ', c.Apellidos) AS NombreCliente, -- Combinar Nombre y Apellidos
            a.NombreComercio AS NombreComercio,
            r.NombreCompleto AS NombreRepartidor -- Usar NombreCompleto para Repartidores
        FROM DetallesPedido dp
        INNER JOIN ProductosDetalle pd ON dp.DetalleID = pd.DetallePedidoID
        INNER JOIN Productos p ON pd.ProductoID = p.ProductoID
        INNER JOIN Clientes c ON dp.ClienteID = c.ClienteID
        INNER JOIN Afiliados a ON dp.AfiliadoID = a.AfiliadoID
        LEFT JOIN Repartidores r ON dp.RepartidorID = r.RepartidorID
        WHERE dp.Estado = 'En proceso' -- Cambiado a "En proceso"
        GROUP BY dp.ClienteID, dp.AfiliadoID, dp.RepartidorID, c.Nombre, c.Apellidos, a.NombreComercio, r.NombreCompleto;

        -- (Opcional) Cambiar el estado de los pedidos en DetallesPedido
        UPDATE DetallesPedido
        SET Estado = 'Completado'
        WHERE Estado = 'En proceso'; -- Cambiar a "Completado" después de moverlos

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Manejo de errores y rollback
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Lanzar el error original
        THROW;
    END CATCH
END;
GO
