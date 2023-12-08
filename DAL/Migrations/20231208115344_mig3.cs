using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Baskets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "Baskets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_AppUser",
                table: "Baskets",
                column: "AppUser");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserIdId",
                table: "Baskets",
                column: "UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_AppUser",
                table: "Baskets",
                column: "AppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_AspNetUsers_UserIdId",
                table: "Baskets",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_AppUser",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_AspNetUsers_UserIdId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_AppUser",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserIdId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "Baskets");
        }
    }
}
