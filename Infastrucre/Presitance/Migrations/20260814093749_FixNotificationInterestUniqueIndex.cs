using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class FixNotificationInterestUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserNotificationInterests_User_Category_SubCategory",
                table: "UserNotificationInterests");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationInterests_User_Category_SubCategory",
                table: "UserNotificationInterests",
                columns: new[] { "UserId", "CategoryId", "SubCategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserNotificationInterests_User_Category_SubCategory",
                table: "UserNotificationInterests");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationInterests_User_Category_SubCategory",
                table: "UserNotificationInterests",
                columns: new[] { "UserId", "CategoryId", "SubCategoryId" },
                unique: true,
                filter: "[SubCategoryId] IS NOT NULL");
        }
    }
}
