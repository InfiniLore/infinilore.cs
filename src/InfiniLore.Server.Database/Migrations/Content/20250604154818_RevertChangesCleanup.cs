using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class RevertChangesCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessProtectionModel_InfiniLoreUserModel_ModelOwnerId",
                table: "AccessProtectionModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessProtectionRuleModel_AccessProtectionModel_OwnerId",
                table: "AccessProtectionRuleModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LsMarkdownFileModel_LoreScopeModel_OwnerId",
                table: "LsMarkdownFileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LsMarkdownFileModel_S3FileMetaDataModel_S3FileMetaDataId",
                table: "LsMarkdownFileModel");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessProtectionModel_InfiniLoreUserModel_ModelOwnerId",
                table: "AccessProtectionModel",
                column: "ModelOwnerId",
                principalTable: "InfiniLoreUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessProtectionRuleModel_AccessProtectionModel_OwnerId",
                table: "AccessProtectionRuleModel",
                column: "OwnerId",
                principalTable: "AccessProtectionModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel",
                column: "OwnerId",
                principalTable: "InfiniLoreUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LsMarkdownFileModel_LoreScopeModel_OwnerId",
                table: "LsMarkdownFileModel",
                column: "OwnerId",
                principalTable: "LoreScopeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LsMarkdownFileModel_S3FileMetaDataModel_S3FileMetaDataId",
                table: "LsMarkdownFileModel",
                column: "S3FileMetaDataId",
                principalTable: "S3FileMetaDataModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessProtectionModel_InfiniLoreUserModel_ModelOwnerId",
                table: "AccessProtectionModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessProtectionRuleModel_AccessProtectionModel_OwnerId",
                table: "AccessProtectionRuleModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LsMarkdownFileModel_LoreScopeModel_OwnerId",
                table: "LsMarkdownFileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_LsMarkdownFileModel_S3FileMetaDataModel_S3FileMetaDataId",
                table: "LsMarkdownFileModel");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessProtectionModel_InfiniLoreUserModel_ModelOwnerId",
                table: "AccessProtectionModel",
                column: "ModelOwnerId",
                principalTable: "InfiniLoreUserModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessProtectionRuleModel_AccessProtectionModel_OwnerId",
                table: "AccessProtectionRuleModel",
                column: "OwnerId",
                principalTable: "AccessProtectionModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_InfiniLoreUserModel_OwnerId",
                table: "LoreScopeModel",
                column: "OwnerId",
                principalTable: "InfiniLoreUserModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LsMarkdownFileModel_LoreScopeModel_OwnerId",
                table: "LsMarkdownFileModel",
                column: "OwnerId",
                principalTable: "LoreScopeModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LsMarkdownFileModel_S3FileMetaDataModel_S3FileMetaDataId",
                table: "LsMarkdownFileModel",
                column: "S3FileMetaDataId",
                principalTable: "S3FileMetaDataModel",
                principalColumn: "Id");
        }
    }
}
