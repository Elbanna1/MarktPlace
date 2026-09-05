using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingOrderingIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workshops_ModerationStatus_ExpireAt",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_WomenClothings_ModerationStatus_ExpireAt",
                table: "WomenClothings");

            migrationBuilder.DropIndex(
                name: "IX_WholesaleTraders_ModerationStatus_ExpireAt",
                table: "WholesaleTraders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ModerationStatus_ExpireAt",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ModerationStatus_ExpireAt",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_ExpireAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_ModerationStatus_ExpireAt",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_ModerationStatus_ExpireAt",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_ModerationStatus_ExpireAt",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ModerationStatus_ExpireAt",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_ModerationStatus_ExpireAt",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_ModerationStatus_ExpireAt",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_MenClothings_ModerationStatus_ExpireAt",
                table: "MenClothings");

            migrationBuilder.DropIndex(
                name: "IX_LostFoundPosts_ModerationStatus_ExpireAt",
                table: "LostFoundPosts");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_ModerationStatus_ExpireAt",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_ModerationStatus_ExpireAt",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_ModerationStatus_ExpireAt",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_ModerationStatus_ExpireAt",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KidsClothings_ModerationStatus_ExpireAt",
                table: "KidsClothings");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ModerationStatus_ExpireAt",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobOpportunities_ModerationStatus_ExpireAt",
                table: "JobOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_Horses_ModerationStatus_ExpireAt",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_ModerationStatus_ExpireAt",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_ModerationStatus_ExpireAt",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_ModerationStatus_ExpireAt",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_ModerationStatus_ExpireAt",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_ModerationStatus_ExpireAt",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_ModerationStatus_ExpireAt",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_ExpireAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_ExpireAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropIndex(
                name: "IX_Fish_ModerationStatus_ExpireAt",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ModerationStatus_ExpireAt",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Factories_ModerationStatus_ExpireAt",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_ModerationStatus_ExpireAt",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Craftsmen_ModerationStatus_ExpireAt",
                table: "Craftsmen");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ModerationStatus_ExpireAt",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModerationStatus_ExpireAt",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_ModerationStatus_ExpireAt",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_ModerationStatus_ExpireAt",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_ModerationStatus_ExpireAt",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_Birds_ModerationStatus_ExpireAt",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_ModerationStatus_ExpireAt",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_ModerationStatus_ExpireAt",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_ModerationStatus_ExpireAt",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ModerationStatus_ExpireAt",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_ModerationStatus_ExpireAt",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_ExpireAt",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_ModerationStatus_ExpireAt",
                table: "Accessories");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ModerationStatus_CreatedAt",
                table: "Workshops",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ModerationStatus_CreatedAt",
                table: "WomenClothings",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_ModerationStatus_CreatedAt",
                table: "WholesaleTraders",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModerationStatus_CreatedAt",
                table: "Suppliers",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Shops",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_CreatedAt",
                table: "ShoppingElectronics",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_ModerationStatus_CreatedAt",
                table: "SheepGoats",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ModerationStatus_CreatedAt",
                table: "Pets",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ModerationStatus_CreatedAt",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_ModerationStatus_CreatedAt",
                table: "OtherAnimals",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ModerationStatus_CreatedAt",
                table: "MenClothings",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_ModerationStatus_CreatedAt",
                table: "LostFoundPosts",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_ModerationStatus_CreatedAt",
                table: "Livestock",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ModerationStatus_CreatedAt",
                table: "KidsClothings",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ModerationStatus_CreatedAt",
                table: "JobRequests",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_ModerationStatus_CreatedAt",
                table: "JobOpportunities",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Horses_ModerationStatus_CreatedAt",
                table: "Horses",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ModerationStatus_CreatedAt",
                table: "HomemadeFoods",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_ModerationStatus_CreatedAt",
                table: "HomeKitchens",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_ModerationStatus_CreatedAt",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_ModerationStatus_CreatedAt",
                table: "GiftToys",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_CreatedAt",
                table: "FruitVegetableMerchants",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Fish_ModerationStatus_CreatedAt",
                table: "Fish",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ModerationStatus_CreatedAt",
                table: "Farms",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ModerationStatus_CreatedAt",
                table: "Factories",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ModerationStatus_CreatedAt",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ModerationStatus_CreatedAt",
                table: "Craftsmen",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ModerationStatus_CreatedAt",
                table: "Cosmetics",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModerationStatus_CreatedAt",
                table: "Companies",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ModerationStatus_CreatedAt",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Camels_ModerationStatus_CreatedAt",
                table: "Camels",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Birds_ModerationStatus_CreatedAt",
                table: "Birds",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bees_ModerationStatus_CreatedAt",
                table: "Bees",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ModerationStatus_CreatedAt",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "CreatedAt" },
                descending: new[] { false, false, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ModerationStatus_CreatedAt",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workshops_ModerationStatus_CreatedAt",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_WomenClothings_ModerationStatus_CreatedAt",
                table: "WomenClothings");

            migrationBuilder.DropIndex(
                name: "IX_WholesaleTraders_ModerationStatus_CreatedAt",
                table: "WholesaleTraders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ModerationStatus_CreatedAt",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_CreatedAt",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_ModerationStatus_CreatedAt",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ModerationStatus_CreatedAt",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_ModerationStatus_CreatedAt",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_ModerationStatus_CreatedAt",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_MenClothings_ModerationStatus_CreatedAt",
                table: "MenClothings");

            migrationBuilder.DropIndex(
                name: "IX_LostFoundPosts_ModerationStatus_CreatedAt",
                table: "LostFoundPosts");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_ModerationStatus_CreatedAt",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KidsClothings_ModerationStatus_CreatedAt",
                table: "KidsClothings");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ModerationStatus_CreatedAt",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobOpportunities_ModerationStatus_CreatedAt",
                table: "JobOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_Horses_ModerationStatus_CreatedAt",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_ModerationStatus_CreatedAt",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_ModerationStatus_CreatedAt",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_ModerationStatus_CreatedAt",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_ModerationStatus_CreatedAt",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_CreatedAt",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropIndex(
                name: "IX_Fish_ModerationStatus_CreatedAt",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ModerationStatus_CreatedAt",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Factories_ModerationStatus_CreatedAt",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_ModerationStatus_CreatedAt",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Craftsmen_ModerationStatus_CreatedAt",
                table: "Craftsmen");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ModerationStatus_CreatedAt",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModerationStatus_CreatedAt",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_ModerationStatus_CreatedAt",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_ModerationStatus_CreatedAt",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_Birds_ModerationStatus_CreatedAt",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_ModerationStatus_CreatedAt",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ModerationStatus_IsPremium_IsFeatured_CreatedAt",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_ModerationStatus_CreatedAt",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_ModerationStatus_CreatedAt",
                table: "Accessories");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ModerationStatus_ExpireAt",
                table: "Workshops",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ModerationStatus_ExpireAt",
                table: "WomenClothings",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_ModerationStatus_ExpireAt",
                table: "WholesaleTraders",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModerationStatus_ExpireAt",
                table: "Suppliers",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ModerationStatus_ExpireAt",
                table: "Shops",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_ExpireAt",
                table: "ShoppingElectronics",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_ModerationStatus_ExpireAt",
                table: "SheepGoats",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_ExpireAt",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ModerationStatus_ExpireAt",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ModerationStatus_ExpireAt",
                table: "Pets",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ModerationStatus_ExpireAt",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_ModerationStatus_ExpireAt",
                table: "OtherAnimals",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ModerationStatus_ExpireAt",
                table: "MenClothings",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_ModerationStatus_ExpireAt",
                table: "LostFoundPosts",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_ModerationStatus_ExpireAt",
                table: "Livestock",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ModerationStatus_ExpireAt",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ModerationStatus_ExpireAt",
                table: "Lands",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ModerationStatus_ExpireAt",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ModerationStatus_ExpireAt",
                table: "KidsClothings",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ModerationStatus_ExpireAt",
                table: "JobRequests",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_ModerationStatus_ExpireAt",
                table: "JobOpportunities",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Horses_ModerationStatus_ExpireAt",
                table: "Horses",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ModerationStatus_ExpireAt",
                table: "HomemadeFoods",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_ModerationStatus_ExpireAt",
                table: "HomeKitchens",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ModerationStatus_ExpireAt",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_ModerationStatus_ExpireAt",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_ModerationStatus_ExpireAt",
                table: "GiftToys",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ModerationStatus_ExpireAt",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_ExpireAt",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_ExpireAt",
                table: "FruitVegetableMerchants",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Fish_ModerationStatus_ExpireAt",
                table: "Fish",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ModerationStatus_ExpireAt",
                table: "Farms",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ModerationStatus_ExpireAt",
                table: "Factories",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ModerationStatus_ExpireAt",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ModerationStatus_ExpireAt",
                table: "Craftsmen",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ModerationStatus_ExpireAt",
                table: "Cosmetics",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModerationStatus_ExpireAt",
                table: "Companies",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ModerationStatus_ExpireAt",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Camels_ModerationStatus_ExpireAt",
                table: "Camels",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_ExpireAt",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Birds_ModerationStatus_ExpireAt",
                table: "Birds",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bees_ModerationStatus_ExpireAt",
                table: "Bees",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ModerationStatus_ExpireAt",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_ExpireAt",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ModerationStatus_ExpireAt",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ModerationStatus_ExpireAt",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_ExpireAt",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "CreatedAt", "CategoryId", "SubCategoryId", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ModerationStatus_ExpireAt",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "ExpireAt" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "CreatedAt" });
        }
    }
}
