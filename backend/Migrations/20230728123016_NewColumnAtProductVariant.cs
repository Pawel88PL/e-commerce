using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    public partial class NewColumnAtProductVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAvailable",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhotoUrlAddress",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "AmountAvailable",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrlAddress",
                table: "ProductVariants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountAvailable",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "PhotoUrlAddress",
                table: "ProductVariants");

            migrationBuilder.AddColumn<int>(
                name: "AmountAvailable",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrlAddress",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
