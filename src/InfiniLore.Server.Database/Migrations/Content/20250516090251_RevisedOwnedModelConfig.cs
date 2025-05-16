using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class RevisedOwnedModelConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoreScopeDocumentModel_OwnerId",
                table: "LoreScopeDocumentModel");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDocumentModel_OwnerId",
                table: "LoreScopeDocumentModel",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel",
                column: "OwnerId",
                principalTable: "InfiniLoreUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkdownFileModel_LoreScopeModel_OwnerId",
                table: "MarkdownFileModel",
                column: "OwnerId",
                principalTable: "LoreScopeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_MarkdownFileModel_LoreScopeModel_OwnerId",
                table: "MarkdownFileModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeDocumentModel_OwnerId",
                table: "LoreScopeDocumentModel");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDocumentModel_OwnerId",
                table: "LoreScopeDocumentModel",
                column: "OwnerId");
        }
    }
}
