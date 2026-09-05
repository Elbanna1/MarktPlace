using Domain.Entities;
using Shared.DTOs.Feedback;

namespace ServicesAbstraction;

public interface IFeedbackRepository
{
    Task<Feedback?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Feedback> Items, int TotalCount)> GetPagedAsync(
        string? ownerId, FeedbackFilterParams filter, CancellationToken cancellationToken = default);

    Task AddAsync(Feedback feedback);

    void Update(Feedback feedback);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
