using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class FluentApiUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopes_AspNetUsers_UserId",
                table: "LoreScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_Multiverses_AspNetUsers_UserId",
                table: "Multiverses");

            migrationBuilder.DropForeignKey(
                name: "FK_Universes_AspNetUsers_UserId",
                table: "Universes");

            migrationBuilder.DropForeignKey(
                name: "FK_Universes_Multiverses_MultiverseModelId",
                table: "Universes");

            migrationBuilder.DropIndex(
                name: "IX_Universes_Name_Id",
                table: "Universes");

            migrationBuilder.DropIndex(
                name: "IX_Multiverses_Name_Id",
                table: "Multiverses");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopes_Id_Name",
                table: "LoreScopes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06e709ed-b734-4144-aad5-675fc161d8e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fff5d2e4-3ae1-4cea-aa02-1874aa44eb07");

            migrationBuilder.RenameColumn(
                name: "MultiverseModelId",
                table: "Universes",
                newName: "MultiverseId");

            migrationBuilder.RenameIndex(
                name: "IX_Universes_MultiverseModelId",
                table: "Universes",
                newName: "IX_Universes_MultiverseId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Universes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Multiverses",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LoreScopes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopes_AspNetUsers_UserId",
                table: "LoreScopes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Multiverses_AspNetUsers_UserId",
                table: "Multiverses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Universes_AspNetUsers_UserId",
                table: "Universes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Universes_Multiverses_MultiverseId",
                table: "Universes",
                column: "MultiverseId",
                principalTable: "Multiverses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopes_AspNetUsers_UserId",
                table: "LoreScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_Multiverses_AspNetUsers_UserId",
                table: "Multiverses");

            migrationBuilder.DropForeignKey(
                name: "FK_Universes_AspNetUsers_UserId",
                table: "Universes");

            migrationBuilder.DropForeignKey(
                name: "FK_Universes_Multiverses_MultiverseId",
                table: "Universes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e9b5e0f-82a1-43a5-ab9b-a53b7d432d7f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e19fffc1-29b6-450e-8d28-5e874b7095b3");

            migrationBuilder.RenameColumn(
                name: "MultiverseId",
                table: "Universes",
                newName: "MultiverseModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Universes_MultiverseId",
                table: "Universes",
                newName: "IX_Universes_MultiverseModelId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Universes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Multiverses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LoreScopes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06e709ed-b734-4144-aad5-675fc161d8e8", null, "admin", "ADMIN" },
                    { "fff5d2e4-3ae1-4cea-aa02-1874aa44eb07", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0ff67fda-a9fc-44b8-be33-5c69415ac22a", "AQAAAAIAAYagAAAAEDI6i5q8eb2a1gxnJZAp4/sHnuHrKfSDGxbhu694LpM85D0K/azW3KenvIhpBYM4mQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Universes_Name_Id",
                table: "Universes",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_Name_Id",
                table: "Multiverses",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopes_Id_Name",
                table: "LoreScopes",
                columns: new[] { "Id", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopes_AspNetUsers_UserId",
                table: "LoreScopes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Multiverses_AspNetUsers_UserId",
                table: "Multiverses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Universes_AspNetUsers_UserId",
                table: "Universes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Universes_Multiverses_MultiverseModelId",
                table: "Universes",
                column: "MultiverseModelId",
                principalTable: "Multiverses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
