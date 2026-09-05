using System.Text.Json.Serialization;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.DTOs.Antiques;
using Shared.DTOs.Clothing;
using Shared.DTOs.FruitVegetableMerchants;
using Shared.DTOs.HomeFurnishing;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.DTOs.OnlineShopping;
using Shared.DTOs.RealEstate;
using Shared.DTOs.Suppliers;
using Shared.DTOs.WholesaleTraders;

namespace Shared.DTOs.Lookups.Forms;

public class CreateAdFormLookupsDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CategoryTreeSubCategoryDto>? SubCategories { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ListingTypeDto>? ListingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<FeatureDto>? Features { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<GovernorateDto>? Governorates { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CenterDto>? Centers { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<WorkshopTypeDto>? WorkshopTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<SpecializationDto>? Specializations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ExperienceLevelDto>? ExperienceLevels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ProductionSpecialtyDto>? ProductionSpecialties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<FarmTypeDto>? FarmTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AvailabilitySeasonDto>? Seasons { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<FarmingMethodDto>? FarmingMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CompanyFieldDto>? CompanyFields { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<SupplierSpecializationDto>? SupplierTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<WholesaleTradeTypeDto>? TradeTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<WholesaleSaleTypeDto>? WholesaleSaleTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<MerchantSaleTypeDto>? MerchantSaleTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<JobFieldDto>? JobFields { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<JobExperienceLevelDto>? JobExperienceLevels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<EducationLevelDto>? EducationLevels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<WorkTypeDto>? WorkTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<SalaryTypeDto>? SalaryTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockBreeds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? LivestockProductions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatBreeds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? SheepGoatVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseBreeds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorsePurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseTrainingLevels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? HorseVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelBreeds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? CamelVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BirdVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetBreeds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetTrainingLevels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? PetVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? FishTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? FishPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? FishAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? FishHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BeeTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BeePurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BeeHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? BeeProductions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalPurposes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalGenders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalHealthStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AnimalLookupItemDto>? OtherAnimalVaccinations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? DecorAntiqueItemTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? DecorAntiqueMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? DecorAntiqueConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? DecorAntiqueOriginalities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? AntiqueTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? AntiqueMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? AntiqueConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? AntiqueWorkingStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? AntiqueOriginalities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? PaintingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? PaintingMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? PaintingOriginalities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? HandmadeTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? HandmadeColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? CoinStampItemTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? CoinStampMetals { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AntiqueLookupItemDto>? CoinStampConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingSizes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? MenClothingSellingMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingSizes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? WomenClothingSellingMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingSizes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ClothingLookupItemDto>? KidsClothingSellingMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? AccessoryTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? AccessoryCategories { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? AccessoryMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? AccessoryColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? CosmeticSections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? CosmeticSuitableFor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? HomeKitchenSections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? HomeKitchenMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? HomeKitchenColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? ShoppingElectronicSections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? ShoppingElectronicCompatibilities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? ShoppingElectronicConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? ShoppingElectronicWarranties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? GiftToyTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? GiftToySuitableFor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? HomemadeFoodSections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OnlineShoppingLookupItemDto>? HomemadeFoodDeliveryAreas { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnitureTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnitureMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnitureColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnitureConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnishingCurtainProductTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnishingCurtainSizes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnishingCurtainMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? FurnishingCurtainColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? LightingDecorProductTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? LightingDecorMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? LightingDecorColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? LightingDecorLightTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? KitchenToolProductTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? KitchenToolMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? KitchenToolColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? HomeApplianceDeviceTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? HomeApplianceBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? HomeApplianceConditions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? HomeApplianceWarranties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? HomeApplianceColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? BathroomSupplyProductTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? BathroomSupplyMaterials { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? BathroomSupplyColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? PlantOrnamentProductTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HomeFurnishingLookupItemDto>? PlantOrnamentSuitableFor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? RealEstateListingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? RealEstateProjects { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandAreaUnits { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandFacadesCounts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandDirections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandRoadTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandLegalStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandReconciliationForms { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandOwnershipDocuments { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandUtilities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandRentTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandMinimumRentPeriods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandRentInclusions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandContractDurations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandExchangeTargets { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandHarvestSeasons { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandSoilTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandIrrigationSources { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandQualityCertificates { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandExistingBuildingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? LandBuildingCompletionRatios { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentOwnershipTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentReceptionPieces { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentFloorTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentFurnishedStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentFinishingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentPropertyAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentDirections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentViewTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentLegalStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentReconciliationForms { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentOwnershipDocuments { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentFeatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentPaymentMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentInstallmentProviders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentRentTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentRentInclusions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentSuitableFor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ApartmentExchangeTargets { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopSuitableActivities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopFloorTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopFacadesCounts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopFacadeDirections { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopFinishingTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopPropertyAges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopEntrancesCounts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopLegalStatuses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopLicenseTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopReconciliationForms { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopOwnershipDocuments { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopUtilities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopPaymentMethods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopInstallmentProviders { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopRentTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopRentInclusions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopRentSuitableActivities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<RealEstateLookupItemDto>? ShopExchangeTargets { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? CarBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? MotorcycleBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? EquipmentBrands { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? CarModels { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? CarColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? TaxiColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? MotorcycleColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarNameOptionDto>? EquipmentColors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarFeatureOptionDto>? PrivateCarFeatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarFeatureOptionDto>? TaxiFeatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarFeatureOptionDto>? MotorcycleFeatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarFeatureOptionDto>? EquipmentFeatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? CarEngineCapacities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? TaxiEngineCapacities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? MotorcycleEngineCapacities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? CarDoorCounts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? CarSeatCounts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CarLookupItemDto>? TaxiPassengerCounts { get; set; }
}

public static class AdFormLookupKeys
{
    public const string SubCategories = "subCategories";
    public const string ListingTypes = "listingTypes";
    public const string Features = "features";
    public const string Governorates = "governorates";
    public const string Centers = "centers";
    public const string WorkshopTypes = "workshopTypes";
    public const string Specializations = "specializations";
    public const string ExperienceLevels = "experienceLevels";

