using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.JobRequests;

namespace Services.Mapping;

public class JobRequestMappingProfile : Profile
{
    public JobRequestMappingProfile()
    {
        CreateMap<JobFieldLookup, JobFieldDto>();
        CreateMap<JobExperienceLevelLookup, JobExperienceLevelDto>();
        CreateMap<EducationLevelLookup, EducationLevelDto>();

        CreateMap<JobRequest, JobRequestListItemDto>()
            .ForMember(d => d.JobFieldName,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldName(j.JobField)))
            .ForMember(d => d.JobFieldGroup,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroup(j.JobField)))
            .ForMember(d => d.JobFieldGroupAr,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroupAr(j.JobField)))
            .ForMember(d => d.ExperienceName,
                o => o.MapFrom(j => JobsCatalog.GetExperienceLevelName(j.Experience)))
            .ForMember(d => d.EducationName,
                o => o.MapFrom(j => JobsCatalog.GetEducationLevelName(j.Education)))
            .ForMember(d => d.HasIntroVideo,
                o => o.MapFrom(j => j.IntroVideoUrl != null));

        CreateMap<JobRequest, JobRequestDetailsDto>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(j => j.UserId))
            .ForMember(d => d.JobFieldName,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldName(j.JobField)))
            .ForMember(d => d.JobFieldGroup,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroup(j.JobField)))
            .ForMember(d => d.JobFieldGroupAr,
                o => o.MapFrom(j => JobsCatalog.GetJobFieldGroupAr(j.JobField)))
            .ForMember(d => d.ExperienceName,
                o => o.MapFrom(j => JobsCatalog.GetExperienceLevelName(j.Experience)))
            .ForMember(d => d.EducationName,
                o => o.MapFrom(j => JobsCatalog.GetEducationLevelName(j.Education)));

        CreateMap<CreateJobRequestRequest, JobRequest>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Governorate, o => o.Ignore())
            .ForMember(d => d.ProfileImagePath, o => o.Ignore())
            .ForMember(d => d.ProfileImageUrl, o => o.Ignore())
            .ForMember(d => d.CvFilePath, o => o.Ignore())
            .ForMember(d => d.CvFileUrl, o => o.Ignore())
            .ForMember(d => d.CvFileName, o => o.Ignore())
            .ForMember(d => d.IntroVideoPath, o => o.Ignore())
            .ForMember(d => d.IntroVideoUrl, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore());
    }
}
