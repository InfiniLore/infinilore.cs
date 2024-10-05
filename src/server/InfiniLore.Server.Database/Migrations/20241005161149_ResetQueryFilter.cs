using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class ResetQueryFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "2ea74fbc-a094-4c0d-9b61-49cba2b99445", null, "admin", "ADMIN" },
                    { "90d84984-0a3b-418a-a94a-82348a43f5d5", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0775ed22-cfdc-41b8-8041-20b16b405db8", "AQAAAAIAAYagAAAAEPh7ZtwoD68mRuC3IStmK2Rzqj/vVlYszuy3lEydkopIF800yw7ZaN8+sjlfjpUcXQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ea74fbc-a094-4c0d-9b61-49cba2b99445");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90d84984-0a3b-418a-a94a-82348a43f5d5");

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
    }
}
