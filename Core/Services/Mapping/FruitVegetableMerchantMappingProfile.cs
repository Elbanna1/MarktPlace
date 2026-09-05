using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.FruitVegetableMerchants;

namespace Services.Mapping;

public class FruitVegetableMerchantMappingProfile : Profile
{
    public FruitVegetableMerchantMappingProfile()
    {
        CreateMap<FruitVegetableMerchantImage, FruitVegetableMerchantImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<MerchantSaleTypeLookup, MerchantSaleTypeDto>();

        CreateMap<FruitVegetableMerchant, FruitVegetableMerchantListItemDto>()
            .ForMember(d => d.SaleTypeName,
                o => o.MapFrom(s => FruitVegetableMerchantCatalog.GetSaleTypeName(s.SaleType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<FruitVegetableMerchant, FruitVegetableMerchantDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SaleTypeName,
                o => o.MapFrom(s => FruitVegetableMerchantCatalog.GetSaleTypeName(s.SaleType)));

        CreateMap<CreateFruitVegetableMerchantRequest, FruitVegetableMerchant>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
