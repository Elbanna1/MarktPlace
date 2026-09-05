using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class AnimalFormLookups
{
    public static bool TryApply(CreateAdFormLookupsDto lookups, string key)
    {
        switch (key)
        {
            case AdFormLookupKeys.LivestockBreeds:
                lookups.LivestockBreeds = LivestockCatalog.BreedOptions;
                return true;
            case AdFormLookupKeys.LivestockPurposes:
                lookups.LivestockPurposes = LivestockCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.LivestockAges:
                lookups.LivestockAges = LivestockCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.LivestockGenders:
                lookups.LivestockGenders = LivestockCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.LivestockHealthStatuses:
                lookups.LivestockHealthStatuses = LivestockCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.LivestockVaccinations:
                lookups.LivestockVaccinations = LivestockCatalog.VaccinationOptions;
                return true;
            case AdFormLookupKeys.LivestockProductions:
                lookups.LivestockProductions = LivestockCatalog.ProductionOptions;
                return true;

            case AdFormLookupKeys.SheepGoatBreeds:
                lookups.SheepGoatBreeds = SheepGoatCatalog.BreedOptions;
                return true;
            case AdFormLookupKeys.SheepGoatPurposes:
                lookups.SheepGoatPurposes = SheepGoatCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.SheepGoatAges:
                lookups.SheepGoatAges = SheepGoatCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.SheepGoatGenders:
                lookups.SheepGoatGenders = SheepGoatCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.SheepGoatHealthStatuses:
                lookups.SheepGoatHealthStatuses = SheepGoatCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.SheepGoatVaccinations:
                lookups.SheepGoatVaccinations = SheepGoatCatalog.VaccinationOptions;
                return true;

            case AdFormLookupKeys.HorseBreeds:
                lookups.HorseBreeds = HorseCatalog.BreedOptions;
                return true;
            case AdFormLookupKeys.HorsePurposes:
                lookups.HorsePurposes = HorseCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.HorseAges:
                lookups.HorseAges = HorseCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.HorseGenders:
                lookups.HorseGenders = HorseCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.HorseHealthStatuses:
                lookups.HorseHealthStatuses = HorseCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.HorseTrainingLevels:
                lookups.HorseTrainingLevels = HorseCatalog.TrainingLevelOptions;
                return true;
            case AdFormLookupKeys.HorseVaccinations:
                lookups.HorseVaccinations = HorseCatalog.VaccinationOptions;
                return true;

            case AdFormLookupKeys.CamelBreeds:
                lookups.CamelBreeds = CamelCatalog.BreedOptions;
                return true;
            case AdFormLookupKeys.CamelPurposes:
                lookups.CamelPurposes = CamelCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.CamelAges:
                lookups.CamelAges = CamelCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.CamelGenders:
                lookups.CamelGenders = CamelCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.CamelHealthStatuses:
                lookups.CamelHealthStatuses = CamelCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.CamelVaccinations:
                lookups.CamelVaccinations = CamelCatalog.VaccinationOptions;
                return true;

            case AdFormLookupKeys.BirdTypes:
                lookups.BirdTypes = BirdCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.BirdPurposes:
                lookups.BirdPurposes = BirdCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.BirdAges:
                lookups.BirdAges = BirdCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.BirdGenders:
                lookups.BirdGenders = BirdCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.BirdHealthStatuses:
                lookups.BirdHealthStatuses = BirdCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.BirdVaccinations:
                lookups.BirdVaccinations = BirdCatalog.VaccinationOptions;
                return true;

            case AdFormLookupKeys.PetBreeds:
                lookups.PetBreeds = PetCatalog.BreedOptions;
                return true;
            case AdFormLookupKeys.PetPurposes:
                lookups.PetPurposes = PetCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.PetAges:
                lookups.PetAges = PetCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.PetGenders:
                lookups.PetGenders = PetCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.PetHealthStatuses:
                lookups.PetHealthStatuses = PetCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.PetTrainingLevels:
                lookups.PetTrainingLevels = PetCatalog.TrainingLevelOptions;
                return true;
            case AdFormLookupKeys.PetVaccinations:
                lookups.PetVaccinations = PetCatalog.VaccinationOptions;
                return true;

            case AdFormLookupKeys.FishTypes:
                lookups.FishTypes = FishCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.FishPurposes:
                lookups.FishPurposes = FishCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.FishAges:
                lookups.FishAges = FishCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.FishHealthStatuses:
                lookups.FishHealthStatuses = FishCatalog.HealthStatusOptions;
                return true;

            case AdFormLookupKeys.BeeTypes:
                lookups.BeeTypes = BeeCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.BeePurposes:
                lookups.BeePurposes = BeeCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.BeeHealthStatuses:
                lookups.BeeHealthStatuses = BeeCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.BeeProductions:
                lookups.BeeProductions = BeeCatalog.ProductionOptions;
                return true;

            case AdFormLookupKeys.OtherAnimalTypes:
                lookups.OtherAnimalTypes = OtherAnimalCatalog.TypeOptions;
                return true;
            case AdFormLookupKeys.OtherAnimalPurposes:
                lookups.OtherAnimalPurposes = OtherAnimalCatalog.PurposeOptions;
                return true;
            case AdFormLookupKeys.OtherAnimalAges:
                lookups.OtherAnimalAges = OtherAnimalCatalog.AgeOptions;
                return true;
            case AdFormLookupKeys.OtherAnimalGenders:
                lookups.OtherAnimalGenders = OtherAnimalCatalog.GenderOptions;
                return true;
            case AdFormLookupKeys.OtherAnimalHealthStatuses:
                lookups.OtherAnimalHealthStatuses = OtherAnimalCatalog.HealthStatusOptions;
                return true;
            case AdFormLookupKeys.OtherAnimalVaccinations:
                lookups.OtherAnimalVaccinations = OtherAnimalCatalog.VaccinationOptions;
                return true;

            default:
                return false;
        }
    }
}
