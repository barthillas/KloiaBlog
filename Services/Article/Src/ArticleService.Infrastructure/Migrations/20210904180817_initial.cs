using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArticleService.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StarCount = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "ArticleContent", "Author", "PublishDate", "StarCount", "Title" },
                values: new object[] { 1, "Totem und Tabu", "Freud", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, null });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "ArticleContent", "Author", "PublishDate", "StarCount", "Title" },
                values: new object[] { 2, "Freud", "Moses and Monotheism", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
