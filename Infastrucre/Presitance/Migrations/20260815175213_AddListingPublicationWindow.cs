using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingPublicationWindow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Workshops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Workshops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Workshops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Workshops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "WomenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "WomenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "WomenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "WomenClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "WholesaleTraders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "WholesaleTraders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "WholesaleTraders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "WholesaleTraders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "ShoppingElectronics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "ShoppingElectronics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "ShoppingElectronics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "ShoppingElectronics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "SheepGoats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "SheepGoats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "SheepGoats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "SheepGoats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "PlantOrnaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "PlantOrnaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "PlantOrnaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "PlantOrnaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Pets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Pets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Pets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Paintings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Paintings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Paintings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Paintings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "OtherAnimals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "OtherAnimals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "OtherAnimals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "OtherAnimals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "MenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "MenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "MenClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "MenClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "LostFoundPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "LostFoundPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "LostFoundPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "LostFoundPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Livestock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Livestock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Livestock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Livestock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "LightingDecors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "LightingDecors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "LightingDecors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "LightingDecors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Lands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Lands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Lands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Lands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "KitchenTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "KitchenTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "KitchenTools",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "KitchenTools",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "KidsClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "KidsClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "KidsClothings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "KidsClothings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "JobRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "JobRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "JobRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "JobRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "JobOpportunities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "JobOpportunities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "JobOpportunities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "JobOpportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Horses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "HomemadeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "HomemadeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "HomemadeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "HomemadeFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "HomeKitchens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "HomeKitchens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "HomeKitchens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "HomeKitchens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "HomeAppliances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "HomeAppliances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "HomeAppliances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "HomeAppliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Handmades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Handmades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Handmades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Handmades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "GiftToys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "GiftToys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "GiftToys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "GiftToys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Furnitures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Furnitures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Furnitures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Furnitures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "FurnishingCurtains",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "FurnishingCurtains",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "FurnishingCurtains",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "FurnishingCurtains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "FruitVegetableMerchants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "FruitVegetableMerchants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "FruitVegetableMerchants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "FruitVegetableMerchants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Fish",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Fish",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Fish",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Fish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Farms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Farms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Farms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Farms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Factories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Factories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Factories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Factories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "DecorAntiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "DecorAntiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "DecorAntiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "DecorAntiques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Craftsmen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Craftsmen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Craftsmen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Craftsmen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Cosmetics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Cosmetics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Cosmetics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Cosmetics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "CoinStamps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "CoinStamps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "CoinStamps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "CoinStamps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Camels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Camels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Camels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Camels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Birds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Birds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Birds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Birds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Bees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Bees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Bees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Bees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "BathroomSupplies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "BathroomSupplies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "BathroomSupplies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "BathroomSupplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Apartments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Apartments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Apartments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Antiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Antiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Antiques",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Antiques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Accessories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPublishedAt",
                table: "Accessories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Accessories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepublishCount",
                table: "Accessories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ExpireAt",
                table: "Workshops",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ExpireAt",
                table: "WomenClothings",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_ExpireAt",
                table: "WholesaleTraders",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ExpireAt",
                table: "Suppliers",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ExpireAt",
                table: "Shops",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ExpireAt",
                table: "ShoppingElectronics",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_ExpireAt",
                table: "SheepGoats",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ExpireAt",
                table: "PlantOrnaments",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ExpireAt",
                table: "Pets",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ExpireAt",
                table: "Paintings",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_ExpireAt",
                table: "OtherAnimals",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ExpireAt",
                table: "MenClothings",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_ExpireAt",
                table: "LostFoundPosts",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_ExpireAt",
                table: "Livestock",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ExpireAt",
                table: "LightingDecors",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ExpireAt",
                table: "Lands",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ExpireAt",
                table: "KitchenTools",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ExpireAt",
                table: "KidsClothings",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ExpireAt",
                table: "JobRequests",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_ExpireAt",
                table: "JobOpportunities",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_ExpireAt",
                table: "Horses",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ExpireAt",
                table: "HomemadeFoods",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_ExpireAt",
                table: "HomeKitchens",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ExpireAt",
                table: "HomeAppliances",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_ExpireAt",
                table: "Handmades",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_ExpireAt",
                table: "GiftToys",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ExpireAt",
                table: "Furnitures",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ExpireAt",
                table: "FurnishingCurtains",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ExpireAt",
                table: "FruitVegetableMerchants",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_ExpireAt",
                table: "Fish",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ExpireAt",
                table: "Farms",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ExpireAt",
                table: "Factories",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ExpireAt",
                table: "DecorAntiques",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ExpireAt",
                table: "Craftsmen",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ExpireAt",
                table: "Cosmetics",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ExpireAt",
                table: "Companies",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ExpireAt",
                table: "CoinStamps",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_ExpireAt",
                table: "Camels",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_ExpireAt",
                table: "Birds",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_ExpireAt",
                table: "Bees",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ExpireAt",
                table: "BathroomSupplies",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ExpireAt",
                table: "Apartments",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ExpireAt",
                table: "Antiques",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ExpireAt",
                table: "Accessories",
                column: "ExpireAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workshops_ExpireAt",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_WomenClothings_ExpireAt",
                table: "WomenClothings");

            migrationBuilder.DropIndex(
                name: "IX_WholesaleTraders_ExpireAt",
                table: "WholesaleTraders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ExpireAt",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ExpireAt",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_ExpireAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_ExpireAt",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_ExpireAt",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ExpireAt",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_ExpireAt",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_ExpireAt",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_MenClothings_ExpireAt",
                table: "MenClothings");

            migrationBuilder.DropIndex(
                name: "IX_LostFoundPosts_ExpireAt",
                table: "LostFoundPosts");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_ExpireAt",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_ExpireAt",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_ExpireAt",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_ExpireAt",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KidsClothings_ExpireAt",
                table: "KidsClothings");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ExpireAt",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobOpportunities_ExpireAt",
                table: "JobOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_Horses_ExpireAt",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_ExpireAt",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_ExpireAt",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_ExpireAt",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_ExpireAt",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_ExpireAt",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_ExpireAt",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_ExpireAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FruitVegetableMerchants_ExpireAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropIndex(
                name: "IX_Fish_ExpireAt",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ExpireAt",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Factories_ExpireAt",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_ExpireAt",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Craftsmen_ExpireAt",
                table: "Craftsmen");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ExpireAt",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ExpireAt",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_ExpireAt",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_ExpireAt",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_Birds_ExpireAt",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_ExpireAt",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_ExpireAt",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ExpireAt",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_ExpireAt",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_ExpireAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "WomenClothings");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "WholesaleTraders");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "ShoppingElectronics");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "SheepGoats");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "PlantOrnaments");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "OtherAnimals");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "MenClothings");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "LostFoundPosts");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Livestock");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "LightingDecors");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "KitchenTools");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "KidsClothings");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "JobOpportunities");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "HomeKitchens");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "HomeAppliances");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Handmades");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "GiftToys");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "FurnishingCurtains");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Fish");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "DecorAntiques");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Craftsmen");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "CoinStamps");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Camels");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Birds");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Bees");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "BathroomSupplies");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Antiques");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "FirstPublishedAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Accessories");

            migrationBuilder.DropColumn(
                name: "RepublishCount",
                table: "Accessories");
        }
    }
}
