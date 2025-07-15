using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyAdministradorInAfiliado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Afiliados_Administradores_AdministradorID",
                table: "Afiliados");

            migrationBuilder.DropIndex(
                name: "IX_Afiliados_AdministradorID",
                table: "Afiliados");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Afiliados_AdministradorID",
                table: "Afiliados",
                column: "AdministradorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Afiliados_Administradores_AdministradorID",
                table: "Afiliados",
                column: "AdministradorID",
                principalTable: "Administradores",
                principalColumn: "AdministradorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
