using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class FixOptionalDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                principalTable: "LoreScopeDocumentModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_LoreScopeDocumentModel_DocumentId",
                table: "LoreScopeModel",
                column: "DocumentId",
                principalTable: "LoreScopeDocumentModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
