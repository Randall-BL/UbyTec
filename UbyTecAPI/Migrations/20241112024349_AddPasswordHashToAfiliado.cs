using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashToAfiliado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Afiliados",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Afiliados");
        }
    }
}
