CREATE TRIGGER trg_UpdateRepartidorOnCompletion
ON DetallesPedido
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Establecer RepartidorID en NULL cuando el estado sea 'Completado'
    UPDATE DetallesPedido
    SET RepartidorID = NULL
    FROM DetallesPedido dp
    JOIN inserted i ON dp.DetalleID = i.DetalleID
    WHERE i.Estado = 'Completado';
END;
GO
