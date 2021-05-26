using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ogłoszenia_Drobne_Web_App.Data.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Atributes");

            migrationBuilder.DropColumn(
                name: "DoubleAtribute_Value",
                table: "Atributes");

            migrationBuilder.DropColumn(
                name: "NumberAtribute_Value",
                table: "Atributes");

            migrationBuilder.DropColumn(
                name: "TextAtribute_Value",
                table: "Atributes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Value",
                table: "Atributes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoubleAtribute_Value",
                table: "Atributes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberAtribute_Value",
                table: "Atributes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAtribute_Value",
                table: "Atributes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
