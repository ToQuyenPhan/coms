using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddTermEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractAnnexTerms",
                columns: table => new
                {
                    ContractAnnexId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexTerms", x => new { x.ContractAnnexId, x.Number });
                    table.ForeignKey(
                        name: "FK_ContractAnnexTerms_ContractAnnexes_ContractAnnexId",
                        column: x => x.ContractAnnexId,
                        principalTable: "ContractAnnexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractTemplateTerms",
                columns: table => new
                {
                    ContractTemplateId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTemplateTerms", x => new { x.ContractTemplateId, x.Number });
                    table.ForeignKey(
                        name: "FK_ContractTemplateTerms_ContractTemplates_ContractTemplateId",
                        column: x => x.ContractTemplateId,
                        principalTable: "ContractTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractTerms",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTerms", x => new { x.ContractId, x.Number });
                    table.ForeignKey(
                        name: "FK_ContractTerms_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecordTerms",
                columns: table => new
                {
                    LiquidationRecordId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecordTerms", x => new { x.LiquidationRecordId, x.Number });
                    table.ForeignKey(
                        name: "FK_LiquidationRecordTerms_LiquidationRecords_LiquidationRecordId",
                        column: x => x.LiquidationRecordId,
                        principalTable: "LiquidationRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAnnexTerms");

            migrationBuilder.DropTable(
                name: "ContractTemplateTerms");

            migrationBuilder.DropTable(
                name: "ContractTerms");

            migrationBuilder.DropTable(
                name: "LiquidationRecordTerms");
        }
    }
}
