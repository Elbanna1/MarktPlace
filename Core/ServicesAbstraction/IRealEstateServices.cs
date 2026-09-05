using Shared.DTOs.Advertisements;
using Shared.DTOs.RealEstate;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ILandService
{
    Task<LandDetailsDto> CreateAsync(
        string userId, CreateLandRequest request,
        IReadOnlyList<UploadImageModel> images, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<LandDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateLandRequest request,
        IReadOnlyList<UploadImageModel> newImages, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<LandListItemDto>> GetListAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default);

    Task<LandDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LandListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LandListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LandListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<LandDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderRealEstateImagesRequest request, CancellationToken cancellationToken = default);

    Task<LandDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync(
        string lookupKey, CancellationToken cancellationToken = default);
}

public interface IApartmentService
{
    Task<ApartmentDetailsDto> CreateAsync(
        string userId, CreateApartmentRequest request,
        IReadOnlyList<UploadImageModel> images, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<ApartmentDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateApartmentRequest request,
        IReadOnlyList<UploadImageModel> newImages, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<ApartmentListItemDto>> GetListAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default);

    Task<ApartmentDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApartmentListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApartmentListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApartmentListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<ApartmentDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderRealEstateImagesRequest request, CancellationToken cancellationToken = default);

    Task<ApartmentDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync(
        string lookupKey, CancellationToken cancellationToken = default);
}

public interface IShopService
{
    Task<ShopDetailsDto> CreateAsync(
        string userId, CreateShopRequest request,
        IReadOnlyList<UploadImageModel> images, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<ShopDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateShopRequest request,
        IReadOnlyList<UploadImageModel> newImages, UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<ShopListItemDto>> GetListAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default);

    Task<ShopDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShopListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShopListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShopListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<ShopDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderRealEstateImagesRequest request, CancellationToken cancellationToken = default);

    Task<ShopDetailsDto> SetPromotionAsync(
        Guid id, PromoteRealEstateRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync(
        string lookupKey, CancellationToken cancellationToken = default);
}
