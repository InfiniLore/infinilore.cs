using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class BetterInhertianceRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.DropTable(
                name: "LoreScopeDocumentModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "LoreScopeModel");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "LoreScopeModel",
                newName: "DescriptionId");

            migrationBuilder.CreateTable(
                name: "LoreScopeDescriptionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CachedRenderedHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CachedRenderedHtmlHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoreScopeDescriptionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoreScopeDescriptionModel_LoreScopeModel_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "LoreScopeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_DescriptionId",
                table: "LoreScopeModel",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDescriptionModel_Id",
                table: "LoreScopeDescriptionModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDescriptionModel_OwnerId",
                table: "LoreScopeDescriptionModel",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDescriptionModel_DescriptionId",
                table: "LoreScopeModel",
                column: "DescriptionId",
                principalTable: "LoreScopeDescriptionModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDescriptionModel_DescriptionId",
                table: "LoreScopeModel");

            migrationBuilder.DropTable(
                name: "LoreScopeDescriptionModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_DescriptionId",
                table: "LoreScopeModel");

            migrationBuilder.RenameColumn(
                name: "DescriptionId",
                table: "LoreScopeModel",
                newName: "DocumentId");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "LoreScopeModel",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LoreScopeDocumentModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HtmlRenderedContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoreScopeDocumentModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDocumentModel_Id",
                table: "LoreScopeDocumentModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeDocumentModel_OwnerId",
                table: "LoreScopeDocumentModel",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                principalTable: "LoreScopeDocumentModel",
                principalColumn: "Id");
        }
    }
}
