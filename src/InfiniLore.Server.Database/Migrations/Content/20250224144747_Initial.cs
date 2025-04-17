#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InfiniLore.Server.Database.Migrations.Content;
/// <inheritdoc />
public partial class Initial : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
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
            "Users",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Auth0IdGoogle = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                Auth0Github = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_Users", columns: x => x.Id);
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

        migrationBuilder.CreateTable(
            "KeyValueStores",
            columns: table => new {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Key = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                Value = table.Column<string>("nvarchar(1024)", maxLength: 1024, nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_KeyValueStores", columns: x => x.Id);
                table.ForeignKey(
                    "FK_KeyValueStores_SystemData_Id",
                    column: x => x.Id,
                    "SystemData",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_UserData_OwnerId",
            "UserData",
            "OwnerId");

        migrationBuilder.CreateIndex(
            "IX_Users_Auth0Github",
            "Users",
            "Auth0Github",
            unique: true,
            filter: "[Auth0Github] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_Users_Auth0IdGoogle",
            "Users",
            "Auth0IdGoogle",
            unique: true,
            filter: "[Auth0IdGoogle] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "KeyValueStores");

        migrationBuilder.DropTable(
            name: "UserData");

        migrationBuilder.DropTable(
            name: "SystemData");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "BasicData");
    }
}
