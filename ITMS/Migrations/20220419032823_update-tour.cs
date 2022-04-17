using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class updatetour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTourRegisteration_tblGuiderCertificate_GuiderId",
                table: "tblTourRegisteration");

            migrationBuilder.DropIndex(
                name: "IX_tblTourRegisteration_GuiderId",
                table: "tblTourRegisteration");

            migrationBuilder.DropColumn(
                name: "GuiderId",
                table: "tblTourRegisteration");

            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "tblTourRegisteration",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TourName",
                table: "tblTour",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblTourRegisteration_TourId",
                table: "tblTourRegisteration",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTourRegisteration_tblTour_TourId",
                table: "tblTourRegisteration",
                column: "TourId",
                principalTable: "tblTour",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTourRegisteration_tblTour_TourId",
                table: "tblTourRegisteration");

            migrationBuilder.DropIndex(
                name: "IX_tblTourRegisteration_TourId",
                table: "tblTourRegisteration");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "tblTourRegisteration");

            migrationBuilder.DropColumn(
                name: "TourName",
                table: "tblTour");

            migrationBuilder.AddColumn<int>(
                name: "GuiderId",
                table: "tblTourRegisteration",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblTourRegisteration_GuiderId",
                table: "tblTourRegisteration",
                column: "GuiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTourRegisteration_tblGuiderCertificate_GuiderId",
                table: "tblTourRegisteration",
                column: "GuiderId",
                principalTable: "tblGuiderCertificate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
