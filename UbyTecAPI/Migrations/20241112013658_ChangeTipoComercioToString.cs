using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTipoComercioToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Afiliados_TiposComercio_TipoComercioID",
                table: "Afiliados");

            migrationBuilder.AlterColumn<int>(
                name: "TipoComercioID",
                table: "Afiliados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TipoComercio",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Afiliados_TiposComercio_TipoComercioID",
                table: "Afiliados",
                column: "TipoComercioID",
                principalTable: "TiposComercio",
                principalColumn: "TipoComercioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Afiliados_TiposComercio_TipoComercioID",
                table: "Afiliados");

            migrationBuilder.DropColumn(
                name: "TipoComercio",
                table: "Afiliados");

            migrationBuilder.AlterColumn<int>(
                name: "TipoComercioID",
                table: "Afiliados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Afiliados_TiposComercio_TipoComercioID",
                table: "Afiliados",
                column: "TipoComercioID",
                principalTable: "TiposComercio",
                principalColumn: "TipoComercioID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
