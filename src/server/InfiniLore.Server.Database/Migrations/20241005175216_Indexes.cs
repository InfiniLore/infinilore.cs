using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e9b5e0f-82a1-43a5-ab9b-a53b7d432d7f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e19fffc1-29b6-450e-8d28-5e874b7095b3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b2ca9c9-b6c0-4e74-bc3f-e0a97750690e", null, "user", "USER" },
                    { "72f392fc-7dae-4c9f-a5a8-9a411c4ec5df", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8459c3d9-2ff7-42f1-990c-419e9e40b0bc", "AQAAAAIAAYagAAAAENWUc+okxlfDE54O7664a2QokK31ny3Fs60/0TX3gisPA/EYkKCPWN/VRHiXQ+qyBA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Universes_Name_MultiverseId",
                table: "Universes",
                columns: new[] { "Name", "MultiverseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_Name_LoreScopeId",
                table: "Multiverses",
                columns: new[] { "Name", "LoreScopeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopes_Name_UserId",
                table: "LoreScopes",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Universes_Name_MultiverseId",
                table: "Universes");

            migrationBuilder.DropIndex(
                name: "IX_Multiverses_Name_LoreScopeId",
                table: "Multiverses");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopes_Name_UserId",
                table: "LoreScopes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b2ca9c9-b6c0-4e74-bc3f-e0a97750690e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72f392fc-7dae-4c9f-a5a8-9a411c4ec5df");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9e9b5e0f-82a1-43a5-ab9b-a53b7d432d7f", null, "user", "USER" },
                    { "e19fffc1-29b6-450e-8d28-5e874b7095b3", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dbd15b09-4e94-48be-9f2a-b7aa57bd4ed8", "AQAAAAIAAYagAAAAEMZMdceEAC7TPHz6InwcGbOrNc8OBAMcRhxgg4Vf2vyKaKKtz+pBh1v5mlcUCmmZFA==" });
        }
    }
}
