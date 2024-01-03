using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiodOdStaniula.Migrations
{
    public partial class CartsTableRenameToShopingCartsWithGuidId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Carts_CartId",
                table: "CartItem");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartItem");

            migrationBuilder.AddColumn<Guid>(
                name: "ShopingCartId",
                table: "CartItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopingCarts",
                columns: table => new
                {
                    ShopingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopingCarts", x => x.ShopingCartId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ShopingCartId",
                table: "CartItem",
                column: "ShopingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_ShopingCarts_ShopingCartId",
                table: "CartItem",
                column: "ShopingCartId",
                principalTable: "ShopingCarts",
                principalColumn: "ShopingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_ShopingCarts_ShopingCartId",
                table: "CartItem");

            migrationBuilder.DropTable(
                name: "ShopingCarts");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_ShopingCartId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "ShopingCartId",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "CartItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Carts_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId");
        }
    }
}
