using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedSocial.Infraestructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImagge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                schema: "Identity",
                table: "Users");
        }
    }
}
