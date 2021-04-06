using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ogłoszenia_Drobne_Web_App.Data.Migrations
{
    public partial class DBvol1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atributes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlackWords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    Extension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfferReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(nullable: false),
                    ReportingUserId = table.Column<int>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ReportingUserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferReports_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferReports_AspNetUsers_ReportingUserId1",
                        column: x => x.ReportingUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfferAtributes",
                columns: table => new
                {
                    OfferId = table.Column<int>(nullable: false),
                    AtributeId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferAtributes", x => new { x.AtributeId, x.OfferId });
                    table.ForeignKey(
                        name: "FK_OfferAtributes_Atributes_AtributeId",
                        column: x => x.AtributeId,
                        principalTable: "Atributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferAtributes_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atributes_CategoryId",
                table: "Atributes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OfferId",
                table: "Files",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferAtributes_OfferId",
                table: "OfferAtributes",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferReports_OfferId",
                table: "OfferReports",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferReports_ReportingUserId1",
                table: "OfferReports",
                column: "ReportingUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackWords");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "OfferAtributes");

            migrationBuilder.DropTable(
                name: "OfferReports");

            migrationBuilder.DropTable(
                name: "Atributes");
        }
    }
}
