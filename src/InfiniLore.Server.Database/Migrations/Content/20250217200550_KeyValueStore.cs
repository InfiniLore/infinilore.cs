using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class KeyValueStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_AspNetUsers_InfiniLoreUserId",
                table: "UserData");

            migrationBuilder.DropForeignKey(
                name: "FK_UserData_AspNetUsers_OwnerId",
                table: "UserData");

            migrationBuilder.DropTable(
                name: "SystemData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserData",
                table: "UserData");

            migrationBuilder.RenameTable(
                name: "UserData",
                newName: "BasicData");

            migrationBuilder.RenameIndex(
                name: "IX_UserData_OwnerId",
                table: "BasicData",
                newName: "IX_BasicData_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserData_InfiniLoreUserId",
                table: "BasicData",
                newName: "IX_BasicData_InfiniLoreUserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "BasicData",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Key",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicData",
                table: "BasicData",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicData_AspNetUsers_InfiniLoreUserId",
                table: "BasicData");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicData_AspNetUsers_OwnerId",
                table: "BasicData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicData",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "BasicData");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "BasicData");

            migrationBuilder.RenameTable(
                name: "BasicData",
                newName: "UserData");

            migrationBuilder.RenameIndex(
                name: "IX_BasicData_OwnerId",
                table: "UserData",
                newName: "IX_UserData_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicData_InfiniLoreUserId",
                table: "UserData",
                newName: "IX_UserData_InfiniLoreUserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "UserData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserData",
                table: "UserData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SystemData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemData", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_AspNetUsers_InfiniLoreUserId",
                table: "UserData",
                column: "InfiniLoreUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_AspNetUsers_OwnerId",
                table: "UserData",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
