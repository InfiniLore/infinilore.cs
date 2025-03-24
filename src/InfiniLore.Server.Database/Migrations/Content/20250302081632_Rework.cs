#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class Rework : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropForeignKey(
            "FK_KeyValueStores_SystemData_Id",
            "KeyValueStores");

        migrationBuilder.DropTable(
            name: "SystemData");

        migrationBuilder.DropTable(
            name: "UserData");

        migrationBuilder.DropTable(
            name: "BasicData");

        migrationBuilder.DropIndex(
            "IX_Users_Username",
            "Users");

        migrationBuilder.AddColumn<DateTime>(
            "CreatedDate",
            "Users",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "LastModifiedDate",
            "Users",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "SoftDeleteDate",
            "Users",
            "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            "CreatedDate",
            "KeyValueStores",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "LastModifiedDate",
            "KeyValueStores",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            "SoftDeleteDate",
            "KeyValueStores",
            "datetime2",
            nullable: true);

        migrationBuilder.CreateTable(
            "LoreScopes",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                ShortDescription = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: false),
                CreatedDate = table.Column<DateTime>("datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>("datetime2", nullable: false),
                SoftDeleteDate = table.Column<DateTime>("datetime2", nullable: true),
                OwnerId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_LoreScopes", columns: x => x.Id);
                table.ForeignKey(
                    "FK_LoreScopes_Users_OwnerId",
                    column: x => x.OwnerId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Users_Id",
            "Users",
            "Id",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Users_Username",
            "Users",
            "Username",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_OwnerId_Name_Unique",
            "LoreScopes",
            new[] { "OwnerId", "Name" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "LoreScopes");

        migrationBuilder.DropIndex(
            "IX_Users_Id",
            "Users");

        migrationBuilder.DropIndex(
            "IX_Users_Username",
            "Users");

        migrationBuilder.DropColumn(
            "CreatedDate",
            "Users");

        migrationBuilder.DropColumn(
            "LastModifiedDate",
            "Users");

        migrationBuilder.DropColumn(
            "SoftDeleteDate",
            "Users");

        migrationBuilder.DropColumn(
            "CreatedDate",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "LastModifiedDate",
            "KeyValueStores");

        migrationBuilder.DropColumn(
            "SoftDeleteDate",
            "KeyValueStores");

        migrationBuilder.CreateTable(
            "BasicData",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                CreatedDate = table.Column<DateTime>("datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>("datetime2", nullable: false),
                SoftDeleteDate = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_BasicData", columns: x => x.Id);
            });

        migrationBuilder.CreateTable(
            "SystemData",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_SystemData", columns: x => x.Id);
                table.ForeignKey(
                    "FK_SystemData_BasicData_Id",
                    column: x => x.Id,
                    "BasicData",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserData",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                OwnerId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_UserData", columns: x => x.Id);
                table.ForeignKey(
                    "FK_UserData_BasicData_Id",
                    column: x => x.Id,
                    "BasicData",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_UserData_Users_OwnerId",
                    column: x => x.OwnerId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Users_Username",
            "Users",
            "Username",
            unique: true,
            filter: "[Username] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_UserData_OwnerId",
            "UserData",
            "OwnerId");

        migrationBuilder.AddForeignKey(
            "FK_KeyValueStores_SystemData_Id",
            "KeyValueStores",
            "Id",
            "SystemData",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
