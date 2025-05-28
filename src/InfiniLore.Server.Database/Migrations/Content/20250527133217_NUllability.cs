using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class NUllability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccessProtectionId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                unique: true,
                filter: "[AccessProtectionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccessProtectionId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                unique: true);
        }
    }
}
