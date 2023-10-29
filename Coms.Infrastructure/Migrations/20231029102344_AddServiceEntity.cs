using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddServiceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractCosts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractCosts_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractAnnexCosts",
                columns: table => new
                {
                    ContractCostId = table.Column<int>(type: "int", nullable: false),
                    ContractAnnexId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexCosts", x => new { x.ContractAnnexId, x.ContractCostId });
                    table.ForeignKey(
                        name: "FK_ContractAnnexCosts_ContractAnnexes_ContractAnnexId",
                        column: x => x.ContractAnnexId,
                        principalTable: "ContractAnnexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractAnnexCosts_ContractCosts_ContractCostId",
                        column: x => x.ContractCostId,
                        principalTable: "ContractCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexCosts_ContractCostId",
                table: "ContractAnnexCosts",
                column: "ContractCostId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCosts_ContractId",
                table: "ContractCosts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCosts_ServiceId",
                table: "ContractCosts",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAnnexCosts");

            migrationBuilder.DropTable(
                name: "ContractCosts");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
