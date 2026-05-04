using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyRequestSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyRequests",
                columns: table => new
                {
                    BuyRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerID = table.Column<string>(type: "TEXT", nullable: false),
                    SellerID = table.Column<string>(type: "TEXT", nullable: false),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    EscrowTransactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyRequests", x => x.BuyRequestId);
                    table.ForeignKey(
                        name: "FK_BuyRequests_AspNetUsers_BuyerID",
                        column: x => x.BuyerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyRequests_AspNetUsers_SellerID",
                        column: x => x.SellerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyRequests_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyRequests_BuyerID",
                table: "BuyRequests",
                column: "BuyerID");

            migrationBuilder.CreateIndex(
                name: "IX_BuyRequests_ItemID",
                table: "BuyRequests",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BuyRequests_SellerID",
                table: "BuyRequests",
                column: "SellerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyRequests");
        }
    }
}
