using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerBookingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BannerPlacementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxSlots = table.Column<int>(type: "int", nullable: false),
                    DesktopWidth = table.Column<int>(type: "int", nullable: false),
                    DesktopHeight = table.Column<int>(type: "int", nullable: false),
                    MobileWidth = table.Column<int>(type: "int", nullable: false),
                    MobileHeight = table.Column<int>(type: "int", nullable: false),
                    MaxImageSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    AllowedFormats = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerPlacementSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannerBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsInternalTarget = table.Column<bool>(type: "bit", nullable: false),
                    DesktopImageFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DesktopImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DesktopImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DesktopImageWidth = table.Column<int>(type: "int", nullable: false),
                    DesktopImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MobileImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MobileImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false),
                    PlacementSettingId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    SlotNumber = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdvertiserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    PaymentProofUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PaymentProofPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PaymentProofFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentRejectionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConfirmationAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RejectionReason = table.Column<int>(type: "int", nullable: true),
                    RejectionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerBookings_AspNetUsers_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BannerBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BannerBookings_BannerPlacementSettings_PlacementSettingId",
                        column: x => x.PlacementSettingId,
                        principalTable: "BannerPlacementSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BannerBookings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BannerBookings_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BannerBookings_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BannerPlacementSettings",
                columns: new[] { "Id", "AllowedFormats", "CreatedAt", "DesktopHeight", "DesktopWidth", "DisplayOrder", "IsActive", "Location", "MaxImageSizeBytes", "MaxSlots", "MobileHeight", "MobileWidth", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, ".jpg,.jpeg,.png,.webp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 533, 1600, 1, true, 1, 5242880L, 3, 720, 1080, 1500m, null },
                    { 2, ".jpg,.jpeg,.png,.webp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 533, 1600, 2, true, 2, 5242880L, 3, 720, 1080, 1000m, null },
                    { 3, ".jpg,.jpeg,.png,.webp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 533, 1600, 3, true, 3, 5242880L, 1, 720, 1080, 500m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_PaymentMethodId",
                table: "BannerBookings",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_PaymentStatus",
                table: "BannerBookings",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_PlacementSettingId",
                table: "BannerBookings",
                column: "PlacementSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_ReviewedBy",
                table: "BannerBookings",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_SlotWindow",
                table: "BannerBookings",
                columns: new[] { "Location", "SlotNumber", "Status", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_Status_StartDate_EndDate",
                table: "BannerBookings",
                columns: new[] { "Status", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_Status_SubmittedAt",
                table: "BannerBookings",
                columns: new[] { "Status", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_SubCategoryId",
                table: "BannerBookings",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_SubCategoryWindow",
                table: "BannerBookings",
                columns: new[] { "CategoryId", "SubCategoryId", "Status", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerBookings_UserId_SubmittedAt",
                table: "BannerBookings",
                columns: new[] { "UserId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerPlacementSettings_IsActive_DisplayOrder",
                table: "BannerPlacementSettings",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BannerPlacementSettings_Location",
                table: "BannerPlacementSettings",
                column: "Location",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannerBookings");

            migrationBuilder.DropTable(
                name: "BannerPlacementSettings");
        }
    }
}
