using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations.Identity
{
    /// <inheritdoc />
    public partial class UpdateUserProfile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileTitle",
                schema: "Security",
                table: "UserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileTitle",
                schema: "Security",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
