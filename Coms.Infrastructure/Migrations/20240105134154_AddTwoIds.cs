using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddTwoIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_Contracts_ContractId",
                table: "PartnerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFlowDetails_Contracts_ContractId",
                table: "UserFlowDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "UserFlowDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ContractAnnexId",
                table: "UserFlowDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiquidationRecordId",
                table: "UserFlowDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractAnnexId",
                table: "PartnerSigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiquidationRecordId",
                table: "PartnerSigns",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "PartnerReviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ContractAnnexId",
                table: "PartnerReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiquidationRecordId",
                table: "PartnerReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "ActionHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ContractAnnexId",
                table: "ActionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiquidationRecordId",
                table: "ActionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractAnnexAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractAnnexId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAnnexAttachments_ContractAnnexes_ContractAnnexId",
                        column: x => x.ContractAnnexId,
                        principalTable: "ContractAnnexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecordAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LiquidationRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecordAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidationRecordAttachments_LiquidationRecords_LiquidationRecordId",
                        column: x => x.LiquidationRecordId,
                        principalTable: "LiquidationRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_ContractAnnexId",
                table: "UserFlowDetails",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlowDetails_LiquidationRecordId",
                table: "UserFlowDetails",
                column: "LiquidationRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerSigns_ContractAnnexId",
                table: "PartnerSigns",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerSigns_LiquidationRecordId",
                table: "PartnerSigns",
                column: "LiquidationRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerReviews_ContractAnnexId",
                table: "PartnerReviews",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerReviews_LiquidationRecordId",
                table: "PartnerReviews",
                column: "LiquidationRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistories_ContractAnnexId",
                table: "ActionHistories",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistories_LiquidationRecordId",
                table: "ActionHistories",
                column: "LiquidationRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexAttachments_ContractAnnexId",
                table: "ContractAnnexAttachments",
                column: "ContractAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordAttachments_LiquidationRecordId",
                table: "LiquidationRecordAttachments",
                column: "LiquidationRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistories_ContractAnnexes_ContractAnnexId",
                table: "ActionHistories",
                column: "ContractAnnexId",
                principalTable: "ContractAnnexes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistories_LiquidationRecords_LiquidationRecordId",
                table: "ActionHistories",
                column: "LiquidationRecordId",
                principalTable: "LiquidationRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_ContractAnnexes_ContractAnnexId",
                table: "PartnerReviews",
                column: "ContractAnnexId",
                principalTable: "ContractAnnexes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_Contracts_ContractId",
                table: "PartnerReviews",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_LiquidationRecords_LiquidationRecordId",
                table: "PartnerReviews",
                column: "LiquidationRecordId",
                principalTable: "LiquidationRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerSigns_ContractAnnexes_ContractAnnexId",
                table: "PartnerSigns",
                column: "ContractAnnexId",
                principalTable: "ContractAnnexes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerSigns_LiquidationRecords_LiquidationRecordId",
                table: "PartnerSigns",
                column: "LiquidationRecordId",
                principalTable: "LiquidationRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlowDetails_ContractAnnexes_ContractAnnexId",
                table: "UserFlowDetails",
                column: "ContractAnnexId",
                principalTable: "ContractAnnexes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlowDetails_Contracts_ContractId",
                table: "UserFlowDetails",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlowDetails_LiquidationRecords_LiquidationRecordId",
                table: "UserFlowDetails",
                column: "LiquidationRecordId",
                principalTable: "LiquidationRecords",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistories_ContractAnnexes_ContractAnnexId",
                table: "ActionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistories_LiquidationRecords_LiquidationRecordId",
                table: "ActionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_ContractAnnexes_ContractAnnexId",
                table: "PartnerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_Contracts_ContractId",
                table: "PartnerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_LiquidationRecords_LiquidationRecordId",
                table: "PartnerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerSigns_ContractAnnexes_ContractAnnexId",
                table: "PartnerSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerSigns_LiquidationRecords_LiquidationRecordId",
                table: "PartnerSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFlowDetails_ContractAnnexes_ContractAnnexId",
                table: "UserFlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFlowDetails_Contracts_ContractId",
                table: "UserFlowDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFlowDetails_LiquidationRecords_LiquidationRecordId",
                table: "UserFlowDetails");

            migrationBuilder.DropTable(
                name: "ContractAnnexAttachments");

            migrationBuilder.DropTable(
                name: "LiquidationRecordAttachments");

            migrationBuilder.DropIndex(
                name: "IX_UserFlowDetails_ContractAnnexId",
                table: "UserFlowDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserFlowDetails_LiquidationRecordId",
                table: "UserFlowDetails");

            migrationBuilder.DropIndex(
                name: "IX_PartnerSigns_ContractAnnexId",
                table: "PartnerSigns");

            migrationBuilder.DropIndex(
                name: "IX_PartnerSigns_LiquidationRecordId",
                table: "PartnerSigns");

            migrationBuilder.DropIndex(
                name: "IX_PartnerReviews_ContractAnnexId",
                table: "PartnerReviews");

            migrationBuilder.DropIndex(
                name: "IX_PartnerReviews_LiquidationRecordId",
                table: "PartnerReviews");

            migrationBuilder.DropIndex(
                name: "IX_ActionHistories_ContractAnnexId",
                table: "ActionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ActionHistories_LiquidationRecordId",
                table: "ActionHistories");

            migrationBuilder.DropColumn(
                name: "ContractAnnexId",
                table: "UserFlowDetails");

            migrationBuilder.DropColumn(
                name: "LiquidationRecordId",
                table: "UserFlowDetails");

            migrationBuilder.DropColumn(
                name: "ContractAnnexId",
                table: "PartnerSigns");

            migrationBuilder.DropColumn(
                name: "LiquidationRecordId",
                table: "PartnerSigns");

            migrationBuilder.DropColumn(
                name: "ContractAnnexId",
                table: "PartnerReviews");

            migrationBuilder.DropColumn(
                name: "LiquidationRecordId",
                table: "PartnerReviews");

            migrationBuilder.DropColumn(
                name: "ContractAnnexId",
                table: "ActionHistories");

            migrationBuilder.DropColumn(
                name: "LiquidationRecordId",
                table: "ActionHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "UserFlowDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "PartnerReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "ActionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistories_Contracts_ContractId",
                table: "ActionHistories",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerReviews_Contracts_ContractId",
                table: "PartnerReviews",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlowDetails_Contracts_ContractId",
                table: "UserFlowDetails",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
