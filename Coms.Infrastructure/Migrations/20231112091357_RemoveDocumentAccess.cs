using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class RemoveDocumentAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Documents_DocumentId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "DocumentAccesses");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_DocumentId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "ActionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistories_ContractId",
                table: "ActionHistories",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ActionHistories_ContractId",
                table: "ActionHistories");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "ActionHistories");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocumentAccesses",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    AccessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAccesses", x => new { x.DocumentId, x.AccessId });
                    table.ForeignKey(
                        name: "FK_DocumentAccesses_Accesses_AccessId",
                        column: x => x.AccessId,
                        principalTable: "Accesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentAccesses_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DocumentId",
                table: "Contracts",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccesses_AccessId",
                table: "DocumentAccesses",
                column: "AccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Documents_DocumentId",
                table: "Contracts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
