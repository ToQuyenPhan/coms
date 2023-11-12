using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractAnnexes_ContractAnnexTemplates_ContractAnnexTemplateID",
                table: "ContractAnnexes");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAnnexes_Documents_DocumentId",
                table: "ContractAnnexes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTemplates_ContractTemplateID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidationRecords_Documents_DocumentId",
                table: "LiquidationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidationRecords_LiquidationRecordTemplates_LiquidationRecordTemplateId",
                table: "LiquidationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerSigns_Documents_DocumentId",
                table: "PartnerSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields");

            migrationBuilder.DropTable(
                name: "ContractAnnexTemplates");

            migrationBuilder.DropTable(
                name: "ContractFields");

            migrationBuilder.DropTable(
                name: "ContractTemplateTerms");

            migrationBuilder.DropTable(
                name: "Document_ActionHistories");

            migrationBuilder.DropTable(
                name: "Document_Attachments");

            migrationBuilder.DropTable(
                name: "DocumentPartnerReviews");

            migrationBuilder.DropTable(
                name: "LiquidationRecordTemplates");

            migrationBuilder.DropTable(
                name: "ContractTemplates");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_LiquidationRecords_DocumentId",
                table: "LiquidationRecords");

            migrationBuilder.DropIndex(
                name: "IX_LiquidationRecords_LiquidationRecordTemplateId",
                table: "LiquidationRecords");

            migrationBuilder.DropIndex(
                name: "IX_ContractAnnexes_ContractAnnexTemplateID",
                table: "ContractAnnexes");

            migrationBuilder.DropIndex(
                name: "IX_ContractAnnexes_DocumentId",
                table: "ContractAnnexes");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "LiquidationRecords");

            migrationBuilder.DropColumn(
                name: "LiquidationRecordTemplateId",
                table: "LiquidationRecords");

            migrationBuilder.DropColumn(
                name: "ContractAnnexTemplateID",
                table: "ContractAnnexes");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "ContractAnnexes");

            migrationBuilder.RenameColumn(
                name: "ContractTemplateId",
                table: "TemplateFields",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateFields_ContractTemplateId",
                table: "TemplateFields",
                newName: "IX_TemplateFields_TemplateId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "PartnerSigns",
                newName: "ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerSigns_DocumentId",
                table: "PartnerSigns",
                newName: "IX_PartnerSigns_ContractId");

            migrationBuilder.RenameColumn(
                name: "ContractTemplateID",
                table: "Contracts",
                newName: "TemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ContractTemplateID",
                table: "Contracts",
                newName: "IX_Contracts_TemplateID");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "PartnerReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TemplateContents",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateContents", x => new { x.ContractId, x.TemplateFieldId });
                    table.ForeignKey(
                        name: "FK_TemplateContents_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateContents_TemplateFields_TemplateFieldId",
                        column: x => x.TemplateFieldId,
                        principalTable: "TemplateFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateTerms",
                columns: table => new
                {
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateTerms", x => new { x.TemplateId, x.Number });
                    table.ForeignKey(
                        name: "FK_TemplateTerms_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerReviews_ContractId",
                table: "PartnerReviews",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ContractId",
                table: "Attachments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateContents_TemplateFieldId",
                table: "TemplateContents",
                column: "TemplateFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_ContractCategoryId",
                table: "Templates",
                column: "ContractCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Contracts_ContractId",
                table: "Attachments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Templates_TemplateID",
                table: "Contracts",
                column: "TemplateID",
                principalTable: "Templates",
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
                name: "FK_PartnerSigns_Contracts_ContractId",
                table: "PartnerSigns",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_Templates_TemplateId",
                table: "TemplateFields",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Contracts_ContractId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Templates_TemplateID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerReviews_Contracts_ContractId",
                table: "PartnerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerSigns_Contracts_ContractId",
                table: "PartnerSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_Templates_TemplateId",
                table: "TemplateFields");

            migrationBuilder.DropTable(
                name: "TemplateContents");

            migrationBuilder.DropTable(
                name: "TemplateTerms");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_PartnerReviews_ContractId",
                table: "PartnerReviews");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ContractId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "PartnerReviews");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "TemplateFields",
                newName: "ContractTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateFields_TemplateId",
                table: "TemplateFields",
                newName: "IX_TemplateFields_ContractTemplateId");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "PartnerSigns",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerSigns_ContractId",
                table: "PartnerSigns",
                newName: "IX_PartnerSigns_DocumentId");

            migrationBuilder.RenameColumn(
                name: "TemplateID",
                table: "Contracts",
                newName: "ContractTemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_TemplateID",
                table: "Contracts",
                newName: "IX_Contracts_ContractTemplateID");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "LiquidationRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiquidationRecordTemplateId",
                table: "LiquidationRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractAnnexTemplateID",
                table: "ContractAnnexes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "ContractAnnexes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContractAnnexTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAnnexTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractAnnexTemplates_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractFields",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractFields", x => new { x.ContractId, x.TemplateFieldId });
                    table.ForeignKey(
                        name: "FK_ContractFields_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractFields_TemplateFields_TemplateFieldId",
                        column: x => x.TemplateFieldId,
                        principalTable: "TemplateFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTemplates_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "LiquidationRecordTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractCategoryId = table.Column<int>(type: "int", nullable: true),
                    LiquidationTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidationRecordTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidationRecordTemplates_ContractCategories_ContractCategoryId",
                        column: x => x.ContractCategoryId,
                        principalTable: "ContractCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LiquidationRecordTemplates_LiquidationTypes_LiquidationTypeId",
                        column: x => x.LiquidationTypeId,
                        principalTable: "LiquidationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractTemplateTerms",
                columns: table => new
                {
                    ContractTemplateId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTemplateTerms", x => new { x.ContractTemplateId, x.Number });
                    table.ForeignKey(
                        name: "FK_ContractTemplateTerms_ContractTemplates_ContractTemplateId",
                        column: x => x.ContractTemplateId,
                        principalTable: "ContractTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document_ActionHistories",
                columns: table => new
                {
                    ActionHistoryId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_ActionHistories", x => new { x.ActionHistoryId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_Document_ActionHistories_ActionHistories_ActionHistoryId",
                        column: x => x.ActionHistoryId,
                        principalTable: "ActionHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_ActionHistories_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document_Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_Attachments", x => new { x.AttachmentId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_Document_Attachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_Attachments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentPartnerReviews",
                columns: table => new
                {
                    PartnerReviewId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPartnerReviews", x => new { x.PartnerReviewId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_DocumentPartnerReviews_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentPartnerReviews_PartnerReviews_PartnerReviewId",
                        column: x => x.PartnerReviewId,
                        principalTable: "PartnerReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecords_DocumentId",
                table: "LiquidationRecords",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecords_LiquidationRecordTemplateId",
                table: "LiquidationRecords",
                column: "LiquidationRecordTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexes_ContractAnnexTemplateID",
                table: "ContractAnnexes",
                column: "ContractAnnexTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexes_DocumentId",
                table: "ContractAnnexes",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAnnexTemplates_ContractCategoryId",
                table: "ContractAnnexTemplates",
                column: "ContractCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractFields_TemplateFieldId",
                table: "ContractFields",
                column: "TemplateFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTemplates_ContractCategoryId",
                table: "ContractTemplates",
                column: "ContractCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ActionHistories_DocumentId",
                table: "Document_ActionHistories",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Attachments_DocumentId",
                table: "Document_Attachments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPartnerReviews_DocumentId",
                table: "DocumentPartnerReviews",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordTemplates_ContractCategoryId",
                table: "LiquidationRecordTemplates",
                column: "ContractCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidationRecordTemplates_LiquidationTypeId",
                table: "LiquidationRecordTemplates",
                column: "LiquidationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAnnexes_ContractAnnexTemplates_ContractAnnexTemplateID",
                table: "ContractAnnexes",
                column: "ContractAnnexTemplateID",
                principalTable: "ContractAnnexTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAnnexes_Documents_DocumentId",
                table: "ContractAnnexes",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTemplates_ContractTemplateID",
                table: "Contracts",
                column: "ContractTemplateID",
                principalTable: "ContractTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidationRecords_Documents_DocumentId",
                table: "LiquidationRecords",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidationRecords_LiquidationRecordTemplates_LiquidationRecordTemplateId",
                table: "LiquidationRecords",
                column: "LiquidationRecordTemplateId",
                principalTable: "LiquidationRecordTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerSigns_Documents_DocumentId",
                table: "PartnerSigns",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_ContractTemplates_ContractTemplateId",
                table: "TemplateFields",
                column: "ContractTemplateId",
                principalTable: "ContractTemplates",
                principalColumn: "Id");
        }
    }
}
