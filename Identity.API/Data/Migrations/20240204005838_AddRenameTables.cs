using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemTypeValue_SystemTypes_SystemTypeId",
                table: "SystemTypeValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemTypeValue",
                table: "SystemTypeValue");

            migrationBuilder.RenameTable(
                name: "SystemTypeValue",
                newName: "SystemTypeValues");

            migrationBuilder.RenameIndex(
                name: "IX_SystemTypeValue_SystemTypeId",
                table: "SystemTypeValues",
                newName: "IX_SystemTypeValues_SystemTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "SystemTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemTypeValues",
                table: "SystemTypeValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTypeValues_SystemTypes_SystemTypeId",
                table: "SystemTypeValues",
                column: "SystemTypeId",
                principalTable: "SystemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemTypeValues_SystemTypes_SystemTypeId",
                table: "SystemTypeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemTypeValues",
                table: "SystemTypeValues");

            migrationBuilder.RenameTable(
                name: "SystemTypeValues",
                newName: "SystemTypeValue");

            migrationBuilder.RenameIndex(
                name: "IX_SystemTypeValues_SystemTypeId",
                table: "SystemTypeValue",
                newName: "IX_SystemTypeValue_SystemTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "SystemTypes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemTypeValue",
                table: "SystemTypeValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTypeValue_SystemTypes_SystemTypeId",
                table: "SystemTypeValue",
                column: "SystemTypeId",
                principalTable: "SystemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
