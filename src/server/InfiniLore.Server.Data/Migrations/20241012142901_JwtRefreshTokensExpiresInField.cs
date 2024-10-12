using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class JwtRefreshTokensExpiresInField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31306476-2b32-4abe-9876-3a1851a07937");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd373b00-064c-4291-9362-c1a8ce9d31dc");

            migrationBuilder.AddColumn<int>(
                name: "ExpiresInDays",
                table: "JwtRefreshTokens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61c88947-ff70-4882-b509-91cc8992c61a", null, "user", "USER" },
                    { "61d818d3-3df3-4b3d-ab22-7cb4f6c8b549", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "920180d9-5360-495b-abb7-fb8365605ab5", "AQAAAAIAAYagAAAAEAJItp0RegzW1PITs52O0VCpDT1DEMMTRzQ4C3Q2RCnLOmmbFwxrKIaDDknLHn23og==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61c88947-ff70-4882-b509-91cc8992c61a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61d818d3-3df3-4b3d-ab22-7cb4f6c8b549");

            migrationBuilder.DropColumn(
                name: "ExpiresInDays",
                table: "JwtRefreshTokens");

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
    }
}
