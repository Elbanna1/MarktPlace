using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.DTOs.RealEstate;

namespace Persistence.Repositories;

internal static class RealEstateLookupReader
{
    public static async Task<IReadOnlyList<RealEstateLookupItemDto>> ReadAsync<TLookup>(
        AppDbContext context, CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup =>
        await context.Set<TLookup>()
            .AsNoTracking()
            .OrderBy(x => EF.Property<int>(x, nameof(IRealEstateLookup.Id)))
            .Select(x => new RealEstateLookupItemDto
            {
                Id = EF.Property<int>(x, nameof(IRealEstateLookup.Id)),
                Name = EF.Property<string>(x, nameof(IRealEstateLookup.Name)),
                NameEn = EF.Property<string>(x, nameof(IRealEstateLookup.NameEn))
            })
            .ToListAsync(cancellationToken);
}
