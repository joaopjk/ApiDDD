using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class NameToNome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "User",
                newName: "Name");
        }
    }
}
