using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class Feat_JwtRefreshTokenData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicData_AspNetUsers_OwnerId",
                table: "BasicData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicData",
                table: "BasicData");

            migrationBuilder.DropIndex(
                name: "IX_BasicData_Id",
                table: "BasicData");

            migrationBuilder.RenameTable(
                name: "BasicData",
                newName: "UserData");

            migrationBuilder.RenameIndex(
                name: "IX_BasicData_OwnerId",
                table: "UserData",
                newName: "IX_UserData_OwnerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "UserData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "UserData",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<int>(
                name: "ExpiresInDays",
                table: "UserData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InfiniLoreUserId",
                table: "UserData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                table: "UserData",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_UserData_InfiniLoreUserId",
                table: "UserData",
                column: "InfiniLoreUserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_UserData_InfiniLoreUserId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "ExpiresInDays",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "InfiniLoreUserId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "UserData");

            migrationBuilder.RenameTable(
                name: "UserData",
                newName: "BasicData");

            migrationBuilder.RenameIndex(
                name: "IX_UserData_OwnerId",
                table: "BasicData",
                newName: "IX_BasicData_OwnerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "BasicData",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "BasicData",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicData",
                table: "BasicData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BasicData_Id",
                table: "BasicData",
                column: "Id",
                unique: true);

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
