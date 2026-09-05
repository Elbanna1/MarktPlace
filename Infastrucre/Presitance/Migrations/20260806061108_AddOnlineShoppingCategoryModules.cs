using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineShoppingCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccessoryType = table.Column<int>(type: "int", nullable: false),
                    OtherAccessoryType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccessoryCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cosmetics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    OtherSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SuitableFor = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ShippingAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosmetics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cosmetics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CosmeticSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmeticSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CosmeticSuitableFor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmeticSuitableFor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtains",
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
                    Size = table.Column<int>(type: "int", nullable: false),
                    OtherSize = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_FurnishingCurtains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FurnishingCurtains_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Furnitures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FurnitureType = table.Column<int>(type: "int", nullable: false),
                    OtherFurnitureType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CanBeDisassembled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Furnitures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Furnitures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FurnitureTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftToys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiftType = table.Column<int>(type: "int", nullable: false),
                    OtherGiftType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SuitableFor = table.Column<int>(type: "int", nullable: false),
                    GiftWrapping = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftToys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftToys_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GiftToySuitableFor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftToySuitableFor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftToyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftToyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchenColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchenColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchenMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchenMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    OtherSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeKitchens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchenSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchenSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomemadeFoodDeliveryAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomemadeFoodDeliveryAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomemadeFoods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    OtherSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PreparedOnDemand = table.Column<bool>(type: "bit", nullable: false),
                    MinimumOrderQuantity = table.Column<int>(type: "int", nullable: false),
                    PreparationTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WeightOrSize = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StorageMethod = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableOrderingHours = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomemadeFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomemadeFoods_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HomemadeFoodSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomemadeFoodSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronicCompatibilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronicCompatibilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronicConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronicConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    OtherSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompatibleWith = table.Column<int>(type: "int", nullable: false),
                    ProductCondition = table.Column<int>(type: "int", nullable: false),
                    Warranty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAvailable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingElectronics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronicSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronicSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronicWarranties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronicWarranties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryColorSelections",
                columns: table => new
                {
                    AccessoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryColorSelections", x => new { x.AccessoryId, x.Color });
                    table.ForeignKey(
                        name: "FK_AccessoryColorSelections_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessoryImages_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CosmeticImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CosmeticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmeticImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CosmeticImages_Cosmetics_CosmeticId",
                        column: x => x.CosmeticId,
                        principalTable: "Cosmetics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainColorSelections",
                columns: table => new
                {
                    FurnishingCurtainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainColorSelections", x => new { x.FurnishingCurtainId, x.Color });
                    table.ForeignKey(
                        name: "FK_FurnishingCurtainColorSelections_FurnishingCurtains_FurnishingCurtainId",
                        column: x => x.FurnishingCurtainId,
                        principalTable: "FurnishingCurtains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FurnishingCurtainImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FurnishingCurtainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnishingCurtainImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FurnishingCurtainImages_FurnishingCurtains_FurnishingCurtainId",
                        column: x => x.FurnishingCurtainId,
                        principalTable: "FurnishingCurtains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureColorSelections",
                columns: table => new
                {
                    FurnitureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureColorSelections", x => new { x.FurnitureId, x.Color });
                    table.ForeignKey(
                        name: "FK_FurnitureColorSelections_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FurnitureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FurnitureImages_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiftToyImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiftToyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftToyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftToyImages_GiftToys_GiftToyId",
                        column: x => x.GiftToyId,
                        principalTable: "GiftToys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchenColorSelections",
                columns: table => new
                {
                    HomeKitchenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchenColorSelections", x => new { x.HomeKitchenId, x.Color });
                    table.ForeignKey(
                        name: "FK_HomeKitchenColorSelections_HomeKitchens_HomeKitchenId",
                        column: x => x.HomeKitchenId,
                        principalTable: "HomeKitchens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeKitchenImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeKitchenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeKitchenImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeKitchenImages_HomeKitchens_HomeKitchenId",
                        column: x => x.HomeKitchenId,
                        principalTable: "HomeKitchens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomemadeFoodDeliveryAreaSelections",
                columns: table => new
                {
                    HomemadeFoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryArea = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomemadeFoodDeliveryAreaSelections", x => new { x.HomemadeFoodId, x.DeliveryArea });
                    table.ForeignKey(
                        name: "FK_HomemadeFoodDeliveryAreaSelections_HomemadeFoods_HomemadeFoodId",
                        column: x => x.HomemadeFoodId,
                        principalTable: "HomemadeFoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomemadeFoodImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomemadeFoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomemadeFoodImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomemadeFoodImages_HomemadeFoods_HomemadeFoodId",
                        column: x => x.HomemadeFoodId,
                        principalTable: "HomemadeFoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingElectronicImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoppingElectronicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingElectronicImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingElectronicImages_ShoppingElectronics_ShoppingElectronicId",
                        column: x => x.ShoppingElectronicId,
                        principalTable: "ShoppingElectronics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccessoryCategories",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "رجالي", "Men" },
                    { 2, "حريمي", "Women" },
                    { 3, "أطفال", "Kids" },
                    { 4, "للجميع", "Everyone" }
                });

            migrationBuilder.InsertData(
                table: "AccessoryColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أسود", "Black" },
                    { 2, "أبيض", "White" },
                    { 3, "فضي", "Silver" },
                    { 4, "ذهبي", "Gold" },
                    { 5, "بني", "Brown" },
                    { 6, "أحمر", "Red" },
                    { 7, "أزرق", "Blue" },
                    { 8, "وردي", "Pink" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "AccessoryMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "فضة", "Silver" },
                    { 2, "ستانلس", "Stainless steel" },
                    { 3, "جلد", "Leather" },
                    { 4, "قماش", "Fabric" },
                    { 5, "خرز", "Beads" },
                    { 6, "معدن", "Metal" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "AccessoryTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ساعات", "Watches" },
                    { 2, "نظارات", "Glasses" },
                    { 3, "محافظ", "Wallets" },
                    { 4, "أحزمة", "Belts" },
                    { 5, "حقائب", "Bags" },
                    { 6, "سلاسل", "Chains" },
                    { 7, "خواتم", "Rings" },
                    { 8, "أساور", "Bracelets" },
                    { 9, "أقراط", "Earrings" },
                    { 10, "إكسسوارات شعر", "Hair accessories" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NameAr" },
                values: new object[] { 9, "Online Shopping", "التسوق أونلاين" });

            migrationBuilder.InsertData(
                table: "CosmeticSections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مكياج", "Makeup" },
                    { 2, "عناية بالبشرة", "Skin care" },
                    { 3, "عناية بالشعر", "Hair care" },
                    { 4, "عطور", "Perfumes" },
                    { 5, "عدسات", "Contact lenses" },
                    { 6, "منتجات طبيعية", "Natural products" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "CosmeticSuitableFor",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نسائي", "Women" },
                    { 2, "رجالي", "Men" },
                    { 3, "أطفال", "Kids" },
                    { 4, "للجميع", "Everyone" }
                });

            migrationBuilder.InsertData(
                table: "FurnishingCurtainColors",
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
                table: "FurnishingCurtainMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "قطن", "Cotton" },
                    { 2, "كتان", "Linen" },
                    { 3, "حرير", "Silk" },
                    { 4, "صوف", "Wool" },
                    { 5, "بوليستر", "Polyester" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "FurnishingCurtainProductTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ستائر", "Curtains" },
                    { 2, "سجاد", "Carpet" },
                    { 3, "موكيت", "Moquette" },
                    { 4, "ملايات", "Bed sheets" },
                    { 5, "بطاطين", "Blankets" },
                    { 6, "لحاف", "Duvet" },
                    { 7, "مفارش", "Bed covers" },
                    { 8, "مخدات", "Pillows" },
                    { 9, "وسائد ديكور", "Decor cushions" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "FurnishingCurtainSizes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مفرد", "Single" },
                    { 2, "نصف مزدوج", "Half double" },
                    { 3, "مزدوج", "Double" },
                    { 4, "كينج", "King" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "FurnitureColors",
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
                table: "FurnitureConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "مستعمل بحالة ممتازة", "Used - excellent condition" },
                    { 3, "مستعمل بحالة جيدة", "Used - good condition" }
                });

            migrationBuilder.InsertData(
                table: "FurnitureMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "زان", "Beech" },
                    { 2, "موسكي", "Moski pine" },
                    { 3, "MDF", "MDF" },
                    { 4, "كونتر", "Plywood" },
                    { 5, "معدن", "Metal" },
                    { 6, "بلاستيك", "Plastic" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "FurnitureTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "غرفة نوم", "Bedroom set" },
                    { 2, "انتريه", "Entree" },
                    { 3, "ركنة", "Corner sofa" },
                    { 4, "صالون", "Salon" },
                    { 5, "غرفة سفرة", "Dining room" },
                    { 6, "كنبة", "Sofa" },
                    { 7, "كرسي", "Chair" },
                    { 8, "ترابيزة", "Table" },
                    { 9, "مكتب", "Desk" },
                    { 10, "مكتبة", "Bookcase" },
                    { 11, "دولاب", "Wardrobe" },
                    { 12, "كومود", "Commode" },
                    { 13, "تسريحة", "Dressing table" },
                    { 14, "وحدة تلفزيون", "TV unit" },
                    { 15, "بوفيه", "Buffet" },
                    { 16, "سرير", "Bed" },
                    { 17, "سرير أطفال", "Kids bed" },
                    { 18, "كرسي مكتب", "Office chair" },
                    { 19, "كرسي جيمنج", "Gaming chair" },
                    { 20, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "GiftToySuitableFor",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أطفال", "Kids" },
                    { 2, "بنات", "Girls" },
                    { 3, "شباب", "Young adults" },
                    { 4, "للجميع", "Everyone" }
                });

            migrationBuilder.InsertData(
                table: "GiftToyTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ألعاب أطفال", "Kids toys" },
                    { 2, "بوكس هدايا", "Gift boxes" },
                    { 3, "ورد", "Flowers" },
                    { 4, "شوكولاتة", "Chocolate" },
                    { 5, "مجات", "Mugs" },
                    { 6, "ميداليات", "Keychains" },
                    { 7, "سلاسل", "Chains" },
                    { 8, "ألعاب تعليمية", "Educational toys" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeKitchenColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "رمادي", "Gray" },
                    { 4, "بني", "Brown" },
                    { 5, "بيج", "Beige" },
                    { 6, "فضي", "Silver" },
                    { 7, "ذهبي", "Gold" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeKitchenMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بلاستيك", "Plastic" },
                    { 2, "خشب", "Wood" },
                    { 3, "زجاج", "Glass" },
                    { 4, "معدن", "Metal" },
                    { 5, "ستانلس", "Stainless steel" },
                    { 6, "سيراميك", "Ceramic" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomeKitchenSections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أدوات مطبخ", "Kitchen tools" },
                    { 2, "أواني", "Cookware" },
                    { 3, "أجهزة مطبخ صغيرة", "Small kitchen appliances" },
                    { 4, "ديكور", "Decor" },
                    { 5, "مفروشات", "Furnishings" },
                    { 6, "تخزين وتنظيم", "Storage & organization" },
                    { 7, "أدوات تنظيف", "Cleaning tools" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HomemadeFoodDeliveryAreas",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "الفيوم", "Fayoum" },
                    { 2, "سنورس", "Sinnuris" },
                    { 3, "طامية", "Tamiya" },
                    { 4, "إطسا", "Itsa" },
                    { 5, "أبشواي", "Ibshaway" },
                    { 6, "يوسف الصديق", "Youssef El Seddik" }
                });

            migrationBuilder.InsertData(
                table: "HomemadeFoodSections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "حلويات", "Sweets" },
                    { 2, "تورت", "Cakes" },
                    { 3, "مخبوزات", "Bakery" },
                    { 4, "أكلات شرقية", "Oriental dishes" },
                    { 5, "أكلات غربية", "Western dishes" },
                    { 6, "مشروبات", "Beverages" },
                    { 7, "سندوتشات", "Sandwiches" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingElectronicCompatibilities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "Android", "Android" },
                    { 2, "iPhone", "iPhone" },
                    { 3, "Windows", "Windows" },
                    { 4, "جميع الأجهزة", "All devices" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingElectronicConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "جديد بتغليفه", "New (sealed)" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingElectronicSections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "سماعات", "Headphones" },
                    { 2, "شواحن", "Chargers" },
                    { 3, "كابلات", "Cables" },
                    { 4, "باور بانك", "Power banks" },
                    { 5, "ساعات ذكية", "Smart watches" },
                    { 6, "جرابات", "Cases" },
                    { 7, "لوحات مفاتيح", "Keyboards" },
                    { 8, "ماوس", "Mice" },
                    { 9, "كاميرات مراقبة", "Security cameras" },
                    { 10, "إضاءة LED", "LED lighting" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingElectronicWarranties",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "متاح", "Available" },
                    { 2, "غير متاح", "Not available" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "NameAr" },
                values: new object[,]
                {
                    { 34, 9, "Accessories", "إكسسوارات" },
                    { 35, 9, "Cosmetics", "مستحضرات التجميل" },
                    { 36, 9, "Home & Kitchen", "المنزل والمطبخ" },
                    { 37, 9, "Electronics", "إلكترونيات" },
                    { 38, 9, "Gifts & Toys", "هدايا وألعاب" },
                    { 39, 9, "Homemade Food", "أكل منزلي" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_AccessoryType",
                table: "Accessories",
                column: "AccessoryType");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Category",
                table: "Accessories",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_CreatedAt",
                table: "Accessories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Material",
                table: "Accessories",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Price",
                table: "Accessories",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ShippingAvailable",
                table: "Accessories",
                column: "ShippingAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_StoreName",
                table: "Accessories",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_UserId",
                table: "Accessories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryCategories_Name",
                table: "AccessoryCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryColors_Name",
                table: "AccessoryColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryColorSelections_Color",
                table: "AccessoryColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryImages_AccessoryId",
                table: "AccessoryImages",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryMaterials_Name",
                table: "AccessoryMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryTypes_Name",
                table: "AccessoryTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticImages_CosmeticId",
                table: "CosmeticImages",
                column: "CosmeticId");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_Brand",
                table: "Cosmetics",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_CreatedAt",
                table: "Cosmetics",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_DiscountAvailable",
                table: "Cosmetics",
                column: "DiscountAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ExpirationDate",
                table: "Cosmetics",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_Price",
                table: "Cosmetics",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_Section",
                table: "Cosmetics",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ShippingAvailable",
                table: "Cosmetics",
                column: "ShippingAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_StoreName",
                table: "Cosmetics",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_SuitableFor",
                table: "Cosmetics",
                column: "SuitableFor");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_UserId",
                table: "Cosmetics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticSections_Name",
                table: "CosmeticSections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticSuitableFor_Name",
                table: "CosmeticSuitableFor",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainColors_Name",
                table: "FurnishingCurtainColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainColorSelections_Color",
                table: "FurnishingCurtainColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainImages_FurnishingCurtainId_SortOrder",
                table: "FurnishingCurtainImages",
                columns: new[] { "FurnishingCurtainId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainMaterials_Name",
                table: "FurnishingCurtainMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainProductTypes_Name",
                table: "FurnishingCurtainProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Center",
                table: "FurnishingCurtains",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_CreatedAt",
                table: "FurnishingCurtains",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_DeliveryAvailable",
                table: "FurnishingCurtains",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_IsPremium_IsFeatured",
                table: "FurnishingCurtains",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Material",
                table: "FurnishingCurtains",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Price",
                table: "FurnishingCurtains",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ProductName",
                table: "FurnishingCurtains",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ProductType",
                table: "FurnishingCurtains",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Size",
                table: "FurnishingCurtains",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_UserId",
                table: "FurnishingCurtains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ViewCount",
                table: "FurnishingCurtains",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtainSizes_Name",
                table: "FurnishingCurtainSizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureColors_Name",
                table: "FurnitureColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureColorSelections_Color",
                table: "FurnitureColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureConditions_Name",
                table: "FurnitureConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureImages_FurnitureId_SortOrder",
                table: "FurnitureImages",
                columns: new[] { "FurnitureId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureMaterials_Name",
                table: "FurnitureMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Center",
                table: "Furnitures",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Condition",
                table: "Furnitures",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_CreatedAt",
                table: "Furnitures",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_DeliveryAvailable",
                table: "Furnitures",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_FurnitureType",
                table: "Furnitures",
                column: "FurnitureType");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_IsPremium_IsFeatured",
                table: "Furnitures",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Material",
                table: "Furnitures",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Price",
                table: "Furnitures",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ProductName",
                table: "Furnitures",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_UserId",
                table: "Furnitures",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ViewCount",
                table: "Furnitures",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureTypes_Name",
                table: "FurnitureTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftToyImages_GiftToyId",
                table: "GiftToyImages",
                column: "GiftToyId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_CreatedAt",
                table: "GiftToys",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_DeliveryAvailable",
                table: "GiftToys",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_GiftType",
                table: "GiftToys",
                column: "GiftType");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_GiftWrapping",
                table: "GiftToys",
                column: "GiftWrapping");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_Price",
                table: "GiftToys",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_StoreName",
                table: "GiftToys",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_SuitableFor",
                table: "GiftToys",
                column: "SuitableFor");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_UserId",
                table: "GiftToys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToySuitableFor_Name",
                table: "GiftToySuitableFor",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftToyTypes_Name",
                table: "GiftToyTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchenColors_Name",
                table: "HomeKitchenColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchenColorSelections_Color",
                table: "HomeKitchenColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchenImages_HomeKitchenId",
                table: "HomeKitchenImages",
                column: "HomeKitchenId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchenMaterials_Name",
                table: "HomeKitchenMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_CreatedAt",
                table: "HomeKitchens",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_DeliveryAvailable",
                table: "HomeKitchens",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_Material",
                table: "HomeKitchens",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_Price",
                table: "HomeKitchens",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_Section",
                table: "HomeKitchens",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_StoreName",
                table: "HomeKitchens",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_UserId",
                table: "HomeKitchens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchenSections_Name",
                table: "HomeKitchenSections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoodDeliveryAreas_Name",
                table: "HomemadeFoodDeliveryAreas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoodDeliveryAreaSelections_DeliveryArea",
                table: "HomemadeFoodDeliveryAreaSelections",
                column: "DeliveryArea");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoodImages_HomemadeFoodId",
                table: "HomemadeFoodImages",
                column: "HomemadeFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_CreatedAt",
                table: "HomemadeFoods",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_DeliveryAvailable",
                table: "HomemadeFoods",
                column: "DeliveryAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_PreparedOnDemand",
                table: "HomemadeFoods",
                column: "PreparedOnDemand");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_Price",
                table: "HomemadeFoods",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ProjectName",
                table: "HomemadeFoods",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_Section",
                table: "HomemadeFoods",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_UserId",
                table: "HomemadeFoods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoodSections_Name",
                table: "HomemadeFoodSections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronicCompatibilities_Name",
                table: "ShoppingElectronicCompatibilities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronicConditions_Name",
                table: "ShoppingElectronicConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronicImages_ShoppingElectronicId",
                table: "ShoppingElectronicImages",
                column: "ShoppingElectronicId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Brand",
                table: "ShoppingElectronics",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_CompatibleWith",
                table: "ShoppingElectronics",
                column: "CompatibleWith");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_CreatedAt",
                table: "ShoppingElectronics",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Price",
                table: "ShoppingElectronics",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ProductCondition",
                table: "ShoppingElectronics",
                column: "ProductCondition");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Section",
                table: "ShoppingElectronics",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ShippingAvailable",
                table: "ShoppingElectronics",
                column: "ShippingAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_StoreName",
                table: "ShoppingElectronics",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_UserId",
                table: "ShoppingElectronics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Warranty",
                table: "ShoppingElectronics",
                column: "Warranty");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronicSections_Name",
                table: "ShoppingElectronicSections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronicWarranties_Name",
                table: "ShoppingElectronicWarranties",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessoryCategories");

            migrationBuilder.DropTable(
                name: "AccessoryColors");

            migrationBuilder.DropTable(
                name: "AccessoryColorSelections");

            migrationBuilder.DropTable(
                name: "AccessoryImages");

            migrationBuilder.DropTable(
                name: "AccessoryMaterials");

            migrationBuilder.DropTable(
                name: "AccessoryTypes");

            migrationBuilder.DropTable(
                name: "CosmeticImages");

            migrationBuilder.DropTable(
                name: "CosmeticSections");

            migrationBuilder.DropTable(
                name: "CosmeticSuitableFor");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainColors");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainColorSelections");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainImages");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainMaterials");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainProductTypes");

            migrationBuilder.DropTable(
                name: "FurnishingCurtainSizes");

            migrationBuilder.DropTable(
                name: "FurnitureColors");

            migrationBuilder.DropTable(
                name: "FurnitureColorSelections");

            migrationBuilder.DropTable(
                name: "FurnitureConditions");

            migrationBuilder.DropTable(
                name: "FurnitureImages");

            migrationBuilder.DropTable(
                name: "FurnitureMaterials");

            migrationBuilder.DropTable(
                name: "FurnitureTypes");

            migrationBuilder.DropTable(
                name: "GiftToyImages");

            migrationBuilder.DropTable(
                name: "GiftToySuitableFor");

            migrationBuilder.DropTable(
                name: "GiftToyTypes");

            migrationBuilder.DropTable(
                name: "HomeKitchenColors");

            migrationBuilder.DropTable(
                name: "HomeKitchenColorSelections");

            migrationBuilder.DropTable(
                name: "HomeKitchenImages");

            migrationBuilder.DropTable(
                name: "HomeKitchenMaterials");

            migrationBuilder.DropTable(
                name: "HomeKitchenSections");

            migrationBuilder.DropTable(
                name: "HomemadeFoodDeliveryAreas");

            migrationBuilder.DropTable(
                name: "HomemadeFoodDeliveryAreaSelections");

            migrationBuilder.DropTable(
                name: "HomemadeFoodImages");

            migrationBuilder.DropTable(
                name: "HomemadeFoodSections");

            migrationBuilder.DropTable(
                name: "ShoppingElectronicCompatibilities");

            migrationBuilder.DropTable(
                name: "ShoppingElectronicConditions");

            migrationBuilder.DropTable(
                name: "ShoppingElectronicImages");

            migrationBuilder.DropTable(
                name: "ShoppingElectronicSections");

            migrationBuilder.DropTable(
                name: "ShoppingElectronicWarranties");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "Cosmetics");

            migrationBuilder.DropTable(
                name: "FurnishingCurtains");

            migrationBuilder.DropTable(
                name: "Furnitures");

            migrationBuilder.DropTable(
                name: "GiftToys");

            migrationBuilder.DropTable(
                name: "HomeKitchens");

            migrationBuilder.DropTable(
                name: "HomemadeFoods");

            migrationBuilder.DropTable(
                name: "ShoppingElectronics");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
