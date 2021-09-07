using Microsoft.EntityFrameworkCore.Migrations;

namespace ReviewService.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Reviewer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "ArticleId", "ReviewContent", "Reviewer" },
                values: new object[,]
                {
                    { 1, 1, "OMG perfect!!", "John Doe" },
                    { 2, 2, "OMG perfect!!", "John Doe" },
                    { 3, 1, "OMG perfect!!", "John Doe" },
                    { 4, 4, "OMG perfect!!", "John Doe" },
                    { 5, 5, "OMG perfect!!", "John Doe" },
                    { 6, 2, "OMG perfect!!", "John Doe" },
                    { 7, 4, "OMG perfect!!", "Jane Doe" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
