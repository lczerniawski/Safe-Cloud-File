using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer_Example.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    FileType = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    IsShared = table.Column<bool>(nullable: false),
                    JsonFileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    RsaKeys_D = table.Column<byte[]>(nullable: true),
                    RsaKeys_DP = table.Column<byte[]>(nullable: true),
                    RsaKeys_DQ = table.Column<byte[]>(nullable: true),
                    RsaKeys_Exponent = table.Column<byte[]>(nullable: true),
                    RsaKeys_InverseQ = table.Column<byte[]>(nullable: true),
                    RsaKeys_Modulus = table.Column<byte[]>(nullable: true),
                    RsaKeys_P = table.Column<byte[]>(nullable: true),
                    RsaKeys_Q = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
