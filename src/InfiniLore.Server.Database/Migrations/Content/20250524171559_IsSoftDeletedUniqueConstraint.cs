using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class IsSoftDeletedUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LoreScopeModel");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LoreScopeModel",
                columns: new[] { "OwnerId", "Name", "SoftDeleteDate" },
                unique: true,
                filter: "[SoftDeleteDate] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LoreScopeModel");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerId_Name_Unique",
                table: "LoreScopeModel",
                columns: new[] { "OwnerId", "Name" },
                unique: true);
        }
    }
}
