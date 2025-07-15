using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePedidoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Afiliados_AfiliadoID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Repartidores_RepartidorID",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_AfiliadoID",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_ClienteID",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_RepartidorID",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_DetallesPedido_PedidoID",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "FechaPedido",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "PedidoID",
                table: "DetallesPedido");

            migrationBuilder.RenameColumn(
                name: "EstadoPedido",
                table: "Pedidos",
                newName: "NombreRepartidor");

            migrationBuilder.AddColumn<int>(
                name: "CantidadProducto",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoProducto",
                table: "Pedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreComercio",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "RepartidorID",
                table: "DetallesPedido",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadProducto",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "MontoProducto",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "NombreComercio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "NombreRepartidor",
                table: "Pedidos",
                newName: "EstadoPedido");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPedido",
                table: "Pedidos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "RepartidorID",
                table: "DetallesPedido",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PedidoID",
                table: "DetallesPedido",
                type: "int",
                nullable: true);

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
                name: "IX_DetallesPedido_PedidoID",
                table: "DetallesPedido",
                column: "PedidoID");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido",
                column: "PedidoID",
                principalTable: "Pedidos",
                principalColumn: "PedidoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Afiliados_AfiliadoID",
                table: "Pedidos",
                column: "AfiliadoID",
                principalTable: "Afiliados",
                principalColumn: "AfiliadoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Repartidores_RepartidorID",
                table: "Pedidos",
                column: "RepartidorID",
                principalTable: "Repartidores",
                principalColumn: "RepartidorID");
        }
    }
}
