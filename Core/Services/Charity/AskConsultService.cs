using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Charity;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Charity;

public class AskConsultService : IAskConsultService
{
    private readonly IAskConsultRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;
    private readonly IUnitOfWork _unitOfWork;

    public AskConsultService(
        IAskConsultRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
        _unitOfWork = unitOfWork;
    }

    public async Task<AskConsultDetailsDto> CreateAsync(
        string userId, CreateAskConsultRequest request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default)
    {
        CharityListings.EnsureResponsibilityAccepted(request.IsResponsibilityAccepted);
        EnsureImageCount(images.Count);

        var entity = new AskConsult
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, startAsPrimary: true, cancellationToken);
            stored.AddRange(added.Select(image => image.ImagePath));

            await _repository.AddAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await _notifications.NotifyAsync(
            userId, ListingModuleType.AskConsult, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildDetailsAsync(entity.Id, userId, cancellationToken, includeUnmoderated: true);
    }

    public async Task<AskConsultDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateAskConsultRequest request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default)
    {
        CharityListings.EnsureResponsibilityAccepted(request.IsResponsibilityAccepted);

        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        ApplyEditableFields(entity, request);

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = entity.Images
                .Where(image => request.RemoveImageIds.Contains(image.Id))
                .ToList();

            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                entity.Images.Remove(image);
            }
        }

        EnsureImageCount(entity.Images.Count + newImages.Count);

        if (newImages.Count > 0)
        {
            var added = await AttachImagesAsync(
                entity, newImages, startAsPrimary: entity.Images.Count == 0, cancellationToken);
            await _repository.AddImagesAsync(added, cancellationToken);
        }

        ListingImages.NormalizeOrder(entity.Images);
        ListingImages.NormalizePrimary(entity.Images);

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.AskConsult,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.Title);

        return await BuildDetailsAsync(id, userId, cancellationToken, includeUnmoderated: true);
    }

    public async Task DeleteAsync(
        string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            entity.UserId, ListingModuleType.AskConsult,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<AskConsultListItemDto>> GetListAsync(
        AskConsultFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);

        var mapped = items.Select(Map<AskConsultListItemDto>).ToList();

        if (viewerUserId is not null && mapped.Count > 0)
        {
            var liked = await _repository.GetLikedIdsAsync(
                mapped.Select(item => item.Id).ToList(), viewerUserId, cancellationToken);

            foreach (var item in mapped)
                item.IsLikedByCurrentUser = liked.Contains(item.Id);
        }

        return new PaginatedResult<AskConsultListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<AskConsultDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId = null, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, viewerUserId, cancellationToken);

    public async Task<AskConsultLikeResultDto> ToggleLikeAsync(
        string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var existing = await _repository.GetLikeAsync(id, userId, cancellationToken);
        bool liked;

        if (existing is null)
        {
            await _repository.AddLikeAsync(
                new AskConsultLike { AskConsultId = id, UserId = userId, CreatedAt = DateTime.UtcNow },
                cancellationToken);

            entity.LikesCount += 1;
            liked = true;
        }
        else
        {
            _repository.RemoveLike(existing);
            entity.LikesCount = Math.Max(0, entity.LikesCount - 1);
            liked = false;
        }

        _repository.Update(entity);

        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception) when (liked && _unitOfWork.IsUniqueConstraintViolation(exception))
        {
            return new AskConsultLikeResultDto { Liked = true, LikesCount = entity.LikesCount };
        }

        if (liked && entity.UserId != userId)
        {
            await _notifications.NotifyAsync(
                entity.UserId, ListingModuleType.AskConsult, NotificationAction.Liked, id, entity.Title);
        }

        return new AskConsultLikeResultDto { Liked = liked, LikesCount = entity.LikesCount };
    }

    public async Task<AskConsultCommentDto> AddCommentAsync(
        string userId, Guid id, CreateAskConsultCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var comment = new AskConsultComment
        {
            Id = Guid.NewGuid(),

            AskConsultId = id,
            UserId = userId,
            Comment = request.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddCommentAsync(comment, cancellationToken);
        entity.CommentsCount += 1;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        if (entity.UserId != userId)
        {
            await _notifications.NotifyAsync(
                entity.UserId, ListingModuleType.AskConsult, NotificationAction.Commented, id, entity.Title);
        }

        var saved = await _repository.GetCommentAsync(comment.Id, cancellationToken)
            ?? throw new NotFoundException("التعليق غير موجود.");

        return MapComment(saved, entity.UserId, userId, isAdmin: false);
    }

    public async Task<AskConsultCommentDto> UpdateCommentAsync(
        string userId, Guid id, Guid commentId, CreateAskConsultCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var comment = await _repository.GetCommentAsync(commentId, cancellationToken)
            ?? throw new NotFoundException("التعليق غير موجود.");

        if (comment.AskConsultId != id)
            throw new NotFoundException("التعليق غير موجود.");

        if (!string.Equals(comment.UserId, userId, StringComparison.Ordinal))
            throw new ForbiddenException("لا يمكنك تعديل تعليق مستخدم آخر.");

        comment.Comment = request.Comment.Trim();
        comment.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        var saved = await _repository.GetCommentAsync(comment.Id, cancellationToken)
            ?? throw new NotFoundException("التعليق غير موجود.");

        return MapComment(saved, entity.UserId, userId, isAdmin: false);
    }

    public async Task<PaginatedResult<AskConsultCommentDto>> GetCommentsAsync(
        Guid id, int pageIndex, int pageSize, string? viewerUserId = null, bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var (items, total) = await _repository.GetCommentsAsync(id, pageIndex, pageSize, cancellationToken);

        var mapped = items
            .Select(comment => MapComment(comment, entity.UserId, viewerUserId, isAdmin))
            .ToList();

        return new PaginatedResult<AskConsultCommentDto>(mapped, total, pageIndex, pageSize);
    }

    public async Task DeleteCommentAsync(
        string userId, bool isAdmin, Guid id, Guid commentId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var comment = await _repository.GetCommentAsync(commentId, cancellationToken)
            ?? throw new NotFoundException("التعليق غير موجود.");

        if (comment.AskConsultId != id)
            throw new NotFoundException("التعليق غير موجود.");

        if (!isAdmin && !string.Equals(comment.UserId, userId, StringComparison.Ordinal))
            throw new ForbiddenException("لا يمكنك حذف تعليق مستخدم آخر.");

        _repository.RemoveComment(comment);
        entity.CommentsCount = Math.Max(0, entity.CommentsCount - 1);

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureImageCount(int count)
    {
        if (count > CharityCatalog.MaxAskConsultImages)
            throw new BadRequestException($"يمكن رفع {CharityCatalog.MaxAskConsultImages} صور بحد أقصى.");
    }

    private async Task<AskConsult> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException("السؤال غير موجود.");
    }

    private static void ApplyEditableFields(AskConsult entity, CreateAskConsultRequest request)
    {
        entity.Category = request.Category!.Value;

        entity.OtherCategory = request.Category == AskConsultCategory.Other
            ? CharityListings.Trimmed(request.OtherCategory)
            : null;

        entity.AskerName = request.AskerName.Trim();
        entity.Title = request.Title.Trim();
        entity.Question = request.Question.Trim();
        entity.Phone = request.Phone.Trim();

        entity.Governorate = null;
        entity.Center = null;
        entity.Latitude = null;
        entity.Longitude = null;

        entity.IsResponsibilityAccepted = request.IsResponsibilityAccepted;
        entity.ResponsibilityAcceptedAt ??= DateTime.UtcNow;
    }

    private async Task<AskConsultDetailsDto> BuildDetailsAsync(
        Guid id, string? viewerUserId, CancellationToken cancellationToken, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("السؤال غير موجود.");

        var dto = Map<AskConsultDetailsDto>(entity);

        if (viewerUserId is not null)
            dto.IsLikedByCurrentUser = await _repository.GetLikeAsync(id, viewerUserId, cancellationToken) is not null;

        return dto;
    }

    private TDto Map<TDto>(AskConsult entity) where TDto : CharityDetailsDtoBase
    {
        var dto = _mapper.Map<TDto>(entity);

        CharityListings.ApplyClassification(
            dto, ListingModuleType.AskConsult, _fileService, OwnerNameOf(entity));

        return dto;
    }

    private static string OwnerNameOf(AskConsult entity) =>
        entity.User is null ? string.Empty : $"{entity.User.FirstName} {entity.User.SecondName}".Trim();

    private static AskConsultCommentDto MapComment(
        AskConsultComment comment, string listingOwnerId, string? viewerUserId, bool isAdmin)
    {
        var authorName = comment.User is null
            ? string.Empty
            : $"{comment.User.FirstName} {comment.User.SecondName}".Trim();

        var isMine = !string.IsNullOrEmpty(viewerUserId) &&
                     string.Equals(comment.UserId, viewerUserId, StringComparison.Ordinal);

        return new AskConsultCommentDto
        {
            Id = comment.Id,
            AskConsultId = comment.AskConsultId,
            Author = new Shared.DTOs.Common.ListingCommentAuthorDto
            {
                Id = comment.UserId,
                Name = authorName,
                ProfileImageUrl = comment.User?.ProfileImageUrl,
                IsListingOwner = string.Equals(comment.UserId, listingOwnerId, StringComparison.Ordinal)
            },
            UserId = comment.UserId,
            UserName = authorName,
            Comment = comment.Comment,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            IsMine = isMine,
            CanEdit = isMine,
            CanDelete = isMine || isAdmin
        };
    }

    private Task<List<AskConsultImage>> AttachImagesAsync(
        AskConsult entity, IReadOnlyList<UploadImageModel> images, bool startAsPrimary,
        CancellationToken cancellationToken) =>
        ListingImages.AttachAsync(
            _fileService,
            entity.Images,
            images,
            FileUploadConstants.AskConsultsFolder,
            startAsPrimary,
            _ => new AskConsultImage { AskConsultId = entity.Id },
            cancellationToken);
}
