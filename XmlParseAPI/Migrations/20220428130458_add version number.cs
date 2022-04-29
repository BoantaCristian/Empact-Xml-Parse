using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlParseAPI.Migrations
{
    public partial class addversionnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VersionNumbers",
                columns: table => new
                {
                    version_number = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionNumbers", x => x.version_number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VersionNumbers");
        }
    }
}
