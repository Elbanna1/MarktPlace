using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.Constants;
using Shared.DTOs.GovernorateGuide;

namespace Persistence.Repositories;

public sealed class CommercialShopRepository
    : DirectoryRepository<CommercialShop, CommercialShopImage, CommercialShopFilterParams>
{
    public CommercialShopRepository(AppDbContext context) : base(context, GovernorateGuideCatalog.CommercialShops) { }

    protected override IQueryable<CommercialShop> Filter(IQueryable<CommercialShop> query, CommercialShopFilterParams filter) =>
        filter.ActivityType is { } activity ? query.Where(x => x.ActivityType == activity) : query;

    protected override IQueryable<CommercialShop> ApplySearch(IQueryable<CommercialShop> query, string pattern) =>
        query.Where(x =>
            EF.Functions.Like(x.Name, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Title, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Description, pattern, EscapeCharacter) ||
            EF.Functions.Like(x.Address, pattern, EscapeCharacter) ||
            (x.OtherActivityType != null && EF.Functions.Like(x.OtherActivityType, pattern, EscapeCharacter)));
}

public sealed class GeneralServiceRepository
    : DirectoryRepository<GeneralService, GeneralServiceImage, GeneralServiceFilterParams>
{
    public GeneralServiceRepository(AppDbContext context) : base(context, GovernorateGuideCatalog.GeneralServices) { }

    protected override IQueryable<GeneralService> Filter(IQueryable<GeneralService> query, GeneralServiceFilterParams filter)
    {
        if (filter.ServiceType is { } type)
            query = query.Where(x => x.ServiceType == type);

        if (filter.ServiceSubtype is { } subtype)
            query = query.Where(x => x.ServiceSubtype == subtype);

        return query;
    }
}

public sealed class ResortHotelAmusementRepository
    : DirectoryRepository<ResortHotelAmusement, ResortHotelAmusementImage, ResortHotelAmusementFilterParams>
{
    public ResortHotelAmusementRepository(AppDbContext context)
        : base(context, GovernorateGuideCatalog.ResortsHotelsAndAmusement) { }
}
