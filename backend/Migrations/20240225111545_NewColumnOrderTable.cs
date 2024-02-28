using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPickupInStore",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPickupInStore",
                table: "Orders");
        }
    }
}
