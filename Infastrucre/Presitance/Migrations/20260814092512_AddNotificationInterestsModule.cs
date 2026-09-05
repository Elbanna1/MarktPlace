using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationInterestsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListingType",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListingNotificationDispatches",
                columns: table => new
                {
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecipientCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingNotificationDispatches", x => new { x.ListingType, x.ListingId });
                });

            migrationBuilder.CreateTable(
                name: "UserNotificationInterests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationInterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotificationInterests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNotificationPreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NewListingsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationPreferences", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserNotificationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationInterests_Category_SubCategory_Enabled",
                table: "UserNotificationInterests",
                columns: new[] { "CategoryId", "SubCategoryId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationInterests_User_Category_SubCategory",
                table: "UserNotificationInterests",
                columns: new[] { "UserId", "CategoryId", "SubCategoryId" },
                unique: true,
                filter: "[SubCategoryId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingNotificationDispatches");

            migrationBuilder.DropTable(
                name: "UserNotificationInterests");

            migrationBuilder.DropTable(
                name: "UserNotificationPreferences");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Notifications");
        }
    }
}
