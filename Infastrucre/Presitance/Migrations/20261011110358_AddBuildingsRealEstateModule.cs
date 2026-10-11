using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingsRealEstateModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AdvertiserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    BuildingType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PricePerMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FurnishedStatus = table.Column<int>(type: "int", nullable: false),
                    FinishingType = table.Column<int>(type: "int", nullable: false),
                    PropertyAge = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    ViewType = table.Column<int>(type: "int", nullable: true),
                    GardenArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FloorsCount = table.Column<int>(type: "int", nullable: true),
                    BathroomsCount = table.Column<int>(type: "int", nullable: true),
                    ReceptionPieces = table.Column<int>(type: "int", nullable: true),
                    RoomsCount = table.Column<int>(type: "int", nullable: true),
                    HasSwimmingPool = table.Column<bool>(type: "bit", nullable: true),
                    ResidentialUnitsCount = table.Column<int>(type: "int", nullable: true),
                    HasIndependentEntrance = table.Column<bool>(type: "bit", nullable: true),
                    ApartmentsCount = table.Column<int>(type: "int", nullable: true),
                    ShopsCount = table.Column<int>(type: "int", nullable: true),
                    HasElevator = table.Column<bool>(type: "bit", nullable: true),
                    HasGarage = table.Column<bool>(type: "bit", nullable: true),
                    LegalStatus = table.Column<int>(type: "int", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseIssuer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReconciliationForm = table.Column<int>(type: "int", nullable: true),
                    OwnershipDocument = table.Column<int>(type: "int", nullable: false),
                    IsRegistered = table.Column<bool>(type: "bit", nullable: true),
                    HasViolations = table.Column<bool>(type: "bit", nullable: true),
                    ViolationDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Project = table.Column<int>(type: "int", nullable: true),
                    OtherProject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    District = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    DownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InstallmentProvider = table.Column<int>(type: "int", nullable: true),
                    HasMaintenanceDeposit = table.Column<bool>(type: "bit", nullable: true),
                    MaintenanceDepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyFees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentType = table.Column<int>(type: "int", nullable: true),
                    RentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentDownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumRentPeriod = table.Column<int>(type: "int", nullable: true),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerConditions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ExchangeWith = table.Column<int>(type: "int", nullable: true),
                    AcceptsDifferencePayment = table.Column<bool>(type: "bit", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BuildingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingFeatureSelections",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Feature = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingFeatureSelections", x => new { x.BuildingId, x.Feature });
                    table.ForeignKey(
                        name: "FK_BuildingFeatureSelections_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingImages_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingRentInclusionSelections",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Inclusion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingRentInclusionSelections", x => new { x.BuildingId, x.Inclusion });
                    table.ForeignKey(
                        name: "FK_BuildingRentInclusionSelections_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BuildingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "فيلا", "Villa" },
                    { 2, "بيت", "House" },
                    { 3, "عمارة", "Apartment building" }
                });

            // Admins can create sub-categories with Max(Id) + 1, so id 65 is checked rather than blindly
            // inserted: a row that is already Buildings is kept, an id held by anything else aborts.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 65 AND ([CategoryId] <> 11 OR [Name] <> N'Buildings'))
BEGIN
    THROW 51000, 'Sub-category id 65 is already used by another sub-category; Buildings was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 65)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (65, 11, NULL, 1, N'Buildings', N'مباني');
");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFeatureSelections_Feature",
                table: "BuildingFeatureSelections",
                column: "Feature");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingImages_BuildingId_SortOrder",
                table: "BuildingImages",
                columns: new[] { "BuildingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Advertisers",
                table: "Buildings",
                columns: new[] { "ModerationStatus", "AdvertiserName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Area",
                table: "Buildings",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingType",
                table: "Buildings",
                column: "BuildingType");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Center",
                table: "Buildings",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CreatedAt",
                table: "Buildings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ExpireAt",
                table: "Buildings",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Feed_MostViewed",
                table: "Buildings",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Feed_PriceAsc",
                table: "Buildings",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Feed_PriceDesc",
                table: "Buildings",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_IsPremium_IsFeatured",
                table: "Buildings",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ListingType",
                table: "Buildings",
                column: "ListingType");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Buildings",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Negotiable",
                table: "Buildings",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Price",
                table: "Buildings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Project",
                table: "Buildings",
                column: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_Title",
                table: "Buildings",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_UserId",
                table: "Buildings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ViewCount",
                table: "Buildings",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingTypes_Name",
                table: "BuildingTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingFeatureSelections");

            migrationBuilder.DropTable(
                name: "BuildingImages");

            migrationBuilder.DropTable(
                name: "BuildingRentInclusionSelections");

            migrationBuilder.DropTable(
                name: "BuildingTypes");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 65);
        }
    }
}
