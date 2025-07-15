using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToAfiliado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Afiliados");
        }
    }
}
