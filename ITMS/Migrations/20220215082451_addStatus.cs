using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class addStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "tblGuiderCertificate");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "tblGuiderCertificate",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblCertificate_status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCertificate_status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblGuiderCertificate_StatusId",
                table: "tblGuiderCertificate",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblGuiderCertificate_tblCertificate_status_StatusId",
                table: "tblGuiderCertificate",
                column: "StatusId",
                principalTable: "tblCertificate_status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblGuiderCertificate_tblCertificate_status_StatusId",
                table: "tblGuiderCertificate");

            migrationBuilder.DropTable(
                name: "tblCertificate_status");

            migrationBuilder.DropIndex(
                name: "IX_tblGuiderCertificate_StatusId",
                table: "tblGuiderCertificate");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "tblGuiderCertificate");

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "tblGuiderCertificate",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
