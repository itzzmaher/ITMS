using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class addTour2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTour",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuId = table.Column<Guid>(nullable: false),
                    PlacesId = table.Column<int>(nullable: false),
                    GuiderId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTour_tblGuiderCertificate_GuiderId",
                        column: x => x.GuiderId,
                        principalTable: "tblGuiderCertificate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblTour_tblPlaces_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "tblPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblTour_GuiderId",
                table: "tblTour",
                column: "GuiderId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTour_PlacesId",
                table: "tblTour",
                column: "PlacesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTour");
        }
    }
}
