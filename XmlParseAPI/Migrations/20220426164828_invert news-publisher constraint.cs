using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlParseAPI.Migrations
{
    public partial class invertnewspublisherconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_News_NewsId",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_NewsId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Publishers");

            migrationBuilder.AddColumn<int>(
                name: "PublishersId",
                table: "News",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_PublishersId",
                table: "News",
                column: "PublishersId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Publishers_PublishersId",
                table: "News",
                column: "PublishersId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Publishers_PublishersId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_PublishersId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "PublishersId",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Publishers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_NewsId",
                table: "Publishers",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_News_NewsId",
                table: "Publishers",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
