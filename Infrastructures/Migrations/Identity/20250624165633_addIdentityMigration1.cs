using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations.Identity
{
    /// <inheritdoc />
    public partial class addIdentityMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles",
                columns: new[] { "Id", "UserRefId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfilesId_UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                columns: new[] { "UserProfilesId", "UserProfilesUserRefId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId_UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                columns: new[] { "UserProfilesId", "UserProfilesUserRefId" },
                principalSchema: "Security",
                principalTable: "UserProfiles",
                principalColumns: new[] { "Id", "UserRefId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId_UserProfilesUserRefId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfilesId_UserProfilesUserRefId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UserProfilesUserRefId",
                schema: "Security",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles",
                column: "Id");

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
    }
}
