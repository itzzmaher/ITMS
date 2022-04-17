using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class addMoment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblMoments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Details = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GuId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMoments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblMoments_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    MomentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFile_tblMoments_MomentId",
                        column: x => x.MomentId,
                        principalTable: "tblMoments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFile_MomentId",
                table: "tblFile",
                column: "MomentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMoments_UserId",
                table: "tblMoments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFile");

            migrationBuilder.DropTable(
                name: "tblMoments");
        }
    }
}
