using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCard");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AppUserId",
                table: "Cards",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BookId",
                table: "Cards",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_AppUserId",
                table: "Cards",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Books_BookId",
                table: "Cards",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_AppUserId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Books_BookId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_AppUserId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BookId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "BookCard",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    CardsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCard", x => new { x.BooksId, x.CardsId });
                    table.ForeignKey(
                        name: "FK_BookCard_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCard_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCard_CardsId",
                table: "BookCard",
                column: "CardsId");
        }
    }
}
