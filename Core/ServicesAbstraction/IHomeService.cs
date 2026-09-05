using Shared.DTOs.Home;

namespace ServicesAbstraction;

public interface IHomeService
{
    Task<HomeConfigurationDto> GetAsync(CancellationToken cancellationToken = default);
}
