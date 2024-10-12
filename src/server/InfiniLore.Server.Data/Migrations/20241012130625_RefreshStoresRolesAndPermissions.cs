using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefreshStoresRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7462aa1c-42b4-4411-8c43-911b433ac367");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e534af70-6412-4ce7-bf5a-91681464461f");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "JwtRefreshTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "JwtRefreshTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31306476-2b32-4abe-9876-3a1851a07937", null, "user", "USER" },
                    { "fd373b00-064c-4291-9362-c1a8ce9d31dc", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "08ba467d-3590-4993-b3e8-0e060040e663", "AQAAAAIAAYagAAAAECz8Du87cqvJ8mnl4CdAA3swmsanBHKChF5Z+iX9sr4f0P/oUyaSacfe2KckQvHRAQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31306476-2b32-4abe-9876-3a1851a07937");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd373b00-064c-4291-9362-c1a8ce9d31dc");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "JwtRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "JwtRefreshTokens");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7462aa1c-42b4-4411-8c43-911b433ac367", null, "admin", "ADMIN" },
                    { "e534af70-6412-4ce7-bf5a-91681464461f", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "46439665-c919-4ac8-afa0-582d6baacad4", "AQAAAAIAAYagAAAAEJe3Fcs3eZKrcFehCkWUrYQVOvEbDuoTsqNr42kyhxZmfbPSwcE3efjRjZ/v5esrbQ==" });
        }
    }
}
