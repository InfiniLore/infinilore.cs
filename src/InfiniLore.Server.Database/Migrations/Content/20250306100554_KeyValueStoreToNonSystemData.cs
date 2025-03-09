#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class KeyValueStoreToNonSystemData : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropPrimaryKey(
            "PK_KeyValueStores",
            "KeyValueStores");

        migrationBuilder.DropIndex(
            "IX_KeyValueStores_Id",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "Id",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "CreatedDate",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "LastModifiedDate",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "SoftDeleteDate",
            "KeyValueStores");

        migrationBuilder.AddPrimaryKey(
            "PK_KeyValueStores",
            "KeyValueStores",
            "Key");

        migrationBuilder.CreateIndex(
            "IX_KeyValueStores_Key",
            "KeyValueStores",
            "Key",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropPrimaryKey(
            "PK_KeyValueStores",
            "KeyValueStores");

        migrationBuilder.DropIndex(
            "IX_KeyValueStores_Key",
            "KeyValueStores");

        migrationBuilder.AddColumn<Guid>(
            "Id",
            "KeyValueStores",
            "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            "CreatedDate",
            "KeyValueStores",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "LastModifiedDate",
            "KeyValueStores",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "SoftDeleteDate",
            "KeyValueStores",
            "datetime2",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            "PK_KeyValueStores",
            "KeyValueStores",
            "Id");

        migrationBuilder.CreateIndex(
            "IX_KeyValueStores_Id",
            "KeyValueStores",
            "Id",
            unique: true);
    }
}
