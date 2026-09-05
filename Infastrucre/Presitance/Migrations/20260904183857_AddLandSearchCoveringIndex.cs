using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddLandSearchCoveringIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lands_Search",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsDeleted", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "Title", "Description", "District", "Address", "OtherLandType", "OtherProject", "CreatedAt", "IsPremium", "IsFeatured", "TotalPrice" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lands_Search",
                table: "Lands");
        }
    }
}
