using Microsoft.EntityFrameworkCore.Migrations;

namespace Ogłoszenia_Drobne_Web_App.Data.Migrations
{
    public partial class ReportOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "reported",
                table: "Offers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reported",
                table: "Offers");
        }
    }
}
