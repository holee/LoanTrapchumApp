using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations.Identity
{
    /// <inheritdoc />
    public partial class addIdentityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_AppUserId",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AppUserId",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.AddColumn<long>(
                name: "UserProfilesId",
                schema: "Security",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permisions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Group = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsBasic = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfilesId",
                schema: "Security",
                table: "Users",
                column: "UserProfilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId",
                schema: "Security",
                table: "Users",
                column: "UserProfilesId",
                principalSchema: "Security",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Permisions",
                schema: "Security");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                schema: "Security",
                table: "UserProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AppUserId",
                schema: "Security",
                table: "UserProfiles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_AppUserId",
                schema: "Security",
                table: "UserProfiles",
                column: "AppUserId",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
