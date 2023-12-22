using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateUUIDs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidationRecordFiles",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractAnnexFiles",
                table: "ContractAnnexFiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContractAnnexFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ContractAnnexFiles");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ContractAnnexFiles");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ContractAnnexFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "UUID",
                table: "LiquidationRecordFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UUID",
                table: "ContractAnnexFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidationRecordFiles",
                table: "LiquidationRecordFiles",
                column: "UUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractAnnexFiles",
                table: "ContractAnnexFiles",
                column: "UUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidationRecordFiles",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractAnnexFiles",
                table: "ContractAnnexFiles");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "LiquidationRecordFiles");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "ContractAnnexFiles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LiquidationRecordFiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "LiquidationRecordFiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "LiquidationRecordFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "LiquidationRecordFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ContractAnnexFiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ContractAnnexFiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ContractAnnexFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ContractAnnexFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidationRecordFiles",
                table: "LiquidationRecordFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractAnnexFiles",
                table: "ContractAnnexFiles",
                column: "Id");
        }
    }
}
