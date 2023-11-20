using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class DeleteOriginalVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalVersion",
                table: "Contracts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalVersion",
                table: "Contracts",
                type: "int",
                nullable: true);
        }
    }
}
