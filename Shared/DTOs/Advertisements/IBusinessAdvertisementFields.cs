using Shared.Enums;

namespace Shared.DTOs.Advertisements;

public interface IBusinessAdvertisementFields
{
    int SubCategoryId { get; }

    string? BusinessName { get; }

    string? Address { get; }
    string? GoogleMapsUrl { get; }
    string? WhatsApp { get; }
    string? Email { get; }

    ProductionSpecialty? ProductionSpecialty { get; }
    string? OtherProductionSpecialty { get; }

    FarmType? FarmType { get; }
    string? OtherFarmType { get; }
    FarmingMethod? FarmingMethod { get; }
    AvailabilitySeason? AvailabilitySeason { get; }

    CompanyField? CompanyField { get; }
    string? OtherCompanyField { get; }

    SupplierType? SupplierType { get; }
    string? OtherSupplierType { get; }

    TradeType? TradeType { get; }
    string? OtherTradeType { get; }

    SaleType? SaleType { get; }
}
