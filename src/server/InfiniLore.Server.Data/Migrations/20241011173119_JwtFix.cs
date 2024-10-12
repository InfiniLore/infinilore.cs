using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class JwtFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6df1c093-a689-48e4-92d3-09e176eb2d8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af7ab570-faa0-41fe-8918-f2ce27716815");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7462aa1c-42b4-4411-8c43-911b433ac367");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e534af70-6412-4ce7-bf5a-91681464461f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6df1c093-a689-48e4-92d3-09e176eb2d8b", null, "admin", "ADMIN" },
                    { "af7ab570-faa0-41fe-8918-f2ce27716815", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "abc24086-a4eb-43f4-8fbf-e6b19f1706dd", "AQAAAAIAAYagAAAAEBIiYuFwYUEEvU6595MDtMrmE0zmZuSt11SjFxfFubRUUUW47x82zclMTJXk6gbQ6Q==" });
        }
    }
}
