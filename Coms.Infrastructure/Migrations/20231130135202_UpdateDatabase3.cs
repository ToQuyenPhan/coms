using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateDatabase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accesses_Contracts_ContractId",
                table: "Accesses");

            migrationBuilder.DropTable(
                name: "UserAccesses");

            migrationBuilder.DropIndex(
                name: "IX_Accesses_ContractId",
                table: "Accesses");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Accesses");

            migrationBuilder.AddColumn<int>(
                name: "ContractCategoryId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Flows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flows_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlowDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowRole = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    FlowID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowDetails_Flows_FlowID",
                        column: x => x.FlowID,
                        principalTable: "Flows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFlowDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    FlowDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlowDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFlowDetails_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFlowDetails_FlowDetails_FlowDetailId",
                        column: x => x.FlowDetailId,
                        principalTable: "FlowDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFlowDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ContractCategoryId",
                table: "Services",
                column: "ContractCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowDetails_FlowID",
                table: "FlowDetails",
                column: "FlowID");

            migrationBuilder.CreateIndex(
                name: "IX_Flows_ContractCategoryId",
                table: "Flows",
                column: "ContractCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_ContractId",
                table: "UserFlowDetails",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_FlowDetailId",
                table: "UserFlowDetails",
                column: "FlowDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_UserId",
                table: "UserFlowDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ContractCategories_ContractCategoryId",
                table: "Services",
                column: "ContractCategoryId",
                principalTable: "ContractCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ContractCategories_ContractCategoryId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "UserFlowDetails");

            migrationBuilder.DropTable(
                name: "FlowDetails");

            migrationBuilder.DropTable(
                name: "Flows");

            migrationBuilder.DropIndex(
                name: "IX_Services_ContractCategoryId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ContractCategoryId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Accesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserAccesses",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccesses", x => new { x.UserId, x.AccessId });
                    table.ForeignKey(
                        name: "FK_UserAccesses_Accesses_AccessId",
                        column: x => x.AccessId,
                        principalTable: "Accesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accesses_ContractId",
                table: "Accesses",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_AccessId",
                table: "UserAccesses",
                column: "AccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accesses_Contracts_ContractId",
                table: "Accesses",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}
