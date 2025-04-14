using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class KeyValueEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyValueStores");

            migrationBuilder.CreateTable(
                name: "KeyValueEntries",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483646, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyValueEntries", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueEntries_Key",
                table: "KeyValueEntries",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyValueEntries");

            migrationBuilder.CreateTable(
                name: "KeyValueStores",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483646, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyValueStores", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueStores_Key",
                table: "KeyValueStores",
                column: "Key",
                unique: true);
        }
    }
}