    public const string ProductionSpecialties = "productionSpecialties";
    public const string FarmTypes = "farmTypes";
    public const string Seasons = "seasons";
    public const string FarmingMethods = "farmingMethods";
    public const string CompanyFields = "companyFields";
    public const string SupplierTypes = "supplierTypes";
    public const string TradeTypes = "tradeTypes";
    public const string WholesaleSaleTypes = "wholesaleSaleTypes";
    public const string MerchantSaleTypes = "merchantSaleTypes";

    public const string JobFields = "jobFields";
    public const string JobExperienceLevels = "jobExperienceLevels";
    public const string EducationLevels = "educationLevels";
    public const string WorkTypes = "workTypes";
    public const string SalaryTypes = "salaryTypes";

    public const string LivestockBreeds = "livestockBreeds";
    public const string LivestockPurposes = "livestockPurposes";
    public const string LivestockAges = "livestockAges";
    public const string LivestockGenders = "livestockGenders";
    public const string LivestockHealthStatuses = "livestockHealthStatuses";
    public const string LivestockVaccinations = "livestockVaccinations";
    public const string LivestockProductions = "livestockProductions";

    public const string SheepGoatBreeds = "sheepGoatBreeds";
    public const string SheepGoatPurposes = "sheepGoatPurposes";
    public const string SheepGoatAges = "sheepGoatAges";
    public const string SheepGoatGenders = "sheepGoatGenders";
    public const string SheepGoatHealthStatuses = "sheepGoatHealthStatuses";
    public const string SheepGoatVaccinations = "sheepGoatVaccinations";

    public const string HorseBreeds = "horseBreeds";
    public const string HorsePurposes = "horsePurposes";
    public const string HorseAges = "horseAges";
    public const string HorseGenders = "horseGenders";
    public const string HorseHealthStatuses = "horseHealthStatuses";
    public const string HorseTrainingLevels = "horseTrainingLevels";
    public const string HorseVaccinations = "horseVaccinations";

