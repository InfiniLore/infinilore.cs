using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Database.MsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfiniLoreUserInfinilorePermission");

            migrationBuilder.CreateTable(
                name: "InfiniLorePermissionInfiniLoreUser",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfiniLorePermissionInfiniLoreUser", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_InfiniLorePermissionInfiniLoreUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfiniLorePermissionInfiniLoreUser_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemInformation",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemInformation", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLorePermissionInfiniLoreUser_UsersId",
                table: "InfiniLorePermissionInfiniLoreUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemInformation_Name",
                table: "SystemInformation",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfiniLorePermissionInfiniLoreUser");

            migrationBuilder.DropTable(
                name: "SystemInformation");

            migrationBuilder.CreateTable(
                name: "InfiniLoreUserInfinilorePermission",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfiniLoreUserInfinilorePermission", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_InfiniLoreUserInfinilorePermission_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfiniLoreUserInfinilorePermission_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfiniLoreUserInfinilorePermission_UsersId",
                table: "InfiniLoreUserInfinilorePermission",
                column: "UsersId");
        }
    }
}
