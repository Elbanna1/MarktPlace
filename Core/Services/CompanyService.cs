using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Enums;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Companies;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public CompanyService(
        ICompanyRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<CompanyDetailsDto> CreateAsync(
        string userId, CreateCompanyRequest request,
        IReadOnlyList<UploadImageModel> images, UploadImageModel? logo)
    {
        var company = _mapper.Map<Company>(request);

        company.Id = Guid.NewGuid();
        company.UserId = userId;
        company.CreatedAt = DateTime.UtcNow;

        if (logo is not null)
        {
            var stored = await _fileService.SaveAsync(logo, ImageConstants.CompaniesFolder);
            company.LogoPath = stored.RelativePath;
            company.LogoUrl = stored.Url;
        }

        await AttachImagesAsync(company, images, startAsPrimary: true);

        await _repository.AddAsync(company);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            userId, ListingModuleType.Company, NotificationAction.Created, company.Id, company.Title);

        return await BuildDetailsAsync(company.Id, includeUnmoderated: true);
    }

    public async Task<CompanyDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateCompanyRequest request,
        IReadOnlyList<UploadImageModel> newImages, UploadImageModel? logo)
    {
        var company = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (company is null)
            throw new NotFoundException("الشركة مش موجودة.");

        company.CompanyName = request.CompanyName;
        company.CompanyField = request.CompanyField;
        company.OtherCompanyField = request.OtherCompanyField?.Trim();
        company.Address = request.Address;
        company.GoogleMaps = request.GoogleMaps?.Trim();
        company.Phone = request.Phone;
        company.WhatsApp = request.WhatsApp;
        company.Email = request.Email?.Trim();
        company.Website = request.Website?.Trim();
        company.Title = request.Title;
        company.Description = request.Description;

        if (logo is not null)
        {
            var previousLogoPath = company.LogoPath;
            var stored = await _fileService.SaveAsync(logo, ImageConstants.CompaniesFolder);
            company.LogoPath = stored.RelativePath;
            company.LogoUrl = stored.Url;

            if (!string.IsNullOrWhiteSpace(previousLogoPath))
                _fileService.Delete(previousLogoPath);
        }
        else if (request.RemoveLogo && !string.IsNullOrWhiteSpace(company.LogoPath))
        {
            _fileService.Delete(company.LogoPath);
            company.LogoPath = null;
            company.LogoUrl = null;
        }

        if (request.RemoveImageIds.Count > 0)
        {
            var toRemove = company.Images.Where(i => request.RemoveImageIds.Contains(i.Id)).ToList();
            foreach (var image in toRemove)
            {
                _fileService.Delete(image.ImagePath);
                _repository.RemoveImage(image);
                company.Images.Remove(image);
            }
        }

        if (newImages.Count > 0)
        {
            var startAsPrimary = company.Images.Count == 0;
            var added = await AttachImagesAsync(company, newImages, startAsPrimary);
            await _repository.AddImagesAsync(added);
        }

        ListingImages.NormalizePrimary(company.Images);

        company.UpdatedAt = DateTime.UtcNow;

        _repository.Update(company);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            company.UserId, ListingModuleType.Company,
            isAdmin && company.UserId != userId ? NotificationAction.AdminUpdated : NotificationAction.Updated,
            id, company.Title);

        return await BuildDetailsAsync(id, includeUnmoderated: true);
    }

    public async Task DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        var company = isAdmin
            ? await _repository.GetByIdAsync(id, asNoTracking: false, includeUnmoderated: true)
            : await _repository.GetOwnedAsync(id, userId);

        if (company is null)
            throw new NotFoundException("الشركة مش موجودة.");

        company.IsDeleted = true;
        company.DeletedAt = DateTime.UtcNow;

        _repository.Update(company);
        await _repository.SaveChangesAsync();

        await _notifications.NotifyAsync(
            company.UserId, ListingModuleType.Company,
            isAdmin && company.UserId != userId ? NotificationAction.AdminDeleted : NotificationAction.Deleted,
            id, company.Title);
    }

    public async Task<PaginatedResult<CompanyListItemDto>> GetListAsync(
        CompanyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, cancellationToken);
        var mapped = _mapper.Map<IReadOnlyList<CompanyListItemDto>>(items);
        return new PaginatedResult<CompanyListItemDto>(mapped, total, filter.PageIndex, filter.PageSize);
    }

    public Task<CompanyDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        BuildDetailsAsync(id, cancellationToken);

    public async Task<IReadOnlyList<CompanyFieldOptionDto>> GetCompanyFieldsAsync(
        CancellationToken cancellationToken = default) =>
        _mapper.Map<IReadOnlyList<CompanyFieldOptionDto>>(
            await _repository.GetCompanyFieldsAsync(cancellationToken));

    private async Task<CompanyDetailsDto> BuildDetailsAsync(
        Guid id, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        var company = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken, includeUnmoderated)
            ?? throw new NotFoundException("الشركة مش موجودة.");

        return _mapper.Map<CompanyDetailsDto>(company);
    }

    private Task<List<CompanyImage>> AttachImagesAsync(
        Company company, IReadOnlyList<UploadImageModel> images, bool startAsPrimary) =>
        ListingImages.AttachAsync(
            _fileService,
            company.Images,
            images,
            ImageConstants.CompaniesFolder,
            startAsPrimary,
            _ => new CompanyImage { CompanyId = company.Id });
}