    public const string CamelBreeds = "camelBreeds";
    public const string CamelPurposes = "camelPurposes";
    public const string CamelAges = "camelAges";
    public const string CamelGenders = "camelGenders";
    public const string CamelHealthStatuses = "camelHealthStatuses";
    public const string CamelVaccinations = "camelVaccinations";

    public const string BirdTypes = "birdTypes";
    public const string BirdPurposes = "birdPurposes";
    public const string BirdAges = "birdAges";
    public const string BirdGenders = "birdGenders";
    public const string BirdHealthStatuses = "birdHealthStatuses";
    public const string BirdVaccinations = "birdVaccinations";

    public const string PetBreeds = "petBreeds";
    public const string PetPurposes = "petPurposes";
    public const string PetAges = "petAges";
    public const string PetGenders = "petGenders";
    public const string PetHealthStatuses = "petHealthStatuses";
    public const string PetTrainingLevels = "petTrainingLevels";
    public const string PetVaccinations = "petVaccinations";

    public const string FishTypes = "fishTypes";
    public const string FishPurposes = "fishPurposes";
    public const string FishAges = "fishAges";
    public const string FishHealthStatuses = "fishHealthStatuses";

    public const string BeeTypes = "beeTypes";
    public const string BeePurposes = "beePurposes";
    public const string BeeHealthStatuses = "beeHealthStatuses";
    public const string BeeProductions = "beeProductions";

    public const string OtherAnimalTypes = "otherAnimalTypes";
    public const string OtherAnimalPurposes = "otherAnimalPurposes";
    public const string OtherAnimalAges = "otherAnimalAges";
    public const string OtherAnimalGenders = "otherAnimalGenders";
    public const string OtherAnimalHealthStatuses = "otherAnimalHealthStatuses";
    public const string OtherAnimalVaccinations = "otherAnimalVaccinations";

    public const string DecorAntiqueItemTypes = "decorAntiqueItemTypes";
    public const string DecorAntiqueMaterials = "decorAntiqueMaterials";
    public const string DecorAntiqueConditions = "decorAntiqueConditions";
    public const string DecorAntiqueOriginalities = "decorAntiqueOriginalities";

    public const string AntiqueTypes = "antiqueTypes";
    public const string AntiqueMaterials = "antiqueMaterials";
    public const string AntiqueConditions = "antiqueConditions";
    public const string AntiqueWorkingStatuses = "antiqueWorkingStatuses";
    public const string AntiqueOriginalities = "antiqueOriginalities";

    public const string PaintingTypes = "paintingTypes";
    public const string PaintingMaterials = "paintingMaterials";
    public const string PaintingOriginalities = "paintingOriginalities";

    public const string HandmadeTypes = "handmadeTypes";
    public const string HandmadeColors = "handmadeColors";

    public const string CoinStampItemTypes = "coinStampItemTypes";
    public const string CoinStampMetals = "coinStampMetals";
    public const string CoinStampConditions = "coinStampConditions";

    public const string MenClothingTypes = "menClothingTypes";
    public const string MenClothingBrands = "menClothingBrands";
    public const string MenClothingSizes = "menClothingSizes";
    public const string MenClothingColors = "menClothingColors";
    public const string MenClothingConditions = "menClothingConditions";
    public const string MenClothingSellingMethods = "menClothingSellingMethods";

    public const string WomenClothingTypes = "womenClothingTypes";
    public const string WomenClothingBrands = "womenClothingBrands";
    public const string WomenClothingSizes = "womenClothingSizes";
    public const string WomenClothingColors = "womenClothingColors";
    public const string WomenClothingConditions = "womenClothingConditions";
    public const string WomenClothingSellingMethods = "womenClothingSellingMethods";

    public const string KidsClothingTypes = "kidsClothingTypes";
    public const string KidsClothingBrands = "kidsClothingBrands";
    public const string KidsClothingSizes = "kidsClothingSizes";
    public const string KidsClothingColors = "kidsClothingColors";
    public const string KidsClothingConditions = "kidsClothingConditions";
    public const string KidsClothingSellingMethods = "kidsClothingSellingMethods";

