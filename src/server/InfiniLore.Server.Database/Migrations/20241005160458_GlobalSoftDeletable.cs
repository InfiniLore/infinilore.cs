using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class GlobalSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49a781f9-0ddf-4350-bd41-40e99db94c8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1f5c111-0880-4d79-9f5b-824ab14113eb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a738b65-fe7a-4d18-9399-8a74eccf0a32", null, "admin", "ADMIN" },
                    { "f920f1b4-6fa3-4364-8615-5ffaa0dbe9c9", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a412891e-e9c0-43d4-977e-b267be052f4d", "AQAAAAIAAYagAAAAEEW9nUOxeFX4B1vlokZOhCyQCtgVCQxKqW4H0v2kW3mKpZmAZtgqO/TvRhhx4spWHQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a738b65-fe7a-4d18-9399-8a74eccf0a32");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f920f1b4-6fa3-4364-8615-5ffaa0dbe9c9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49a781f9-0ddf-4350-bd41-40e99db94c8b", null, "admin", "ADMIN" },
                    { "d1f5c111-0880-4d79-9f5b-824ab14113eb", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b3e2fb83-8582-4350-8f72-849673b4afa6", "AQAAAAIAAYagAAAAEI5cirrqxpFxPfMkxeFjsg6HTskbF+WHKukr8hOn14YvwgRpf5G0zjSEtty0VIMsyg==" });
        }
    }
}
