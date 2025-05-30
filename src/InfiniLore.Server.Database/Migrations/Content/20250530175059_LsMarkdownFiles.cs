using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class LsMarkdownFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LsMarkdownFileModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUserToEditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    S3FileMetaDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LsMarkdownFileModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LsMarkdownFileModel_LoreScopeModel_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "LoreScopeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LsMarkdownFileModel_S3FileMetaDataModel_S3FileMetaDataId",
                        column: x => x.S3FileMetaDataId,
                        principalTable: "S3FileMetaDataModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LsMarkdownFileModel_Id",
                table: "LsMarkdownFileModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LsMarkdownFileModel_OwnerId",
                table: "LsMarkdownFileModel",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LsMarkdownFileModel_S3FileMetaDataId",
                table: "LsMarkdownFileModel",
                column: "S3FileMetaDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LsMarkdownFileModel",
                columns: new[] { "OwnerId", "Name", "SoftDeleteDate" },
                unique: true,
                filter: "[SoftDeleteDate] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LsMarkdownFileModel");
        }
    }
}
