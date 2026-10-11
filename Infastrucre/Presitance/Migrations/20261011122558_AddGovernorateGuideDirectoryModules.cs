using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernorateGuideDirectoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommercialShops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: true),
                    OtherActivityType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_CommercialShops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommercialShops_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeneralServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    ServiceSubtype = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_GeneralServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralServices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResortsHotelsAndAmusement",
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
                    table.PrimaryKey("PK_ResortsHotelsAndAmusement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResortsHotelsAndAmusement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommercialShopImages",
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
                    table.PrimaryKey("PK_CommercialShopImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommercialShopImages_CommercialShops_ListingId",
                        column: x => x.ListingId,
                        principalTable: "CommercialShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralServiceImages",
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
                    table.PrimaryKey("PK_GeneralServiceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralServiceImages_GeneralServices_ListingId",
                        column: x => x.ListingId,
                        principalTable: "GeneralServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResortHotelAmusementImages",
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
                    table.PrimaryKey("PK_ResortHotelAmusementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResortHotelAmusementImages_ResortsHotelsAndAmusement_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ResortsHotelsAndAmusement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Admins can create sub-categories with Max(Id) + 1, so ids 66-68 are checked rather than blindly
            // inserted: a row that is already the expected sub-category is kept, an id held by anything else aborts.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 66 AND ([CategoryId] <> 2 OR [Name] <> N'Commercial Shops'))
BEGIN
    THROW 51000, 'Sub-category id 66 is already used by another sub-category; Commercial Shops was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 66)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (66, 2, NULL, 1, N'Commercial Shops', N'محلات تجارية');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 67 AND ([CategoryId] <> 2 OR [Name] <> N'General Services'))
BEGIN
    THROW 51000, 'Sub-category id 67 is already used by another sub-category; General Services was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 67)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (67, 2, NULL, 1, N'General Services', N'خدمات عامة');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 68 AND ([CategoryId] <> 2 OR [Name] <> N'Resorts, Hotels & Amusement'))
BEGIN
    THROW 51000, 'Sub-category id 68 is already used by another sub-category; Resorts, Hotels & Amusement was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 68)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (68, 2, NULL, 1, N'Resorts, Hotels & Amusement', N'قرى سياحية وفنادق وملاهي');
");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShopImages_ListingId_SortOrder",
                table: "CommercialShopImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_Advertisers",
                table: "CommercialShops",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_ExpireAt",
                table: "CommercialShops",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_Feed_MostViewed",
                table: "CommercialShops",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_IsDeleted",
                table: "CommercialShops",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_ModerationStatus_ActivityType_CreatedAt",
                table: "CommercialShops",
                columns: new[] { "ModerationStatus", "ActivityType", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_ModerationStatus_CreatedAt_Id",
                table: "CommercialShops",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialShops_UserId_CreatedAt",
                table: "CommercialShops",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServiceImages_ListingId_SortOrder",
                table: "GeneralServiceImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_Advertisers",
                table: "GeneralServices",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_ExpireAt",
                table: "GeneralServices",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_Feed_MostViewed",
                table: "GeneralServices",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_IsDeleted",
                table: "GeneralServices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_ModerationStatus_CreatedAt_Id",
                table: "GeneralServices",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_ModerationStatus_ServiceType_ServiceSubtype_CreatedAt",
                table: "GeneralServices",
                columns: new[] { "ModerationStatus", "ServiceType", "ServiceSubtype", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralServices_UserId_CreatedAt",
                table: "GeneralServices",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ResortHotelAmusementImages_ListingId_SortOrder",
                table: "ResortHotelAmusementImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_Advertisers",
                table: "ResortsHotelsAndAmusement",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_ExpireAt",
                table: "ResortsHotelsAndAmusement",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_Feed_MostViewed",
                table: "ResortsHotelsAndAmusement",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_IsDeleted",
                table: "ResortsHotelsAndAmusement",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_ModerationStatus_CreatedAt_Id",
                table: "ResortsHotelsAndAmusement",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ResortsHotelsAndAmusement_UserId_CreatedAt",
                table: "ResortsHotelsAndAmusement",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommercialShopImages");

            migrationBuilder.DropTable(
                name: "GeneralServiceImages");

            migrationBuilder.DropTable(
                name: "ResortHotelAmusementImages");

            migrationBuilder.DropTable(
                name: "CommercialShops");

            migrationBuilder.DropTable(
                name: "GeneralServices");

            migrationBuilder.DropTable(
                name: "ResortsHotelsAndAmusement");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 68);
        }
    }
}
