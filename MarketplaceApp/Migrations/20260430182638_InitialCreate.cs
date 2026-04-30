using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Offers_RelatedOfferID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerID",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RelatedOfferID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: -1);

            migrationBuilder.RenameColumn(
                name: "RelatedOfferID",
                table: "Notifications",
                newName: "RelatedEntityID");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationID",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfferID",
                table: "Notifications",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetUrl",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MessageID",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "ConversationID",
                table: "Conversations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "/images/categories/electronics.jpg", "Electronics" },
                    { 2, "/images/categories/furniture.jpg", "Furniture" },
                    { 3, "/images/categories/fashion.jpg", "Fashion" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_OfferID",
                table: "Notifications",
                column: "OfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Offers_OfferID",
                table: "Notifications",
                column: "OfferID",
                principalTable: "Offers",
                principalColumn: "OfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerID",
                table: "Offers",
                column: "BuyerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Offers_OfferID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerID",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_OfferID",
                table: "Notifications");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OfferID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetUrl",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "RelatedEntityID",
                table: "Notifications",
                newName: "RelatedOfferID");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationID",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "MessageID",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "ConversationID",
                table: "Conversations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { -3, "/images/categories/fashion.jpg", "Fashion" },
                    { -2, "/images/categories/furniture.jpg", "Furniture" },
                    { -1, "/images/categories/electronics.jpg", "Electronics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RelatedOfferID",
                table: "Notifications",
                column: "RelatedOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Offers_RelatedOfferID",
                table: "Notifications",
                column: "RelatedOfferID",
                principalTable: "Offers",
                principalColumn: "OfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_BuyerID",
                table: "Offers",
                column: "BuyerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
