using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixToAuditLogRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeAuditLogs_LoreScopes_ContentId",
                table: "LoreScopeAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MultiverseAuditLogs_Multiverses_ContentId",
                table: "MultiverseAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UniverseAuditLogs_Universes_ContentId",
                table: "UniverseAuditLogs");

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
                    { "9f39ee7b-17d9-4453-bbb0-ab4b48f9dbea", null, "user", "USER" },
                    { "aa3b7b95-05eb-4f9f-b205-168724dab6c4", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e5e4e92-b254-4cd5-9bff-32db083aad93", "AQAAAAIAAYagAAAAEE0wBRX3R8P/lSotNOSTzD6AXu3eitUUY8d6L3GJvpJrZn00jwliCJxyDibRCq918w==" });

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeAuditLogs_LoreScopes_ContentId",
                table: "LoreScopeAuditLogs",
                column: "ContentId",
                principalTable: "LoreScopes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MultiverseAuditLogs_Multiverses_ContentId",
                table: "MultiverseAuditLogs",
                column: "ContentId",
                principalTable: "Multiverses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UniverseAuditLogs_Universes_ContentId",
                table: "UniverseAuditLogs",
                column: "ContentId",
                principalTable: "Universes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeAuditLogs_LoreScopes_ContentId",
                table: "LoreScopeAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MultiverseAuditLogs_Multiverses_ContentId",
                table: "MultiverseAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UniverseAuditLogs_Universes_ContentId",
                table: "UniverseAuditLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f39ee7b-17d9-4453-bbb0-ab4b48f9dbea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa3b7b95-05eb-4f9f-b205-168724dab6c4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeAuditLogs_LoreScopes_ContentId",
                table: "LoreScopeAuditLogs",
                column: "ContentId",
                principalTable: "LoreScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultiverseAuditLogs_Multiverses_ContentId",
                table: "MultiverseAuditLogs",
                column: "ContentId",
                principalTable: "Multiverses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniverseAuditLogs_Universes_ContentId",
                table: "UniverseAuditLogs",
                column: "ContentId",
                principalTable: "Universes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
