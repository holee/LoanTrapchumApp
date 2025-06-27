using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations.Identity
{
    /// <inheritdoc />
    public partial class UpdateUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UserRefId",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UserProfilesId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles",
                column: "UserRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                column: "UserProfilesUserRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                column: "UserProfilesUserRefId",
                principalSchema: "Security",
                principalTable: "UserProfiles",
                principalColumn: "UserRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesUserRefId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfilesUserRefId",
                schema: "Security",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                schema: "Security",
                table: "UserProfiles");

            migrationBuilder.AddColumn<long>(
                name: "UserProfilesId",
                schema: "Security",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                schema: "Security",
                table: "UserProfiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserRefId",
                schema: "Security",
                table: "UserProfiles",
                column: "UserRefId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfilesId_UserProfilesUserRefId",
                schema: "Security",
                table: "Users",
                columns: new[] { "UserProfilesId", "UserProfilesUserRefId" },
                principalSchema: "Security",
                principalTable: "UserProfiles",
                principalColumns: new[] { "Id", "UserRefId" });
        }
    }
}
