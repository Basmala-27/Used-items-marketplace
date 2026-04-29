using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalPrice",
                table: "Transactions",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionID",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "SwapRequestId",
                table: "SwapRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "SwapRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewerID",
                table: "Reviews",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionID1",
                table: "Reviews",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Offers",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "OfferID",
                table: "Offers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Offers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "Notifications",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Items",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ImageID",
                table: "ItemImages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "FavoriteID",
                table: "Favorites",
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

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Conversations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Conversations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_ApplicationUserId",
                table: "SwapRequests",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_OfferedItemId",
                table: "SwapRequests",
                column: "OfferedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_RequestedItemId",
                table: "SwapRequests",
                column: "RequestedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerID",
                table: "Reviews",
                column: "ReviewerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SellerID",
                table: "Reviews",
                column: "SellerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TransactionID1",
                table: "Reviews",
                column: "TransactionID1");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ApplicationUserId",
                table: "Offers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ApplicationUserId",
                table: "Conversations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ApplicationUserId1",
                table: "Conversations",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_ApplicationUserId",
                table: "Conversations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_ApplicationUserId1",
                table: "Conversations",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerID",
                table: "Reviews",
                column: "ReviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_SellerID",
                table: "Reviews",
                column: "SellerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Transactions_TransactionID1",
                table: "Reviews",
                column: "TransactionID1",
                principalTable: "Transactions",
                principalColumn: "TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_AspNetUsers_ApplicationUserId",
                table: "SwapRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_Items_OfferedItemId",
                table: "SwapRequests",
                column: "OfferedItemId",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SwapRequests_Items_RequestedItemId",
                table: "SwapRequests",
                column: "RequestedItemId",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_ApplicationUserId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_ApplicationUserId1",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_SellerID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Transactions_TransactionID1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SwapRequests_AspNetUsers_ApplicationUserId",
                table: "SwapRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SwapRequests_Items_OfferedItemId",
                table: "SwapRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SwapRequests_Items_RequestedItemId",
                table: "SwapRequests");

            migrationBuilder.DropIndex(
                name: "IX_SwapRequests_ApplicationUserId",
                table: "SwapRequests");

            migrationBuilder.DropIndex(
                name: "IX_SwapRequests_OfferedItemId",
                table: "SwapRequests");

            migrationBuilder.DropIndex(
                name: "IX_SwapRequests_RequestedItemId",
                table: "SwapRequests");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewerID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_SellerID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_TransactionID1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_ApplicationUserId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_ApplicationUserId1",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "SwapRequests");

            migrationBuilder.DropColumn(
                name: "TransactionID1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Conversations");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalPrice",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionID",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "SwapRequestId",
                table: "SwapRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerID",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewID",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Offers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "OfferID",
                table: "Offers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "Notifications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "NotificationID",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "MessageID",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Items",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImageID",
                table: "ItemImages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "FavoriteID",
                table: "Favorites",
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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }
    }
}
