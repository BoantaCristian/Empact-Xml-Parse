using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlParseAPI.Migrations
{
    public partial class newsgroupsfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsGroup_Group_GroupsId",
                table: "NewsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsGroup_News_NewsId",
                table: "NewsGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsGroup",
                table: "NewsGroup");

            migrationBuilder.RenameTable(
                name: "NewsGroup",
                newName: "NewsGroups");

            migrationBuilder.RenameIndex(
                name: "IX_NewsGroup_NewsId",
                table: "NewsGroups",
                newName: "IX_NewsGroups_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsGroup_GroupsId",
                table: "NewsGroups",
                newName: "IX_NewsGroups_GroupsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsGroups",
                table: "NewsGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsGroups_Group_GroupsId",
                table: "NewsGroups",
                column: "GroupsId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsGroups_News_NewsId",
                table: "NewsGroups",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsGroups_Group_GroupsId",
                table: "NewsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsGroups_News_NewsId",
                table: "NewsGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsGroups",
                table: "NewsGroups");

            migrationBuilder.RenameTable(
                name: "NewsGroups",
                newName: "NewsGroup");

            migrationBuilder.RenameIndex(
                name: "IX_NewsGroups_NewsId",
                table: "NewsGroup",
                newName: "IX_NewsGroup_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsGroups_GroupsId",
                table: "NewsGroup",
                newName: "IX_NewsGroup_GroupsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsGroup",
                table: "NewsGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsGroup_Group_GroupsId",
                table: "NewsGroup",
                column: "GroupsId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsGroup_News_NewsId",
                table: "NewsGroup",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
