using Shared.DTOs.Settings;

namespace ServicesAbstraction;

public interface IPublicSettingsService
{
    Task<PublicSettingsDto> GetAsync(CancellationToken cancellationToken = default);
}
