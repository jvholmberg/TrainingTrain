using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Migrations
{
    public partial class migration_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenValid",
                table: "User",
                newName: "TokenExpires");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenExpires",
                table: "User",
                newName: "TokenValid");
        }
    }
}
