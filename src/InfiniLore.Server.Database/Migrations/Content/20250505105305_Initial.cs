using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfiniLoreUserModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Auth0IdGoogle = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Auth0Github = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Auth0MailPassword = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfiniLoreUserModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyValueEntryModel",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483646, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyValueEntryModel", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "LoreScopeModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoreScopeModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarkdownFileModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkdownFileModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_Auth0Github",
                table: "InfiniLoreUserModel",
                column: "Auth0Github",
                unique: true,
                filter: "[Auth0Github] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_Auth0IdGoogle",
                table: "InfiniLoreUserModel",
                column: "Auth0IdGoogle",
                unique: true,
                filter: "[Auth0IdGoogle] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_Auth0MailPassword",
                table: "InfiniLoreUserModel",
                column: "Auth0MailPassword",
                unique: true,
                filter: "[Auth0MailPassword] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_Id",
                table: "InfiniLoreUserModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_Username",
                table: "InfiniLoreUserModel",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueEntryModel_Key",
                table: "KeyValueEntryModel",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_Id",
                table: "LoreScopeModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_OwnerId",
                table: "LoreScopeModel",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LoreScopeModel",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarkdownFileModel_Id",
                table: "MarkdownFileModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarkdownFileModel_OwnerId",
                table: "MarkdownFileModel",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfiniLoreUserModel");

            migrationBuilder.DropTable(
                name: "KeyValueEntryModel");

            migrationBuilder.DropTable(
                name: "LoreScopeModel");

            migrationBuilder.DropTable(
                name: "MarkdownFileModel");
        }
    }
}
