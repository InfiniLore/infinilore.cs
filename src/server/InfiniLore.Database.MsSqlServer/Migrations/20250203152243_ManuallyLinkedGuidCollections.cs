using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Database.MsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ManuallyLinkedGuidCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LorescopeIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MultiverseIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniverseIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LorescopeIds",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MultiverseIds",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniverseIds",
                table: "AspNetUsers");
        }
    }
}
