using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddLiquidationRecordEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiquidationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecordTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LiquidationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecordTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidationRecordTemplates_LiquidationTypes_LiquidationTypeId",
                        column: x => x.LiquidationTypeId,
                        principalTable: "LiquidationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiquidationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    LiquidationRecordTemplateId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidationRecords_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiquidationRecords_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId");
                    table.ForeignKey(
                        name: "FK_LiquidationRecords_LiquidationRecordTemplates_LiquidationRecordTemplateId",
                        column: x => x.LiquidationRecordTemplateId,
                        principalTable: "LiquidationRecordTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecords_ContractId",
                table: "LiquidationRecords",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecords_DocumentId",
                table: "LiquidationRecords",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecords_LiquidationRecordTemplateId",
                table: "LiquidationRecords",
                column: "LiquidationRecordTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordTemplates_LiquidationTypeId",
                table: "LiquidationRecordTemplates",
                column: "LiquidationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiquidationRecords");

            migrationBuilder.DropTable(
                name: "LiquidationRecordTemplates");

            migrationBuilder.DropTable(
                name: "LiquidationTypes");
        }
    }
}
