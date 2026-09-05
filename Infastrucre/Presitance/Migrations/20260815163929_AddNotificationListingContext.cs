using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationListingContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Notifications",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListingTitle",
                table: "Notifications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Notifications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Notifications",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategoryName",
                table: "Notifications",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ListingTitle",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SubCategoryName",
                table: "Notifications");
        }
    }
}
