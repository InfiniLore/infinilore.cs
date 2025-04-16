#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class KeyValueEntry : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "KeyValueStores");

        migrationBuilder.CreateTable(
            "KeyValueEntries",
            columns: table => new {
                Key = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: false),
                Value = table.Column<string>("nvarchar(max)", maxLength: 2147483646, nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_KeyValueEntries", columns: x => x.Key);
            });

        migrationBuilder.CreateIndex(
            "IX_KeyValueEntries_Key",
            "KeyValueEntries",
            "Key",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "KeyValueEntries");

        migrationBuilder.CreateTable(
            "KeyValueStores",
            columns: table => new {
                Key = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: false),
                Value = table.Column<string>("nvarchar(max)", maxLength: 2147483646, nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_KeyValueStores", columns: x => x.Key);
            });

        migrationBuilder.CreateIndex(
            "IX_KeyValueStores_Key",
            "KeyValueStores",
            "Key",
            unique: true);
    }
}
