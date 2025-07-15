using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAdministradorToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministradorID",
                table: "Afiliados");

            migrationBuilder.AddColumn<string>(
                name: "Administrador",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrador",
                table: "Afiliados");

            migrationBuilder.AddColumn<int>(
                name: "AdministradorID",
                table: "Afiliados",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
