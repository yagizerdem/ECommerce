using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_AppUser",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_AppUser",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Baskets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Baskets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_UserId",
                table: "Baskets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_UserId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Baskets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Baskets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_AppUser",
                table: "Baskets",
                column: "AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_AppUser",
                table: "Baskets",
                column: "AppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
