using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Enums;

namespace Persistence.Repositories;

public class AccountClosureRepository : IAccountClosureRepository
{
    public static readonly IReadOnlyList<string> OwnerProperties =
        [ModerationQueryExtensions.OwnerUserIdProperty, "OwnerId"];

    private static readonly MethodInfo HideMethod =
        typeof(AccountClosureRepository)
            .GetMethod(nameof(HideAsync), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private readonly AppDbContext _context;

    public AccountClosureRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AccountClosureResult> CloseAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var hidden = 0;

        foreach (var (clrType, ownerProperty) in OwnedListingTypes())
        {
            var task = (Task<int>)HideMethod
                .MakeGenericMethod(clrType)
                .Invoke(this, [userId, ownerProperty, cancellationToken])!;

            hidden += await task;
        }

        var interests = await _context.UserNotificationInterests
            .Where(interest => interest.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return new AccountClosureResult(hidden, interests);
    }

    private IEnumerable<(Type ClrType, string OwnerProperty)> OwnedListingTypes() =>
        _context.Model
            .GetEntityTypes()
            .Where(entityType =>
                !entityType.IsOwned() &&
                entityType.ClrType is { IsClass: true, IsAbstract: false } &&
                typeof(IModeratedListing).IsAssignableFrom(entityType.ClrType))
            .Select(entityType => (
                entityType.ClrType,
                OwnerProperty: OwnerProperties.FirstOrDefault(
                    name => entityType.FindProperty(name) is not null)))
            .Where(candidate => candidate.OwnerProperty is not null)
            .Select(candidate => (candidate.ClrType, candidate.OwnerProperty!))
            .Distinct();

    private Task<int> HideAsync<TEntity>(
        string userId, string ownerProperty, CancellationToken cancellationToken)
        where TEntity : class, IModeratedListing =>
        _context.Set<TEntity>()
            .IncludingUnmoderated()
            .Where(entity =>
                EF.Property<string>(entity, ownerProperty) == userId &&
                (entity.ModerationStatus == ModerationStatus.Approved ||
                 entity.ModerationStatus == ModerationStatus.Pending))
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(entity => entity.ModerationStatus, ModerationStatus.Suspended),
                cancellationToken);
}
