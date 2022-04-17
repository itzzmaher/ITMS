using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class update_car : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "tblFuel",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "tblFuel",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
