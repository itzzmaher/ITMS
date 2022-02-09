using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class AddCertificate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblGuiderCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuId = table.Column<Guid>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ImgName = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGuiderCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblGuiderCertificate_tblCity_CityId",
                        column: x => x.CityId,
                        principalTable: "tblCity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblGuiderCertificate_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblGuiderCertificate_CityId",
                table: "tblGuiderCertificate",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblGuiderCertificate_UserId",
                table: "tblGuiderCertificate",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblGuiderCertificate");
        }
    }
}
