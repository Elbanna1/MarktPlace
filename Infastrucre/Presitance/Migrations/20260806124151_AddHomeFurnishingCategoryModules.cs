using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeFurnishingCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BathroomSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    OtherProductType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BathroomSupplies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BathroomSupplyColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplyColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BathroomSupplyMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplyMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BathroomSupplyProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplyProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceDeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceDeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeAppliances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    OtherDeviceType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<int>(type: "int", nullable: false),
                    OtherBrand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Warranty = table.Column<int>(type: "int", nullable: false),
                    WarrantyDuration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PowerRating = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeAppliances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeAppliances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceWarranties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceWarranties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenToolColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenToolColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenToolMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenToolMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenToolProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenToolProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitchenTools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    OtherProductType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenTools_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorLightTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorLightTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    OtherProductType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LightType = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LightingDecors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlantOrnamentProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantOrnamentProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantOrnaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    OtherProductType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SuitableFor = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantOrnaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantOrnaments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlantOrnamentSuitableFors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantOrnamentSuitableFors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BathroomSupplyColorSelections",
                columns: table => new
                {
                    BathroomSupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplyColorSelections", x => new { x.BathroomSupplyId, x.Color });
                    table.ForeignKey(
                        name: "FK_BathroomSupplyColorSelections_BathroomSupplies_BathroomSupplyId",
                        column: x => x.BathroomSupplyId,
                        principalTable: "BathroomSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BathroomSupplyImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BathroomSupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathroomSupplyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BathroomSupplyImages_BathroomSupplies_BathroomSupplyId",
                        column: x => x.BathroomSupplyId,
                        principalTable: "BathroomSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceColorSelections",
                columns: table => new
                {
                    HomeApplianceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceColorSelections", x => new { x.HomeApplianceId, x.Color });
                    table.ForeignKey(
                        name: "FK_HomeApplianceColorSelections_HomeAppliances_HomeApplianceId",
                        column: x => x.HomeApplianceId,
                        principalTable: "HomeAppliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeApplianceImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeApplianceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeApplianceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeApplianceImages_HomeAppliances_HomeApplianceId",
                        column: x => x.HomeApplianceId,
                        principalTable: "HomeAppliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenToolColorSelections",
                columns: table => new
                {
                    KitchenToolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenToolColorSelections", x => new { x.KitchenToolId, x.Color });
                    table.ForeignKey(
                        name: "FK_KitchenToolColorSelections_KitchenTools_KitchenToolId",
                        column: x => x.KitchenToolId,
                        principalTable: "KitchenTools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenToolImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KitchenToolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenToolImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitchenToolImages_KitchenTools_KitchenToolId",
                        column: x => x.KitchenToolId,
                        principalTable: "KitchenTools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorColorSelections",
                columns: table => new
                {
                    LightingDecorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorColorSelections", x => new { x.LightingDecorId, x.Color });
                    table.ForeignKey(
                        name: "FK_LightingDecorColorSelections_LightingDecors_LightingDecorId",
                        column: x => x.LightingDecorId,
                        principalTable: "LightingDecors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LightingDecorImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LightingDecorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingDecorImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LightingDecorImages_LightingDecors_LightingDecorId",
                        column: x => x.LightingDecorId,
                        principalTable: "LightingDecors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantOrnamentImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlantOrnamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantOrnamentImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantOrnamentImages_PlantOrnaments_PlantOrnamentId",
                        column: x => x.PlantOrnamentId,
                        principalTable: "PlantOrnaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BathroomSupplyColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "بني", "Brown" },
                    { 4, "بيج", "Beige" },
                    { 5, "رمادي", "Gray" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BathroomSupplyMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ستانلس", "Stainless steel" },
                    { 2, "بلاستيك", "Plastic" },
                    { 3, "زجاج", "Glass" },
                    { 4, "خشب", "Wood" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BathroomSupplyProductTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "وحدة حمام", "Bathroom unit" },
                    { 2, "مرآة", "Mirror" },
                    { 3, "خلاط", "Mixer tap" },
                    { 4, "دش", "Shower" },
                    { 5, "حامل مناشف", "Towel holder" },
                    { 6, "ستارة حمام", "Shower curtain" },
                    { 7, "رفوف", "Shelves" },
                    { 8, "سلة غسيل", "Laundry basket" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NameAr" },
                values: new object[] { 10, "Home Furnishing", "افرش بيتك" });

            migrationBuilder.InsertData(
                table: "HomeApplianceBrands",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "LG", "LG" },
                    { 2, "Samsung", "Samsung" },
                    { 3, "Toshiba", "Toshiba" },
                    { 4, "Sharp", "Sharp" },
                    { 5, "Fresh", "Fresh" },
                    { 6, "Unionaire", "Unionaire" },
                    { 7, "Zanussi", "Zanussi" },
                    { 8, "Bosch", "Bosch" },
                    { 9, "Philips", "Philips" },
                    { 10, "Moulinex", "Moulinex" },
                    { 11, "Tefal", "Tefal" },
                    { 12, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeApplianceColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "بني", "Brown" },
                    { 4, "بيج", "Beige" },
                    { 5, "رمادي", "Gray" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeApplianceConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "مستعمل بحالة ممتازة", "Used - excellent condition" },
                    { 3, "مستعمل بحالة جيدة", "Used - good condition" }
                });

            migrationBuilder.InsertData(
                table: "HomeApplianceDeviceTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ثلاجة", "Refrigerator" },
                    { 2, "ديب فريزر", "Deep freezer" },
                    { 3, "غسالة", "Washing machine" },
                    { 4, "غسالة أطباق", "Dishwasher" },
                    { 5, "بوتاجاز", "Cooker" },
                    { 6, "فرن كهربائي", "Electric oven" },
                    { 7, "ميكروويف", "Microwave" },
                    { 8, "شفاط", "Cooker hood" },
                    { 9, "مكنسة كهربائية", "Vacuum cleaner" },
                    { 10, "مروحة", "Fan" },
                    { 11, "تكييف", "Air conditioner" },
                    { 12, "سخان", "Water heater" },
                    { 13, "كاتيل", "Kettle" },
                    { 14, "خلاط", "Blender" },
                    { 15, "كبة", "Chopper" },
                    { 16, "عصارة", "Juicer" },
                    { 17, "محضر طعام", "Food processor" },
                    { 18, "ماكينة قهوة", "Coffee machine" },
                    { 19, "قلاية هوائية", "Air fryer" },
                    { 20, "مكواة", "Iron" },
                    { 21, "ماكينة خياطة", "Sewing machine" },
                    { 22, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeApplianceWarranties",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يوجد ضمان", "Available" },
                    { 2, "لا يوجد ضمان", "Not available" }
                });

            migrationBuilder.InsertData(
                table: "KitchenToolColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "بني", "Brown" },
                    { 4, "بيج", "Beige" },
                    { 5, "رمادي", "Gray" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "KitchenToolMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ستانلس", "Stainless steel" },
                    { 2, "جرانيت", "Granite" },
                    { 3, "تيفال", "Teflon" },
                    { 4, "ألومنيوم", "Aluminium" },
                    { 5, "زجاج", "Glass" },
                    { 6, "سيليكون", "Silicone" },
                    { 7, "بلاستيك", "Plastic" },
                    { 8, "خشب", "Wood" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "KitchenToolProductTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أطقم حلل", "Pot sets" },
                    { 2, "مقالي", "Pans" },
                    { 3, "صواني", "Trays" },
                    { 4, "أطباق", "Plates" },
                    { 5, "أكواب", "Cups" },
                    { 6, "ملاعق وشوك", "Cutlery" },
                    { 7, "سكاكين", "Knives" },
                    { 8, "برطمانات", "Jars" },
                    { 9, "أدوات تخزين", "Storage tools" },
                    { 10, "أدوات تقديم", "Serving tools" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LightingDecorColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "بني", "Brown" },
                    { 4, "بيج", "Beige" },
                    { 5, "رمادي", "Gray" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LightingDecorLightTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أصفر", "Yellow" },
                    { 3, "RGB", "RGB" }
                });

            migrationBuilder.InsertData(
                table: "LightingDecorMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "زجاج", "Glass" },
                    { 2, "كريستال", "Crystal" },
                    { 3, "معدن", "Metal" },
                    { 4, "خشب", "Wood" },
                    { 5, "بلاستيك", "Plastic" },
                    { 6, "سيراميك", "Ceramic" },
                    { 7, "قماش", "Fabric" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LightingDecorProductTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نجفة", "Chandelier" },
                    { 2, "أباجورة", "Table lamp" },
                    { 3, "سبوت", "Spotlight" },
                    { 4, "شريط LED", "LED strip" },
                    { 5, "ساعة حائط", "Wall clock" },
                    { 6, "مرآة", "Mirror" },
                    { 7, "لوحة", "Painting" },
                    { 8, "فازة", "Vase" },
                    { 9, "شمعدان", "Candlestick" },
                    { 10, "رفوف", "Shelves" },
                    { 11, "ديكور حائط", "Wall decor" },
                    { 12, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PlantOrnamentProductTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نبات طبيعي", "Natural plant" },
                    { 2, "نبات صناعي", "Artificial plant" },
                    { 3, "أصيص", "Pot" },
                    { 4, "زهور", "Flowers" },
                    { 5, "شجرة زينة", "Ornamental tree" },
                    { 6, "نافورة ديكور", "Decorative fountain" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PlantOrnamentSuitableFors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "داخلي", "Indoor" },
                    { 2, "خارجي", "Outdoor" },
                    { 3, "داخلي وخارجي", "Indoor & outdoor" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "NameAr" },
                values: new object[,]
                {
                    { 40, 10, "Furniture", "أثاث" },
                    { 41, 10, "Furnishings & Curtains", "مفروشات وستائر" },
                    { 42, 10, "Lighting & Decor", "إضاءة وديكور" },
                    { 43, 10, "Kitchen Tools", "أدوات المطبخ" },
                    { 44, 10, "Home Appliances", "أجهزة كهربائية منزلية" },
                    { 45, 10, "Bathroom Supplies", "مستلزمات الحمام" },
                    { 46, 10, "Plants & Ornaments", "نباتات وزينة" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Center",
                table: "BathroomSupplies",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_CreatedAt",
                table: "BathroomSupplies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_IsPremium_IsFeatured",
                table: "BathroomSupplies",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Material",
                table: "BathroomSupplies",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Price",
                table: "BathroomSupplies",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ProductName",
                table: "BathroomSupplies",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ProductType",
                table: "BathroomSupplies",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_UserId",
                table: "BathroomSupplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ViewCount",
                table: "BathroomSupplies",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplyColors_Name",
                table: "BathroomSupplyColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplyColorSelections_Color",
                table: "BathroomSupplyColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplyImages_BathroomSupplyId_SortOrder",
                table: "BathroomSupplyImages",
                columns: new[] { "BathroomSupplyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplyMaterials_Name",
                table: "BathroomSupplyMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplyProductTypes_Name",
                table: "BathroomSupplyProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceBrands_Name",
                table: "HomeApplianceBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceColors_Name",
                table: "HomeApplianceColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceColorSelections_Color",
                table: "HomeApplianceColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceConditions_Name",
                table: "HomeApplianceConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceDeviceTypes_Name",
                table: "HomeApplianceDeviceTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceImages_HomeApplianceId_SortOrder",
                table: "HomeApplianceImages",
                columns: new[] { "HomeApplianceId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Brand",
                table: "HomeAppliances",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Center",
                table: "HomeAppliances",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Condition",
                table: "HomeAppliances",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_CreatedAt",
                table: "HomeAppliances",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_DeliveryAvailable",
                table: "HomeAppliances",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_DeviceType",
                table: "HomeAppliances",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_IsPremium_IsFeatured",
                table: "HomeAppliances",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Price",
                table: "HomeAppliances",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ProductName",
                table: "HomeAppliances",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_UserId",
                table: "HomeAppliances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ViewCount",
                table: "HomeAppliances",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Warranty",
                table: "HomeAppliances",
                column: "Warranty");

            migrationBuilder.CreateIndex(
                name: "IX_HomeApplianceWarranties_Name",
                table: "HomeApplianceWarranties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KitchenToolColors_Name",
                table: "KitchenToolColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KitchenToolColorSelections_Color",
                table: "KitchenToolColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenToolImages_KitchenToolId_SortOrder",
                table: "KitchenToolImages",
                columns: new[] { "KitchenToolId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenToolMaterials_Name",
                table: "KitchenToolMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KitchenToolProductTypes_Name",
                table: "KitchenToolProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Center",
                table: "KitchenTools",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_CreatedAt",
                table: "KitchenTools",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_IsPremium_IsFeatured",
                table: "KitchenTools",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Material",
                table: "KitchenTools",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Price",
                table: "KitchenTools",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ProductName",
                table: "KitchenTools",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ProductType",
                table: "KitchenTools",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_UserId",
                table: "KitchenTools",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ViewCount",
                table: "KitchenTools",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorColors_Name",
                table: "LightingDecorColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorColorSelections_Color",
                table: "LightingDecorColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorImages_LightingDecorId_SortOrder",
                table: "LightingDecorImages",
                columns: new[] { "LightingDecorId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorLightTypes_Name",
                table: "LightingDecorLightTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorMaterials_Name",
                table: "LightingDecorMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecorProductTypes_Name",
                table: "LightingDecorProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Center",
                table: "LightingDecors",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_CreatedAt",
                table: "LightingDecors",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_IsPremium_IsFeatured",
                table: "LightingDecors",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_LightType",
                table: "LightingDecors",
                column: "LightType");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Material",
                table: "LightingDecors",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Price",
                table: "LightingDecors",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ProductName",
                table: "LightingDecors",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ProductType",
                table: "LightingDecors",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_UserId",
                table: "LightingDecors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ViewCount",
                table: "LightingDecors",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnamentImages_PlantOrnamentId_SortOrder",
                table: "PlantOrnamentImages",
                columns: new[] { "PlantOrnamentId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnamentProductTypes_Name",
                table: "PlantOrnamentProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_Center",
                table: "PlantOrnaments",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_CreatedAt",
                table: "PlantOrnaments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_IsPremium_IsFeatured",
                table: "PlantOrnaments",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_Price",
                table: "PlantOrnaments",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ProductName",
                table: "PlantOrnaments",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ProductType",
                table: "PlantOrnaments",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_SuitableFor",
                table: "PlantOrnaments",
                column: "SuitableFor");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_UserId",
                table: "PlantOrnaments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ViewCount",
                table: "PlantOrnaments",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnamentSuitableFors_Name",
                table: "PlantOrnamentSuitableFors",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BathroomSupplyColors");

            migrationBuilder.DropTable(
                name: "BathroomSupplyColorSelections");

            migrationBuilder.DropTable(
                name: "BathroomSupplyImages");

            migrationBuilder.DropTable(
                name: "BathroomSupplyMaterials");

            migrationBuilder.DropTable(
                name: "BathroomSupplyProductTypes");

            migrationBuilder.DropTable(
                name: "HomeApplianceBrands");

            migrationBuilder.DropTable(
                name: "HomeApplianceColors");

            migrationBuilder.DropTable(
                name: "HomeApplianceColorSelections");

            migrationBuilder.DropTable(
                name: "HomeApplianceConditions");

            migrationBuilder.DropTable(
                name: "HomeApplianceDeviceTypes");

            migrationBuilder.DropTable(
                name: "HomeApplianceImages");

            migrationBuilder.DropTable(
                name: "HomeApplianceWarranties");

            migrationBuilder.DropTable(
                name: "KitchenToolColors");

            migrationBuilder.DropTable(
                name: "KitchenToolColorSelections");

            migrationBuilder.DropTable(
                name: "KitchenToolImages");

            migrationBuilder.DropTable(
                name: "KitchenToolMaterials");

            migrationBuilder.DropTable(
                name: "KitchenToolProductTypes");

            migrationBuilder.DropTable(
                name: "LightingDecorColors");

            migrationBuilder.DropTable(
                name: "LightingDecorColorSelections");

            migrationBuilder.DropTable(
                name: "LightingDecorImages");

            migrationBuilder.DropTable(
                name: "LightingDecorLightTypes");

            migrationBuilder.DropTable(
                name: "LightingDecorMaterials");

            migrationBuilder.DropTable(
                name: "LightingDecorProductTypes");

            migrationBuilder.DropTable(
                name: "PlantOrnamentImages");

            migrationBuilder.DropTable(
                name: "PlantOrnamentProductTypes");

            migrationBuilder.DropTable(
                name: "PlantOrnamentSuitableFors");

            migrationBuilder.DropTable(
                name: "BathroomSupplies");

            migrationBuilder.DropTable(
                name: "HomeAppliances");

            migrationBuilder.DropTable(
                name: "KitchenTools");

            migrationBuilder.DropTable(
                name: "LightingDecors");

            migrationBuilder.DropTable(
                name: "PlantOrnaments");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
