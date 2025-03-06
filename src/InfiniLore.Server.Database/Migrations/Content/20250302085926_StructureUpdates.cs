#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class StructureUpdates : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateIndex(
            "IX_KeyValueStores_Id",
            "KeyValueStores",
            "Id",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropIndex(
            "IX_KeyValueStores_Id",
            "KeyValueStores");
    }
}
