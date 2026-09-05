using Shared.DTOs.Advertisements;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IBannerBookingService
{
    Task<IReadOnlyList<BannerPlacementDto>> GetPlacementsAsync(CancellationToken cancellationToken = default);

    Task<BannerAvailabilityDto> GetAvailabilityAsync(
        BannerAvailabilityQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerPaymentMethodDto>> GetPaymentMethodsAsync(
        CancellationToken cancellationToken = default);

    Task<BannerBookingSummaryDto> GetQuoteAsync(
        BannerBookingQuoteQuery query, CancellationToken cancellationToken = default);

    Task<BannerPreviewDto> PreviewImagesAsync(
        BannerLocation location,
        UploadImageModel? desktopImage,
        UploadImageModel? mobileImage,
        CancellationToken cancellationToken = default);

    IReadOnlyList<BannerRejectionReasonDto> GetRejectionReasons();

    IReadOnlyList<BannerBookingStatusDto> GetStatuses();

    Task<BannerBookingDto> CreateAsync(
        string userId,
        CreateBannerBookingRequest request,
        UploadImageModel? desktopImage,
        UploadImageModel? mobileImage,
        UploadImageModel? paymentProof,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerBookingListItemDto>> GetMyBookingsAsync(
        string userId, BannerBookingStatus? status = null, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> GetByIdAsync(
        Guid id, string userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> CancelAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PublishedBannerDto>> GetPublishedAsync(
        BannerLocation? location = null,
        int? categoryId = null,
        int? subCategoryId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PublishedBannerDto>> GetPublishedSliderAsync(
        BannerLocation location, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PublishedBannerDto>> GetPublishedSubCategoryAsync(
        int categoryId, int subCategoryId, CancellationToken cancellationToken = default);

    Task<PaginatedResult<BannerBookingListItemDto>> GetAllAsync(
        BannerBookingFilterParams filter, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> ApprovePaymentAsync(
        Guid id, string adminId, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> RejectPaymentAsync(
        Guid id, string adminId, RejectBannerPaymentRequest request, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> ApproveAsync(Guid id, string adminId, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> RejectAsync(
        Guid id, string adminId, RejectBannerBookingRequest request, CancellationToken cancellationToken = default);

    Task<BannerBookingDto> ExpireAsync(Guid id, string adminId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
