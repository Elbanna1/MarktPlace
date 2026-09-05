using Domain.Entities;
using Shared.DTOs.Charity;

namespace ServicesAbstraction;

public interface ICharityRepository<TListing, TFilter>
    where TListing : CharityListing
    where TFilter : CharityFilterParamsBase
{
    Task<TListing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false);

    Task<TListing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<TListing> Items, int TotalCount)> GetPagedAsync(
        TFilter filter, CancellationToken cancellationToken = default);

    Task AddAsync(TListing listing, CancellationToken cancellationToken = default);

    void Update(TListing listing);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRescueRepository : ICharityRepository<Rescue, RescueFilterParams>
{
    Task AddImagesAsync(IEnumerable<RescueImage> images, CancellationToken cancellationToken = default);
    void RemoveImage(RescueImage image);
}

public interface IBloodRequestRepository : ICharityRepository<BloodRequest, BloodRequestFilterParams>
{
    Task AddImagesAsync(IEnumerable<BloodRequestImage> images, CancellationToken cancellationToken = default);
    void RemoveImage(BloodRequestImage image);
}

public interface IAskConsultRepository : ICharityRepository<AskConsult, AskConsultFilterParams>
{
    Task AddImagesAsync(IEnumerable<AskConsultImage> images, CancellationToken cancellationToken = default);
    void RemoveImage(AskConsultImage image);

    Task<AskConsultLike?> GetLikeAsync(Guid listingId, string userId, CancellationToken cancellationToken = default);

    Task AddLikeAsync(AskConsultLike like, CancellationToken cancellationToken = default);

    void RemoveLike(AskConsultLike like);

    Task<IReadOnlySet<Guid>> GetLikedIdsAsync(
        IReadOnlyCollection<Guid> listingIds, string userId, CancellationToken cancellationToken = default);

    Task AddCommentAsync(AskConsultComment comment, CancellationToken cancellationToken = default);

    Task<AskConsultComment?> GetCommentAsync(Guid commentId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AskConsultComment> Items, int TotalCount)> GetCommentsAsync(
        Guid listingId, int pageIndex, int pageSize, CancellationToken cancellationToken = default);

    void RemoveComment(AskConsultComment comment);
}
