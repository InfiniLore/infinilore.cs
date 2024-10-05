using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03ef54ae-d3ee-415c-91f1-d140497292cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cf8faf5-8eeb-4623-bccb-007539f2607c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7c5d7b7f-da97-4e35-be84-02ce97af0dc3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6cc18578-e0f6-4ac8-8fc2-e2ce750b4059", null, "admin", null },
                    { "6f194527-77ed-4480-b446-637042bd0126", null, "user", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d957c0f8-e90e-4068-a968-4f4b49fc165c", 0, "f116f26c-96a3-434c-854b-62c824143893", "testuser@example.com", true, false, null, "TESTUSER@EXAMPLE.COM", "TESTUSER", "AQAAAAIAAYagAAAAEBwGiiEQrgmEaKIToxca9YMrNvKSQH3gem2M9Wq0mVtNNXAbipgFm/ZjPoIk5Lf+hQ==", null, false, "869eef5d-11e5-4518-b331-bbbcd9e6ed4e", false, "testuser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cc18578-e0f6-4ac8-8fc2-e2ce750b4059");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f194527-77ed-4480-b446-637042bd0126");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03ef54ae-d3ee-415c-91f1-d140497292cc", null, "admin", null },
                    { "2cf8faf5-8eeb-4623-bccb-007539f2607c", null, "user", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7c5d7b7f-da97-4e35-be84-02ce97af0dc3", 0, "9897f546-75fc-468d-aea7-876fd5494f5a", "testuser@example.com", true, false, null, null, null, "AQAAAAIAAYagAAAAEGtkZ+TWycL1JS+UdLVQaMOPNt1MbohWcRC1AcDl0wXHjwRLPLc7Cv2WfP6DHUBD6Q==", null, false, "c0a209ca-c220-4b78-913e-76c5c223fa14", false, "testuser" });
        }
    }
}
