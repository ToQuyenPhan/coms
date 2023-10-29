using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddContractAnnexEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Contracts",
                newName: "UpdatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LiquidationRecordTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "LiquidationRecordTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ContractTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ContractAnnexTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAnnexTemplates_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractAnnexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractAnnexName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    ContractAnnexTemplateID = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAnnexes_ContractAnnexTemplates_ContractAnnexTemplateID",
                        column: x => x.ContractAnnexTemplateID,
                        principalTable: "ContractAnnexTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractAnnexes_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractAnnexes_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexes_ContractAnnexTemplateID",
                table: "ContractAnnexes",
                column: "ContractAnnexTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexes_ContractId",
                table: "ContractAnnexes",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexes_DocumentId",
                table: "ContractAnnexes",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexTemplates_ContractCategoryId",
                table: "ContractAnnexTemplates",
                column: "ContractCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAnnexes");

            migrationBuilder.DropTable(
                name: "ContractAnnexTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LiquidationRecordTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "LiquidationRecordTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ContractTemplates");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Contracts",
                newName: "UpdateDate");
        }
    }
}
