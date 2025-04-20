using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class DbContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarkdownFile_LoreScopes_LoreScopeId",
                table: "MarkdownFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MarkdownFile",
                table: "MarkdownFile");

            migrationBuilder.RenameTable(
                name: "MarkdownFile",
                newName: "MarkdownFiles");

            migrationBuilder.RenameIndex(
                name: "IX_MarkdownFile_LoreScopeId",
                table: "MarkdownFiles",
                newName: "IX_MarkdownFiles_LoreScopeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarkdownFiles",
                table: "MarkdownFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarkdownFiles_LoreScopes_LoreScopeId",
                table: "MarkdownFiles",
                column: "LoreScopeId",
                principalTable: "LoreScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarkdownFiles_LoreScopes_LoreScopeId",
                table: "MarkdownFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MarkdownFiles",
                table: "MarkdownFiles");

            migrationBuilder.RenameTable(
                name: "MarkdownFiles",
                newName: "MarkdownFile");

            migrationBuilder.RenameIndex(
                name: "IX_MarkdownFiles_LoreScopeId",
                table: "MarkdownFile",
                newName: "IX_MarkdownFile_LoreScopeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarkdownFile",
                table: "MarkdownFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarkdownFile_LoreScopes_LoreScopeId",
                table: "MarkdownFile",
                column: "LoreScopeId",
                principalTable: "LoreScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
