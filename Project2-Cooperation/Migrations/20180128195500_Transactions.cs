using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Project2Cooperation.Migrations
{
    public partial class Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalAccount_AspNetUsers_ApplicationUserId",
                table: "InternalAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InternalAccount",
                table: "InternalAccount");

            migrationBuilder.RenameTable(
                name: "InternalAccount",
                newName: "InternalAccounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InternalAccounts",
                table: "InternalAccounts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalAccounts_AspNetUsers_ApplicationUserId",
                table: "InternalAccounts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalAccounts_AspNetUsers_ApplicationUserId",
                table: "InternalAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InternalAccounts",
                table: "InternalAccounts");

            migrationBuilder.RenameTable(
                name: "InternalAccounts",
                newName: "InternalAccount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InternalAccount",
                table: "InternalAccount",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalAccount_AspNetUsers_ApplicationUserId",
                table: "InternalAccount",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
