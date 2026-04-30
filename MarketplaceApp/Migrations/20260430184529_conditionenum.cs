using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class conditionenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Condition",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "Items",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
