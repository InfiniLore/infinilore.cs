using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "423543a8-8970-4708-9164-ae855b2bc6dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2232dd7-ac2f-4e67-a05e-1f3254fe5678");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "423543a8-8970-4708-9164-ae855b2bc6dc", null, "user", null },
                    { "c2232dd7-ac2f-4e67-a05e-1f3254fe5678", null, "admin", null }
                });
        }
    }
}
