using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class ChangeSenderIdPRoperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Templates_TemplateID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_Users_UserId",
                table: "PartnerReviews");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "PartnerReviews");

            migrationBuilder.RenameColumn(
                name: "TemplateID",
                table: "Contracts",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_TemplateID",
                table: "Contracts",
                newName: "IX_Contracts_TemplateId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PartnerReviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Templates_TemplateId",
                table: "Contracts",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_Users_UserId",
                table: "PartnerReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Templates_TemplateId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_Users_UserId",
                table: "PartnerReviews");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "Contracts",
                newName: "TemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_TemplateId",
                table: "Contracts",
                newName: "IX_Contracts_TemplateID");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PartnerReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "PartnerReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Templates_TemplateID",
                table: "Contracts",
                column: "TemplateID",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_Users_UserId",
                table: "PartnerReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
