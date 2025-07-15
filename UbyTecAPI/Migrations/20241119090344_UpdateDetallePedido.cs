using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDetallePedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Productos_ProductoID",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "DetallesPedido");

            migrationBuilder.RenameColumn(
                name: "ProductoID",
                table: "DetallesPedido",
                newName: "RepartidorID");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "DetallesPedido",
                newName: "ClienteID");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesPedido_ProductoID",
                table: "DetallesPedido",
                newName: "IX_DetallesPedido_RepartidorID");

            migrationBuilder.AddColumn<int>(
                name: "AfiliadoID",
                table: "DetallesPedido",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "DetallesPedido",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductosDetalle",
                columns: table => new
                {
                    ProductoDetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    DetallePedidoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosDetalle", x => x.ProductoDetalleID);
                    table.ForeignKey(
                        name: "FK_ProductosDetalle_DetallesPedido_DetallePedidoID",
                        column: x => x.DetallePedidoID,
                        principalTable: "DetallesPedido",
                        principalColumn: "DetalleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductosDetalle_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_AfiliadoID",
                table: "DetallesPedido",
                column: "AfiliadoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_ClienteID",
                table: "DetallesPedido",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosDetalle_DetallePedidoID",
                table: "ProductosDetalle",
                column: "DetallePedidoID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosDetalle_ProductoID",
                table: "ProductosDetalle",
                column: "ProductoID");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Afiliados_AfiliadoID",
                table: "DetallesPedido",
                column: "AfiliadoID",
                principalTable: "Afiliados",
                principalColumn: "AfiliadoID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Clientes_ClienteID",
                table: "DetallesPedido",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Repartidores_RepartidorID",
                table: "DetallesPedido",
                column: "RepartidorID",
                principalTable: "Repartidores",
                principalColumn: "RepartidorID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Afiliados_AfiliadoID",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Clientes_ClienteID",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Repartidores_RepartidorID",
                table: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "ProductosDetalle");

            migrationBuilder.DropIndex(
                name: "IX_DetallesPedido_AfiliadoID",
                table: "DetallesPedido");

            migrationBuilder.DropIndex(
                name: "IX_DetallesPedido_ClienteID",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "AfiliadoID",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "DetallesPedido");

            migrationBuilder.RenameColumn(
                name: "RepartidorID",
                table: "DetallesPedido",
                newName: "ProductoID");

            migrationBuilder.RenameColumn(
                name: "ClienteID",
                table: "DetallesPedido",
                newName: "Cantidad");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesPedido_RepartidorID",
                table: "DetallesPedido",
                newName: "IX_DetallesPedido_ProductoID");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "DetallesPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Productos_ProductoID",
                table: "DetallesPedido",
                column: "ProductoID",
                principalTable: "Productos",
                principalColumn: "ProductoID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
