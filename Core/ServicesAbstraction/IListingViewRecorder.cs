using Shared.Enums;

namespace ServicesAbstraction;

public readonly record struct ListingViewEvent(
    ListingModuleType Type,
    Guid ListingId,
    string? ViewerUserId,
    string? IpAddress,
    bool IncrementModuleCounter,
    DateTime OccurredAtUtc);

public interface IListingViewRecorder
{
    bool Record(ListingViewEvent view);
}
