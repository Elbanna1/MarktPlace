using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingInteractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ListingFavorites",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingFavorites", x => new { x.UserId, x.ListingType, x.ListingId });
                    table.ForeignKey(
                        name: "FK_ListingFavorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListingReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListingOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListingTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingReports_AspNetUsers_ReporterUserId",
                        column: x => x.ReporterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListingViewCounters",
                columns: table => new
                {
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastViewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingViewCounters", x => new { x.ListingType, x.ListingId });
                });

            migrationBuilder.CreateTable(
                name: "ListingViewers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewerKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingViewers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingFavorites_Listing",
                table: "ListingFavorites",
                columns: new[] { "ListingType", "ListingId" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingFavorites_User_CreatedAt",
                table: "ListingFavorites",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingReports_Listing",
                table: "ListingReports",
                columns: new[] { "ListingType", "ListingId" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingReports_Listing_Reporter",
                table: "ListingReports",
                columns: new[] { "ListingType", "ListingId", "ReporterUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingReports_ReporterUserId",
                table: "ListingReports",
                column: "ReporterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingReports_Status_CreatedAt",
                table: "ListingReports",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingViewers_Listing_Viewer",
                table: "ListingViewers",
                columns: new[] { "ListingType", "ListingId", "ViewerKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingViewers_User_LastViewedAt",
                table: "ListingViewers",
                columns: new[] { "UserId", "LastViewedAt" },
                filter: "[UserId] IS NOT NULL");

            // Carry the سيارات view counts over to the shared counter.
            //
            // ListingViewCounters becomes the single source of truth for views across all 45 modules,
            // including الإعلانات, whose totals used to live in Advertisements.Views. Without this
            // backfill every existing advertisement would silently drop to zero views on deploy.
            // ListingType 1 is ListingModuleType.Advertisement.
            migrationBuilder.Sql(@"
                INSERT INTO [ListingViewCounters] ([ListingType], [ListingId], [TotalViews], [LastViewedAt])
                SELECT 1, a.[Id], a.[Views],
                       (SELECT MAX(v.[ViewedAt]) FROM [AdvertisementViews] v WHERE v.[AdvertisementId] = a.[Id])
                FROM [Advertisements] a
                WHERE a.[Views] > 0
                  AND NOT EXISTS (
                      SELECT 1 FROM [ListingViewCounters] c
                      WHERE c.[ListingType] = 1 AND c.[ListingId] = a.[Id]);");

            // Carry the per-viewer history over too, so the deduplication window keeps working for
            // people who already viewed something, and شوهد مؤخرًا is not empty on day one. One row
            // per (advertisement, viewer): the old table logged one row per visit, so they are folded
            // together here the way the new table stores them.
            migrationBuilder.Sql($@"
                INSERT INTO [ListingViewers]
                    ([ListingType], [ListingId], [ViewerKey], [UserId], [FirstViewedAt], [LastViewedAt], [ViewCount])
                SELECT 1,
                       v.[AdvertisementId],
                       COALESCE(v.[ViewerUserId], 'ip:' + v.[IpAddress]),
                       v.[ViewerUserId],
                       MIN(v.[ViewedAt]),
                       MAX(v.[ViewedAt]),
                       COUNT(*)
                FROM [AdvertisementViews] v
                WHERE v.[ViewerUserId] IS NOT NULL OR v.[IpAddress] IS NOT NULL
                GROUP BY v.[AdvertisementId], COALESCE(v.[ViewerUserId], 'ip:' + v.[IpAddress]), v.[ViewerUserId];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingFavorites");

            migrationBuilder.DropTable(
                name: "ListingReports");

            migrationBuilder.DropTable(
                name: "ListingViewCounters");

            migrationBuilder.DropTable(
                name: "ListingViewers");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Advertisements");
        }
    }
}
