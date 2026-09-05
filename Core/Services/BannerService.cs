using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Banners;
using Shared.Exceptions;

namespace Services;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public BannerService(IBannerRepository repository, IFileService fileService, IMapper mapper)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BannerDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var banners = await _repository.GetActiveAsync(now, cancellationToken);

        return ToDtos(banners, now);
    }

    public async Task<IReadOnlyList<BannerDto>> GetAllAsync(
        BannerFilterParams filter, CancellationToken cancellationToken = default)
    {
        var banners = await _repository.GetAllAsync(filter.IsActive, cancellationToken);

        return ToDtos(banners, DateTime.UtcNow);
    }

    public async Task<BannerDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("البانر مش موجود.");

        return ToDto(banner, DateTime.UtcNow);
    }

    public async Task<BannerDto> CreateAsync(
        CreateBannerRequest request, UploadImageModel? image, CancellationToken cancellationToken = default)
    {
        if (image is null)
            throw new BadRequestException("صورة البانر مطلوبة.");

        EnsureSupportedFormat(image);
        EnsureValidWindow(request.StartDate, request.EndDate);

        var stored = await _fileService.SaveAsync(image, ImageConstants.BannersFolder, cancellationToken);

        var banner = new Banner
        {
            Id = Guid.NewGuid(),
            Title = Trim(request.Title),
            Description = Trim(request.Description),
            ImageFileName = stored.FileName,
            ImagePath = stored.RelativePath,
            ImageUrl = stored.Url,
            RedirectUrl = Trim(request.RedirectUrl),
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        _repository.Add(banner);
        await _repository.SaveChangesAsync(cancellationToken);

        return ToDto(banner, DateTime.UtcNow);
    }

    public async Task<BannerDto> UpdateAsync(
        Guid id, UpdateBannerRequest request, UploadImageModel? image,
        CancellationToken cancellationToken = default)
    {
        var banner = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("البانر مش موجود.");

        EnsureValidWindow(request.StartDate, request.EndDate);

        string? replacedImagePath = null;

        if (image is not null)
        {
            EnsureSupportedFormat(image);

            var stored = await _fileService.SaveAsync(image, ImageConstants.BannersFolder, cancellationToken);

            replacedImagePath = banner.ImagePath;

            banner.ImageFileName = stored.FileName;
            banner.ImagePath = stored.RelativePath;
            banner.ImageUrl = stored.Url;
        }

        banner.Title = Trim(request.Title);
        banner.Description = Trim(request.Description);
        banner.RedirectUrl = Trim(request.RedirectUrl);
        banner.DisplayOrder = request.DisplayOrder;
        banner.IsActive = request.IsActive;
        banner.StartDate = request.StartDate;
        banner.EndDate = request.EndDate;
        banner.UpdatedAt = DateTime.UtcNow;

        _repository.Update(banner);
        await _repository.SaveChangesAsync(cancellationToken);

        if (replacedImagePath is not null)
            _fileService.Delete(replacedImagePath);

        return ToDto(banner, DateTime.UtcNow);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("البانر مش موجود.");

        var imagePath = banner.ImagePath;

        _repository.Remove(banner);
        await _repository.SaveChangesAsync(cancellationToken);

        _fileService.Delete(imagePath);
    }

    private static void EnsureSupportedFormat(UploadImageModel image)
    {
        var format = ImageFormatCatalog.Detect(image.Content);

        var isSupported =
            format is not null &&
            (format == ImageFormatCatalog.Jpeg ||
             format == ImageFormatCatalog.Png ||
             format == ImageFormatCatalog.Webp ||
             format == ImageFormatCatalog.Gif);

        if (!isSupported)
        {
            throw new BadRequestException(
                $"صورة البانر لازم تكون واحدة من: {BannerCatalog.AllowedImageFormatNames}.");
        }
    }

    private static void EnsureValidWindow(DateTime? startDate, DateTime? endDate)
    {
        if (startDate is { } start && endDate is { } end && end <= start)
            throw new BadRequestException("تاريخ نهاية البانر لازم يكون بعد تاريخ البداية.");
    }

    private static string? Trim(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private IReadOnlyList<BannerDto> ToDtos(IReadOnlyList<Banner> banners, DateTime utcNow) =>
        banners.Select(banner => ToDto(banner, utcNow)).ToList();

    private BannerDto ToDto(Banner banner, DateTime utcNow)
    {
        var dto = _mapper.Map<BannerDto>(banner);

        dto.IsCurrentlyVisible =
            banner.IsActive &&
            (banner.StartDate is null || banner.StartDate <= utcNow) &&
            (banner.EndDate is null || banner.EndDate >= utcNow);

        return dto;
    }
}
