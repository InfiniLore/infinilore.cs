using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class tpt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicData_AspNetUsers_InfiniLoreUserId",
                table: "BasicData");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicData_AspNetUsers_OwnerId",
                table: "BasicData");

            migrationBuilder.DropIndex(
                name: "IX_BasicData_InfiniLoreUserId",
                table: "BasicData");

            migrationBuilder.DropIndex(
                name: "IX_BasicData_OwnerId",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "ExpiresInDays",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "InfiniLoreUserId",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "BasicData");

            migrationBuilder.CreateTable(
                name: "SystemData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemData_BasicData_Id",
                        column: x => x.Id,
                        principalTable: "BasicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserData_BasicData_Id",
                        column: x => x.Id,
                        principalTable: "BasicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeyValueStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyValueStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyValueStores_SystemData_Id",
                        column: x => x.Id,
                        principalTable: "SystemData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JwtRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiresInDays = table.Column<int>(type: "int", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InfiniLoreUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JwtRefreshTokens_AspNetUsers_InfiniLoreUserId",
                        column: x => x.InfiniLoreUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JwtRefreshTokens_UserData_Id",
                        column: x => x.Id,
                        principalTable: "UserData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwtRefreshTokens_InfiniLoreUserId",
                table: "JwtRefreshTokens",
                column: "InfiniLoreUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_OwnerId",
                table: "UserData",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwtRefreshTokens");

            migrationBuilder.DropTable(
                name: "KeyValueStores");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "SystemData");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BasicData",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpiresInDays",
                table: "BasicData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InfiniLoreUserId",
                table: "BasicData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "BasicData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "BasicData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "BasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "BasicData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                table: "BasicData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "BasicData",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasicData_InfiniLoreUserId",
                table: "BasicData",
                column: "InfiniLoreUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicData_OwnerId",
                table: "BasicData",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicData_AspNetUsers_InfiniLoreUserId",
                table: "BasicData",
                column: "InfiniLoreUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicData_AspNetUsers_OwnerId",
                table: "BasicData",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
