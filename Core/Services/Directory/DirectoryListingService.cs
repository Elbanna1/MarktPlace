using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Directory;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Directory;

public abstract class DirectoryListingService<TListing, TImage, TCreate, TUpdate, TDto, TFilter>
    : IDirectoryListingService<TCreate, TUpdate, TDto, TFilter>
    where TListing : DirectoryListing<TImage>, new()
    where TImage : DirectoryListingImage, new()
    where TCreate : CreateDirectoryListingRequest
    where TUpdate : TCreate, IDirectoryGalleryEdit
    where TDto : DirectoryListingDto
    where TFilter : DirectoryFilterParams
{
    private readonly IDirectoryRepository<TListing, TImage, TFilter> _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    protected DirectoryListingService(
        IDirectoryRepository<TListing, TImage, TFilter> repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    protected abstract DirectoryModule Module { get; }

    protected virtual void ApplyModuleFields(TListing entity, TCreate request)
    {
    }

    public async Task<TDto> CreateAsync(
        string userId, TCreate request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default)
    {
        EnsureHasImages(images.Count);

        var entity = new TListing
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        ApplyEditableFields(entity, request);

        var stored = new List<string>();
        try
        {
            var added = await AttachImagesAsync(entity, images, cancellationToken);
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
            userId, Module.Type, NotificationAction.Created, entity.Id, entity.Title);

        return await BuildAsync(entity.Id, cancellationToken, includeUnmoderated: true);
    }

    public async Task<TDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, TUpdate request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default)
    {
        var entity = await LoadForWriteAsync(userId, isAdmin, id, cancellationToken);

        var removed = request.RemoveImageIds.Count == 0
            ? new List<TImage>()
            : entity.Images.Where(image => request.RemoveImageIds.Contains(image.Id)).ToList();

        EnsureHasImages(entity.Images.Count - removed.Count + newImages.Count);

        ApplyEditableFields(entity, request);

        foreach (var image in removed)
        {
            _fileService.Delete(image.ImagePath);
            _repository.RemoveImage(image);
            entity.Images.Remove(image);
        }

        if (newImages.Count > 0)
        {
            var added = await AttachImagesAsync(entity, newImages, cancellationToken);
            await _repository.AddImagesAsync(added, cancellationToken);
        }

        ListingImages.NormalizeOrder(entity.Images);
        ListingImages.NormalizePrimary(entity.Images);

        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        await _notifications.NotifyAsync(
            entity.UserId, Module.Type,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, entity.Title);

        return await BuildAsync(id, cancellationToken, includeUnmoderated: true);
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
            entity.UserId, Module.Type,
            isAdmin && entity.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, entity.Title);
    }

    public async Task<PaginatedResult<TDto>> GetListAsync(
        TFilter filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);

        return new PaginatedResult<TDto>(
            items.Select(Map).ToList(), total, filter.PageIndex, filter.PageSize);
    }

    public Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildAsync(id, cancellationToken);

    private async Task<TListing> LoadForWriteAsync(
        string userId, bool isAdmin, Guid id, CancellationToken cancellationToken)
    {
        var entity = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId, cancellationToken);

        return entity ?? throw new NotFoundException(Module.NotFoundMessage);
    }

    private void ApplyEditableFields(TListing entity, TCreate request)
    {
        entity.Name = request.Name.Trim();
        entity.Title = request.Title.Trim();
        entity.Description = request.Description.Trim();
        entity.Address = request.Address.Trim();
        entity.GoogleMapsUrl = Trimmed(request.GoogleMapsUrl);
        entity.Phone = request.Phone.Trim();
        entity.WhatsApp = Trimmed(request.WhatsApp);
        entity.Email = Trimmed(request.Email);

        ApplyModuleFields(entity, request);
    }

    protected static string? Trimmed(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private void EnsureHasImages(int imageCount)
    {
        if (Module.ImagesRequired && imageCount <= 0)
            throw new BadRequestException(DirectoryListingRules.ImagesRequiredMessage);
    }

    private async Task<TDto> BuildAsync(
        Guid id, CancellationToken cancellationToken, bool includeUnmoderated = false)
    {
        var entity = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException(Module.NotFoundMessage);

        return Map(entity);
    }

    private TDto Map(TListing entity)
    {
        var dto = _mapper.Map<TDto>(entity);

        var subCategory = CategoryCatalog.FindSubCategory(Module.SubCategory)!;
        var category = CategoryCatalog.FindCategory(subCategory.Category)!;

        dto.ListingType = Module.Type;
        dto.ListingTypeName = Module.EntityName;
        dto.CategoryId = (int)category.Id;
        dto.CategoryName = category.NameAr;
        dto.SubCategoryId = (int)Module.SubCategory;
        dto.SubCategoryName = subCategory.NameAr;
        dto.OwnerName = entity.User is null
            ? string.Empty
            : $"{entity.User.FirstName} {entity.User.SecondName}".Trim();
        dto.ShareUrl = _fileService.BuildPublicUrl($"/{ListingModuleCatalog.RouteOf(Module.Type)}/{dto.Id}");
        dto.PrimaryImageUrl = dto.Images
            .OrderByDescending(image => image.IsPrimary)
            .ThenBy(image => image.SortOrder)
            .Select(image => image.Url)
            .FirstOrDefault();

        return dto;
    }

    private Task<List<TImage>> AttachImagesAsync(
        TListing entity, IReadOnlyList<UploadImageModel> images, CancellationToken cancellationToken) =>
        ListingImages.AttachOrderedAsync(
            _fileService,
            entity.Images,
            images,
            Module.ImagesFolder,
            _ => new TImage { ListingId = entity.Id },
            cancellationToken);
}
