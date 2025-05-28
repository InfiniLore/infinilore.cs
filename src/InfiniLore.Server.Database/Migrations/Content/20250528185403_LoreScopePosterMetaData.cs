using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class LoreScopePosterMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "S3FileMetaDataModel",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "S3FileMetaDataModel",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "PosterImageMetaDataId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_PosterImageMetaDataId",
                table: "LoreScopeModel",
                column: "PosterImageMetaDataId",
                unique: true,
                filter: "[PosterImageMetaDataId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_S3FileMetaDataModel_PosterImageMetaDataId",
                table: "LoreScopeModel",
                column: "PosterImageMetaDataId",
                principalTable: "S3FileMetaDataModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_S3FileMetaDataModel_PosterImageMetaDataId",
                table: "LoreScopeModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_PosterImageMetaDataId",
                table: "LoreScopeModel");

            migrationBuilder.DropColumn(
                name: "PosterImageMetaDataId",
                table: "LoreScopeModel");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "S3FileMetaDataModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "S3FileMetaDataModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
