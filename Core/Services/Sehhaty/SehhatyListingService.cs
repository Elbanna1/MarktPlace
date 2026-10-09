using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Sehhaty;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Sehhaty;

public abstract class SehhatyListingService<TListing, TImage, TCreate, TUpdate, TDto, TFilter>
    : ISehhatyListingService<TCreate, TUpdate, TDto, TFilter>
    where TListing : SehhatyListing<TImage>, new()
    where TImage : SehhatyListingImage, new()
    where TCreate : CreateSehhatyListingRequest
    where TUpdate : TCreate, ISehhatyGalleryEdit
    where TDto : SehhatyListingDto
    where TFilter : SehhatyFilterParams
{
    public const string ImagesRequiredMessage = "لازم ترفع صورة واحدة على الأقل.";

    private readonly ISehhatyRepository<TListing, TImage, TFilter> _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    protected SehhatyListingService(
        ISehhatyRepository<TListing, TImage, TFilter> repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    protected abstract SehhatyModule Module { get; }

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

    private static void EnsureHasImages(int imageCount)
    {
        if (SehhatyCatalog.ImagesRequired && imageCount <= 0)
            throw new BadRequestException(ImagesRequiredMessage);
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

        dto.ListingType = Module.Type;
        dto.ListingTypeName = Module.EntityName;
        dto.CategoryId = (int)CategoryType.Sehhaty;
        dto.CategoryName = SehhatyCatalog.CategoryName;
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

public sealed class DoctorService
    : SehhatyListingService<Doctor, DoctorImage, CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>,
        IDoctorService
{
    public DoctorService(
        ISehhatyRepository<Doctor, DoctorImage, DoctorFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.Doctors;

    protected override void ApplyModuleFields(Doctor entity, CreateDoctorRequest request)
    {
        entity.Specialty = request.Specialty!.Value;

        entity.OtherSpecialty = entity.Specialty == DoctorSpecialty.Other
            ? Trimmed(request.OtherSpecialty)
            : null;
    }
}

public sealed class MedicalLaboratoryService
    : SehhatyListingService<MedicalLaboratory, MedicalLaboratoryImage, CreateMedicalLaboratoryRequest,
            UpdateMedicalLaboratoryRequest, MedicalLaboratoryDto, MedicalLaboratoryFilterParams>,
        IMedicalLaboratoryService
{
    public MedicalLaboratoryService(
        ISehhatyRepository<MedicalLaboratory, MedicalLaboratoryImage, MedicalLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.MedicalLaboratories;
}

public sealed class DentalLaboratoryService
    : SehhatyListingService<DentalLaboratory, DentalLaboratoryImage, CreateDentalLaboratoryRequest,
            UpdateDentalLaboratoryRequest, DentalLaboratoryDto, DentalLaboratoryFilterParams>,
        IDentalLaboratoryService
{
    public DentalLaboratoryService(
        ISehhatyRepository<DentalLaboratory, DentalLaboratoryImage, DentalLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.DentalLaboratories;
}

public sealed class RadiologyLaboratoryService
    : SehhatyListingService<RadiologyLaboratory, RadiologyLaboratoryImage, CreateRadiologyLaboratoryRequest,
            UpdateRadiologyLaboratoryRequest, RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>,
        IRadiologyLaboratoryService
{
    public RadiologyLaboratoryService(
        ISehhatyRepository<RadiologyLaboratory, RadiologyLaboratoryImage, RadiologyLaboratoryFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.RadiologyLaboratories;
}

public sealed class MedicalCenterService
    : SehhatyListingService<MedicalCenter, MedicalCenterImage, CreateMedicalCenterRequest,
            UpdateMedicalCenterRequest, MedicalCenterDto, MedicalCenterFilterParams>,
        IMedicalCenterService
{
    public MedicalCenterService(
        ISehhatyRepository<MedicalCenter, MedicalCenterImage, MedicalCenterFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.MedicalCenters;
}

public sealed class HospitalService
    : SehhatyListingService<Hospital, HospitalImage, CreateHospitalRequest,
            UpdateHospitalRequest, HospitalDto, HospitalFilterParams>,
        IHospitalService
{
    public HospitalService(
        ISehhatyRepository<Hospital, HospitalImage, HospitalFilterParams> repository,
        IFileService fileService, IMapper mapper, INotificationService notifications)
        : base(repository, fileService, mapper, notifications)
    {
    }

    protected override SehhatyModule Module => SehhatyCatalog.Hospitals;
}
