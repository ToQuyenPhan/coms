using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddAccessRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AccessId",
                table: "Contracts",
                column: "AccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Accesses_AccessId",
                table: "Contracts",
                column: "AccessId",
                principalTable: "Accesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Accesses_AccessId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AccessId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AccessId",
                table: "Contracts");
        }
    }
}
