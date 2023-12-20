using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateFlowDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFlowDetails_Users_UserId",
                table: "UserFlowDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserFlowDetails_UserId",
                table: "UserFlowDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFlowDetails");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FlowDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowDetails_UserId",
                table: "FlowDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDetails_Users_UserId",
                table: "FlowDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowDetails_Users_UserId",
                table: "FlowDetails");

            migrationBuilder.DropIndex(
                name: "IX_FlowDetails_UserId",
                table: "FlowDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlowDetails");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserFlowDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_UserId",
                table: "UserFlowDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlowDetails_Users_UserId",
                table: "UserFlowDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
