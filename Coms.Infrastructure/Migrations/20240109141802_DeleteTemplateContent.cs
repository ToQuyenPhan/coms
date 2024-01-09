using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class DeleteTemplateContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateContents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateContents",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateContents", x => new { x.ContractId, x.TemplateFieldId });
                    table.ForeignKey(
                        name: "FK_TemplateContents_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateContents_TemplateFields_TemplateFieldId",
                        column: x => x.TemplateFieldId,
                        principalTable: "TemplateFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateContents_TemplateFieldId",
                table: "TemplateContents",
                column: "TemplateFieldId");
        }
    }
}
