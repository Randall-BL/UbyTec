CREATE PROCEDURE UpdateDetallePedido
    @DetalleID INT,
    @Estado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar el estado
    UPDATE DetallesPedido
    SET Estado = @Estado
    WHERE DetalleID = @DetalleID;

    -- Si el estado es "Completado", actualizar RepartidorID a NULL
    IF @Estado = 'Completado'
    BEGIN
        UPDATE DetallesPedido
        SET RepartidorID = NULL
        WHERE DetalleID = @DetalleID;
    END
END;
