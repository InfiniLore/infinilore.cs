using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditToAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b36c0049-c478-4144-83bf-06930cc69eeb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b626e60c-12e9-46bd-a6c3-5d5af41c1911");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1486299b-2f69-43d7-9834-bdf7943a8ba8", null, "user", "USER" },
                    { "6375acbb-dd31-4a9b-b847-91a414156dc1", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e51c2865-b281-4762-9496-30696a19dadc", "AQAAAAIAAYagAAAAEHyiFepOULVofm+u32Y74WGXm2VEAgzwgVVasA0U6hbUl/y5HTCD4zvWN4gPnlMsBg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1486299b-2f69-43d7-9834-bdf7943a8ba8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6375acbb-dd31-4a9b-b847-91a414156dc1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b36c0049-c478-4144-83bf-06930cc69eeb", null, "user", "USER" },
                    { "b626e60c-12e9-46bd-a6c3-5d5af41c1911", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ebf4ad55-575a-41ca-8676-81d01cfb25c7", "AQAAAAIAAYagAAAAEMMTeMhcvyhIysHDxg3c7FbxNf8EyWFD+8QaQ2HggMU4P/yJARIlkgh26RTVTEVtFg==" });
        }
    }
}
