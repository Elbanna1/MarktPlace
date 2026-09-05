using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceSortIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt" },
                descending: new[] { false, false, false, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements");
        }
    }
}
