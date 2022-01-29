using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class add_cat_rate_city : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "tblPlaces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "tblPlaces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<int>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    PlacesId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRating_tblPlaces_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "tblPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblRating_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPlaces_CategoryId",
                table: "tblPlaces",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPlaces_CityId",
                table: "tblPlaces",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRating_PlacesId",
                table: "tblRating",
                column: "PlacesId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRating_UserId",
                table: "tblRating",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPlaces_tblCategory_CategoryId",
                table: "tblPlaces",
                column: "CategoryId",
                principalTable: "tblCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPlaces_tblCity_CityId",
                table: "tblPlaces",
                column: "CityId",
                principalTable: "tblCity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPlaces_tblCategory_CategoryId",
                table: "tblPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPlaces_tblCity_CityId",
                table: "tblPlaces");

            migrationBuilder.DropTable(
                name: "tblCategory");

            migrationBuilder.DropTable(
                name: "tblCity");

            migrationBuilder.DropTable(
                name: "tblRating");

            migrationBuilder.DropIndex(
                name: "IX_tblPlaces_CategoryId",
                table: "tblPlaces");

            migrationBuilder.DropIndex(
                name: "IX_tblPlaces_CityId",
                table: "tblPlaces");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "tblPlaces");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "tblPlaces");
        }
    }
}
