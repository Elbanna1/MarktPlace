using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListingOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListingTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingRatings", x => x.Id);
                    table.CheckConstraint("CK_ListingRatings_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_ListingRatings_AspNetUsers_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingRatings_Listing",
                table: "ListingRatings",
                columns: new[] { "ListingType", "ListingId" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingRatings_Listing_Reviewer",
                table: "ListingRatings",
                columns: new[] { "ListingType", "ListingId", "ReviewerUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingRatings_Reviewer_CreatedAt",
                table: "ListingRatings",
                columns: new[] { "ReviewerUserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingRatings");
        }
    }
}
