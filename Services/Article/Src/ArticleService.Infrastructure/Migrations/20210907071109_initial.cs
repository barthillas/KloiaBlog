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
                values: new object[,]
                {
                    { 1, "Totem und Tabu", "Sigmund Freud", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Totem und Tabu" },
                    { 2, "Madde ve Bellek", "Henri Bergson", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Madde ve Bellek" },
                    { 3, "Moses and Monotheism", "Sigmund Freud", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Moses and Monotheism" },
                    { 4, "Little Prince", "Antoine de saint exupery", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Little Prince" },
                    { 5, "Lage de raison", "Jean-Paul Sartre", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Lage de raison" },
                    { 6, "ostre sledovane vlaky rozbor", "Bohumil Hrabal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "ostre sledovane vlaky rozbor" },
                    { 7, "Ningen Shikkaku", "Osamu Dazai", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5, "Ningen Shikkaku" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
