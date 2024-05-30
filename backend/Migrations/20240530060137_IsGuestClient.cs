using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    /// <inheritdoc />
    public partial class IsGuestClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGuestClient",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGuestClient",
                table: "AspNetUsers");
        }
    }
}
