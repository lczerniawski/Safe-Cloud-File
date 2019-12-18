using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer_Example.Migrations
{
    public partial class AddisSharedfieldtoFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "File",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "File");
        }
    }
}
