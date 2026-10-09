using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddSehhatyCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DentalLaboratories",
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
                    table.PrimaryKey("PK_DentalLaboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DentalLaboratories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<int>(type: "int", nullable: false),
                    OtherSpecialty = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
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
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hospitals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalCenters",
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
                    table.PrimaryKey("PK_MedicalCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalCenters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalLaboratories",
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
                    table.PrimaryKey("PK_MedicalLaboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalLaboratories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RadiologyLaboratories",
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
                    table.PrimaryKey("PK_RadiologyLaboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadiologyLaboratories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DentalLaboratoryImages",
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
                    table.PrimaryKey("PK_DentalLaboratoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DentalLaboratoryImages_DentalLaboratories_ListingId",
                        column: x => x.ListingId,
                        principalTable: "DentalLaboratories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorImages",
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
                    table.PrimaryKey("PK_DoctorImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorImages_Doctors_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HospitalImages",
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
                    table.PrimaryKey("PK_HospitalImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalImages_Hospitals_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalCenterImages",
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
                    table.PrimaryKey("PK_MedicalCenterImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalCenterImages_MedicalCenters_ListingId",
                        column: x => x.ListingId,
                        principalTable: "MedicalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalLaboratoryImages",
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
                    table.PrimaryKey("PK_MedicalLaboratoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalLaboratoryImages_MedicalLaboratories_ListingId",
                        column: x => x.ListingId,
                        principalTable: "MedicalLaboratories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyLaboratoryImages",
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
                    table.PrimaryKey("PK_RadiologyLaboratoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadiologyLaboratoryImages_RadiologyLaboratories_ListingId",
                        column: x => x.ListingId,
                        principalTable: "RadiologyLaboratories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Admins can create categories with Max(Id) + 1, so the seeded ids are checked rather than
            // blindly inserted: a row that is already Sehhaty is kept, an id held by anything else aborts.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 13 AND [Name] <> N'Sehhaty')
BEGIN
    THROW 51000, 'Category id 13 is already used by another category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Id] = 13)
    INSERT INTO [Categories] ([Id], [Icon], [IsActive], [Name], [NameAr]) VALUES (13, NULL, 1, N'Sehhaty', N'صحتي');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 53 AND ([CategoryId] <> 13 OR [Name] <> N'Doctors'))
BEGIN
    THROW 51000, 'Sub-category id 53 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 53)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (53, 13, NULL, 1, N'Doctors', N'الأطباء');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 54 AND ([CategoryId] <> 13 OR [Name] <> N'Medical Laboratories'))
BEGIN
    THROW 51000, 'Sub-category id 54 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 54)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (54, 13, NULL, 1, N'Medical Laboratories', N'معامل التحاليل الطبية');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 55 AND ([CategoryId] <> 13 OR [Name] <> N'Dental Laboratories'))
BEGIN
    THROW 51000, 'Sub-category id 55 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 55)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (55, 13, NULL, 1, N'Dental Laboratories', N'معامل الأسنان');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 56 AND ([CategoryId] <> 13 OR [Name] <> N'Radiology Laboratories'))
BEGIN
    THROW 51000, 'Sub-category id 56 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 56)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (56, 13, NULL, 1, N'Radiology Laboratories', N'معامل الأشعة');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 57 AND ([CategoryId] <> 13 OR [Name] <> N'Medical Centers'))
BEGIN
    THROW 51000, 'Sub-category id 57 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 57)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (57, 13, NULL, 1, N'Medical Centers', N'مراكز طبية');
IF EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 58 AND ([CategoryId] <> 13 OR [Name] <> N'Hospitals'))
BEGIN
    THROW 51000, 'Sub-category id 58 is already used by another sub-category; Sehhaty was not seeded.', 1;
END;
IF NOT EXISTS (SELECT 1 FROM [SubCategories] WHERE [Id] = 58)
    INSERT INTO [SubCategories] ([Id], [CategoryId], [Icon], [IsActive], [Name], [NameAr]) VALUES (58, 13, NULL, 1, N'Hospitals', N'مستشفيات');
");

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_Advertisers",
                table: "DentalLaboratories",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_ExpireAt",
                table: "DentalLaboratories",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_Feed_MostViewed",
                table: "DentalLaboratories",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_IsDeleted",
                table: "DentalLaboratories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_ModerationStatus_CreatedAt_Id",
                table: "DentalLaboratories",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratories_UserId_CreatedAt",
                table: "DentalLaboratories",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DentalLaboratoryImages_ListingId_SortOrder",
                table: "DentalLaboratoryImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorImages_ListingId_SortOrder",
                table: "DoctorImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Advertisers",
                table: "Doctors",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ExpireAt",
                table: "Doctors",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Feed_MostViewed",
                table: "Doctors",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_IsDeleted",
                table: "Doctors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ModerationStatus_CreatedAt_Id",
                table: "Doctors",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ModerationStatus_Specialty_CreatedAt",
                table: "Doctors",
                columns: new[] { "ModerationStatus", "Specialty", "CreatedAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId_CreatedAt",
                table: "Doctors",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HospitalImages_ListingId_SortOrder",
                table: "HospitalImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Advertisers",
                table: "Hospitals",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ExpireAt",
                table: "Hospitals",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Feed_MostViewed",
                table: "Hospitals",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_IsDeleted",
                table: "Hospitals",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ModerationStatus_CreatedAt_Id",
                table: "Hospitals",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_UserId_CreatedAt",
                table: "Hospitals",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenterImages_ListingId_SortOrder",
                table: "MedicalCenterImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_Advertisers",
                table: "MedicalCenters",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_ExpireAt",
                table: "MedicalCenters",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_Feed_MostViewed",
                table: "MedicalCenters",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_IsDeleted",
                table: "MedicalCenters",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_ModerationStatus_CreatedAt_Id",
                table: "MedicalCenters",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCenters_UserId_CreatedAt",
                table: "MedicalCenters",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_Advertisers",
                table: "MedicalLaboratories",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_ExpireAt",
                table: "MedicalLaboratories",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_Feed_MostViewed",
                table: "MedicalLaboratories",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_IsDeleted",
                table: "MedicalLaboratories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_ModerationStatus_CreatedAt_Id",
                table: "MedicalLaboratories",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratories_UserId_CreatedAt",
                table: "MedicalLaboratories",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLaboratoryImages_ListingId_SortOrder",
                table: "MedicalLaboratoryImages",
                columns: new[] { "ListingId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_Advertisers",
                table: "RadiologyLaboratories",
                columns: new[] { "ModerationStatus", "Name" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_ExpireAt",
                table: "RadiologyLaboratories",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_Feed_MostViewed",
                table: "RadiologyLaboratories",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_IsDeleted",
                table: "RadiologyLaboratories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_ModerationStatus_CreatedAt_Id",
                table: "RadiologyLaboratories",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratories_UserId_CreatedAt",
                table: "RadiologyLaboratories",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyLaboratoryImages_ListingId_SortOrder",
                table: "RadiologyLaboratoryImages",
                columns: new[] { "ListingId", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalLaboratoryImages");

            migrationBuilder.DropTable(
                name: "DoctorImages");

            migrationBuilder.DropTable(
                name: "HospitalImages");

            migrationBuilder.DropTable(
                name: "MedicalCenterImages");

            migrationBuilder.DropTable(
                name: "MedicalLaboratoryImages");

            migrationBuilder.DropTable(
                name: "RadiologyLaboratoryImages");

            migrationBuilder.DropTable(
                name: "DentalLaboratories");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "MedicalCenters");

            migrationBuilder.DropTable(
                name: "MedicalLaboratories");

            migrationBuilder.DropTable(
                name: "RadiologyLaboratories");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
