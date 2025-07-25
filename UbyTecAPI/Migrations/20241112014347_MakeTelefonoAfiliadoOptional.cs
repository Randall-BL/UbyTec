﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UbyTecAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeTelefonoAfiliadoOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AfiliadoID",
                table: "TelefonosAfiliados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AfiliadoID",
                table: "TelefonosAfiliados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
