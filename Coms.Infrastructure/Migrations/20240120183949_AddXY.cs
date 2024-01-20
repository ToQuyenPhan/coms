using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddXY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "ContractFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "ContractFiles",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "ContractFiles");
        }
    }
}
