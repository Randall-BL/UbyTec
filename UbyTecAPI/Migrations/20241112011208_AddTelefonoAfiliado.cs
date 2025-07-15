using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTelefonoAfiliado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Afiliados");

            migrationBuilder.CreateTable(
                name: "TelefonosAfiliados",
                columns: table => new
                {
                    TelefonoAfiliadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AfiliadoID = table.Column<int>(type: "int", nullable: false),
                    NumeroTelefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonosAfiliados", x => x.TelefonoAfiliadoID);
                    table.ForeignKey(
                        name: "FK_TelefonosAfiliados_Afiliados_AfiliadoID",
                        column: x => x.AfiliadoID,
                        principalTable: "Afiliados",
                        principalColumn: "AfiliadoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelefonosAfiliados_AfiliadoID",
                table: "TelefonosAfiliados",
                column: "AfiliadoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelefonosAfiliados");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
