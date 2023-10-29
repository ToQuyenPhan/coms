using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class AddPartnerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Representative = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RepresentativePosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerReviews_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartnerReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartnerSigns",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    SignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerSigns", x => new { x.PartnerId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_PartnerSigns_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerSigns_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentPartnerReviews",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    PartnerReviewId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PartnerComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplyId = table.Column<int>(type: "int", nullable: true),
                    PartnerReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerComments_PartnerReviews_PartnerReviewId",
                        column: x => x.PartnerReviewId,
                        principalTable: "PartnerReviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPartnerReviews_DocumentId",
                table: "DocumentPartnerReviews",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerComments_PartnerReviewId",
                table: "PartnerComments",
                column: "PartnerReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerReviews_PartnerId",
                table: "PartnerReviews",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerReviews_UserId",
                table: "PartnerReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerSigns_DocumentId",
                table: "PartnerSigns",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentPartnerReviews");

            migrationBuilder.DropTable(
                name: "PartnerComments");

            migrationBuilder.DropTable(
                name: "PartnerSigns");

            migrationBuilder.DropTable(
                name: "PartnerReviews");

            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}
