using Shared.DTOs.Account;

namespace ServicesAbstraction;

public interface IAccountService
{
    Task<DeactivatedAccountDto> DeactivateAsync(
        string userId, DeactivateAccountRequest request, CancellationToken cancellationToken = default);
}
