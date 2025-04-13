#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class AddUsername : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<string>(
            "Username",
            "Users",
            "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            "IX_Users_Username",
            "Users",
            "Username",
            unique: true,
            filter: "[Username] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropIndex(
            "IX_Users_Username",
            "Users");

        migrationBuilder.DropColumn(
            "Username",
            "Users");
    }
}
