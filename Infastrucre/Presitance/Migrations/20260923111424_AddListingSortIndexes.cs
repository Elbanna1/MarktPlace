using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddListingSortIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt",
                table: "Advertisements");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Feed_MostViewed",
                table: "Shops",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Feed_PriceAsc",
                table: "Shops",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Feed_PriceDesc",
                table: "Shops",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Feed_PriceAsc",
                table: "ShoppingElectronics",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingElectronics_Feed_PriceDesc",
                table: "ShoppingElectronics",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_Feed_MostViewed",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_Feed_MostViewed",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_Feed_PriceAsc",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantOrnaments_Feed_PriceDesc",
                table: "PlantOrnaments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Feed_PriceAsc",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Feed_PriceDesc",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Feed_MostViewed",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Feed_PriceAsc",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LightingDecors_Feed_PriceDesc",
                table: "LightingDecors",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Feed_MostViewed",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Feed_PriceAsc",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "TotalPrice", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Feed_PriceDesc",
                table: "Lands",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "TotalPrice", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Feed_MostViewed",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Feed_PriceAsc",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTools_Feed_PriceDesc",
                table: "KitchenTools",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_Feed_PriceAsc",
                table: "HomemadeFoods",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomemadeFoods_Feed_PriceDesc",
                table: "HomemadeFoods",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_Feed_PriceAsc",
                table: "HomeKitchens",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeKitchens_Feed_PriceDesc",
                table: "HomeKitchens",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Feed_MostViewed",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Feed_PriceAsc",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeAppliances_Feed_PriceDesc",
                table: "HomeAppliances",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Feed_PriceAsc",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Feed_PriceDesc",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_Feed_PriceAsc",
                table: "GiftToys",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiftToys_Feed_PriceDesc",
                table: "GiftToys",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Feed_MostViewed",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Feed_PriceAsc",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Feed_PriceDesc",
                table: "Furnitures",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Feed_MostViewed",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Feed_PriceAsc",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FurnishingCurtains_Feed_PriceDesc",
                table: "FurnishingCurtains",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Feed_PriceAsc",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Feed_PriceDesc",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_Feed_PriceAsc",
                table: "Cosmetics",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_Feed_PriceDesc",
                table: "Cosmetics",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Feed_PriceAsc",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Feed_PriceDesc",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_Feed_MostViewed",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Feed_MostViewed",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Feed_PriceAsc",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BathroomSupplies_Feed_PriceDesc",
                table: "BathroomSupplies",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_Feed_MostViewed",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Feed_MostViewed",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "ViewCount", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Feed_PriceAsc",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Feed_PriceDesc",
                table: "Apartments",
                columns: new[] { "ModerationStatus", "IsPremium", "IsFeatured", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Feed_PriceAsc",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Feed_PriceDesc",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt_Id",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt_Id",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Views", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Feed_PriceAsc",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Feed_PriceDesc",
                table: "Accessories",
                columns: new[] { "ModerationStatus", "Price", "CreatedAt", "Id" },
                descending: new[] { false, true, true, true })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shops_Feed_MostViewed",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_Feed_PriceAsc",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_Feed_PriceDesc",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_Feed_PriceAsc",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingElectronics_Feed_PriceDesc",
                table: "ShoppingElectronics");

            migrationBuilder.DropIndex(
                name: "IX_Rescues_Feed_MostViewed",
                table: "Rescues");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_Feed_MostViewed",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_Feed_PriceAsc",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_PlantOrnaments_Feed_PriceDesc",
                table: "PlantOrnaments");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_Feed_PriceAsc",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_Feed_PriceDesc",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_Feed_MostViewed",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_Feed_PriceAsc",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_LightingDecors_Feed_PriceDesc",
                table: "LightingDecors");

            migrationBuilder.DropIndex(
                name: "IX_Lands_Feed_MostViewed",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_Lands_Feed_PriceAsc",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_Lands_Feed_PriceDesc",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_Feed_MostViewed",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_Feed_PriceAsc",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_KitchenTools_Feed_PriceDesc",
                table: "KitchenTools");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_Feed_PriceAsc",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomemadeFoods_Feed_PriceDesc",
                table: "HomemadeFoods");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_Feed_PriceAsc",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeKitchens_Feed_PriceDesc",
                table: "HomeKitchens");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_Feed_MostViewed",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_Feed_PriceAsc",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_HomeAppliances_Feed_PriceDesc",
                table: "HomeAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_Feed_PriceAsc",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_Feed_PriceDesc",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_Feed_PriceAsc",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_GiftToys_Feed_PriceDesc",
                table: "GiftToys");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_Feed_MostViewed",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_Feed_PriceAsc",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_Feed_PriceDesc",
                table: "Furnitures");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_Feed_MostViewed",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_Feed_PriceAsc",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_FurnishingCurtains_Feed_PriceDesc",
                table: "FurnishingCurtains");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_Feed_PriceAsc",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_Feed_PriceDesc",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_Feed_PriceAsc",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_Feed_PriceDesc",
                table: "Cosmetics");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_Feed_PriceAsc",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_Feed_PriceDesc",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_Feed_MostViewed",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_Feed_MostViewed",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_Feed_PriceAsc",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_BathroomSupplies_Feed_PriceDesc",
                table: "BathroomSupplies");

            migrationBuilder.DropIndex(
                name: "IX_AskConsults_Feed_MostViewed",
                table: "AskConsults");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_Feed_MostViewed",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_Feed_PriceAsc",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_Feed_PriceDesc",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_Feed_PriceAsc",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_Feed_PriceDesc",
                table: "Antiques");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt_Id",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt_Id",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_Feed_PriceAsc",
                table: "Accessories");

            migrationBuilder.DropIndex(
                name: "IX_Accessories_Feed_PriceDesc",
                table: "Accessories");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceAsc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt" },
                descending: new[] { false, false, false, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Feed_PriceDesc",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Price", "CreatedAt" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_CreatedAt",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "CreatedAt" },
                descending: new[] { false, false, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price", "Views" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ModerationStatus_Status_Views_CreatedAt",
                table: "Advertisements",
                columns: new[] { "ModerationStatus", "Status", "Views", "CreatedAt" },
                descending: new[] { false, false, true, true })
                .Annotation("SqlServer:Include", new[] { "ExpireAt", "CategoryId", "SubCategoryId", "Price" });
        }
    }
}
