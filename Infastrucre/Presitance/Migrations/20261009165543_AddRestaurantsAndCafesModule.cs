using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantsAndCafesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantsAndCafes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMapsUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepublishCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModeratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModeratedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectionReason = table.Column<int>(type: "int", nullable: true),
                    ModerationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantsAndCafes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantsAndCafes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantAndCafeImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantAndCafeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantAndCafeImages_RestaurantsAndCafes_ListingId",
                        column: x => x.ListingId,
                        principalTable: "RestaurantsAndCafes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Admins can create categories with Max(Id) + 1, so the seeded ids are checked rather than
            // blindly inserted: a row that is already Restaurants & Cafes is kept, an id held by anything else aborts.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 15 AND [Name] <> N'Restaurants & Cafes')
BEGIN
    THROW 51000, 'Category id 15 is already used by another category; Restaurants & Cafes was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 15)
    INSERT INTO [Categories] ([Id], [Icon], [IsActive], [Name], [NameAr]) VALUES (15, NULL, 1, N'Restaurants & Cafes', N'مطاعم وكافيهات');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 64 AND ([CategoryId] <> 15 OR [Name] <> N'Restaurants & Cafes'))
BEGIN
    THROW 51000, 'Sub-category id 64 is already used by another sub-category; Restaurants & Cafes was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 64)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (64, 15, NULL, 1, N'Restaurants & Cafes', N'مطاعم وكافيهات');
");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantAndCafeImages_ListingId_SortOrder",
                table: "RestaurantAndCafeImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_Advertisers",
                table: "RestaurantsAndCafes",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_ExpireAt",
                table: "RestaurantsAndCafes",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_Feed_MostViewed",
                table: "RestaurantsAndCafes",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_IsDeleted",
                table: "RestaurantsAndCafes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_ModerationStatus_CreatedAt_Id",
                table: "RestaurantsAndCafes",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsAndCafes_UserId_CreatedAt",
                table: "RestaurantsAndCafes",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantAndCafeImages");

            migrationBuilder.DropTable(
                name: "RestaurantsAndCafes");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
