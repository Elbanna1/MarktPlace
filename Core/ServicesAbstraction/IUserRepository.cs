using Domain.Entities;

namespace ServicesAbstraction;

public interface IUserRepository
{
    Task<bool> IsPhoneNumberTakenAsync(
        string phoneNumber, string? excludeUserId = null, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> FindByPasswordResetTokenHashAsync(string tokenHash);

    Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken);

    Task<IReadOnlyList<string>> GetUserIdsInRoleAsync(
        string roleName, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRoleNamesAsync(
        string userId, CancellationToken cancellationToken = default);

    Task<UserDisplayName?> GetDisplayNameAsync(
        string userId, CancellationToken cancellationToken = default);
}

public sealed record UserDisplayName(string? FirstName, string? SecondName, string? UserName);
