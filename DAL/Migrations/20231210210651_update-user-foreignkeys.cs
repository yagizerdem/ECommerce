using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateuserforeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_UserIdId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserIdId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Baskets");

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

            migrationBuilder.AddColumn<int>(
                name: "StockCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Baskets",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StockCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Baskets");

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Baskets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserIdId",
                table: "Baskets",
                column: "UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_UserIdId",
                table: "Baskets",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
