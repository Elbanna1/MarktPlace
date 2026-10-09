using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Sehhaty;

namespace Services.Mapping;

public class SehhatyMappingProfile : Profile
{
    public SehhatyMappingProfile()
    {
        MapListing<Doctor, DoctorImage, DoctorDto>()
            .ForMember(d => d.SpecialtyName,
                o => o.MapFrom(s => SehhatyCatalog.GetSpecialtyName(s.Specialty)))
            .ForMember(d => d.DisplaySpecialty,
                o => o.MapFrom(s => SehhatyCatalog.DisplaySpecialty(s.Specialty, s.OtherSpecialty)));

        MapListing<MedicalLaboratory, MedicalLaboratoryImage, MedicalLaboratoryDto>();
        MapListing<DentalLaboratory, DentalLaboratoryImage, DentalLaboratoryDto>();
        MapListing<RadiologyLaboratory, RadiologyLaboratoryImage, RadiologyLaboratoryDto>();
        MapListing<MedicalCenter, MedicalCenterImage, MedicalCenterDto>();
        MapListing<Hospital, HospitalImage, HospitalDto>();
    }

    private IMappingExpression<TListing, TDto> MapListing<TListing, TImage, TDto>()
        where TListing : SehhatyListing<TImage>
        where TImage : SehhatyListingImage
        where TDto : SehhatyListingDto
    {
        CreateMap<TImage, SehhatyImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        return CreateMap<TListing, TDto>()
            .ForMember(d => d.ApprovedAt, o => o.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.OwnerName, o => o.Ignore())
            .ForMember(d => d.ListingType, o => o.Ignore())
            .ForMember(d => d.ListingTypeName, o => o.Ignore())
            .ForMember(d => d.CategoryId, o => o.Ignore())
            .ForMember(d => d.CategoryName, o => o.Ignore())
            .ForMember(d => d.SubCategoryId, o => o.Ignore())
            .ForMember(d => d.SubCategoryName, o => o.Ignore())
            .ForMember(d => d.ShareUrl, o => o.Ignore())
            .ForMember(d => d.PrimaryImageUrl, o => o.Ignore())
            .ForMember(d => d.Views, o => o.Ignore())
            .ForMember(d => d.AverageRating, o => o.Ignore())
            .ForMember(d => d.RatingsCount, o => o.Ignore())
            .ForMember(d => d.IsFavorite, o => o.Ignore())
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)));
    }
}
