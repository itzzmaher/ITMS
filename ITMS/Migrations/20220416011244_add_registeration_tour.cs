using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class add_registeration_tour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "tblTour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImgName",
                table: "tblTour",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblTour",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxTourist",
                table: "tblTour",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "tblTour",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tblRegisterationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRegisterationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTourRegisteration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuId = table.Column<Guid>(nullable: false),
                    GuiderId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RegStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTourRegisteration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTourRegisteration_tblGuiderCertificate_GuiderId",
                        column: x => x.GuiderId,
                        principalTable: "tblGuiderCertificate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTourRegisteration_tblRegisterationStatus_RegStatusId",
                        column: x => x.RegStatusId,
                        principalTable: "tblRegisterationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTourRegisteration_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblTourRegisteration_GuiderId",
                table: "tblTourRegisteration",
                column: "GuiderId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTourRegisteration_RegStatusId",
                table: "tblTourRegisteration",
                column: "RegStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTourRegisteration_UserId",
                table: "tblTourRegisteration",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTourRegisteration");

            migrationBuilder.DropTable(
                name: "tblRegisterationStatus");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "ImgName",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "MaxTourist",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "tblTour");
        }
    }
}
