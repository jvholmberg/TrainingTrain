using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Migrations
{
    public partial class migration_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Role_RoleId",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_RoleId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Special",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Update",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Write",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Special",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Write",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Permission",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleId",
                table: "Permission",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Role_RoleId",
                table: "Permission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
