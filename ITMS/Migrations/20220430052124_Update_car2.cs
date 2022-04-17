using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class Update_car2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarName",
                table: "tblCar");

            migrationBuilder.AddColumn<int>(
                name: "FuelEco",
                table: "tblCar",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelEco",
                table: "tblCar");

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "tblCar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
