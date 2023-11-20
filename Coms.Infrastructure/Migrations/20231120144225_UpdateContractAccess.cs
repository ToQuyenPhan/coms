using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateContractAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "OriginalVersion",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Accesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accesses_ContractId",
                table: "Accesses",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accesses_Contracts_ContractId",
                table: "Accesses",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accesses_Contracts_ContractId",
                table: "Accesses");

            migrationBuilder.DropIndex(
                name: "IX_Accesses_ContractId",
                table: "Accesses");

            migrationBuilder.DropColumn(
                name: "OriginalVersion",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Accesses");

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
    }
}
