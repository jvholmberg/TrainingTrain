using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Migrations
{
    public partial class migration_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpires",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "User",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "RefreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "User",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "User",
                newName: "Email");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
