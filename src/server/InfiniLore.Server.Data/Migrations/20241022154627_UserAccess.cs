using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfiniLore.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserAccess : Migration
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

            migrationBuilder.DropTable(
                name: "LoreScopeAuditLogs");

            migrationBuilder.DropTable(
                name: "MultiverseAuditLogs");

            migrationBuilder.DropTable(
                name: "UniverseAuditLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61c88947-ff70-4882-b509-91cc8992c61a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61d818d3-3df3-4b3d-ab22-7cb4f6c8b549");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Universes");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Universes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Multiverses");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Multiverses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LoreScopes");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "LoreScopes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Universes",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Universes_UserId",
                table: "Universes",
                newName: "IX_Universes_OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Multiverses",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Multiverses_UserId",
                table: "Multiverses",
                newName: "IX_Multiverses_OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LoreScopes",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_LoreScopes_UserId",
                table: "LoreScopes",
                newName: "IX_LoreScopes_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_LoreScopes_Name_UserId",
                table: "LoreScopes",
                newName: "IX_LoreScopes_Name_OwnerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Universes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Multiverses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "LoreScopes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserContentAccess<LoreScopeModel>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    LoreScopeModelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SoftDeleteDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentAccess<LoreScopeModel>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<LoreScopeModel>_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<LoreScopeModel>_LoreScopes_LoreScopeModelId",
                        column: x => x.LoreScopeModelId,
                        principalTable: "LoreScopes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserContentAccess<MultiverseModel>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    MultiverseModelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SoftDeleteDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentAccess<MultiverseModel>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<MultiverseModel>_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<MultiverseModel>_Multiverses_MultiverseModelId",
                        column: x => x.MultiverseModelId,
                        principalTable: "Multiverses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserContentAccess<UniverseModel>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    UniverseModelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SoftDeleteDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentAccess<UniverseModel>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<UniverseModel>_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentAccess<UniverseModel>_Universes_UniverseModelId",
                        column: x => x.UniverseModelId,
                        principalTable: "Universes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ff7761f-b907-493b-8b89-43b2ea6f644f", null, "admin", "ADMIN" },
                    { "f443a138-c5da-4f16-abac-b55ae062c731", null, "user", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "89bd417a-a110-4cf2-8a83-09dc79e47335", "AQAAAAIAAYagAAAAEB5p3i60R7of7y4lr9a8K13lTFUUhOYvDP6a38zvLQMYNCmfqaQ0KL2r9XF0KYX4sw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Universes_Id_OwnerId_IsPublic",
                table: "Universes",
                columns: new[] { "Id", "OwnerId", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_Multiverses_Id_OwnerId_IsPublic",
                table: "Multiverses",
                columns: new[] { "Id", "OwnerId", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopes_Id_OwnerId_IsPublic",
                table: "LoreScopes",
                columns: new[] { "Id", "OwnerId", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<LoreScopeModel>_LoreScopeModelId",
                table: "UserContentAccess<LoreScopeModel>",
                column: "LoreScopeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<LoreScopeModel>_UserId",
                table: "UserContentAccess<LoreScopeModel>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<MultiverseModel>_MultiverseModelId",
                table: "UserContentAccess<MultiverseModel>",
                column: "MultiverseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<MultiverseModel>_UserId",
                table: "UserContentAccess<MultiverseModel>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<UniverseModel>_UniverseModelId",
                table: "UserContentAccess<UniverseModel>",
                column: "UniverseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentAccess<UniverseModel>_UserId",
                table: "UserContentAccess<UniverseModel>",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopes_AspNetUsers_OwnerId",
                table: "LoreScopes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Multiverses_AspNetUsers_OwnerId",
                table: "Multiverses",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Universes_AspNetUsers_OwnerId",
                table: "Universes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopes_AspNetUsers_OwnerId",
                table: "LoreScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_Multiverses_AspNetUsers_OwnerId",
                table: "Multiverses");

            migrationBuilder.DropForeignKey(
                name: "FK_Universes_AspNetUsers_OwnerId",
                table: "Universes");

            migrationBuilder.DropTable(
                name: "UserContentAccess<LoreScopeModel>");

            migrationBuilder.DropTable(
                name: "UserContentAccess<MultiverseModel>");

            migrationBuilder.DropTable(
                name: "UserContentAccess<UniverseModel>");

            migrationBuilder.DropIndex(
                name: "IX_Universes_Id_OwnerId_IsPublic",
                table: "Universes");

            migrationBuilder.DropIndex(
                name: "IX_Multiverses_Id_OwnerId_IsPublic",
                table: "Multiverses");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopes_Id_OwnerId_IsPublic",
                table: "LoreScopes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ff7761f-b907-493b-8b89-43b2ea6f644f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f443a138-c5da-4f16-abac-b55ae062c731");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Universes");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Multiverses");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "LoreScopes");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Universes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Universes_OwnerId",
                table: "Universes",
                newName: "IX_Universes_UserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Multiverses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Multiverses_OwnerId",
                table: "Multiverses",
                newName: "IX_Multiverses_UserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "LoreScopes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LoreScopes_OwnerId",
                table: "LoreScopes",
                newName: "IX_LoreScopes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LoreScopes_Name_OwnerId",
                table: "LoreScopes",
                newName: "IX_LoreScopes_Name_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Universes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Universes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Multiverses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Multiverses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LoreScopes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "LoreScopes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoreScopeAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChangeType = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedProperties = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoreScopeAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoreScopeAuditLogs_LoreScopes_ContentId",
                        column: x => x.ContentId,
                        principalTable: "LoreScopes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MultiverseAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChangeType = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedProperties = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiverseAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultiverseAuditLogs_Multiverses_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Multiverses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UniverseAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChangeType = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedProperties = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniverseAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniverseAuditLogs_Universes_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Universes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61c88947-ff70-4882-b509-91cc8992c61a", null, "user", "USER" },
                    { "61d818d3-3df3-4b3d-ab22-7cb4f6c8b549", null, "admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d957c0f8-e90e-4068-a968-4f4b49fc165c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "920180d9-5360-495b-abb7-fb8365605ab5", "AQAAAAIAAYagAAAAEAJItp0RegzW1PITs52O0VCpDT1DEMMTRzQ4C3Q2RCnLOmmbFwxrKIaDDknLHn23og==" });

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeAuditLogs_ContentId",
                table: "LoreScopeAuditLogs",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiverseAuditLogs_ContentId",
                table: "MultiverseAuditLogs",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_UniverseAuditLogs_ContentId",
                table: "UniverseAuditLogs",
                column: "ContentId");

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
        }
    }
}
