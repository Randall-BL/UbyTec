using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTelefonosAdministradoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Administradores");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Administradores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.CreateTable(
                name: "TelefonosAdministradores",
                columns: table => new
                {
                    TelefonoAdministradorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministradorID = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonosAdministradores", x => x.TelefonoAdministradorID);
                    table.ForeignKey(
                        name: "FK_TelefonosAdministradores_Administradores_AdministradorID",
                        column: x => x.AdministradorID,
                        principalTable: "Administradores",
                        principalColumn: "AdministradorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelefonosAdministradores_AdministradorID",
                table: "TelefonosAdministradores",
                column: "AdministradorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelefonosAdministradores");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Administradores",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Administradores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
