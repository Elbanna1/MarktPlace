using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficesCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountantOffices",
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
                    table.PrimaryKey("PK_AccountantOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantOffices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
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
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EngineeringConsultancyOffices",
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
                    table.PrimaryKey("PK_EngineeringConsultancyOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngineeringConsultancyOffices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IndustrialConsultancyOffices",
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
                    table.PrimaryKey("PK_IndustrialConsultancyOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustrialConsultancyOffices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LawyerOffices",
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
                    table.PrimaryKey("PK_LawyerOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LawyerOffices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountantOfficeImages",
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
                    table.PrimaryKey("PK_AccountantOfficeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantOfficeImages_AccountantOffices_ListingId",
                        column: x => x.ListingId,
                        principalTable: "AccountantOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractorImages",
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
                    table.PrimaryKey("PK_ContractorImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractorImages_Contractors_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EngineeringConsultancyOfficeImages",
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
                    table.PrimaryKey("PK_EngineeringConsultancyOfficeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngineeringConsultancyOfficeImages_EngineeringConsultancyOffices_ListingId",
                        column: x => x.ListingId,
                        principalTable: "EngineeringConsultancyOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndustrialConsultancyOfficeImages",
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
                    table.PrimaryKey("PK_IndustrialConsultancyOfficeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustrialConsultancyOfficeImages_IndustrialConsultancyOffices_ListingId",
                        column: x => x.ListingId,
                        principalTable: "IndustrialConsultancyOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LawyerOfficeImages",
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
                    table.PrimaryKey("PK_LawyerOfficeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LawyerOfficeImages_LawyerOffices_ListingId",
                        column: x => x.ListingId,
                        principalTable: "LawyerOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Admins can create categories with Max(Id) + 1, so the seeded ids are checked rather than
            // blindly inserted: a row that is already Offices is kept, an id held by anything else aborts.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 14 AND [Name] <> N'Offices')
BEGIN
    THROW 51000, 'Category id 14 is already used by another category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 14)
    INSERT INTO [Categories] ([Id], [Icon], [IsActive], [Name], [NameAr]) VALUES (14, NULL, 1, N'Offices', N'مكاتب');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 59 AND ([CategoryId] <> 14 OR [Name] <> N'Lawyer Offices'))
BEGIN
    THROW 51000, 'Sub-category id 59 is already used by another sub-category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 59)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (59, 14, NULL, 1, N'Lawyer Offices', N'مكاتب محامين');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 60 AND ([CategoryId] <> 14 OR [Name] <> N'Engineering Consultancy Offices'))
BEGIN
    THROW 51000, 'Sub-category id 60 is already used by another sub-category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 60)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (60, 14, NULL, 1, N'Engineering Consultancy Offices', N'مكاتب استشارية هندسية');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 61 AND ([CategoryId] <> 14 OR [Name] <> N'Demolition & Construction Contractors'))
BEGIN
    THROW 51000, 'Sub-category id 61 is already used by another sub-category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 61)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (61, 14, NULL, 1, N'Demolition & Construction Contractors', N'مقاولين (هدم وبناء)');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 62 AND ([CategoryId] <> 14 OR [Name] <> N'Industrial Consultancy Offices'))
BEGIN
    THROW 51000, 'Sub-category id 62 is already used by another sub-category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 62)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (62, 14, NULL, 1, N'Industrial Consultancy Offices', N'مكاتب استشارية صناعية');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 63 AND ([CategoryId] <> 14 OR [Name] <> N'Certified Public Accountant Offices'))
BEGIN
    THROW 51000, 'Sub-category id 63 is already used by another sub-category; Offices was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 63)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (63, 14, NULL, 1, N'Certified Public Accountant Offices', N'مكاتب محاسبين قانونيين');
");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOfficeImages_ListingId_SortOrder",
                table: "AccountantOfficeImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_Advertisers",
                table: "AccountantOffices",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_ExpireAt",
                table: "AccountantOffices",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_Feed_MostViewed",
                table: "AccountantOffices",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_IsDeleted",
                table: "AccountantOffices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_ModerationStatus_CreatedAt_Id",
                table: "AccountantOffices",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountantOffices_UserId_CreatedAt",
                table: "AccountantOffices",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractorImages_ListingId_SortOrder",
                table: "ContractorImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_Advertisers",
                table: "Contractors",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_ExpireAt",
                table: "Contractors",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_Feed_MostViewed",
                table: "Contractors",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_IsDeleted",
                table: "Contractors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_ModerationStatus_CreatedAt_Id",
                table: "Contractors",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_UserId_CreatedAt",
                table: "Contractors",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOfficeImages_ListingId_SortOrder",
                table: "EngineeringConsultancyOfficeImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_Advertisers",
                table: "EngineeringConsultancyOffices",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_ExpireAt",
                table: "EngineeringConsultancyOffices",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_Feed_MostViewed",
                table: "EngineeringConsultancyOffices",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_IsDeleted",
                table: "EngineeringConsultancyOffices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_ModerationStatus_CreatedAt_Id",
                table: "EngineeringConsultancyOffices",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringConsultancyOffices_UserId_CreatedAt",
                table: "EngineeringConsultancyOffices",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOfficeImages_ListingId_SortOrder",
                table: "IndustrialConsultancyOfficeImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_Advertisers",
                table: "IndustrialConsultancyOffices",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_ExpireAt",
                table: "IndustrialConsultancyOffices",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_Feed_MostViewed",
                table: "IndustrialConsultancyOffices",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_IsDeleted",
                table: "IndustrialConsultancyOffices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_ModerationStatus_CreatedAt_Id",
                table: "IndustrialConsultancyOffices",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialConsultancyOffices_UserId_CreatedAt",
                table: "IndustrialConsultancyOffices",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOfficeImages_ListingId_SortOrder",
                table: "LawyerOfficeImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_Advertisers",
                table: "LawyerOffices",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_ExpireAt",
                table: "LawyerOffices",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_Feed_MostViewed",
                table: "LawyerOffices",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_IsDeleted",
                table: "LawyerOffices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_ModerationStatus_CreatedAt_Id",
                table: "LawyerOffices",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerOffices_UserId_CreatedAt",
                table: "LawyerOffices",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountantOfficeImages");

            migrationBuilder.DropTable(
                name: "ContractorImages");

            migrationBuilder.DropTable(
                name: "EngineeringConsultancyOfficeImages");

            migrationBuilder.DropTable(
                name: "IndustrialConsultancyOfficeImages");

            migrationBuilder.DropTable(
                name: "LawyerOfficeImages");

            migrationBuilder.DropTable(
                name: "AccountantOffices");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "EngineeringConsultancyOffices");

            migrationBuilder.DropTable(
                name: "IndustrialConsultancyOffices");

            migrationBuilder.DropTable(
                name: "LawyerOffices");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
