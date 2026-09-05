using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Feedback;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _repository;
    private readonly IFileService _fileService;
    private readonly INotificationService _notifications;
    private readonly IMapper _mapper;

    public FeedbackService(
        IFeedbackRepository repository,
        IFileService fileService,
        INotificationService notifications,
        IMapper mapper)
    {
        _repository = repository;
        _fileService = fileService;
        _notifications = notifications;
        _mapper = mapper;
    }

    public async Task<FeedbackDetailsDto> CreateAsync(
        string userId,
        CreateFeedbackRequest request,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default)
    {
        var feedback = _mapper.Map<Feedback>(request);
        var now = DateTime.UtcNow;

        feedback.Id = Guid.NewGuid();
        feedback.UserId = userId;

        var ratingLabel = $"تقييم {feedback.Rating} من {FeedbackCatalog.MaxRating}";

        if (string.IsNullOrWhiteSpace(feedback.Title))
            feedback.Title = ratingLabel;
        else
            feedback.Title = feedback.Title.Trim();

        if (string.IsNullOrWhiteSpace(feedback.Description))
            feedback.Description = ratingLabel;
        else
            feedback.Description = feedback.Description.Trim();

        feedback.Status = FeedbackStatus.New;
        feedback.CreatedAt = now;

        if (images.Count > 0)
        {
            await ListingImages.AttachAsync(
                _fileService,
                feedback.Images,
                images,
                ImageConstants.FeedbackFolder,
                startAsPrimary: true,
                _ => new FeedbackImage { FeedbackId = feedback.Id },
                cancellationToken);
        }

        await _repository.AddAsync(feedback);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.CreateAsync(
            userId,
            "تم استلام ملاحظتك",
            $"تم استلام \"{feedback.Title}\" وسيتم مراجعتها من قبل الإدارة.",
            NotificationType.FeedbackSubmitted,
            feedback.Id,
            NotificationReferenceTypes.Feedback,
            NotificationAction.Created,
            NotificationCatalog.Feedback.Icon,
            NotificationCatalog.Feedback.EntityName,
            $"{NotificationCatalog.Feedback.Route}/{feedback.Id}");

        return await BuildDetailsAsync(feedback.Id, includeReviewer: false, cancellationToken);
    }

    public async Task<PaginatedResult<FeedbackListItemDto>> GetMyFeedbackAsync(
        string userId, FeedbackFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(userId, filter, cancellationToken);

        var mapped = items.Select(item => _mapper.Map<FeedbackListItemDto>(item)).ToList();

        return new PaginatedResult<FeedbackListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<PaginatedResult<FeedbackListItemDto>> GetAllAsync(
        FeedbackFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(
            ownerId: null, filter, cancellationToken);

        var mapped = items.Select(item =>
        {
            var dto = _mapper.Map<FeedbackListItemDto>(item);

            dto.User = _mapper.Map<OwnerDto>(item.User);
            return dto;
        }).ToList();

        return new PaginatedResult<FeedbackListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<FeedbackDetailsDto> GetByIdAsync(
        Guid id, string requesterUserId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var feedback = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("الملاحظة غير موجودة.");

        if (!isAdmin && feedback.UserId != requesterUserId)
            throw new NotFoundException("الملاحظة غير موجودة.");

        return ToDetails(feedback, includeReviewer: isAdmin, includeAuthor: isAdmin);
    }

    public async Task<FeedbackDetailsDto> UpdateStatusAsync(
        Guid id,
        string adminUserId,
        UpdateFeedbackStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var feedback = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("الملاحظة غير موجودة.");

        var now = DateTime.UtcNow;
        var previousStatus = feedback.Status;

        feedback.Status = request.Status;

        if (!string.IsNullOrWhiteSpace(request.AdminReply))
            feedback.AdminReply = request.AdminReply.Trim();

        feedback.ReviewedBy = adminUserId;
        feedback.ReviewedAt = now;
        feedback.UpdatedAt = now;

        feedback.ClosedAt = FeedbackCatalog.IsClosed(request.Status)
            ? feedback.ClosedAt ?? now
            : null;

        _repository.Update(feedback);
        await _repository.SaveChangesAsync(cancellationToken);

        if (previousStatus != request.Status || !string.IsNullOrWhiteSpace(request.AdminReply))
        {
            await _notifications.CreateAsync(
                feedback.UserId,
                $"تحديث على {NotificationCatalog.Feedback.EntityName}",
                BuildStatusMessage(feedback),
                NotificationType.FeedbackUpdated,
                feedback.Id,
                NotificationReferenceTypes.Feedback,
                NotificationAction.AdminUpdated,
                NotificationCatalog.Feedback.Icon,
                NotificationCatalog.Feedback.EntityName,
                $"{NotificationCatalog.Feedback.Route}/{feedback.Id}");
        }

        return await BuildDetailsAsync(id, includeReviewer: true, cancellationToken);
    }

    public FeedbackMetadataDto GetMetadata() =>
        new()
        {
            Types = FeedbackCatalog.TypeNames
                .Select(entry => new FeedbackOptionDto
                {
                    Id = (int)entry.Key,
                    Key = entry.Key.ToString(),
                    Name = entry.Value
                })
                .ToList(),

            Statuses = FeedbackCatalog.StatusNames
                .Select(entry => new FeedbackOptionDto
                {
                    Id = (int)entry.Key,
                    Key = entry.Key.ToString(),
                    Name = entry.Value
                })
                .ToList(),

            MaxTitleLength = FeedbackCatalog.MaxTitleLength,
            MaxDescriptionLength = FeedbackCatalog.MaxDescriptionLength,
            MinRating = FeedbackCatalog.MinRating,
            MaxRating = FeedbackCatalog.MaxRating,
            MaxImages = FeedbackCatalog.MaxImages
        };

    private static string BuildStatusMessage(Feedback feedback)
    {
        var status = FeedbackCatalog.NameOf(feedback.Status);

        return string.IsNullOrWhiteSpace(feedback.AdminReply)
            ? $"تم تحديث حالة ملاحظتك \"{feedback.Title}\" إلى: {status}."
            : $"تم تحديث حالة ملاحظتك \"{feedback.Title}\" إلى: {status}. رد الإدارة: {feedback.AdminReply}";
    }

    private async Task<FeedbackDetailsDto> BuildDetailsAsync(
        Guid id, bool includeReviewer, CancellationToken cancellationToken)
    {
        var feedback = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("الملاحظة غير موجودة.");

        return ToDetails(feedback, includeReviewer, includeAuthor: includeReviewer);
    }

    private FeedbackDetailsDto ToDetails(Feedback feedback, bool includeReviewer, bool includeAuthor)
    {
        var dto = _mapper.Map<FeedbackDetailsDto>(feedback);

        if (includeAuthor && feedback.User is not null)
            dto.User = _mapper.Map<OwnerDto>(feedback.User);

        dto.ReviewedBy = includeReviewer ? feedback.ReviewedBy : null;

        return dto;
    }
}
