using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.JobRequests;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class JobRequestService : IJobRequestService
{
    private readonly IJobRequestRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public JobRequestService(
        IJobRequestRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<JobRequestDetailsDto> CreateAsync(
        string userId,
        CreateJobRequestRequest request,
        UploadImageModel? profileImage,
        UploadImageModel? cvFile,
        UploadImageModel? introVideo,
        CancellationToken cancellationToken = default)
    {
        var jobRequest = _mapper.Map<JobRequest>(request);

        jobRequest.Id = Guid.NewGuid();
        jobRequest.UserId = userId;
        jobRequest.Governorate = LocationConstants.Governorate;
        jobRequest.CreatedAt = DateTime.UtcNow;
        jobRequest.OtherJobField = request.OtherJobField?.Trim();

        var stored = new List<string>();
        try
        {
            if (profileImage is not null)
            {
                var image = await _fileService.SaveAsync(
                    profileImage, FileUploadConstants.JobRequestsFolder, cancellationToken);
                stored.Add(image.RelativePath);

                jobRequest.ProfileImagePath = image.RelativePath;
                jobRequest.ProfileImageUrl = image.Url;
            }

            if (cvFile is not null)
            {
                var cv = await _fileService.SaveDocumentAsync(
                    cvFile, FileUploadConstants.JobRequestCvFolder, cancellationToken);
                stored.Add(cv.RelativePath);

                jobRequest.CvFilePath = cv.RelativePath;
                jobRequest.CvFileUrl = cv.Url;
                jobRequest.CvFileName = cvFile.FileName;
            }

            if (introVideo is not null)
            {
                var video = await _fileService.SaveVideoAsync(
                    introVideo, FileUploadConstants.JobRequestVideoFolder, cancellationToken);
                stored.Add(video.RelativePath);

                jobRequest.IntroVideoPath = video.RelativePath;
                jobRequest.IntroVideoUrl = video.Url;
            }

            await _repository.AddAsync(jobRequest);
            await _repository.SaveChangesAsync();
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await _notifications.NotifyAsync(
            userId, ListingModuleType.JobRequest, NotificationAction.Created, jobRequest.Id, jobRequest.Title);

        return await BuildDetailsAsync(jobRequest.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<JobRequestDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateJobRequestRequest request,
        UploadImageModel? profileImage,
        UploadImageModel? cvFile,
        UploadImageModel? introVideo,
        CancellationToken cancellationToken = default)
    {
        var jobRequest = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        if (jobRequest is null)
            throw new NotFoundException("طلب العمل مش موجود.");

        jobRequest.ApplicantName = request.ApplicantName;
        jobRequest.Phone = request.Phone;
        jobRequest.WhatsApp = request.WhatsApp;
        jobRequest.JobField = request.JobField;
        jobRequest.OtherJobField = request.OtherJobField?.Trim();
        jobRequest.Experience = request.Experience;
        jobRequest.Education = request.Education;
        jobRequest.Skills = request.Skills;
        jobRequest.Center = request.Center;
        jobRequest.Address = request.Address;
        jobRequest.Title = request.Title;
        jobRequest.Description = request.Description;

        var replaced = new List<string>();
        var written = new List<string>();

        try
        {
            if (profileImage is not null)
            {
                var image = await _fileService.SaveAsync(
                    profileImage, FileUploadConstants.JobRequestsFolder, cancellationToken);
                written.Add(image.RelativePath);

                if (!string.IsNullOrWhiteSpace(jobRequest.ProfileImagePath))
                    replaced.Add(jobRequest.ProfileImagePath);

                jobRequest.ProfileImagePath = image.RelativePath;
                jobRequest.ProfileImageUrl = image.Url;
            }
            else if (request.RemoveProfileImage && !string.IsNullOrWhiteSpace(jobRequest.ProfileImagePath))
            {
                replaced.Add(jobRequest.ProfileImagePath);
                jobRequest.ProfileImagePath = null;
                jobRequest.ProfileImageUrl = null;
            }

            if (cvFile is not null)
            {
                var cv = await _fileService.SaveDocumentAsync(
                    cvFile, FileUploadConstants.JobRequestCvFolder, cancellationToken);
                written.Add(cv.RelativePath);

                if (!string.IsNullOrWhiteSpace(jobRequest.CvFilePath))
                    replaced.Add(jobRequest.CvFilePath);

                jobRequest.CvFilePath = cv.RelativePath;
                jobRequest.CvFileUrl = cv.Url;
                jobRequest.CvFileName = cvFile.FileName;
            }

            if (introVideo is not null)
            {
                var video = await _fileService.SaveVideoAsync(
                    introVideo, FileUploadConstants.JobRequestVideoFolder, cancellationToken);
                written.Add(video.RelativePath);

                if (!string.IsNullOrWhiteSpace(jobRequest.IntroVideoPath))
                    replaced.Add(jobRequest.IntroVideoPath);

                jobRequest.IntroVideoPath = video.RelativePath;
                jobRequest.IntroVideoUrl = video.Url;
            }
            else if (request.RemoveIntroVideo && !string.IsNullOrWhiteSpace(jobRequest.IntroVideoPath))
            {
                replaced.Add(jobRequest.IntroVideoPath);
                jobRequest.IntroVideoPath = null;
                jobRequest.IntroVideoUrl = null;
            }

            jobRequest.UpdatedAt = DateTime.UtcNow;

            _repository.Update(jobRequest);
            await _repository.SaveChangesAsync();
        }
        catch
        {
            foreach (var path in written)
                _fileService.Delete(path);

            throw;
        }

        foreach (var path in replaced)
            _fileService.Delete(path);

        await _notifications.NotifyAsync(
            jobRequest.UserId, ListingModuleType.JobRequest,
            isAdmin && jobRequest.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, jobRequest.Title);

        return await BuildDetailsAsync(id, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var jobRequest = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (jobRequest is null)
            throw new NotFoundException("طلب العمل مش موجود.");

        jobRequest.IsDeleted = true;
        jobRequest.DeletedAt = DateTime.UtcNow;

        _repository.Update(jobRequest);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            jobRequest.UserId, ListingModuleType.JobRequest,
            isAdmin && jobRequest.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, jobRequest.Title);
    }

    public async Task<PaginatedResult<JobRequestListItemDto>> GetListAsync(
        JobRequestFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<JobRequestListItemDto>>(items);
        return new PaginatedResult<JobRequestListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<JobRequestDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
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

    public async Task<IReadOnlyList<EducationLevelDto>> GetEducationLevelsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetEducationLevelsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EducationLevelDto>>(rows);
    }

    private async Task<JobRequestDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var jobRequest = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("طلب العمل مش موجود.");

        return _mapper.Map<JobRequestDetailsDto>(jobRequest);
    }
}
