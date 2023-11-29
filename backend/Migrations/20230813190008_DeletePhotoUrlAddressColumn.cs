using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    public partial class DeletePhotoUrlAddressColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrlAddress",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrlAddress",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
