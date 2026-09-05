using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationPresentationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeepLink",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "Notifications",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Notifications",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_ReferenceType",
                table: "Notifications",
                columns: new[] { "UserId", "ReferenceType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_ReferenceType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeepLink",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Notifications");
        }
    }
}
