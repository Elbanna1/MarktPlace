using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminDashboardV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "SubCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountNameSnapshot",
                table: "Payments",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsSnapshot",
                table: "Payments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentInfoSnapshot",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodNameSnapshot",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Governorates",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Governorates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Centers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Centers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountNameSnapshot",
                table: "BannerBookings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsSnapshot",
                table: "BannerBookings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentInfoSnapshot",
                table: "BannerBookings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodNameSnapshot",
                table: "BannerBookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusChangedBy",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusReason",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdFormFieldOverrides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCustom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LabelEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Section = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Placeholder = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    HelpText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Pattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatternMessage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MaxSelections = table.Column<int>(type: "int", nullable: true),
                    VisibleWhenField = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VisibleWhenValues = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RequiredWhenField = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequiredWhenValues = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdFormFieldOverrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdFormFieldOverrides_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdFormFieldOverrides_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HomeSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LinkText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    ItemCount = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeSections_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SiteNameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaviconUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FaviconPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TelegramUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TikTokUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivacyPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceMode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettings", x => x.Id);
                    table.CheckConstraint("CK_PlatformSettings_SingleRow", "[Id] = 1");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CenterId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdFormFieldOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldOverrideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LabelEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Group = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdFormFieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdFormFieldOptions_AdFormFieldOverrides_FieldOverrideId",
                        column: x => x.FieldOverrideId,
                        principalTable: "AdFormFieldOverrides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "HomeSections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "ImagePath", "ImageUrl", "IsVisible", "ItemCount", "Key", "LinkText", "LinkUrl", "SortOrder", "Subtitle", "Title", "TitleEn", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, "hero", null, null, 1, "سوق الفيوم — كل ما تحتاجه في مكان واحد", "شبيك لبيك", "Shobik Lobik", 1, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, "slider1", null, null, 2, null, "إعلانات مميزة", "Featured", 2, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, "category-strip", null, null, 3, null, "الأقسام", "Categories", 4, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, "slider2", null, null, 4, null, "إعلانات", "Advertisements", 3, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, "recently-viewed", null, null, 5, null, "شوهد مؤخرًا", "Recently Viewed", 5, null },
                    { 101, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-cars", null, null, 10, null, "أحدث السيارات", "Latest Cars", 6, null },
                    { 102, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-workshops", null, null, 15, null, "الورش والحرفيين", "Workshops & Craftsmen", 6, null },
                    { 103, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-lost-found", null, null, 20, null, "المفقودات", "Lost & Found", 6, null },
                    { 104, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-business", null, null, 14, null, "رجال أعمال", "Business", 6, null },
                    { 105, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-jobs", null, null, 12, null, "فرص العمل", "Jobs", 6, null },
                    { 106, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-animals", null, null, 13, null, "الحيوانات", "Animals", 6, null },
                    { 107, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-antiques", null, null, 19, null, "التحف والأنتيكات", "Antiques", 6, null },
                    { 108, 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-clothing", null, null, 16, null, "الملابس", "Clothing", 6, null },
                    { 109, 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-online-shopping", null, null, 17, null, "التسوق أونلاين", "Online Shopping", 6, null },
                    { 110, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-home-furnishing", null, null, 18, null, "افرش بيتك", "Home Furnishing", 6, null },
                    { 111, 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 8, "category-real-estate", null, null, 11, null, "أحدث العقارات", "Latest Real Estate", 6, null }
                });

            migrationBuilder.InsertData(
                table: "PlatformSettings",
                columns: new[] { "Id", "AboutUs", "Address", "Description", "Email", "FacebookUrl", "FaviconPath", "FaviconUrl", "InstagramUrl", "LinkedInUrl", "LogoPath", "LogoUrl", "MaintenanceMessage", "PhoneNumber", "PrivacyPolicy", "SiteName", "SiteNameEn", "TelegramUrl", "TermsAndConditions", "TikTokUrl", "TwitterUrl", "UpdatedAt", "UpdatedBy", "WhatsAppNumber", "YouTubeUrl" },
                values: new object[] { 1, null, null, "سوق الفيوم الإلكتروني", null, null, null, null, null, null, null, null, null, null, null, "شبيك لبيك", "Shobik Lobik", null, null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CenterId", "IsActive", "Name" },
                values: new object[] { 1, 7, true, "ابني بيتك" });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Icon", "IsActive" },
                values: new object[] { null, true });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_SortOrder_Id",
                table: "SubCategories",
                columns: new[] { "CategoryId", "SortOrder", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SortOrder_Id",
                table: "Categories",
                columns: new[] { "SortOrder", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Status",
                table: "AspNetUsers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AdFormFieldOptions_FieldOverrideId_Value",
                table: "AdFormFieldOptions",
                columns: new[] { "FieldOverrideId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdFormFieldOverrides_CategoryId_SubCategoryId_FieldName",
                table: "AdFormFieldOverrides",
                columns: new[] { "CategoryId", "SubCategoryId", "FieldName" },
                unique: true,
                filter: "[SubCategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdFormFieldOverrides_SubCategoryId",
                table: "AdFormFieldOverrides",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSections_CategoryId",
                table: "HomeSections",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSections_Key",
                table: "HomeSections",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeSections_SortOrder",
                table: "HomeSections",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CenterId_Name",
                table: "Projects",
                columns: new[] { "CenterId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdFormFieldOptions");

            migrationBuilder.DropTable(
                name: "HomeSections");

            migrationBuilder.DropTable(
                name: "PlatformSettings");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "AdFormFieldOverrides");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_SortOrder_Id",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SortOrder_Id",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "AccountNameSnapshot",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InstructionsSnapshot",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentInfoSnapshot",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodNameSnapshot",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Centers");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Centers");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AccountNameSnapshot",
                table: "BannerBookings");

            migrationBuilder.DropColumn(
                name: "InstructionsSnapshot",
                table: "BannerBookings");

            migrationBuilder.DropColumn(
                name: "PaymentInfoSnapshot",
                table: "BannerBookings");

            migrationBuilder.DropColumn(
                name: "PaymentMethodNameSnapshot",
                table: "BannerBookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusChangedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");
        }
    }
}
