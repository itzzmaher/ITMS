using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class update_registeration_tour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "tblTourRegisteration",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TourDate",
                table: "tblTourRegisteration",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "tblTourRegisteration");

            migrationBuilder.DropColumn(
                name: "TourDate",
                table: "tblTourRegisteration");
        }
    }
}
