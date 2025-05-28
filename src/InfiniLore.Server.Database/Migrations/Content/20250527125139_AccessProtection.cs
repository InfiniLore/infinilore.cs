using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfiniLore.Server.Database.Migrations.Content
{
    /// <inheritdoc />
    public partial class AccessProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccessProtectionId",
                table: "LoreScopeModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AccessProtectionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProtectedModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessProtectionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessProtectionModel_InfiniLoreUserModel_ModelOwnerId",
                        column: x => x.ModelOwnerId,
                        principalTable: "InfiniLoreUserModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccessProtectionRuleModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccessProtectionModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessProtectionRuleModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessProtectionRuleModel_AccessProtectionModel_AccessProtectionModelId",
                        column: x => x.AccessProtectionModelId,
                        principalTable: "AccessProtectionModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccessProtectionRuleModel_AccessProtectionModel_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AccessProtectionModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionModel_Id",
                table: "AccessProtectionModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionModel_ModelOwnerId",
                table: "AccessProtectionModel",
                column: "ModelOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionModel_ProtectedModelId",
                table: "AccessProtectionModel",
                column: "ProtectedModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionRuleModel_AccessProtectionModelId",
                table: "AccessProtectionRuleModel",
                column: "AccessProtectionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionRuleModel_Id",
                table: "AccessProtectionRuleModel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessProtectionRuleModel_OwnerId",
                table: "AccessProtectionRuleModel",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserId_Permission_Unique",
                table: "AccessProtectionRuleModel",
                columns: new[] { "UserId", "Permission" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel",
                column: "AccessProtectionId",
                principalTable: "AccessProtectionModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoreScopeModel_AccessProtectionModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.DropTable(
                name: "AccessProtectionRuleModel");

            migrationBuilder.DropTable(
                name: "AccessProtectionModel");

            migrationBuilder.DropIndex(
                name: "IX_LoreScopeModel_AccessProtectionId",
                table: "LoreScopeModel");

            migrationBuilder.DropColumn(
                name: "AccessProtectionId",
                table: "LoreScopeModel");
        }
    }
}
