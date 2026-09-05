using Shared.DTOs.Notifications;
using Shared.Enums;

namespace ServicesAbstraction;

public interface INotificationInterestService
{
    Task<NotificationInterestsDto> GetMineAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<NotificationInterestOptionsDto> GetOptionsAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<NotificationInterestDto> AddAsync(
        string userId, AddNotificationInterestRequest request, CancellationToken cancellationToken = default);

    Task<NotificationInterestsDto> ReplaceAsync(
        string userId, ReplaceNotificationInterestsRequest request,
        CancellationToken cancellationToken = default);

    Task<NotificationInterestDto> SetEnabledAsync(
        string userId, Guid interestId, bool isEnabled, CancellationToken cancellationToken = default);

    Task RemoveAsync(string userId, Guid interestId, CancellationToken cancellationToken = default);

    Task<NotificationPreferencesDto> GetPreferencesAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<NotificationPreferencesDto> SetPreferencesAsync(
        string userId, UpdateNotificationPreferencesRequest request,
        CancellationToken cancellationToken = default);
}

public interface IListingInterestNotifier
{
    Task<int> AnnounceAsync(
        ListingModuleType listingType, Guid listingId, CancellationToken cancellationToken = default);
}
