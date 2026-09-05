using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingVisibilityIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_Rescues_ModerationStatus",
                table: "Rescues");

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
                name: "IX_BloodRequests_ModerationStatus",
                table: "BloodRequests");

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
                name: "IX_AskConsults_ModerationStatus",
                table: "AskConsults");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Rescues_ModerationStatus",
                table: "Rescues",
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
                name: "IX_BloodRequests_ModerationStatus",
                table: "BloodRequests",
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
                name: "IX_AskConsults_ModerationStatus",
                table: "AskConsults",
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
        }
    }
}
