using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Commnets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Commnets_BookId",
                table: "Commnets",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commnets_Books_BookId",
                table: "Commnets",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commnets_Books_BookId",
                table: "Commnets");

            migrationBuilder.DropIndex(
                name: "IX_Commnets_BookId",
                table: "Commnets");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Commnets");
        }
    }
}
