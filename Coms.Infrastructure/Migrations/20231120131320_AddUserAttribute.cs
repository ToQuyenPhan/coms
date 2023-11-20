using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddUserAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_UserId",
                table: "Templates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Users_UserId",
                table: "Templates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Users_UserId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_UserId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Templates");
        }
    }
}
