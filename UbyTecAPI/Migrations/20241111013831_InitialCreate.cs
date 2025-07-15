using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    AdministradorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionProvincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionCanton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionDistrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.AdministradorID);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionProvincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionCanton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionDistrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Repartidores",
                columns: table => new
                {
                    RepartidorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionProvincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionCanton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionDistrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repartidores", x => x.RepartidorID);
                });

            migrationBuilder.CreateTable(
                name: "TiposComercio",
                columns: table => new
                {
                    TipoComercioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComercio", x => x.TipoComercioID);
                });

            migrationBuilder.CreateTable(
                name: "EstadoRepartidor",
                columns: table => new
                {
                    EstadoRepartidorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepartidorID = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoRepartidor", x => x.EstadoRepartidorID);
                    table.ForeignKey(
                        name: "FK_EstadoRepartidor_Repartidores_RepartidorID",
                        column: x => x.RepartidorID,
                        principalTable: "Repartidores",
                        principalColumn: "RepartidorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Afiliados",
                columns: table => new
                {
                    AfiliadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCedulaJuridica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreComercio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoComercioID = table.Column<int>(type: "int", nullable: false),
                    DireccionProvincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionCanton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionDistrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroSINPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdministradorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afiliados", x => x.AfiliadoID);
                    table.ForeignKey(
                        name: "FK_Afiliados_Administradores_AdministradorID",
                        column: x => x.AdministradorID,
                        principalTable: "Administradores",
                        principalColumn: "AdministradorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Afiliados_TiposComercio_TipoComercioID",
                        column: x => x.TipoComercioID,
                        principalTable: "TiposComercio",
                        principalColumn: "TipoComercioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    PedidoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    AfiliadoID = table.Column<int>(type: "int", nullable: false),
                    RepartidorID = table.Column<int>(type: "int", nullable: true),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoPedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.PedidoID);
                    table.ForeignKey(
                        name: "FK_Pedidos_Afiliados_AfiliadoID",
                        column: x => x.AfiliadoID,
                        principalTable: "Afiliados",
                        principalColumn: "AfiliadoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Repartidores_RepartidorID",
                        column: x => x.RepartidorID,
                        principalTable: "Repartidores",
                        principalColumn: "RepartidorID");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AfiliadoID = table.Column<int>(type: "int", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoID);
                    table.ForeignKey(
                        name: "FK_Productos_Afiliados_AfiliadoID",
                        column: x => x.AfiliadoID,
                        principalTable: "Afiliados",
                        principalColumn: "AfiliadoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedido",
                columns: table => new
                {
                    DetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoID = table.Column<int>(type: "int", nullable: false),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedido", x => x.DetalleID);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Pedidos_PedidoID",
                        column: x => x.PedidoID,
                        principalTable: "Pedidos",
                        principalColumn: "PedidoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afiliados_AdministradorID",
                table: "Afiliados",
                column: "AdministradorID");

            migrationBuilder.CreateIndex(
                name: "IX_Afiliados_TipoComercioID",
                table: "Afiliados",
                column: "TipoComercioID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_PedidoID",
                table: "DetallesPedido",
                column: "PedidoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_ProductoID",
                table: "DetallesPedido",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoRepartidor_RepartidorID",
                table: "EstadoRepartidor",
                column: "RepartidorID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_AfiliadoID",
                table: "Pedidos",
                column: "AfiliadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteID",
                table: "Pedidos",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_RepartidorID",
                table: "Pedidos",
                column: "RepartidorID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_AfiliadoID",
                table: "Productos",
                column: "AfiliadoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "EstadoRepartidor");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Repartidores");

            migrationBuilder.DropTable(
                name: "Afiliados");

            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "TiposComercio");
        }
    }
}
