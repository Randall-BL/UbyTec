IF OBJECT_ID('trg_TelefonoDelete', 'TR') IS NOT NULL
    DROP TRIGGER trg_TelefonoDelete;
GO

CREATE TRIGGER trg_TelefonoDelete
ON TelefonosAfiliados
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AfiliadoID INT;
    SELECT @AfiliadoID = AfiliadoID FROM deleted;

    -- Verificar si el afiliado sigue existiendo antes de comprobar los teléfonos
    IF EXISTS (SELECT 1 FROM Afiliados WHERE AfiliadoID = @AfiliadoID)
    BEGIN
        -- Verificar si no quedan teléfonos asociados
        IF NOT EXISTS (SELECT 1 FROM TelefonosAfiliados WHERE AfiliadoID = @AfiliadoID)
        BEGIN
            RAISERROR ('Un Afiliado debe tener al menos un teléfono.', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END
END;
GO
