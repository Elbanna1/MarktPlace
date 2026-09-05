using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingOrderingTieBreaker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_Accessories_ModerationStatus_CreatedAt",
                table: "Accessories");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ModerationStatus_CreatedAt_Id",
                table: "Workshops",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WomenClothings_ModerationStatus_CreatedAt_Id",
                table: "WomenClothings",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_ModerationStatus_CreatedAt_Id",
                table: "WholesaleTraders",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModerationStatus_CreatedAt_Id",
                table: "Suppliers",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Shops",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_CreatedAt_Id",
                table: "ShoppingElectronics",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_ModerationStatus_CreatedAt_Id",
                table: "SheepGoats",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt_Id",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ModerationStatus_CreatedAt_Id",
                table: "Pets",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_ModerationStatus_CreatedAt_Id",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_ModerationStatus_CreatedAt_Id",
                table: "OtherAnimals",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MenClothings_ModerationStatus_CreatedAt_Id",
                table: "MenClothings",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_ModerationStatus_CreatedAt_Id",
                table: "LostFoundPosts",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_ModerationStatus_CreatedAt_Id",
                table: "Livestock",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KidsClothings_ModerationStatus_CreatedAt_Id",
                table: "KidsClothings",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ModerationStatus_CreatedAt_Id",
                table: "JobRequests",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_ModerationStatus_CreatedAt_Id",
                table: "JobOpportunities",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Horses_ModerationStatus_CreatedAt_Id",
                table: "Horses",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_ModerationStatus_CreatedAt_Id",
                table: "HomemadeFoods",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_ModerationStatus_CreatedAt_Id",
                table: "HomeKitchens",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_ModerationStatus_CreatedAt_Id",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_ModerationStatus_CreatedAt_Id",
                table: "GiftToys",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_CreatedAt_Id",
                table: "FruitVegetableMerchants",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Fish_ModerationStatus_CreatedAt_Id",
                table: "Fish",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ModerationStatus_CreatedAt_Id",
                table: "Farms",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ModerationStatus_CreatedAt_Id",
                table: "Factories",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_ModerationStatus_CreatedAt_Id",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ModerationStatus_CreatedAt_Id",
                table: "Craftsmen",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ModerationStatus_CreatedAt_Id",
                table: "Cosmetics",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ModerationStatus_CreatedAt_Id",
                table: "Companies",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_ModerationStatus_CreatedAt_Id",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Camels_ModerationStatus_CreatedAt_Id",
                table: "Camels",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt_Id",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Birds_ModerationStatus_CreatedAt_Id",
                table: "Birds",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bees_ModerationStatus_CreatedAt_Id",
                table: "Bees",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt_Id",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_ModerationStatus_CreatedAt_Id",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_ModerationStatus_CreatedAt_Id",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "CreatedAt", "Id" },
                descending: new[] { false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workshops_ModerationStatus_CreatedAt_Id",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_WomenClothings_ModerationStatus_CreatedAt_Id",
                table: "WomenClothings");

            migrationBuilder.DropIndex(
                name: "IX_WholesaleTraders_ModerationStatus_CreatedAt_Id",
                table: "WholesaleTraders");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ModerationStatus_CreatedAt_Id",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_ModerationStatus_CreatedAt_Id",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_ModerationStatus_CreatedAt_Id",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt_Id",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ModerationStatus_CreatedAt_Id",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_ModerationStatus_CreatedAt_Id",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_ModerationStatus_CreatedAt_Id",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_MenClothings_ModerationStatus_CreatedAt_Id",
                table: "MenClothings");

            migrationBuilder.DropIndex(
                name: "IX_LostFoundPosts_ModerationStatus_CreatedAt_Id",
                table: "LostFoundPosts");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_ModerationStatus_CreatedAt_Id",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KidsClothings_ModerationStatus_CreatedAt_Id",
                table: "KidsClothings");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ModerationStatus_CreatedAt_Id",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobOpportunities_ModerationStatus_CreatedAt_Id",
                table: "JobOpportunities");

            migrationBuilder.DropIndex(
                name: "IX_Horses_ModerationStatus_CreatedAt_Id",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_ModerationStatus_CreatedAt_Id",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_ModerationStatus_CreatedAt_Id",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_ModerationStatus_CreatedAt_Id",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_ModerationStatus_CreatedAt_Id",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FruitVegetableMerchants_ModerationStatus_CreatedAt_Id",
                table: "FruitVegetableMerchants");

            migrationBuilder.DropIndex(
                name: "IX_Fish_ModerationStatus_CreatedAt_Id",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ModerationStatus_CreatedAt_Id",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Factories_ModerationStatus_CreatedAt_Id",
                table: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_ModerationStatus_CreatedAt_Id",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Craftsmen_ModerationStatus_CreatedAt_Id",
                table: "Craftsmen");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ModerationStatus_CreatedAt_Id",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ModerationStatus_CreatedAt_Id",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_ModerationStatus_CreatedAt_Id",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_ModerationStatus_CreatedAt_Id",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt_Id",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_Birds_ModerationStatus_CreatedAt_Id",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_ModerationStatus_CreatedAt_Id",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt_Id",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ModerationStatus_IsPremium_IsFeatured_CreatedAt_Id",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_ModerationStatus_CreatedAt_Id",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_ModerationStatus_CreatedAt_Id",
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
                name: "IX_Accessories_ModerationStatus_CreatedAt",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });
        }
    }
}
