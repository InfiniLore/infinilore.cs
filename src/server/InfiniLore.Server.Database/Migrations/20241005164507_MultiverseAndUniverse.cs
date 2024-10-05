using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class MultiverseAndUniverse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ea74fbc-a094-4c0d-9b61-49cba2b99445");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90d84984-0a3b-418a-a94a-82348a43f5d5");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LoreScopes",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LoreScopes",
                type: "TEXT",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Multiverses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoreScopeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multiverses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Multiverses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Multiverses_LoreScopes_LoreScopeId",
                        column: x => x.LoreScopeId,
                        principalTable: "LoreScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Universes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MultiverseModelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Universes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Universes_Multiverses_MultiverseModelId",
                        column: x => x.MultiverseModelId,
                        principalTable: "Multiverses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_LoreScopes_Id_Name",
                table: "LoreScopes",
                columns: new[] { "Id", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_LoreScopeId",
                table: "Multiverses",
                column: "LoreScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_Name_Id",
                table: "Multiverses",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_UserId",
                table: "Multiverses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Universes_MultiverseModelId",
                table: "Universes",
                column: "MultiverseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Universes_Name_Id",
                table: "Universes",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Universes_UserId",
                table: "Universes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Universes");

            migrationBuilder.DropTable(
                name: "Multiverses");

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

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LoreScopes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LoreScopes");

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
    }
}
