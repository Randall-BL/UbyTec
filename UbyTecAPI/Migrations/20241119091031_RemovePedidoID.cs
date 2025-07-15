using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovePedidoID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido");

            migrationBuilder.AlterColumn<int>(
                name: "PedidoID",
                table: "DetallesPedido",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido",
                column: "PedidoID",
                principalTable: "Pedidos",
                principalColumn: "PedidoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido");

            migrationBuilder.AlterColumn<int>(
                name: "PedidoID",
                table: "DetallesPedido",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Pedidos_PedidoID",
                table: "DetallesPedido",
                column: "PedidoID",
                principalTable: "Pedidos",
                principalColumn: "PedidoID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
