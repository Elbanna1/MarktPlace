using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertiserLookupIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_SellerName",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_Pets_SellerName",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_SellerName",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_SellerName",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_SellerName",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_Horses_SellerName",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_SellerName",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_Fish_SellerName",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_SellerName",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_SellerName",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_SellerName",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_Birds_SellerName",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_SellerName",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_SellerName",
                table: "Antiques");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Advertisers",
                table: "SheepGoats",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Advertisers",
                table: "Pets",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_Advertisers",
                table: "Paintings",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_Advertisers",
                table: "OtherAnimals",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Advertisers",
                table: "Livestock",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Advertisers",
                table: "Horses",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_Advertisers",
                table: "Handmades",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Fish_Advertisers",
                table: "Fish",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_Advertisers",
                table: "DecorAntiques",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_Advertisers",
                table: "CoinStamps",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Advertisers",
                table: "Camels",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Birds_Advertisers",
                table: "Birds",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bees_Advertisers",
                table: "Bees",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_Advertisers",
                table: "Antiques",
                columns: new[] { "ModerationStatus", "SellerName" })
                .Annotation("SqlServer:Include", new[] { "IsDeleted", "ExpireAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SheepGoats_Advertisers",
                table: "SheepGoats");

            migrationBuilder.DropIndex(
                name: "IX_Pets_Advertisers",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_Advertisers",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_OtherAnimals_Advertisers",
                table: "OtherAnimals");

            migrationBuilder.DropIndex(
                name: "IX_Livestock_Advertisers",
                table: "Livestock");

            migrationBuilder.DropIndex(
                name: "IX_Horses_Advertisers",
                table: "Horses");

            migrationBuilder.DropIndex(
                name: "IX_Handmades_Advertisers",
                table: "Handmades");

            migrationBuilder.DropIndex(
                name: "IX_Fish_Advertisers",
                table: "Fish");

            migrationBuilder.DropIndex(
                name: "IX_DecorAntiques_Advertisers",
                table: "DecorAntiques");

            migrationBuilder.DropIndex(
                name: "IX_CoinStamps_Advertisers",
                table: "CoinStamps");

            migrationBuilder.DropIndex(
                name: "IX_Camels_Advertisers",
                table: "Camels");

            migrationBuilder.DropIndex(
                name: "IX_Birds_Advertisers",
                table: "Birds");

            migrationBuilder.DropIndex(
                name: "IX_Bees_Advertisers",
                table: "Bees");

            migrationBuilder.DropIndex(
                name: "IX_Antiques_Advertisers",
                table: "Antiques");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_SellerName",
                table: "SheepGoats",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_SellerName",
                table: "Pets",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_SellerName",
                table: "Paintings",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_SellerName",
                table: "OtherAnimals",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_SellerName",
                table: "Livestock",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_SellerName",
                table: "Horses",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Handmades_SellerName",
                table: "Handmades",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_SellerName",
                table: "Fish",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_DecorAntiques_SellerName",
                table: "DecorAntiques",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_CoinStamps_SellerName",
                table: "CoinStamps",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_SellerName",
                table: "Camels",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_SellerName",
                table: "Birds",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_SellerName",
                table: "Bees",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Antiques_SellerName",
                table: "Antiques",
                column: "SellerName");
        }
    }
}
