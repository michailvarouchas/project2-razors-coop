using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Project2Cooperation.Migrations
{
    public partial class dbcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCartApplicationUserId",
                table: "CartItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCart",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCart", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_UserCart_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_UserCart_UserCartApplicationUserId",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "UserCart");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_UserCartApplicationUserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserCartApplicationUserId",
                table: "CartItems");
        }
    }
}
