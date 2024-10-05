using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedQueryFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "661b5751-2f5c-4835-a956-93616aaa10ef", null, "user", "USER" },
                    { "df0498e6-5048-4207-83d6-59f446e8a18c", null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "502aab93-4968-4739-b660-c435f8550bac", 0, "84d14afa-a368-445b-a252-5fdb14804780", "testuser@example.com", true, false, null, "TESTUSER@EXAMPLE.COM", "TESTUSER", "AQAAAAIAAYagAAAAEBaGWk9QEHC1B1L1BsItBnb5MPjNSeIWjkzZbUFI1+lQZDo1dutjpBqlpyEWJXFwQw==", null, false, "898ed40c-12db-4c99-840d-31130fe7f6da", false, "testuser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "661b5751-2f5c-4835-a956-93616aaa10ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df0498e6-5048-4207-83d6-59f446e8a18c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "502aab93-4968-4739-b660-c435f8550bac");

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
    }
}
