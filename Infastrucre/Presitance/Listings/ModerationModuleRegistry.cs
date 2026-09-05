using Domain.Entities;
using Shared.Enums;

namespace Persistence.Listings;

public static class ModerationModuleRegistry
{
    public static IReadOnlyDictionary<ListingModuleType, Type> EntityTypes { get; } =
        new Dictionary<ListingModuleType, Type>
        {
            [ListingModuleType.Advertisement] = typeof(Advertisement),

            [ListingModuleType.Craftsman] = typeof(Craftsman),
            [ListingModuleType.Workshop] = typeof(Workshop),

            [ListingModuleType.Factory] = typeof(Factory),
            [ListingModuleType.Farm] = typeof(Farm),
            [ListingModuleType.Company] = typeof(Company),
            [ListingModuleType.Supplier] = typeof(Supplier),
            [ListingModuleType.WholesaleTrader] = typeof(WholesaleTrader),
            [ListingModuleType.FruitVegetableMerchant] = typeof(FruitVegetableMerchant),

            [ListingModuleType.JobRequest] = typeof(JobRequest),
            [ListingModuleType.JobOpportunity] = typeof(JobOpportunity),

            [ListingModuleType.Livestock] = typeof(Livestock),
            [ListingModuleType.SheepGoat] = typeof(SheepGoat),
            [ListingModuleType.Horse] = typeof(Horse),
            [ListingModuleType.Camel] = typeof(Camel),
            [ListingModuleType.Bird] = typeof(Bird),
            [ListingModuleType.Pet] = typeof(Pet),
            [ListingModuleType.Fish] = typeof(Fish),
            [ListingModuleType.Bee] = typeof(Bee),
            [ListingModuleType.OtherAnimal] = typeof(OtherAnimal),

            [ListingModuleType.DecorAntique] = typeof(DecorAntique),
            [ListingModuleType.Antique] = typeof(Antique),
            [ListingModuleType.Painting] = typeof(Painting),
            [ListingModuleType.Handmade] = typeof(Handmade),
            [ListingModuleType.CoinStamp] = typeof(CoinStamp),

            [ListingModuleType.MenClothing] = typeof(MenClothing),
            [ListingModuleType.WomenClothing] = typeof(WomenClothing),
            [ListingModuleType.KidsClothing] = typeof(KidsClothing),

            [ListingModuleType.Accessory] = typeof(Accessory),
            [ListingModuleType.Cosmetic] = typeof(Cosmetic),
            [ListingModuleType.HomeKitchen] = typeof(HomeKitchen),
            [ListingModuleType.ShoppingElectronic] = typeof(ShoppingElectronic),
            [ListingModuleType.GiftToy] = typeof(GiftToy),
            [ListingModuleType.HomemadeFood] = typeof(HomemadeFood),

            [ListingModuleType.Furniture] = typeof(Furniture),
            [ListingModuleType.FurnishingCurtain] = typeof(FurnishingCurtain),
            [ListingModuleType.LightingDecor] = typeof(LightingDecor),
            [ListingModuleType.KitchenTool] = typeof(KitchenTool),
            [ListingModuleType.HomeAppliance] = typeof(HomeAppliance),
            [ListingModuleType.BathroomSupply] = typeof(BathroomSupply),
            [ListingModuleType.PlantOrnament] = typeof(PlantOrnament),

            [ListingModuleType.Land] = typeof(Land),
            [ListingModuleType.Apartment] = typeof(Apartment),
            [ListingModuleType.Shop] = typeof(Shop),

            [ListingModuleType.Rescue] = typeof(Rescue),
            [ListingModuleType.BloodRequest] = typeof(BloodRequest),
            [ListingModuleType.AskConsult] = typeof(AskConsult),

            [ListingModuleType.LostItem] = typeof(LostFoundPost),
            [ListingModuleType.FoundItem] = typeof(LostFoundPost)
        };
}
