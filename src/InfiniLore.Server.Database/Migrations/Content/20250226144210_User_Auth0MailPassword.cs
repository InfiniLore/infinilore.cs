#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class User_Auth0MailPassword : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.AddColumn<string>(
            "Auth0MailPassword",
            "Users",
            "nvarchar(256)",
            maxLength: 256,
            nullable: true);

        migrationBuilder.CreateIndex(
            "IX_Users_Auth0MailPassword",
            "Users",
            "Auth0MailPassword",
            unique: true,
            filter: "[Auth0MailPassword] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropIndex(
            "IX_Users_Auth0MailPassword",
            "Users");

        migrationBuilder.DropColumn(
            "Auth0MailPassword",
            "Users");
    }
}
