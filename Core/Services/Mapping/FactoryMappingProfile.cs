using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Factories;

namespace Services.Mapping;

public class FactoryMappingProfile : Profile
{
    public FactoryMappingProfile()
    {
        CreateMap<FactoryImage, FactoryImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<ProductionSpecialtyLookup, ProductionSpecialtyOptionDto>();

        CreateMap<Factory, FactoryListItemDto>()
            .ForMember(d => d.ProductionSpecialtyName,
                o => o.MapFrom(s => BusinessCatalog.GetProductionSpecialtyName(s.ProductionSpecialty)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(s =>
                s.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<Factory, FactoryDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.ProductionSpecialtyName,
                o => o.MapFrom(s => BusinessCatalog.GetProductionSpecialtyName(s.ProductionSpecialty)));

        CreateMap<CreateFactoryRequest, Factory>()
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
