using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatetobasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "Baskets");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Baskets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Baskets");

            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "Baskets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
