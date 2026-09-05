using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingModerationSystem : Migration
    {
        /// <summary>
        /// Every table that gained the moderation block, used by the backfill at the end of
        /// <c>Up</c>. Listed once here rather than repeated in forty-five SQL statements.
        /// </summary>
        private static readonly string[] ModeratedTables =
        {
            "Accessories", "Advertisements", "Antiques", "Apartments", "BathroomSupplies", "Bees",
            "Birds", "Camels", "CoinStamps", "Companies", "Cosmetics", "Craftsmen", "DecorAntiques",
            "Factories", "Farms", "Fish", "FruitVegetableMerchants", "FurnishingCurtains",
            "Furnitures", "GiftToys", "Handmades", "HomeAppliances", "HomeKitchens", "HomemadeFoods",
            "Horses", "JobOpportunities", "JobRequests", "KidsClothings", "KitchenTools", "Lands",
            "LightingDecors", "Livestock", "LostFoundPosts", "MenClothings", "OtherAnimals",
            "Paintings", "Pets", "PlantOrnaments", "SheepGoats", "ShoppingElectronics", "Shops",
            "Suppliers", "WholesaleTraders", "WomenClothings", "Workshops"
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Workshops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Workshops",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Workshops",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Workshops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Workshops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "WomenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "WomenClothings",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "WomenClothings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "WomenClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "WomenClothings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "WholesaleTraders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "WholesaleTraders",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "WholesaleTraders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "WholesaleTraders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "WholesaleTraders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Suppliers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Suppliers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Shops",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Shops",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "ShoppingElectronics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "ShoppingElectronics",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "ShoppingElectronics",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "ShoppingElectronics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "ShoppingElectronics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "SheepGoats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "SheepGoats",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "SheepGoats",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "SheepGoats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "SheepGoats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "PlantOrnaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "PlantOrnaments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "PlantOrnaments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "PlantOrnaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "PlantOrnaments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Pets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Pets",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Pets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Paintings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Paintings",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Paintings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Paintings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Paintings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "OtherAnimals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "OtherAnimals",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "OtherAnimals",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "OtherAnimals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "OtherAnimals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "MenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "MenClothings",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "MenClothings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "MenClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "MenClothings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "LostFoundPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "LostFoundPosts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "LostFoundPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "LostFoundPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "LostFoundPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Livestock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Livestock",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Livestock",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Livestock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Livestock",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "LightingDecors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "LightingDecors",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "LightingDecors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "LightingDecors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "LightingDecors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Lands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Lands",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Lands",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Lands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Lands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "KitchenTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "KitchenTools",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "KitchenTools",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "KitchenTools",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "KitchenTools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "KidsClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "KidsClothings",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "KidsClothings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "KidsClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "KidsClothings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "JobRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "JobRequests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "JobRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "JobRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "JobRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "JobOpportunities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "JobOpportunities",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "JobOpportunities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "JobOpportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "JobOpportunities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Horses",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Horses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Horses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Horses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "HomemadeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "HomemadeFoods",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "HomemadeFoods",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "HomemadeFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "HomemadeFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "HomeKitchens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "HomeKitchens",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "HomeKitchens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "HomeKitchens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "HomeKitchens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "HomeAppliances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "HomeAppliances",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "HomeAppliances",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "HomeAppliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "HomeAppliances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Handmades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Handmades",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Handmades",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Handmades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Handmades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "GiftToys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "GiftToys",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "GiftToys",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "GiftToys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "GiftToys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Furnitures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Furnitures",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Furnitures",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Furnitures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Furnitures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "FurnishingCurtains",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "FurnishingCurtains",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "FurnishingCurtains",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "FurnishingCurtains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "FurnishingCurtains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "FruitVegetableMerchants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "FruitVegetableMerchants",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "FruitVegetableMerchants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "FruitVegetableMerchants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "FruitVegetableMerchants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Fish",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Fish",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Fish",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Fish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Fish",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Farms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Farms",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Farms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Farms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Farms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Factories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Factories",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Factories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Factories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Factories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "DecorAntiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "DecorAntiques",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "DecorAntiques",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "DecorAntiques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "DecorAntiques",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Craftsmen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Craftsmen",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Craftsmen",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Craftsmen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Craftsmen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Cosmetics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Cosmetics",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Cosmetics",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Cosmetics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Cosmetics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Companies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Companies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "CoinStamps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "CoinStamps",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "CoinStamps",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "CoinStamps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "CoinStamps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Camels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Camels",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Camels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Camels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Camels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Birds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Birds",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Birds",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Birds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Birds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Bees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Bees",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Bees",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Bees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Bees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "BathroomSupplies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "BathroomSupplies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "BathroomSupplies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "BathroomSupplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "BathroomSupplies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Apartments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Apartments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Apartments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Apartments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Antiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Antiques",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Antiques",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Antiques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Antiques",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Advertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Advertisements",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Advertisements",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedAt",
                table: "Accessories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedBy",
                table: "Accessories",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "Accessories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Accessories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectionReason",
                table: "Accessories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ModerationStatus",
                table: "Workshops",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ModerationStatus",
                table: "WomenClothings",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_ModerationStatus",
                table: "WholesaleTraders",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModerationStatus",
                table: "Suppliers",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ModerationStatus",
                table: "Shops",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ModerationStatus",
                table: "ShoppingElectronics",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_ModerationStatus",
                table: "SheepGoats",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ModerationStatus",
                table: "PlantOrnaments",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ModerationStatus",
                table: "Pets",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ModerationStatus",
                table: "Paintings",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_ModerationStatus",
                table: "OtherAnimals",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ModerationStatus",
                table: "MenClothings",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_ModerationStatus",
                table: "LostFoundPosts",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_ModerationStatus",
                table: "Livestock",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ModerationStatus",
                table: "LightingDecors",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ModerationStatus",
                table: "Lands",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ModerationStatus",
                table: "KitchenTools",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ModerationStatus",
                table: "KidsClothings",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ModerationStatus",
                table: "JobRequests",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_ModerationStatus",
                table: "JobOpportunities",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_ModerationStatus",
                table: "Horses",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ModerationStatus",
                table: "HomemadeFoods",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_ModerationStatus",
                table: "HomeKitchens",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ModerationStatus",
                table: "HomeAppliances",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_ModerationStatus",
                table: "Handmades",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_ModerationStatus",
                table: "GiftToys",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ModerationStatus",
                table: "Furnitures",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ModerationStatus",
                table: "FurnishingCurtains",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus",
                table: "FruitVegetableMerchants",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_ModerationStatus",
                table: "Fish",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ModerationStatus",
                table: "Farms",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ModerationStatus",
                table: "Factories",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ModerationStatus",
                table: "DecorAntiques",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ModerationStatus",
                table: "Craftsmen",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ModerationStatus",
                table: "Cosmetics",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModerationStatus",
                table: "Companies",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ModerationStatus",
                table: "CoinStamps",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_ModerationStatus",
                table: "Camels",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_ModerationStatus",
                table: "Birds",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_ModerationStatus",
                table: "Bees",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ModerationStatus",
                table: "BathroomSupplies",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ModerationStatus",
                table: "Apartments",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ModerationStatus",
                table: "Antiques",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus",
                table: "Advertisements",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ModerationStatus",
                table: "Accessories",
                column: "ModerationStatus");

            // Everything already on the site was published under the old rules and stays published.
            // The review queue is for what arrives from now on, not for the whole back catalogue —
            // without this, adding the column would silently unpublish every existing listing,
            // because the column's default is Pending.
            //
            // Safe to run unconditionally: it executes immediately after the columns are created, so
            // every row these statements touch is by definition one that predates moderation.
            // 1 = ModerationStatus.Approved.
            foreach (var table in ModeratedTables)
                migrationBuilder.Sql($"UPDATE [{table}] SET [ModerationStatus] = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workshops_ModerationStatus",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_WomenClothings_ModerationStatus",
                table: "WomenClothings");

            migrationBuilder.DropIndex(
                name: "IX_WholesaleTraders_ModerationStatus",
                table: "WholesaleTraders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ModerationStatus",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ModerationStatus",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_ModerationStatus",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_ModerationStatus",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_ModerationStatus",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ModerationStatus",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_ModerationStatus",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_ModerationStatus",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_MenClothings_ModerationStatus",
                table: "MenClothings");

            migrationBuilder.DropIndex(
                name: "IX_LostFoundPosts_ModerationStatus",
                table: "LostFoundPosts");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_ModerationStatus",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_ModerationStatus",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_ModerationStatus",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_ModerationStatus",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KidsClothings_ModerationStatus",
                table: "KidsClothings");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ModerationStatus",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobOpportunities_ModerationStatus",
                table: "JobOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_Horses_ModerationStatus",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_ModerationStatus",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_ModerationStatus",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_ModerationStatus",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_ModerationStatus",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_ModerationStatus",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_ModerationStatus",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_ModerationStatus",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropIndex(
                name: "IX_Fish_ModerationStatus",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ModerationStatus",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Factories_ModerationStatus",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_ModerationStatus",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Craftsmen_ModerationStatus",
                table: "Craftsmen");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ModerationStatus",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModerationStatus",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_ModerationStatus",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_ModerationStatus",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_Birds_ModerationStatus",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_ModerationStatus",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_ModerationStatus",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ModerationStatus",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_ModerationStatus",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_ModerationStatus",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ModeratedAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "ModerationStatus",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Accessories");
        }
    }
}
