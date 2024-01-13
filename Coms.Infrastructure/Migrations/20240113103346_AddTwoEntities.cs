using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddTwoEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "LiquidationRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "ContractAnnexes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractAnnexFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractAnnexId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAnnexFields_ContractAnnexes_ContractAnnexId",
                        column: x => x.ContractAnnexId,
                        principalTable: "ContractAnnexes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecordFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LiquidationRecordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecordFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidationRecordFields_LiquidationRecords_LiquidationRecordId",
                        column: x => x.LiquidationRecordId,
                        principalTable: "LiquidationRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexFields_ContractAnnexId",
                table: "ContractAnnexFields",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordFields_LiquidationRecordId",
                table: "LiquidationRecordFields",
                column: "LiquidationRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAnnexFields");

            migrationBuilder.DropTable(
                name: "LiquidationRecordFields");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "LiquidationRecords");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "ContractAnnexes");
        }
    }
}
