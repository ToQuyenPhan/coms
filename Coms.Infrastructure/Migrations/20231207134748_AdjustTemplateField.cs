using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AdjustTemplateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "TemplateFields");

            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "TemplateFields");

            migrationBuilder.DropColumn(
                name: "PositionY",
                table: "TemplateFields");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "TemplateFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionX",
                table: "TemplateFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionY",
                table: "TemplateFields",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
