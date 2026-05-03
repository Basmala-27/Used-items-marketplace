using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemForSwapAndSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForSale",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForSwap",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableForSale",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsAvailableForSwap",
                table: "Items");
        }
    }
}
