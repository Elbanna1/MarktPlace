using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.JobOpportunities;

namespace Services.Mapping;

public class JobOpportunityMappingProfile : Profile
{
    public JobOpportunityMappingProfile()
    {
        CreateMap<JobOpportunityImage, JobOpportunityImageDto>()
            .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<WorkTypeLookup, WorkTypeDto>();
        CreateMap<SalaryTypeLookup, SalaryTypeDto>();

        CreateMap<JobOpportunity, JobOpportunityListItemDto>()
            .ForMember(d => d.JobFieldName,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldName(j.JobField)))
            .ForMember(d => d.JobFieldGroup,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroup(j.JobField)))
            .ForMember(d => d.JobFieldGroupAr,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroupAr(j.JobField)))
            .ForMember(d => d.RequiredExperienceName,
                o => o.MapFrom(j => JobsCatalog.GetExperienceLevelName(j.RequiredExperience)))
            .ForMember(d => d.WorkTypeName,
                o => o.MapFrom(j => JobsCatalog.GetWorkTypeName(j.WorkType)))
            .ForMember(d => d.SalaryTypeName,
                o => o.MapFrom(j => JobsCatalog.GetSalaryTypeName(j.SalaryType)))
            .ForMember(d => d.PrimaryImageUrl, o => o.MapFrom(j =>
                j.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()));

        CreateMap<JobOpportunity, JobOpportunityDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(j => j.UserId))
            .ForMember(d => d.JobFieldName,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldName(j.JobField)))
            .ForMember(d => d.JobFieldGroup,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroup(j.JobField)))
            .ForMember(d => d.JobFieldGroupAr,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroupAr(j.JobField)))
            .ForMember(d => d.RequiredExperienceName,
                o => o.MapFrom(j => JobsCatalog.GetExperienceLevelName(j.RequiredExperience)))
            .ForMember(d => d.WorkTypeName,
                o => o.MapFrom(j => JobsCatalog.GetWorkTypeName(j.WorkType)))
            .ForMember(d => d.SalaryTypeName,
                o => o.MapFrom(j => JobsCatalog.GetSalaryTypeName(j.SalaryType)));

        CreateMap<CreateJobOpportunityRequest, JobOpportunity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.LogoPath, o => o.Ignore())
            .ForMember(d => d.LogoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore());
    }
}
