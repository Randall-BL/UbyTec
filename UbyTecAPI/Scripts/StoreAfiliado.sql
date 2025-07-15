CREATE PROCEDURE InsertarAfiliadoConTelefonos
    @NumeroCedulaJuridica NVARCHAR(50),
    @NombreComercio NVARCHAR(100),
    @TipoComercioID INT,
    @DireccionProvincia NVARCHAR(50),
    @DireccionCanton NVARCHAR(50),
    @DireccionDistrito NVARCHAR(50),
    @CorreoElectronico NVARCHAR(100),
    @NumeroSINPE NVARCHAR(50),
    @AdministradorID INT,
    @Telefonos NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar el Afiliado
        DECLARE @NuevoAfiliadoID INT;
        INSERT INTO Afiliados (NumeroCedulaJuridica, NombreComercio, TipoComercioID, DireccionProvincia, DireccionCanton, DireccionDistrito, CorreoElectronico, NumeroSINPE, AdministradorID)
        VALUES (@NumeroCedulaJuridica, @NombreComercio, @TipoComercioID, @DireccionProvincia, @DireccionCanton, @DireccionDistrito, @CorreoElectronico, @NumeroSINPE, @AdministradorID);

        SET @NuevoAfiliadoID = SCOPE_IDENTITY();

        -- Insertar los Teléfonos
        DECLARE @Telefono NVARCHAR(20);
        DECLARE @Pos INT;

        WHILE LEN(@Telefonos) > 0
        BEGIN
            SET @Pos = CHARINDEX(',', @Telefonos);
            IF @Pos = 0
            BEGIN
                SET @Telefono = @Telefonos;
                SET @Telefonos = '';
            END
            ELSE
            BEGIN
                SET @Telefono = LEFT(@Telefonos, @Pos - 1);
                SET @Telefonos = SUBSTRING(@Telefonos, @Pos + 1, LEN(@Telefonos) - @Pos);
            END

            INSERT INTO TelefonosAfiliados (AfiliadoID, NumeroTelefono) VALUES (@NuevoAfiliadoID, @Telefono);
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
