using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class KeyValueStoreToNonSystemData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyValueStores",
                table: "KeyValueStores");

            migrationBuilder.DropIndex(
                name: "IX_KeyValueStores_Id",
                table: "KeyValueStores");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "KeyValueStores");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "KeyValueStores");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "KeyValueStores");

            migrationBuilder.DropColumn(
                name: "SoftDeleteDate",
                table: "KeyValueStores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyValueStores",
                table: "KeyValueStores",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueStores_Key",
                table: "KeyValueStores",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyValueStores",
                table: "KeyValueStores");

            migrationBuilder.DropIndex(
                name: "IX_KeyValueStores_Key",
                table: "KeyValueStores");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "KeyValueStores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "KeyValueStores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "KeyValueStores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SoftDeleteDate",
                table: "KeyValueStores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyValueStores",
                table: "KeyValueStores",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueStores_Id",
                table: "KeyValueStores",
                column: "Id",
                unique: true);
        }
    }
}
