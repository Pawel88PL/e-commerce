using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    public partial class Popularity_DateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Products");
        }
    }
}
