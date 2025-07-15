using Microsoft.EntityFrameworkCore;

namespace UbyTecAPI.Models
{
    public class UbyTecContext : DbContext
    {
        public UbyTecContext(DbContextOptions<UbyTecContext> options) : base(options) { }

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Afiliado> Afiliados { get; set; }
        public DbSet<TipoComercio> TiposComercio { get; set; }
        public DbSet<Repartidor> Repartidores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<ProductoDetalle> ProductosDetalle { get; set; } // Nuevo DbSet para ProductoDetalle
        public DbSet<EstadoRepartidor> EstadoRepartidor { get; set; }
        public DbSet<TelefonoAfiliado> TelefonosAfiliados { get; set; }
        public DbSet<TelefonoAdministrador> TelefonosAdministradores { get; set; }
        public DbSet<TelefonoRepartidor> TelefonosRepartidores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la relación entre DetallePedido y ProductoDetalle (uno a muchos)
        modelBuilder.Entity<DetallePedido>()
            .HasMany(dp => dp.Productos)
            .WithOne(pd => pd.DetallePedido)
            .HasForeignKey(pd => pd.DetallePedidoID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de la relación entre ProductoDetalle y Producto
        modelBuilder.Entity<ProductoDetalle>()
            .HasOne(pd => pd.Producto)
            .WithMany()
            .HasForeignKey(pd => pd.ProductoID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de la relación entre DetallePedido y Afiliado
        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Afiliado)
            .WithMany()
            .HasForeignKey(dp => dp.AfiliadoID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de la relación entre DetallePedido y Cliente
        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Cliente)
            .WithMany()
            .HasForeignKey(dp => dp.ClienteID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de la relación entre DetallePedido y Repartidor
        modelBuilder.Entity<DetallePedido>()
            .HasOne(dp => dp.Repartidor)
            .WithMany()
            .HasForeignKey(dp => dp.RepartidorID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de la relación entre Afiliado y TelefonoAfiliado (uno a muchos)
        modelBuilder.Entity<TelefonoAfiliado>()
            .HasOne(t => t.Afiliado)
            .WithMany(a => a.Telefonos)
            .HasForeignKey(t => t.AfiliadoID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de la relación entre Afiliado y Producto (uno a muchos)
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Afiliado)
            .WithMany(a => a.Productos)
            .HasForeignKey(p => p.AfiliadoID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de la relación entre Administrador y TelefonoAdministrador (uno a muchos)
        modelBuilder.Entity<TelefonoAdministrador>()
            .HasOne<Administrador>()
            .WithMany(a => a.Telefonos)
            .HasForeignKey(t => t.AdministradorID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de la relación entre Repartidor y TelefonoRepartidor (uno a muchos)
        modelBuilder.Entity<TelefonoRepartidor>()
            .HasOne<Repartidor>()
            .WithMany(r => r.Telefonos)
            .HasForeignKey(t => t.RepartidorID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración para generar automáticamente TelefonoRepartidorID
        modelBuilder.Entity<TelefonoRepartidor>()
            .Property(t => t.TelefonoRepartidorID)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }


public void CrearElementosDeBaseDeDatos()
{
    // Crear el procedimiento almacenado InsertarAfiliadoConTelefonos
    string procedimientoInsertarAfiliadoConTelefonos = @"
        IF OBJECT_ID('InsertarAfiliadoConTelefonos', 'P') IS NOT NULL
            DROP PROCEDURE InsertarAfiliadoConTelefonos;

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
        END";

    Database.ExecuteSqlRaw(procedimientoInsertarAfiliadoConTelefonos);

    // Eliminar el trigger trg_TelefonoDelete si existe
    string eliminarTrigger = @"
        IF OBJECT_ID('trg_TelefonoDelete', 'TR') IS NOT NULL
            DROP TRIGGER trg_TelefonoDelete;";
    Database.ExecuteSqlRaw(eliminarTrigger);

    // Crear el trigger trg_TelefonoDelete
    string triggerTelefonoDelete = @"
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
        END";

    Database.ExecuteSqlRaw(triggerTelefonoDelete);

    // Crear el trigger para liberar el RepartidorID
    string triggerLiberarRepartidor = @"
        CREATE TRIGGER trg_UpdateRepartidorOnCompletion
        ON DetallesPedido
        AFTER UPDATE
        AS
        BEGIN
            SET NOCOUNT ON;

            -- Actualizar el RepartidorID a 0 cuando el estado cambia a 'Completado'
            UPDATE DetallesPedido
            SET RepartidorID = 0
            FROM DetallesPedido dp
            JOIN inserted i ON dp.DetalleID = i.DetalleID
            WHERE i.Estado = 'Completado';
        END;";
    Database.ExecuteSqlRaw(triggerLiberarRepartidor);

    // Crear procedimiento almacenado para desactivar el trigger
    string procedimientoDesactivarTrigger = @"
        IF OBJECT_ID('DesactivarTriggerTelefonos', 'P') IS NOT NULL
            DROP PROCEDURE DesactivarTriggerTelefonos;

        CREATE PROCEDURE DesactivarTriggerTelefonos
        AS
        BEGIN
            DISABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados;
        END";

    Database.ExecuteSqlRaw(procedimientoDesactivarTrigger);

    // Crear procedimiento almacenado para activar el trigger
    string procedimientoActivarTrigger = @"
        IF OBJECT_ID('ActivarTriggerTelefonos', 'P') IS NOT NULL
            DROP PROCEDURE ActivarTriggerTelefonos;

        CREATE PROCEDURE ActivarTriggerTelefonos
        AS
        BEGIN
            ENABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados;
        END";

    Database.ExecuteSqlRaw(procedimientoActivarTrigger);
}
    }
}