    public const string AccessoryTypes = "accessoryTypes";
    public const string AccessoryCategories = "accessoryCategories";
    public const string AccessoryMaterials = "accessoryMaterials";
    public const string AccessoryColors = "accessoryColors";

    public const string CosmeticSections = "cosmeticSections";
    public const string CosmeticSuitableFor = "cosmeticSuitableFor";

    public const string HomeKitchenSections = "homeKitchenSections";
    public const string HomeKitchenMaterials = "homeKitchenMaterials";
    public const string HomeKitchenColors = "homeKitchenColors";

    public const string ShoppingElectronicSections = "shoppingElectronicSections";
    public const string ShoppingElectronicCompatibilities = "shoppingElectronicCompatibilities";
    public const string ShoppingElectronicConditions = "shoppingElectronicConditions";
    public const string ShoppingElectronicWarranties = "shoppingElectronicWarranties";

    public const string GiftToyTypes = "giftToyTypes";
    public const string GiftToySuitableFor = "giftToySuitableFor";

    public const string HomemadeFoodSections = "homemadeFoodSections";
    public const string HomemadeFoodDeliveryAreas = "homemadeFoodDeliveryAreas";

    public const string FurnitureTypes = "furnitureTypes";
    public const string FurnitureMaterials = "furnitureMaterials";
    public const string FurnitureColors = "furnitureColors";
    public const string FurnitureConditions = "furnitureConditions";

    public const string FurnishingCurtainProductTypes = "furnishingCurtainProductTypes";
    public const string FurnishingCurtainSizes = "furnishingCurtainSizes";
    public const string FurnishingCurtainMaterials = "furnishingCurtainMaterials";
    public const string FurnishingCurtainColors = "furnishingCurtainColors";

    public const string LightingDecorProductTypes = "lightingDecorProductTypes";
    public const string LightingDecorMaterials = "lightingDecorMaterials";
    public const string LightingDecorColors = "lightingDecorColors";
    public const string LightingDecorLightTypes = "lightingDecorLightTypes";

    public const string KitchenToolProductTypes = "kitchenToolProductTypes";
    public const string KitchenToolMaterials = "kitchenToolMaterials";
    public const string KitchenToolColors = "kitchenToolColors";

    public const string HomeApplianceDeviceTypes = "homeApplianceDeviceTypes";
    public const string HomeApplianceBrands = "homeApplianceBrands";
    public const string HomeApplianceConditions = "homeApplianceConditions";
    public const string HomeApplianceWarranties = "homeApplianceWarranties";
    public const string HomeApplianceColors = "homeApplianceColors";

    public const string BathroomSupplyProductTypes = "bathroomSupplyProductTypes";
    public const string BathroomSupplyMaterials = "bathroomSupplyMaterials";
    public const string BathroomSupplyColors = "bathroomSupplyColors";

    public const string PlantOrnamentProductTypes = "plantOrnamentProductTypes";
    public const string PlantOrnamentSuitableFor = "plantOrnamentSuitableFor";

    public const string RealEstateListingTypes = "realEstateListingTypes";
    public const string RealEstateProjects = "realEstateProjects";

    public const string LandTypes = "landTypes";
    public const string LandAreaUnits = "landAreaUnits";
    public const string LandFacadesCounts = "landFacadesCounts";
    public const string LandDirections = "landDirections";
    public const string LandRoadTypes = "landRoadTypes";
    public const string LandLegalStatuses = "landLegalStatuses";
    public const string LandReconciliationForms = "landReconciliationForms";
    public const string LandOwnershipDocuments = "landOwnershipDocuments";
    public const string LandUtilities = "landUtilities";
    public const string LandRentTypes = "landRentTypes";
    public const string LandMinimumRentPeriods = "landMinimumRentPeriods";
    public const string LandRentInclusions = "landRentInclusions";
    public const string LandContractDurations = "landContractDurations";
    public const string LandExchangeTargets = "landExchangeTargets";
    public const string LandHarvestSeasons = "landHarvestSeasons";
    public const string LandSoilTypes = "landSoilTypes";
    public const string LandIrrigationSources = "landIrrigationSources";
    public const string LandQualityCertificates = "landQualityCertificates";
    public const string LandExistingBuildingTypes = "landExistingBuildingTypes";
    public const string LandBuildingCompletionRatios = "landBuildingCompletionRatios";

