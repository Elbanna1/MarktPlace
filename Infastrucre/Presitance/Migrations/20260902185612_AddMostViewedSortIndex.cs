using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddMostViewedSortIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Views", "CreatedAt" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt",
                table: "Advertisements");
        }
    }
}
