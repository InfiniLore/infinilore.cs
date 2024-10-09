using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixToUserContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "0210e8ea-c838-4ba8-88eb-3629e109b0c3", null, "user", "USER" },
                    { "8b6779f2-a11b-44d1-910e-379744812c68", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "16413e5e-aa9a-488e-b73d-c820b5fcfb99", "AQAAAAIAAYagAAAAEBxk4nZ7xfXNkuAbJ+z2J9e5nkSNPeQLhgmjoSAKRkT8Rgd/13oOUDPRBdVfLWALbg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0210e8ea-c838-4ba8-88eb-3629e109b0c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b6779f2-a11b-44d1-910e-379744812c68");

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
    }
}