    public const string ApartmentTypes = "apartmentTypes";
    public const string ApartmentOwnershipTypes = "apartmentOwnershipTypes";
    public const string ApartmentReceptionPieces = "apartmentReceptionPieces";
    public const string ApartmentFloorTypes = "apartmentFloorTypes";
    public const string ApartmentFurnishedStatuses = "apartmentFurnishedStatuses";
    public const string ApartmentFinishingTypes = "apartmentFinishingTypes";
    public const string ApartmentPropertyAges = "apartmentPropertyAges";
    public const string ApartmentDirections = "apartmentDirections";
    public const string ApartmentViewTypes = "apartmentViewTypes";
    public const string ApartmentLegalStatuses = "apartmentLegalStatuses";
    public const string ApartmentReconciliationForms = "apartmentReconciliationForms";
    public const string ApartmentOwnershipDocuments = "apartmentOwnershipDocuments";
    public const string ApartmentFeatures = "apartmentFeatures";
    public const string ApartmentPaymentMethods = "apartmentPaymentMethods";
    public const string ApartmentInstallmentProviders = "apartmentInstallmentProviders";
    public const string ApartmentRentTypes = "apartmentRentTypes";
    public const string ApartmentRentInclusions = "apartmentRentInclusions";
    public const string ApartmentSuitableFor = "apartmentSuitableFor";
    public const string ApartmentExchangeTargets = "apartmentExchangeTargets";

    public const string ShopSuitableActivities = "shopSuitableActivities";
    public const string ShopFloorTypes = "shopFloorTypes";
    public const string ShopFacadesCounts = "shopFacadesCounts";
    public const string ShopFacadeDirections = "shopFacadeDirections";
    public const string ShopFinishingTypes = "shopFinishingTypes";
    public const string ShopPropertyAges = "shopPropertyAges";
    public const string ShopEntrancesCounts = "shopEntrancesCounts";
    public const string ShopLegalStatuses = "shopLegalStatuses";
    public const string ShopLicenseTypes = "shopLicenseTypes";
    public const string ShopReconciliationForms = "shopReconciliationForms";
    public const string ShopOwnershipDocuments = "shopOwnershipDocuments";
    public const string ShopUtilities = "shopUtilities";
    public const string ShopPaymentMethods = "shopPaymentMethods";
    public const string ShopInstallmentProviders = "shopInstallmentProviders";
    public const string ShopRentTypes = "shopRentTypes";
    public const string ShopRentInclusions = "shopRentInclusions";
    public const string ShopRentSuitableActivities = "shopRentSuitableActivities";
    public const string ShopExchangeTargets = "shopExchangeTargets";

    public const string CarBrands = "carBrands";
    public const string MotorcycleBrands = "motorcycleBrands";
    public const string EquipmentBrands = "equipmentBrands";
    public const string CarModels = "carModels";

    public const string CarColors = "carColors";
    public const string TaxiColors = "taxiColors";
    public const string MotorcycleColors = "motorcycleColors";
    public const string EquipmentColors = "equipmentColors";

    public const string PrivateCarFeatures = "privateCarFeatures";
    public const string TaxiFeatures = "taxiFeatures";
    public const string MotorcycleFeatures = "motorcycleFeatures";
    public const string EquipmentFeatures = "equipmentFeatures";

    public const string CarEngineCapacities = "carEngineCapacities";
    public const string TaxiEngineCapacities = "taxiEngineCapacities";
    public const string MotorcycleEngineCapacities = "motorcycleEngineCapacities";
    public const string CarDoorCounts = "carDoorCounts";
    public const string CarSeatCounts = "carSeatCounts";
    public const string TaxiPassengerCounts = "taxiPassengerCounts";
}
