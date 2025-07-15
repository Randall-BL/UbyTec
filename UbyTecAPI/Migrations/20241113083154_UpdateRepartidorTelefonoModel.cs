using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepartidorTelefonoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Repartidores");

            migrationBuilder.CreateTable(
                name: "TelefonosRepartidores",
                columns: table => new
                {
                    TelefonoRepartidorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepartidorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonosRepartidores", x => x.TelefonoRepartidorID);
                    table.ForeignKey(
                        name: "FK_TelefonosRepartidores_Repartidores_RepartidorID",
                        column: x => x.RepartidorID,
                        principalTable: "Repartidores",
                        principalColumn: "RepartidorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelefonosRepartidores_RepartidorID",
                table: "TelefonosRepartidores",
                column: "RepartidorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelefonosRepartidores");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Repartidores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
