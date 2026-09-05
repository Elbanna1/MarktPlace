using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.LostFound;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class LostFoundService : ILostFoundService
{
    private readonly ILostFoundRepository _repository;
    private readonly IFileService _fileService;
    private readonly INotificationService _notificationService;
    private readonly IListingInteractionService _interactions;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LostFoundService(
        ILostFoundRepository repository,
        IFileService fileService,
        INotificationService notificationService,
        IListingInteractionService interactions,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _fileService = fileService;
        _notificationService = notificationService;
        _interactions = interactions;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<LostFoundPostDetailsDto> CreateAsync(
        string userId,
        CreateLostFoundPostRequest request,
        IReadOnlyList<UploadImageModel> images)
    {
        var post = _mapper.Map<LostFoundPost>(request);
        var now = DateTime.UtcNow;

        post.Id = Guid.NewGuid();
        post.UserId = userId;
        post.Status = PostStatus.Active;
        post.Governorate = LocationConstants.Governorate;
        post.LikesCount = 0;
        post.CommentsCount = 0;
        post.CreatedAt = now;

        if (post.PostType == PostType.Lost)
            post.FoundDate = null;
        else
            post.LostDate = null;

        await AttachImagesAsync(post, images, startAsPrimary: true);

        await _repository.AddAsync(post);
        await _repository.SaveChangesAsync();

        await _notificationService.NotifyAsync(
            userId, SubjectFor(post.PostType), NotificationAction.Created, post.Id, post.ItemName);

        return await BuildDetailsAsync(post.Id, userId, includeUnmoderated: true);
    }

    public async Task<LostFoundPostDetailsDto> UpdateAsync(
        string userId,
        Guid id,
        UpdateLostFoundPostRequest request,
        IReadOnlyList<UploadImageModel> newImages)
    {
        var post = await _repository.GetOwnedAsync(id, userId)
            ?? throw new NotFoundException("المنشور مش موجود.");

        post.Name = request.Name;
        post.ItemName = request.ItemName;
        post.Description = request.Description;
        post.PhoneNumber = request.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.Center))
            post.Center = request.Center;

        if (post.PostType == PostType.Lost)
        {
            if (request.LostDate is null)
                throw new BadRequestException("تاريخ الضياع مطلوب في منشور «ضايع مني».");
            post.LostDate = request.LostDate;
            post.FoundDate = null;
        }
        else
        {
            if (request.FoundDate is null)
                throw new BadRequestException("تاريخ اللقطة مطلوب في منشور «لقيت».");
            post.FoundDate = request.FoundDate;
            post.LostDate = null;
        }

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = post.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                post.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = post.Images.Count == 0;
            var added = await AttachImagesAsync(post, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(post.Images);

        post.UpdatedAt = DateTime.UtcNow;

        _repository.Update(post);
        await _repository.SaveChangesAsync();

        await _notificationService.NotifyAsync(
            userId, SubjectFor(post.PostType), NotificationAction.Updated, post.Id, post.ItemName);

        return await BuildDetailsAsync(id, userId, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var post = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (post is null)
            throw new NotFoundException("المنشور مش موجود.");

        post.IsDeleted = true;
        post.DeletedAt = DateTime.UtcNow;

        _repository.Update(post);
        await _repository.SaveChangesAsync();

        await _interactions.PurgeListingAsync(
            ListingModuleCatalog.ModuleOf(post.PostType), post.Id);

        await _notificationService.NotifyAsync(
            post.UserId, SubjectFor(post.PostType),
            isAdmin && post.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            post.Id, post.ItemName);
    }

    private static NotificationSubject SubjectFor(PostType postType) =>
        postType == PostType.Lost ? NotificationCatalog.LostItem : NotificationCatalog.FoundItem;

    public async Task<PaginatedResult<LostFoundPostListItemDto>> GetFeedAsync(
        LostFoundFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default)
    {
        if (filter.PostType is null)
        {
            throw new BadRequestException(
                "نوع الإعلان مطلوب: اختر \"ضايع مني\" أو \"لقيت\" (postType=Lost أو postType=Found).");
        }

        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);

        var mapped = items.Select(post => _mapper.Map<LostFoundPostListItemDto>(post)).ToList();

        await ApplyCountersAsync(mapped, items, viewerUserId, cancellationToken);

        return new PaginatedResult<LostFoundPostListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public async Task<LostFoundPostDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId, string? viewerIpAddress = null,
        CancellationToken cancellationToken = default)
    {
        var details = await BuildDetailsAsync(id, viewerUserId, cancellationToken);

        try
        {
            var view = await _interactions.RecordViewAsync(
                details.TypeId, id, viewerUserId, viewerIpAddress, cancellationToken);

            details.Views = view.TotalViews;
        }
        catch (OperationCanceledException)
        {
        }
        catch (NotFoundException)
        {
        }

        return details;
    }

    public async Task<LostFoundPostDetailsDto> MarkAsReturnedAsync(string userId, Guid id)
    {
        var post = await _repository.GetOwnedAsync(id, userId)
            ?? throw new NotFoundException("المنشور مش موجود.");

        if (post.Status == PostStatus.Returned)
            throw new BadRequestException("المنشور متعلّم عليه إنه رجع بالفعل.");

        post.Status = PostStatus.Returned;
        post.UpdatedAt = DateTime.UtcNow;

        _repository.Update(post);
        await _repository.SaveChangesAsync();

        await _notificationService.CreateAsync(
            userId,
            "تحديث حالة الإعلان",
            "تم تحديث حالة الإعلان إلى تم العثور عليه.",
            NotificationType.LostItemReturned,
            post.Id,
            NotificationReferenceTypes.LostFoundPost);

        return await BuildDetailsAsync(id, userId, includeUnmoderated: true);
    }

    public async Task<LikeResultDto> ToggleLikeAsync(string userId, Guid id)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var existing = await _repository.GetLikeAsync(id, userId);
        bool liked;

        if (existing is null)
        {
            await _repository.AddLikeAsync(new LostFoundLike
            {
                PostId = id,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
            post.LikesCount += 1;
            liked = true;
        }
        else
        {
            _repository.RemoveLike(existing);
            post.LikesCount = Math.Max(0, post.LikesCount - 1);
            liked = false;
        }

        _repository.Update(post);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception exception) when (liked && _unitOfWork.IsUniqueConstraintViolation(exception))
        {
            return new LikeResultDto { Liked = true, LikesCount = post.LikesCount };
        }

        if (liked && post.UserId != userId)
        {
            await _notificationService.CreateAsync(
                post.UserId,
                "إعجاب جديد",
                "أعجب أحد المستخدمين بإعلانك.",
                NotificationType.NewLike,
                post.Id,
                NotificationReferenceTypes.LostFoundPost);
        }

        return new LikeResultDto { Liked = liked, LikesCount = post.LikesCount };
    }

    public async Task<LostFoundCommentDto> AddCommentAsync(string userId, Guid id, CreateCommentRequest request)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var comment = new LostFoundComment
        {
            Id = Guid.NewGuid(),
            PostId = id,
            UserId = userId,
            Comment = request.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddCommentAsync(comment);
        post.CommentsCount += 1;
        _repository.Update(post);
        await _repository.SaveChangesAsync();

        if (post.UserId != userId)
        {
            await _notificationService.CreateAsync(
                post.UserId,
                "تعليق جديد",
                "قام أحد المستخدمين بالتعليق على إعلانك.",
                NotificationType.NewComment,
                post.Id,
                NotificationReferenceTypes.LostFoundPost);
        }

        var saved = await _repository.GetCommentAsync(comment.Id)
            ?? throw new NotFoundException("التعليق مش موجود.");

        return MapComment(saved, post.UserId, userId, isAdmin: false);
    }

    public async Task<LostFoundCommentDto> UpdateCommentAsync(
        string userId, Guid id, Guid commentId, CreateCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var comment = await _repository.GetCommentForUpdateAsync(commentId, cancellationToken)
            ?? throw new NotFoundException("التعليق مش موجود.");

        if (comment.PostId != id)
            throw new NotFoundException("التعليق مش موجود.");

        if (!string.Equals(comment.UserId, userId, StringComparison.Ordinal))
            throw new ForbiddenException("مش ممكن تعدل تعليق حد تاني.");

        comment.Comment = request.Comment.Trim();
        comment.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        var saved = await _repository.GetCommentAsync(comment.Id)
            ?? throw new NotFoundException("التعليق مش موجود.");

        return MapComment(saved, post.UserId, userId, isAdmin: false);
    }

    public async Task DeleteCommentAsync(
        string userId, bool isAdmin, Guid id, Guid commentId,
        CancellationToken cancellationToken = default)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var comment = await _repository.GetCommentForUpdateAsync(commentId, cancellationToken)
            ?? throw new NotFoundException("التعليق مش موجود.");

        if (comment.PostId != id)
            throw new NotFoundException("التعليق مش موجود.");

        if (!isAdmin && !string.Equals(comment.UserId, userId, StringComparison.Ordinal))
            throw new ForbiddenException("مش ممكن تمسح تعليق حد تاني.");

        _repository.RemoveComment(comment);

        post.CommentsCount = Math.Max(0, post.CommentsCount - 1);

        _repository.Update(post);
        await _repository.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<LostFoundCommentDto>> GetCommentsAsync(
        Guid id, string? viewerUserId = null, bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated: true)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var comments = await _repository.GetCommentsAsync(id, cancellationToken);

        return comments
            .Select(comment => MapComment(comment, post.UserId, viewerUserId, isAdmin))
            .ToList();
    }

    private static LostFoundCommentDto MapComment(
        LostFoundComment comment, string postOwnerId, string? viewerUserId, bool isAdmin)
    {
        var authorName = comment.User is null
            ? string.Empty
            : $"{comment.User.FirstName} {comment.User.SecondName}".Trim();

        var isMine = !string.IsNullOrEmpty(viewerUserId) &&
                     string.Equals(comment.UserId, viewerUserId, StringComparison.Ordinal);

        return new LostFoundCommentDto
        {
            Id = comment.Id,
            PostId = comment.PostId,
            Author = new Shared.DTOs.Common.ListingCommentAuthorDto
            {
                Id = comment.UserId,
                Name = authorName,
                ProfileImageUrl = comment.User?.ProfileImageUrl,
                IsListingOwner = string.Equals(comment.UserId, postOwnerId, StringComparison.Ordinal)
            },
            UserName = authorName,
            Comment = comment.Comment,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            IsMine = isMine,
            CanEdit = isMine,
            CanDelete = isMine || isAdmin
        };
    }

    private async Task<LostFoundPostDetailsDto> BuildDetailsAsync(
        Guid id, string? viewerUserId, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false)
    {
        var post = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("المنشور مش موجود.");

        var dto = _mapper.Map<LostFoundPostDetailsDto>(post);

        var (module, categoryId, subCategoryId) = ClassificationOf(post.PostType);
        dto.TypeId = module;
        dto.CategoryId = categoryId;
        dto.SubCategoryId = subCategoryId;

        var counters = await _interactions.GetCountersAsync(
            dto.TypeId, post.Id, viewerUserId, cancellationToken);

        dto.Views = counters.Views;
        dto.FavoriteCount = counters.FavoriteCount;
        dto.AverageRating = counters.AverageRating;
        dto.RatingsCount = counters.RatingsCount;
        dto.IsFavorite = counters.IsFavorite;

        if (!string.IsNullOrEmpty(viewerUserId))
            dto.IsLikedByCurrentUser = await _repository.HasLikedAsync(id, viewerUserId, cancellationToken);

        return dto;
    }

    private static (ListingModuleType Module, int CategoryId, int SubCategoryId) ClassificationOf(
        PostType postType)
    {
        var module = ListingModuleCatalog.ModuleOf(postType);
        var subCategory = ListingModuleCatalog.SubCategoryOf(module);

        var categoryId = subCategory is { } sub
            ? (int)(ListingModuleCatalog.CategoryOf(sub) ?? 0)
            : 0;

        return (module, categoryId, (int)(subCategory ?? 0));
    }

    private async Task ApplyCountersAsync(
        IReadOnlyList<LostFoundPostListItemDto> cards,
        IReadOnlyList<LostFoundPost> posts,
        string? viewerUserId,
        CancellationToken cancellationToken)
    {
        if (cards.Count == 0)
            return;

        for (var index = 0; index < cards.Count; index++)
        {
            var (module, categoryId, subCategoryId) = ClassificationOf(posts[index].PostType);
            cards[index].TypeId = module;
            cards[index].CategoryId = categoryId;
            cards[index].SubCategoryId = subCategoryId;
        }

        var keys = cards.Select(card => (card.TypeId, card.Id)).ToList();
        var counters = await _interactions.GetCountersAsync(keys, viewerUserId, cancellationToken);

        foreach (var card in cards)
        {
            if (!counters.TryGetValue((card.TypeId, card.Id), out var counter))
                continue;

            card.Views = counter.Views;
            card.FavoriteCount = counter.FavoriteCount;
            card.AverageRating = counter.AverageRating;
            card.RatingsCount = counter.RatingsCount;

            card.IsFavorite = counter.IsFavorite;
        }

        if (string.IsNullOrEmpty(viewerUserId))
            return;

        var liked = await _repository.GetLikedIdsAsync(
            cards.Select(card => card.Id).ToList(), viewerUserId, cancellationToken);

        foreach (var card in cards)
            card.IsLikedByCurrentUser = liked.Contains(card.Id);
    }

    private Task<List<LostFoundImage>> AttachImagesAsync(
        LostFoundPost post, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            post.Images,
            images,
            ImageConstants.LostFoundFolder,
            startAsPrimary,
            _ => new LostFoundImage { PostId = post.Id });
}
