using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class JobOpportunityService : IJobOpportunityService
{
    private readonly IJobOpportunityRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public JobOpportunityService(
        IJobOpportunityRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<JobOpportunityDetailsDto> CreateAsync(
        string userId,
        CreateJobOpportunityRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? logo,
        CancellationToken cancellationToken = default)
    {
        var jobOpportunity = _mapper.Map<JobOpportunity>(request);

        jobOpportunity.Id = Guid.NewGuid();
        jobOpportunity.UserId = userId;
        jobOpportunity.Governorate = LocationConstants.Governorate;
        jobOpportunity.CreatedAt = DateTime.UtcNow;
        jobOpportunity.OtherJobField = request.OtherJobField?.Trim();
        jobOpportunity.GoogleMaps = request.GoogleMaps?.Trim();

        jobOpportunity.Salary = NormalizeSalary(request.SalaryType, request.Salary);

        if (logo is not null)
        {
            var stored = await _fileService.SaveAsync(
                logo, FileUploadConstants.JobOpportunitiesFolder, cancellationToken);

            jobOpportunity.LogoPath = stored.RelativePath;
            jobOpportunity.LogoUrl = stored.Url;
        }

        await AttachImagesAsync(jobOpportunity, images, startAsPrimary: true, cancellationToken);

        await _repository.AddAsync(jobOpportunity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.JobOpportunity, NotificationAction.Created, jobOpportunity.Id, jobOpportunity.Title);

        return await BuildDetailsAsync(jobOpportunity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<JobOpportunityDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateJobOpportunityRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? logo,
        CancellationToken cancellationToken = default)
    {
        var jobOpportunity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (jobOpportunity is null)
            throw new NotFoundException("فرصة العمل مش موجودة.");

        jobOpportunity.EmployerName = request.EmployerName;
        jobOpportunity.Phone = request.Phone;
        jobOpportunity.WhatsApp = request.WhatsApp;
        jobOpportunity.JobTitle = request.JobTitle;
        jobOpportunity.JobField = request.JobField;
        jobOpportunity.OtherJobField = request.OtherJobField?.Trim();
        jobOpportunity.RequiredExperience = request.RequiredExperience;
        jobOpportunity.WorkType = request.WorkType;
        jobOpportunity.SalaryType = request.SalaryType;
        jobOpportunity.Salary = NormalizeSalary(request.SalaryType, request.Salary);
        jobOpportunity.Center = request.Center;
        jobOpportunity.Address = request.Address;
        jobOpportunity.GoogleMaps = request.GoogleMaps?.Trim();
        jobOpportunity.Title = request.Title;
        jobOpportunity.Description = request.Description;

        var replacedLogoPath = (string?)null;

        if (logo is not null)
        {
            var stored = await _fileService.SaveAsync(
                logo, FileUploadConstants.JobOpportunitiesFolder, cancellationToken);

            replacedLogoPath = jobOpportunity.LogoPath;
            jobOpportunity.LogoPath = stored.RelativePath;
            jobOpportunity.LogoUrl = stored.Url;
        }
        else if (request.RemoveLogo && !string.IsNullOrWhiteSpace(jobOpportunity.LogoPath))
        {
            replacedLogoPath = jobOpportunity.LogoPath;
            jobOpportunity.LogoPath = null;
            jobOpportunity.LogoUrl = null;
        }

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = jobOpportunity.Images
                .Where(i => request.RemoveImageIds.Contains(i.Id))
                .ToList();

            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                jobOpportunity.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = jobOpportunity.Images.Count == 0;
            var added = await AttachImagesAsync(
                jobOpportunity, newImages, startAsPrimary, cancellationToken);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(jobOpportunity.Images);

        jobOpportunity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(jobOpportunity);
        await _repository.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(replacedLogoPath))
            _fileService.Delete(replacedLogoPath);

        await _notifications.NotifyAsync(
            jobOpportunity.UserId, ListingModuleType.JobOpportunity,
            isAdmin && jobOpportunity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, jobOpportunity.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var jobOpportunity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (jobOpportunity is null)
            throw new NotFoundException("فرصة العمل مش موجودة.");

        jobOpportunity.IsDeleted = true;
        jobOpportunity.DeletedAt = DateTime.UtcNow;

        _repository.Update(jobOpportunity);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            jobOpportunity.UserId, ListingModuleType.JobOpportunity,
            isAdmin && jobOpportunity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, jobOpportunity.Title);
    }

    public async Task<PaginatedResult<JobOpportunityListItemDto>> GetListAsync(
        JobOpportunityFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<JobOpportunityListItemDto>>(items);
        return new PaginatedResult<JobOpportunityListItemDto>(
            mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<JobOpportunityDetailsDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<JobFieldDto>> GetJobFieldsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetJobFieldsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<JobFieldDto>>(rows);
    }

    public async Task<IReadOnlyList<JobExperienceLevelDto>> GetExperienceLevelsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetExperienceLevelsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<JobExperienceLevelDto>>(rows);
    }

    public async Task<IReadOnlyList<WorkTypeDto>> GetWorkTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetWorkTypesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<WorkTypeDto>>(rows);
    }

    public async Task<IReadOnlyList<SalaryTypeDto>> GetSalaryTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetSalaryTypesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<SalaryTypeDto>>(rows);
    }

    private static decimal? NormalizeSalary(SalaryType salaryType, decimal? salary) =>
        salaryType == SalaryType.Specified ? salary : null;

    private async Task<JobOpportunityDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var jobOpportunity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("فرصة العمل مش موجودة.");

        return _mapper.Map<JobOpportunityDetailsDto>(jobOpportunity);
    }

    private Task<List<JobOpportunityImage>> AttachImagesAsync(
        JobOpportunity jobOpportunity,
        IReadOnlyList<UploadImageModel> images,
        bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            jobOpportunity.Images,
            images,
            FileUploadConstants.JobOpportunitiesFolder,
            startAsPrimary,
            _ => new JobOpportunityImage { JobOpportunityId = jobOpportunity.Id },
            cancellationToken);
}
