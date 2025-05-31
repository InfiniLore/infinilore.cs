using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class UserProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageMetaDataId",
                table: "InfiniLoreUserModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserModel_ProfileImageMetaDataId",
                table: "InfiniLoreUserModel",
                column: "ProfileImageMetaDataId",
                unique: true,
                filter: "[ProfileImageMetaDataId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InfiniLoreUserModel_S3FileMetaDataModel_ProfileImageMetaDataId",
                table: "InfiniLoreUserModel",
                column: "ProfileImageMetaDataId",
                principalTable: "S3FileMetaDataModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfiniLoreUserModel_S3FileMetaDataModel_ProfileImageMetaDataId",
                table: "InfiniLoreUserModel");

            migrationBuilder.DropIndex(
                name: "IX_InfiniLoreUserModel_ProfileImageMetaDataId",
                table: "InfiniLoreUserModel");

            migrationBuilder.DropColumn(
                name: "ProfileImageMetaDataId",
                table: "InfiniLoreUserModel");
        }
    }
}
