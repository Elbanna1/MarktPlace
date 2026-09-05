using Domain.Entities;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;

namespace ServicesAbstraction;

public interface IAdminSettingsService
{
    Task<AdminSettingsDto> GetAsync(CancellationToken cancellationToken = default);

    Task<AdminSettingsDto> UpdateAsync(
        string adminUserId, UpdateSettingsRequest request, CancellationToken cancellationToken = default);

    Task<AdminSettingsDto> UpdateLogoAsync(
        string adminUserId, UploadImageModel? logo, CancellationToken cancellationToken = default);

    Task<AdminSettingsDto> UpdateFaviconAsync(
        string adminUserId, UploadImageModel? favicon, CancellationToken cancellationToken = default);

    Task<AdminUploadResultDto> UploadAsync(
        UploadImageModel? file, string folder, AdminUploadKind kind,
        CancellationToken cancellationToken = default);

    AdminUploadLimitsDto GetUploadLimits();
}

public enum AdminUploadKind
{
    Image = 1,

    Document = 2,

    Video = 3
}

public interface IAdminSettingsRepository
{
    Task<PlatformSetting> GetAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
