using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class AddCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuId",
                table: "tblRating",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblRating",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "tblPlaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuId",
                table: "tblRating");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblRating");

            migrationBuilder.DropColumn(
                name: "location",
                table: "tblPlaces");
        }
    }
}
