using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ogłoszenia_Drobne_Web_App.Data.Migrations
{
    public partial class atributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Atributes");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Atributes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Value",
                table: "Atributes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoubleAtribute_Value",
                table: "Atributes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberAtribute_Value",
                table: "Atributes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAtribute_Value",
                table: "Atributes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Atributes");

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

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Atributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
