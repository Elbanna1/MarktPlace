using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Suppliers;

namespace Services.Mapping;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<SupplierImage, SupplierImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<SupplierSpecializationLookup, SupplierSpecializationDto>();

        CreateMap<Supplier, SupplierListItemDto>()
            .ForMember(d => d.SupplierTypeName,
                o => o.MapFrom(s => SupplierCatalog.GetName(s.SupplierType)))
            .ForMember(d => d.SupplierTypeGroup,
                o => o.MapFrom(s => SupplierCatalog.GetGroup(s.SupplierType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Supplier, SupplierDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.SupplierTypeName,
                o => o.MapFrom(s => SupplierCatalog.GetName(s.SupplierType)))
            .ForMember(d => d.SupplierTypeGroup,
                o => o.MapFrom(s => SupplierCatalog.GetGroup(s.SupplierType)));

        CreateMap<CreateSupplierRequest, Supplier>()
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
