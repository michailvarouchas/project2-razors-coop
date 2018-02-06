using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Project2Cooperation.Migrations
{
    public partial class usercartitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_UserCart_UserCartApplicationUserId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_UserCartApplicationUserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserCartApplicationUserId",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "UserCartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UserCartApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_UserCartItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCartItems_UserCart_UserCartApplicationUserId",
                        column: x => x.UserCartApplicationUserId,
                        principalTable: "UserCart",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCartItems_OrderId",
                table: "UserCartItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCartItems_ProductId",
                table: "UserCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCartItems_UserCartApplicationUserId",
                table: "UserCartItems",
                column: "UserCartApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCartItems");

            migrationBuilder.AddColumn<string>(
                name: "UserCartApplicationUserId",
                table: "CartItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserCartApplicationUserId",
                table: "CartItems",
                column: "UserCartApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_UserCart_UserCartApplicationUserId",
                table: "CartItems",
                column: "UserCartApplicationUserId",
                principalTable: "UserCart",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
