using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class add_car : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblFuel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuelName = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFuel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    FuelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCar_tblFuel_FuelId",
                        column: x => x.FuelId,
                        principalTable: "tblFuel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblCar_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCar_FuelId",
                table: "tblCar",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCar_UserId",
                table: "tblCar",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCar");

            migrationBuilder.DropTable(
                name: "tblFuel");
        }
    }
}
