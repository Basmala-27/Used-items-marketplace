using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContextStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_BuyerID",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_BuyerID",
                table: "Offers",
                column: "BuyerID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_BuyerID",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_BuyerID",
                table: "Offers",
                column: "BuyerID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
