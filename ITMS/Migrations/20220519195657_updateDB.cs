using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class updateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "tblFile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "tblCar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "tblFile");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "tblCar");
        }
    }
}
