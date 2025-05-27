using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                principalTable: "AccessProtectionModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                principalTable: "AccessProtectionModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
