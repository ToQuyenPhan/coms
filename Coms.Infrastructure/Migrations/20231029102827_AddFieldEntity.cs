using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddFieldEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields");

            migrationBuilder.AlterColumn<int>(
                name: "ContractTemplateId",
                table: "TemplateFields",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ContractFields",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractFields", x => new { x.ContractId, x.TemplateFieldId });
                    table.ForeignKey(
                        name: "FK_ContractFields_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractFields_TemplateFields_TemplateFieldId",
                        column: x => x.TemplateFieldId,
                        principalTable: "TemplateFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractFields_TemplateFieldId",
                table: "ContractFields",
                column: "TemplateFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields",
                column: "ContractTemplateId",
                principalTable: "ContractTemplates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields");

            migrationBuilder.DropTable(
                name: "ContractFields");

            migrationBuilder.AlterColumn<int>(
                name: "ContractTemplateId",
                table: "TemplateFields",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields",
                column: "ContractTemplateId",
                principalTable: "ContractTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
