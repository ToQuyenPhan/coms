using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coms.Infrastructure.Migrations
{
    public partial class UpdateContractFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractFiles",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ContractFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "UUID",
                table: "ContractFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractFiles",
                table: "ContractFiles",
                column: "UUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractFiles",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "ContractFiles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ContractFiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ContractFiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ContractFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ContractFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractFiles",
                table: "ContractFiles",
                column: "Id");
        }
    }
}
