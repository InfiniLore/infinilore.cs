using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDescriptionModel_DescriptionId",
                table: "LoreScopeModel");

            migrationBuilder.DropTable(
                name: "LoreScopeDescriptionModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_DescriptionId",
                table: "LoreScopeModel");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "LoreScopeModel");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LoreScopeModel",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LoreScopeModel");

            migrationBuilder.AddColumn<Guid>(
                name: "DescriptionId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoreScopeDescriptionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CachedRenderedHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CachedRenderedHtmlHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
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
    }
}
