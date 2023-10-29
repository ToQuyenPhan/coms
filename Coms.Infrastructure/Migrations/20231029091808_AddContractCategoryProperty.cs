using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddContractCategoryProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractCategoryId",
                table: "LiquidationRecordTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordTemplates_ContractCategoryId",
                table: "LiquidationRecordTemplates",
                column: "ContractCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidationRecordTemplates_ContractCategories_ContractCategoryId",
                table: "LiquidationRecordTemplates",
                column: "ContractCategoryId",
                principalTable: "ContractCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiquidationRecordTemplates_ContractCategories_ContractCategoryId",
                table: "LiquidationRecordTemplates");

            migrationBuilder.DropIndex(
                name: "IX_LiquidationRecordTemplates_ContractCategoryId",
                table: "LiquidationRecordTemplates");

            migrationBuilder.DropColumn(
                name: "ContractCategoryId",
                table: "LiquidationRecordTemplates");
        }
    }
}
