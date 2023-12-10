using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_AppUser",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_AppUser",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookName",
                table: "Books",
                newName: "Author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "BookName");

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AppUser",
                table: "Books",
                column: "AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_AppUser",
                table: "Books",
                column: "AppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
