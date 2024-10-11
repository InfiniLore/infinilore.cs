using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class JwtTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d6b18e7-aef0-4e30-a833-77ad4c0351c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6c26b9f-e5a5-4901-9282-e09e2b516e94");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "4d6b18e7-aef0-4e30-a833-77ad4c0351c9", null, "admin", "ADMIN" },
                    { "b6c26b9f-e5a5-4901-9282-e09e2b516e94", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d8a1242a-64f9-464a-89c5-bfd73630cd34", "AQAAAAIAAYagAAAAEFex7auDpf5VzGIQub2NxsTmFq7WUb1LmF7YkRhh0Twdtn2s8yEBrNEmrL6qZybg3g==" });
        }
    }
}
