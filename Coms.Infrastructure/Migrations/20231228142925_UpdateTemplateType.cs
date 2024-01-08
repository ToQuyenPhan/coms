using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateTemplateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TemplateTypes_TemplateTypeId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_TemplateTypeId",
                table: "Templates");

            migrationBuilder.RenameColumn(
                name: "TemplateTypeId",
                table: "Templates",
                newName: "TemplateType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemplateType",
                table: "Templates",
                newName: "TemplateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TemplateTypeId",
                table: "Templates",
                column: "TemplateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TemplateTypes_TemplateTypeId",
                table: "Templates",
                column: "TemplateTypeId",
                principalTable: "TemplateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
