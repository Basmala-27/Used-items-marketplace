using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class at : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 4, "/images/categories/phones.jpg", "Phones" },
                    { 5, "/images/categories/laptops.jpg", "Laptops" },
                    { 6, "/images/categories/accessories.jpg", "Accessories" },
                    { 7, "/images/categories/gaming.jpg", "Gaming" },
                    { 8, "/images/categories/vehicles.jpg", "Vehicles" },
                    { 9, "/images/categories/study.jpg", "Study Materials" },
                    { 10, "/images/categories/others.jpg", "Others" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 10);
        }
    }
}
