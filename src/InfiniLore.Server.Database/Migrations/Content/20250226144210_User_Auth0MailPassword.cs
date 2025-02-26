using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class User_Auth0MailPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth0MailPassword",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Auth0MailPassword",
                table: "Users",
                column: "Auth0MailPassword",
                unique: true,
                filter: "[Auth0MailPassword] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Auth0MailPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Auth0MailPassword",
                table: "Users");
        }
    }
}
