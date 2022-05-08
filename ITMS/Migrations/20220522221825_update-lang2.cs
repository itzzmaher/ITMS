using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMS.Migrations
{
    public partial class updatelang2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblLangGuider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuiderId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLangGuider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblLangGuider_tblGuiderCertificate_GuiderId",
                        column: x => x.GuiderId,
                        principalTable: "tblGuiderCertificate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblLangGuider_tblLanguage_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "tblLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblLangGuider_GuiderId",
                table: "tblLangGuider",
                column: "GuiderId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLangGuider_LanguageId",
                table: "tblLangGuider",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblLangGuider");
        }
    }
}
