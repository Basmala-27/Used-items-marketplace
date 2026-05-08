using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class FixItemDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemID1",
                table: "Favorites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ItemID1",
                table: "Favorites",
                column: "ItemID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Items_ItemID1",
                table: "Favorites",
                column: "ItemID1",
                principalTable: "Items",
                principalColumn: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Items_ItemID1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ItemID1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ItemID1",
                table: "Favorites");
        }
    }
}
