using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddClothingCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntiqueConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AntiqueMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AntiqueOriginalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueOriginalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Antiques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AntiqueName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AntiqueType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ManufactureYear = table.Column<int>(type: "int", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    WorkingStatus = table.Column<int>(type: "int", nullable: false),
                    Originality = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Antiques_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AntiqueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AntiqueWorkingStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueWorkingStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinStampConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStampConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinStampItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStampItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinStampMetals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStampMetals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinStamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueYear = table.Column<int>(type: "int", nullable: true),
                    Denomination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Metal = table.Column<int>(type: "int", nullable: false),
                    OtherMetal = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    IsOriginal = table.Column<bool>(type: "bit", nullable: false),
                    IsRare = table.Column<bool>(type: "bit", nullable: false),
                    HasCertificate = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinStamps_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueOriginalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueOriginalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    OtherItemType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Originality = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecorAntiques_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HandmadeColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandmadeColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Handmades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HandmadeType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsFullyHandmade = table.Column<bool>(type: "bit", nullable: false),
                    CustomOrder = table.Column<bool>(type: "bit", nullable: false),
                    ProductionTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handmades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Handmades_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HandmadeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandmadeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SellingMethod = table.Column<int>(type: "int", nullable: false),
                    ClothingType = table.Column<int>(type: "int", nullable: false),
                    OtherClothingType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<int>(type: "int", nullable: false),
                    OtherBrand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_KidsClothings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidsClothings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingSellingMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingSellingMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SellingMethod = table.Column<int>(type: "int", nullable: false),
                    ClothingType = table.Column<int>(type: "int", nullable: false),
                    OtherClothingType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<int>(type: "int", nullable: false),
                    OtherBrand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_MenClothings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenClothings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenClothingSellingMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingSellingMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaintingMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintingMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaintingOriginalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintingOriginalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paintings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaintingName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PaintingType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ArtistName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ExecutionYear = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Material = table.Column<int>(type: "int", nullable: false),
                    OtherMaterial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Framed = table.Column<bool>(type: "bit", nullable: false),
                    Originality = table.Column<int>(type: "int", nullable: false),
                    SignedByArtist = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paintings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paintings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaintingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SellingMethod = table.Column<int>(type: "int", nullable: false),
                    ClothingType = table.Column<int>(type: "int", nullable: false),
                    OtherClothingType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Brand = table.Column<int>(type: "int", nullable: false),
                    OtherBrand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OtherColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_WomenClothings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WomenClothings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingSellingMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingSellingMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AntiqueImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AntiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiqueImages_Antiques_AntiqueId",
                        column: x => x.AntiqueId,
                        principalTable: "Antiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiqueVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AntiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiqueVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiqueVideos_Antiques_AntiqueId",
                        column: x => x.AntiqueId,
                        principalTable: "Antiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoinStampImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoinStampId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStampImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinStampImages_CoinStamps_CoinStampId",
                        column: x => x.CoinStampId,
                        principalTable: "CoinStamps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoinStampVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoinStampId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinStampVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinStampVideos_CoinStamps_CoinStampId",
                        column: x => x.CoinStampId,
                        principalTable: "CoinStamps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DecorAntiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecorAntiqueImages_DecorAntiques_DecorAntiqueId",
                        column: x => x.DecorAntiqueId,
                        principalTable: "DecorAntiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecorAntiqueVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DecorAntiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecorAntiqueVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecorAntiqueVideos_DecorAntiques_DecorAntiqueId",
                        column: x => x.DecorAntiqueId,
                        principalTable: "DecorAntiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandmadeColorSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandmadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandmadeColorSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandmadeColorSelections_Handmades_HandmadeId",
                        column: x => x.HandmadeId,
                        principalTable: "Handmades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandmadeImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandmadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandmadeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandmadeImages_Handmades_HandmadeId",
                        column: x => x.HandmadeId,
                        principalTable: "Handmades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandmadeVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandmadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandmadeVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandmadeVideos_Handmades_HandmadeId",
                        column: x => x.HandmadeId,
                        principalTable: "Handmades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingColorSelections",
                columns: table => new
                {
                    KidsClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingColorSelections", x => new { x.KidsClothingId, x.Color });
                    table.ForeignKey(
                        name: "FK_KidsClothingColorSelections_KidsClothings_KidsClothingId",
                        column: x => x.KidsClothingId,
                        principalTable: "KidsClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KidsClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidsClothingImages_KidsClothings_KidsClothingId",
                        column: x => x.KidsClothingId,
                        principalTable: "KidsClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidsClothingSizeSelections",
                columns: table => new
                {
                    KidsClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsClothingSizeSelections", x => new { x.KidsClothingId, x.Size });
                    table.ForeignKey(
                        name: "FK_KidsClothingSizeSelections_KidsClothings_KidsClothingId",
                        column: x => x.KidsClothingId,
                        principalTable: "KidsClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingColorSelections",
                columns: table => new
                {
                    MenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingColorSelections", x => new { x.MenClothingId, x.Color });
                    table.ForeignKey(
                        name: "FK_MenClothingColorSelections_MenClothings_MenClothingId",
                        column: x => x.MenClothingId,
                        principalTable: "MenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenClothingImages_MenClothings_MenClothingId",
                        column: x => x.MenClothingId,
                        principalTable: "MenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenClothingSizeSelections",
                columns: table => new
                {
                    MenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenClothingSizeSelections", x => new { x.MenClothingId, x.Size });
                    table.ForeignKey(
                        name: "FK_MenClothingSizeSelections_MenClothings_MenClothingId",
                        column: x => x.MenClothingId,
                        principalTable: "MenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintingImages_Paintings_PaintingId",
                        column: x => x.PaintingId,
                        principalTable: "Paintings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintingVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintingVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaintingVideos_Paintings_PaintingId",
                        column: x => x.PaintingId,
                        principalTable: "Paintings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingColorSelections",
                columns: table => new
                {
                    WomenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingColorSelections", x => new { x.WomenClothingId, x.Color });
                    table.ForeignKey(
                        name: "FK_WomenClothingColorSelections_WomenClothings_WomenClothingId",
                        column: x => x.WomenClothingId,
                        principalTable: "WomenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WomenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WomenClothingImages_WomenClothings_WomenClothingId",
                        column: x => x.WomenClothingId,
                        principalTable: "WomenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WomenClothingSizeSelections",
                columns: table => new
                {
                    WomenClothingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WomenClothingSizeSelections", x => new { x.WomenClothingId, x.Size });
                    table.ForeignKey(
                        name: "FK_WomenClothingSizeSelections_WomenClothings_WomenClothingId",
                        column: x => x.WomenClothingId,
                        principalTable: "WomenClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AntiqueConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة جدًا", "Very good" },
                    { 3, "جيدة", "Good" },
                    { 4, "تحتاج صيانة", "Needs maintenance" }
                });

            migrationBuilder.InsertData(
                table: "AntiqueMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "خشب", "Wood" },
                    { 2, "نحاس", "Copper" },
                    { 3, "حديد", "Iron" },
                    { 4, "زجاج", "Glass" },
                    { 5, "برونز", "Bronze" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "AntiqueOriginalities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أصلي", "Original" },
                    { 2, "نسخة", "Replica" },
                    { 3, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "AntiqueTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ساعة", "Clock" },
                    { 2, "راديو", "Radio" },
                    { 3, "تليفون", "Telephone" },
                    { 4, "كاميرا", "Camera" },
                    { 5, "ماكينة خياطة", "Sewing machine" },
                    { 6, "آلة كاتبة", "Typewriter" },
                    { 7, "جرامافون", "Gramophone" },
                    { 8, "أثاث قديم", "Old furniture" },
                    { 9, "مصباح", "Lamp" },
                    { 10, "سيارة قديمة", "Classic car" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "AntiqueWorkingStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يعمل", "Working" },
                    { 2, "لا يعمل", "Not working" },
                    { 3, "يعمل جزئيًا", "Partially working" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NameAr" },
                values: new object[,]
                {
                    { 7, "Antiques", "التحف والأنتيكات" },
                    { 8, "Clothing", "الملابس" }
                });

            migrationBuilder.InsertData(
                table: "CoinStampConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "UNC", "UNC" },
                    { 2, "ممتازة", "Excellent" },
                    { 3, "جيدة جدًا", "Very good" },
                    { 4, "جيدة", "Good" }
                });

            migrationBuilder.InsertData(
                table: "CoinStampItemTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عملة معدنية", "Coin" },
                    { 2, "عملة ورقية", "Banknote" },
                    { 3, "طابع بريد", "Postage stamp" },
                    { 4, "مجموعة عملات", "Coin collection" },
                    { 5, "مجموعة طوابع", "Stamp collection" },
                    { 6, "ميدالية تذكارية", "Commemorative medal" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "CoinStampMetals",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذهب", "Gold" },
                    { 2, "فضة", "Silver" },
                    { 3, "نحاس", "Copper" },
                    { 4, "نيكل", "Nickel" },
                    { 5, "برونز", "Bronze" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "DecorAntiqueConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديدة", "New" },
                    { 2, "ممتازة", "Excellent" },
                    { 3, "جيدة جدًا", "Very good" },
                    { 4, "جيدة", "Good" },
                    { 5, "تحتاج ترميم", "Needs restoration" }
                });

            migrationBuilder.InsertData(
                table: "DecorAntiqueItemTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "فازة", "Vase" },
                    { 2, "تمثال", "Statue" },
                    { 3, "شمعدان", "Candlestick" },
                    { 4, "مرآة", "Mirror" },
                    { 5, "ساعة", "Clock" },
                    { 6, "نجفة", "Chandelier" },
                    { 7, "صندوق خشبي", "Wooden box" },
                    { 8, "ديكور جداري", "Wall decor" },
                    { 9, "مزهرية", "Flower pot" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "DecorAntiqueMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "خشب", "Wood" },
                    { 2, "نحاس", "Copper" },
                    { 3, "برونز", "Bronze" },
                    { 4, "زجاج", "Glass" },
                    { 5, "كريستال", "Crystal" },
                    { 6, "رخام", "Marble" },
                    { 7, "سيراميك", "Ceramic" },
                    { 8, "خزف", "Porcelain" },
                    { 9, "حجر", "Stone" },
                    { 10, "راتنج", "Resin" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "DecorAntiqueOriginalities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أصلية", "Original" },
                    { 2, "نسخة", "Replica" }
                });

            migrationBuilder.InsertData(
                table: "HandmadeColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أبيض", "White" },
                    { 2, "أسود", "Black" },
                    { 3, "أحمر", "Red" },
                    { 4, "أزرق", "Blue" },
                    { 5, "أخضر", "Green" },
                    { 6, "بني", "Brown" },
                    { 7, "بيج", "Beige" },
                    { 8, "ذهبي", "Gold" },
                    { 9, "فضي", "Silver" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HandmadeTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كروشيه", "Crochet" },
                    { 2, "مكرمية", "Macrame" },
                    { 3, "ريزن", "Resin" },
                    { 4, "شموع", "Candles" },
                    { 5, "صابون طبيعي", "Natural soap" },
                    { 6, "تطريز", "Embroidery" },
                    { 7, "فخار", "Pottery" },
                    { 8, "منتجات خشبية", "Wooden products" },
                    { 9, "منتجات جلدية", "Leather products" },
                    { 10, "إكسسوارات", "Accessories" },
                    { 11, "ديكور منزلي", "Home decor" },
                    { 12, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingBrands",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مستورد", "Imported" },
                    { 2, "محلي", "Local" },
                    { 3, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أسود", "Black" },
                    { 2, "أبيض", "White" },
                    { 3, "رمادي", "Gray" },
                    { 4, "أزرق", "Blue" },
                    { 5, "سماوي", "Sky blue" },
                    { 6, "أخضر", "Green" },
                    { 7, "أصفر", "Yellow" },
                    { 8, "أحمر", "Red" },
                    { 9, "وردي", "Pink" },
                    { 10, "بنفسجي", "Purple" },
                    { 11, "برتقالي", "Orange" },
                    { 12, "متعدد الألوان", "Multi-color" },
                    { 13, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "جديد بتصفيات", "New (clearance)" },
                    { 3, "مستعمل بحالة ممتازة", "Used - excellent condition" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingSellingMethods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محل", "Store" },
                    { 2, "أونلاين", "Online" },
                    { 3, "محل + أونلاين", "Store + online" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingSizes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "حديث ولادة", "Newborn" },
                    { 2, "0-3 شهور", "0-3 months" },
                    { 3, "3-6 شهور", "3-6 months" },
                    { 4, "6-12 شهر", "6-12 months" },
                    { 5, "سنة", "1 year" },
                    { 6, "سنتين", "2 years" },
                    { 7, "3 سنوات", "3 years" },
                    { 8, "4 سنوات", "4 years" },
                    { 9, "5 سنوات", "5 years" },
                    { 10, "6 سنوات", "6 years" },
                    { 11, "7-8 سنوات", "7-8 years" },
                    { 12, "9-10 سنوات", "9-10 years" },
                    { 13, "11-12 سنة", "11-12 years" },
                    { 14, "13-14 سنة", "13-14 years" }
                });

            migrationBuilder.InsertData(
                table: "KidsClothingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تيشيرت أطفال", "Kids T-shirt" },
                    { 2, "فستان أطفال", "Kids dress" },
                    { 3, "بنطلون أطفال", "Kids trousers" },
                    { 4, "طقم أطفال", "Kids set" },
                    { 5, "جاكيت أطفال", "Kids jacket" },
                    { 6, "بيجامة أطفال", "Kids pajamas" },
                    { 7, "ملابس مواليد", "Newborn clothing" },
                    { 8, "ملابس داخلية أطفال", "Kids underwear" },
                    { 9, "ملابس رياضية أطفال", "Kids sportswear" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingBrands",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مستورد", "Imported" },
                    { 2, "محلي", "Local" },
                    { 3, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أسود", "Black" },
                    { 2, "أبيض", "White" },
                    { 3, "رمادي", "Gray" },
                    { 4, "كحلي", "Navy" },
                    { 5, "أزرق", "Blue" },
                    { 6, "سماوي", "Sky blue" },
                    { 7, "أخضر", "Green" },
                    { 8, "زيتي", "Olive" },
                    { 9, "بني", "Brown" },
                    { 10, "بيج", "Beige" },
                    { 11, "أحمر", "Red" },
                    { 12, "نبيتي", "Maroon" },
                    { 13, "برتقالي", "Orange" },
                    { 14, "أصفر", "Yellow" },
                    { 15, "متعدد الألوان", "Multi-color" },
                    { 16, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "جديد بتصفيات", "New (clearance)" },
                    { 3, "مستعمل بحالة ممتازة", "Used - excellent condition" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingSellingMethods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محل", "Store" },
                    { 2, "أونلاين", "Online" },
                    { 3, "محل + أونلاين", "Store + online" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingSizes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "XS", "XS" },
                    { 2, "S", "S" },
                    { 3, "M", "M" },
                    { 4, "L", "L" },
                    { 5, "XL", "XL" },
                    { 6, "XXL", "XXL" },
                    { 7, "XXXL", "XXXL" },
                    { 8, "XXXXL", "XXXXL" }
                });

            migrationBuilder.InsertData(
                table: "MenClothingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تيشيرت", "T-shirt" },
                    { 2, "قميص", "Shirt" },
                    { 3, "بنطلون", "Trousers" },
                    { 4, "جينز", "Jeans" },
                    { 5, "بدلة", "Suit" },
                    { 6, "جاكيت", "Jacket" },
                    { 7, "سويت شيرت", "Sweatshirt" },
                    { 8, "ملابس داخلية", "Underwear" },
                    { 9, "ملابس رياضية", "Sportswear" },
                    { 10, "بيجامات", "Pajamas" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PaintingMaterials",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كانفاس", "Canvas" },
                    { 2, "خشب", "Wood" },
                    { 3, "ورق", "Paper" },
                    { 4, "قماش", "Fabric" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PaintingOriginalities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أصلية", "Original" },
                    { 2, "مطبوعة", "Printed" }
                });

            migrationBuilder.InsertData(
                table: "PaintingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "زيتية", "Oil" },
                    { 2, "أكريليك", "Acrylic" },
                    { 3, "مائية", "Watercolor" },
                    { 4, "فحم", "Charcoal" },
                    { 5, "رصاص", "Pencil" },
                    { 6, "باستيل", "Pastel" },
                    { 7, "خط عربي", "Arabic calligraphy" },
                    { 8, "فن رقمي مطبوع", "Printed digital art" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingBrands",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مستورد", "Imported" },
                    { 2, "محلي", "Local" },
                    { 3, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingColors",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أسود", "Black" },
                    { 2, "أبيض", "White" },
                    { 3, "رمادي", "Gray" },
                    { 4, "كحلي", "Navy" },
                    { 5, "أزرق", "Blue" },
                    { 6, "سماوي", "Sky blue" },
                    { 7, "أخضر", "Green" },
                    { 8, "زيتي", "Olive" },
                    { 9, "بني", "Brown" },
                    { 10, "بيج", "Beige" },
                    { 11, "أحمر", "Red" },
                    { 12, "نبيتي", "Maroon" },
                    { 13, "برتقالي", "Orange" },
                    { 14, "أصفر", "Yellow" },
                    { 15, "وردي", "Pink" },
                    { 16, "بنفسجي", "Purple" },
                    { 17, "متعدد الألوان", "Multi-color" },
                    { 18, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingConditions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "جديد بتصفيات", "New (clearance)" },
                    { 3, "مستعمل بحالة ممتازة", "Used - excellent condition" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingSellingMethods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محل", "Store" },
                    { 2, "أونلاين", "Online" },
                    { 3, "محل + أونلاين", "Store + online" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingSizes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "XS", "XS" },
                    { 2, "S", "S" },
                    { 3, "M", "M" },
                    { 4, "L", "L" },
                    { 5, "XL", "XL" },
                    { 6, "XXL", "XXL" },
                    { 7, "XXXL", "XXXL" }
                });

            migrationBuilder.InsertData(
                table: "WomenClothingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "فستان", "Dress" },
                    { 2, "عباية", "Abaya" },
                    { 3, "طرح وحجاب", "Scarves & hijab" },
                    { 4, "بلوزة", "Blouse" },
                    { 5, "بنطلون", "Trousers" },
                    { 6, "جيبة", "Skirt" },
                    { 7, "طقم", "Set" },
                    { 8, "جاكيت", "Jacket" },
                    { 9, "ملابس داخلية", "Underwear" },
                    { 10, "ملابس رياضية", "Sportswear" },
                    { 11, "بيجامات", "Pajamas" },
                    { 12, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "NameAr" },
                values: new object[,]
                {
                    { 26, 7, "Antiques Decor", "تحف" },
                    { 27, 7, "Antiques", "أنتيكات" },
                    { 28, 7, "Paintings", "لوحات فنية" },
                    { 29, 7, "Handmade", "أعمال يدوية" },
                    { 30, 7, "Coins & Stamps", "عملات وطوابع" },
                    { 31, 8, "Men Clothing", "ملابس رجالي" },
                    { 32, 8, "Women Clothing", "ملابس حريمي" },
                    { 33, 8, "Kids Clothing", "ملابس أطفال" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueConditions_Name",
                table: "AntiqueConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueImages_AntiqueId",
                table: "AntiqueImages",
                column: "AntiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueMaterials_Name",
                table: "AntiqueMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueOriginalities_Name",
                table: "AntiqueOriginalities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_AntiqueType",
                table: "Antiques",
                column: "AntiqueType");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Center",
                table: "Antiques",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Condition",
                table: "Antiques",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_CountryOfOrigin",
                table: "Antiques",
                column: "CountryOfOrigin");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_CreatedAt",
                table: "Antiques",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ManufactureYear",
                table: "Antiques",
                column: "ManufactureYear");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Negotiable",
                table: "Antiques",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Originality",
                table: "Antiques",
                column: "Originality");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Price",
                table: "Antiques",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_SellerName",
                table: "Antiques",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_UserId",
                table: "Antiques",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueTypes_Name",
                table: "AntiqueTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueVideos_AntiqueId",
                table: "AntiqueVideos",
                column: "AntiqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiqueWorkingStatuses_Name",
                table: "AntiqueWorkingStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinStampConditions_Name",
                table: "CoinStampConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinStampImages_CoinStampId",
                table: "CoinStampImages",
                column: "CoinStampId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStampItemTypes_Name",
                table: "CoinStampItemTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinStampMetals_Name",
                table: "CoinStampMetals",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Center",
                table: "CoinStamps",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Condition",
                table: "CoinStamps",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Country",
                table: "CoinStamps",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_CreatedAt",
                table: "CoinStamps",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_IsOriginal",
                table: "CoinStamps",
                column: "IsOriginal");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_IsRare",
                table: "CoinStamps",
                column: "IsRare");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_IssueYear",
                table: "CoinStamps",
                column: "IssueYear");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ItemType",
                table: "CoinStamps",
                column: "ItemType");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Negotiable",
                table: "CoinStamps",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Price",
                table: "CoinStamps",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_SellerName",
                table: "CoinStamps",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_UserId",
                table: "CoinStamps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStampVideos_CoinStampId",
                table: "CoinStampVideos",
                column: "CoinStampId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueConditions_Name",
                table: "DecorAntiqueConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueImages_DecorAntiqueId",
                table: "DecorAntiqueImages",
                column: "DecorAntiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueItemTypes_Name",
                table: "DecorAntiqueItemTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueMaterials_Name",
                table: "DecorAntiqueMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueOriginalities_Name",
                table: "DecorAntiqueOriginalities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Center",
                table: "DecorAntiques",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Condition",
                table: "DecorAntiques",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_CreatedAt",
                table: "DecorAntiques",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ItemType",
                table: "DecorAntiques",
                column: "ItemType");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Material",
                table: "DecorAntiques",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Negotiable",
                table: "DecorAntiques",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Originality",
                table: "DecorAntiques",
                column: "Originality");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Price",
                table: "DecorAntiques",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_SellerName",
                table: "DecorAntiques",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_UserId",
                table: "DecorAntiques",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiqueVideos_DecorAntiqueId",
                table: "DecorAntiqueVideos",
                column: "DecorAntiqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeColors_Name",
                table: "HandmadeColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeColorSelections_Color",
                table: "HandmadeColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeColorSelections_HandmadeId_Color",
                table: "HandmadeColorSelections",
                columns: new[] { "HandmadeId", "Color" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeImages_HandmadeId",
                table: "HandmadeImages",
                column: "HandmadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Center",
                table: "Handmades",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_CreatedAt",
                table: "Handmades",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_CustomOrder",
                table: "Handmades",
                column: "CustomOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_HandmadeType",
                table: "Handmades",
                column: "HandmadeType");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_IsFullyHandmade",
                table: "Handmades",
                column: "IsFullyHandmade");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Negotiable",
                table: "Handmades",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Price",
                table: "Handmades",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_SellerName",
                table: "Handmades",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_UserId",
                table: "Handmades",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeTypes_Name",
                table: "HandmadeTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HandmadeVideos_HandmadeId",
                table: "HandmadeVideos",
                column: "HandmadeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingBrands_Name",
                table: "KidsClothingBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingColors_Name",
                table: "KidsClothingColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingColorSelections_Color",
                table: "KidsClothingColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingConditions_Name",
                table: "KidsClothingConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingImages_KidsClothingId",
                table: "KidsClothingImages",
                column: "KidsClothingId");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_Brand",
                table: "KidsClothings",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_Center",
                table: "KidsClothings",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ClothingType",
                table: "KidsClothings",
                column: "ClothingType");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_Condition",
                table: "KidsClothings",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_CreatedAt",
                table: "KidsClothings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_Price",
                table: "KidsClothings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_SellingMethod",
                table: "KidsClothings",
                column: "SellingMethod");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_StoreName",
                table: "KidsClothings",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_UserId",
                table: "KidsClothings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingSellingMethods_Name",
                table: "KidsClothingSellingMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingSizes_Name",
                table: "KidsClothingSizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingSizeSelections_Size",
                table: "KidsClothingSizeSelections",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothingTypes_Name",
                table: "KidsClothingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingBrands_Name",
                table: "MenClothingBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingColors_Name",
                table: "MenClothingColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingColorSelections_Color",
                table: "MenClothingColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingConditions_Name",
                table: "MenClothingConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingImages_MenClothingId",
                table: "MenClothingImages",
                column: "MenClothingId");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_Brand",
                table: "MenClothings",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_Center",
                table: "MenClothings",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ClothingType",
                table: "MenClothings",
                column: "ClothingType");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_Condition",
                table: "MenClothings",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_CreatedAt",
                table: "MenClothings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_Price",
                table: "MenClothings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_SellingMethod",
                table: "MenClothings",
                column: "SellingMethod");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_StoreName",
                table: "MenClothings",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_UserId",
                table: "MenClothings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingSellingMethods_Name",
                table: "MenClothingSellingMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingSizes_Name",
                table: "MenClothingSizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingSizeSelections_Size",
                table: "MenClothingSizeSelections",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothingTypes_Name",
                table: "MenClothingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintingImages_PaintingId",
                table: "PaintingImages",
                column: "PaintingId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintingMaterials_Name",
                table: "PaintingMaterials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintingOriginalities_Name",
                table: "PaintingOriginalities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ArtistName",
                table: "Paintings",
                column: "ArtistName");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Center",
                table: "Paintings",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_CreatedAt",
                table: "Paintings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Framed",
                table: "Paintings",
                column: "Framed");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Negotiable",
                table: "Paintings",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Originality",
                table: "Paintings",
                column: "Originality");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_PaintingType",
                table: "Paintings",
                column: "PaintingType");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Price",
                table: "Paintings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_SellerName",
                table: "Paintings",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_UserId",
                table: "Paintings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintingTypes_Name",
                table: "PaintingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintingVideos_PaintingId",
                table: "PaintingVideos",
                column: "PaintingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingBrands_Name",
                table: "WomenClothingBrands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingColors_Name",
                table: "WomenClothingColors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingColorSelections_Color",
                table: "WomenClothingColorSelections",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingConditions_Name",
                table: "WomenClothingConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingImages_WomenClothingId",
                table: "WomenClothingImages",
                column: "WomenClothingId");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_Brand",
                table: "WomenClothings",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_Center",
                table: "WomenClothings",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ClothingType",
                table: "WomenClothings",
                column: "ClothingType");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_Condition",
                table: "WomenClothings",
                column: "Condition");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_CreatedAt",
                table: "WomenClothings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_Price",
                table: "WomenClothings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_SellingMethod",
                table: "WomenClothings",
                column: "SellingMethod");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_StoreName",
                table: "WomenClothings",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_UserId",
                table: "WomenClothings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingSellingMethods_Name",
                table: "WomenClothingSellingMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingSizes_Name",
                table: "WomenClothingSizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingSizeSelections_Size",
                table: "WomenClothingSizeSelections",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothingTypes_Name",
                table: "WomenClothingTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntiqueConditions");

            migrationBuilder.DropTable(
                name: "AntiqueImages");

            migrationBuilder.DropTable(
                name: "AntiqueMaterials");

            migrationBuilder.DropTable(
                name: "AntiqueOriginalities");

            migrationBuilder.DropTable(
                name: "AntiqueTypes");

            migrationBuilder.DropTable(
                name: "AntiqueVideos");

            migrationBuilder.DropTable(
                name: "AntiqueWorkingStatuses");

            migrationBuilder.DropTable(
                name: "CoinStampConditions");

            migrationBuilder.DropTable(
                name: "CoinStampImages");

            migrationBuilder.DropTable(
                name: "CoinStampItemTypes");

            migrationBuilder.DropTable(
                name: "CoinStampMetals");

            migrationBuilder.DropTable(
                name: "CoinStampVideos");

            migrationBuilder.DropTable(
                name: "DecorAntiqueConditions");

            migrationBuilder.DropTable(
                name: "DecorAntiqueImages");

            migrationBuilder.DropTable(
                name: "DecorAntiqueItemTypes");

            migrationBuilder.DropTable(
                name: "DecorAntiqueMaterials");

            migrationBuilder.DropTable(
                name: "DecorAntiqueOriginalities");

            migrationBuilder.DropTable(
                name: "DecorAntiqueVideos");

            migrationBuilder.DropTable(
                name: "HandmadeColors");

            migrationBuilder.DropTable(
                name: "HandmadeColorSelections");

            migrationBuilder.DropTable(
                name: "HandmadeImages");

            migrationBuilder.DropTable(
                name: "HandmadeTypes");

            migrationBuilder.DropTable(
                name: "HandmadeVideos");

            migrationBuilder.DropTable(
                name: "KidsClothingBrands");

            migrationBuilder.DropTable(
                name: "KidsClothingColors");

            migrationBuilder.DropTable(
                name: "KidsClothingColorSelections");

            migrationBuilder.DropTable(
                name: "KidsClothingConditions");

            migrationBuilder.DropTable(
                name: "KidsClothingImages");

            migrationBuilder.DropTable(
                name: "KidsClothingSellingMethods");

            migrationBuilder.DropTable(
                name: "KidsClothingSizes");

            migrationBuilder.DropTable(
                name: "KidsClothingSizeSelections");

            migrationBuilder.DropTable(
                name: "KidsClothingTypes");

            migrationBuilder.DropTable(
                name: "MenClothingBrands");

            migrationBuilder.DropTable(
                name: "MenClothingColors");

            migrationBuilder.DropTable(
                name: "MenClothingColorSelections");

            migrationBuilder.DropTable(
                name: "MenClothingConditions");

            migrationBuilder.DropTable(
                name: "MenClothingImages");

            migrationBuilder.DropTable(
                name: "MenClothingSellingMethods");

            migrationBuilder.DropTable(
                name: "MenClothingSizes");

            migrationBuilder.DropTable(
                name: "MenClothingSizeSelections");

            migrationBuilder.DropTable(
                name: "MenClothingTypes");

            migrationBuilder.DropTable(
                name: "PaintingImages");

            migrationBuilder.DropTable(
                name: "PaintingMaterials");

            migrationBuilder.DropTable(
                name: "PaintingOriginalities");

            migrationBuilder.DropTable(
                name: "PaintingTypes");

            migrationBuilder.DropTable(
                name: "PaintingVideos");

            migrationBuilder.DropTable(
                name: "WomenClothingBrands");

            migrationBuilder.DropTable(
                name: "WomenClothingColors");

            migrationBuilder.DropTable(
                name: "WomenClothingColorSelections");

            migrationBuilder.DropTable(
                name: "WomenClothingConditions");

            migrationBuilder.DropTable(
                name: "WomenClothingImages");

            migrationBuilder.DropTable(
                name: "WomenClothingSellingMethods");

            migrationBuilder.DropTable(
                name: "WomenClothingSizes");

            migrationBuilder.DropTable(
                name: "WomenClothingSizeSelections");

            migrationBuilder.DropTable(
                name: "WomenClothingTypes");

            migrationBuilder.DropTable(
                name: "Antiques");

            migrationBuilder.DropTable(
                name: "CoinStamps");

            migrationBuilder.DropTable(
                name: "DecorAntiques");

            migrationBuilder.DropTable(
                name: "Handmades");

            migrationBuilder.DropTable(
                name: "KidsClothings");

            migrationBuilder.DropTable(
                name: "MenClothings");

            migrationBuilder.DropTable(
                name: "Paintings");

            migrationBuilder.DropTable(
                name: "WomenClothings");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
