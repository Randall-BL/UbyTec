CREATE PROCEDURE AsignarPedidoAMensajero
    @DetallePedidoID INT -- ID del pedido que se quiere asignar
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Verificar si el pedido existe
        DECLARE @AfiliadoProvincia NVARCHAR(50);
        SELECT @AfiliadoProvincia = a.DireccionProvincia
        FROM DetallesPedido dp
        INNER JOIN Afiliados a ON dp.AfiliadoID = a.AfiliadoID
        WHERE dp.DetalleID = @DetallePedidoID;

        IF @AfiliadoProvincia IS NULL
        BEGIN
            RAISERROR ('El pedido o el afiliado no existen.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Verificar los repartidores disponibles (sin pedidos activos)
        DECLARE @RepartidorID INT;

        SELECT TOP 1 @RepartidorID = r.RepartidorID
        FROM Repartidores r
        WHERE NOT EXISTS (
            SELECT 1
            FROM DetallesPedido dp
            WHERE dp.RepartidorID = r.RepartidorID
              AND dp.Estado IN ('Recibido', 'En proceso', 'En camino')
        )
        AND r.DireccionProvincia = @AfiliadoProvincia
        ORDER BY r.RepartidorID; -- Ordenar por proximidad o cualquier otra lógica

        IF @RepartidorID IS NULL
        BEGIN
            RAISERROR ('No hay repartidores disponibles en la provincia del afiliado.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Asignar el repartidor al pedido
        UPDATE DetallesPedido
        SET RepartidorID = @RepartidorID,
            Estado = 'En camino'
        WHERE DetalleID = @DetallePedidoID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
