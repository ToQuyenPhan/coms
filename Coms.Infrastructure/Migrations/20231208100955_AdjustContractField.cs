using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AdjustContractField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractFields_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractFields_ContractId",
                table: "ContractFields",
                column: "ContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractFields");
        }
    }
}
