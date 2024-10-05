using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixUserContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "49a781f9-0ddf-4350-bd41-40e99db94c8b", null, "admin", "ADMIN" },
                    { "d1f5c111-0880-4d79-9f5b-824ab14113eb", null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d957c0f8-e90e-4068-a968-4f4b49fc165c", 0, "b3e2fb83-8582-4350-8f72-849673b4afa6", "testuser@example.com", true, false, null, "TESTUSER@EXAMPLE.COM", "TESTUSER", "AQAAAAIAAYagAAAAEI5cirrqxpFxPfMkxeFjsg6HTskbF+WHKukr8hOn14YvwgRpf5G0zjSEtty0VIMsyg==", null, false, "d957c0f8-e90e-4068-a968-4f4b49fc165b", false, "testuser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49a781f9-0ddf-4350-bd41-40e99db94c8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1f5c111-0880-4d79-9f5b-824ab14113eb");

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
    }
}
