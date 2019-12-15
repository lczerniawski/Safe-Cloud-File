using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer_Example.Migrations
{
    public partial class AddJsonFileIdfieldtoFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonFileId",
                table: "File",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonFileId",
                table: "File");
        }
    }
}
