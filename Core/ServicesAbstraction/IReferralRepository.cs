using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.DTOs.Referrals;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IReferralRepository
{
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> FindUserByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> FindUserAsync(string userId, CancellationToken cancellationToken = default);

    Task AddAsync(Referral referral, CancellationToken cancellationToken = default);

    Task<Referral?> FindByReferredUserAsync(
        string referredUserId, CancellationToken cancellationToken = default);

    Task<Referral?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<ReferralStatus, int>> CountByStatusAsync(
        string referrerUserId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<ReferralStatus, int>> CountAllByStatusAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountSinceAsync(
        string referrerUserId, DateTime fromUtc, CancellationToken cancellationToken = default);

    Task<int> CountAllSinceAsync(DateTime fromUtc, CancellationToken cancellationToken = default);

    Task<int> CountDistinctReferrersAsync(CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ReferredUserDto> Items, int Total)> GetReferredUsersAsync(
        string referrerUserId, MyReferralFilterParams filter, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AdminReferralDto> Items, int Total)> GetForAdminAsync(
        AdminReferralFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminReferralDto?> GetAdminReferralAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminTopReferrerDto>> GetTopReferrersAsync(
        int count, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AdminReferrerDto> Items, int Total)> GetReferrersForAdminAsync(
        AdminReferrerFilterParams filter, CancellationToken cancellationToken = default);

    Task AddLinkEventAsync(ReferralLinkEvent linkEvent, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountLinkEventsAsync(
        string referrerUserId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<ReferralLinkEventType, int>> CountAllLinkEventsAsync(
        CancellationToken cancellationToken = default);
}
