using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;

namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsPhoneNumberTakenAsync(
        string phoneNumber, string? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.Where(u => u.PhoneNumber == phoneNumber);

        if (!string.IsNullOrEmpty(excludeUserId))
            query = query.Where(u => u.Id != excludeUserId);

        return query.AnyAsync(cancellationToken);
    }

    public Task<ApplicationUser?> FindByPasswordResetTokenHashAsync(string tokenHash) =>
        _context.Users.FirstOrDefaultAsync(u => u.PasswordResetTokenHash == tokenHash);

    public Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken) =>
        _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

    public async Task<IReadOnlyList<string>> GetUserIdsInRoleAsync(
        string roleName, CancellationToken cancellationToken = default)
    {
        var normalized = roleName.ToUpperInvariant();

        return await _context.UserRoles
            .AsNoTracking()
            .Where(userRole => _context.Roles
                .Any(role => role.Id == userRole.RoleId && role.NormalizedName == normalized))
            .Select(userRole => userRole.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetRoleNamesAsync(
        string userId, CancellationToken cancellationToken = default) =>
        await _context.UserRoles
            .AsNoTracking()
            .Where(userRole => userRole.UserId == userId)
            .Join(_context.Roles, userRole => userRole.RoleId, role => role.Id, (_, role) => role.Name)
            .Where(name => name != null)
            .Select(name => name!)
            .ToListAsync(cancellationToken);

    public async Task<UserDisplayName?> GetDisplayNameAsync(
        string userId, CancellationToken cancellationToken = default) =>
        await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new UserDisplayName(user.FirstName, user.SecondName, user.UserName))
            .FirstOrDefaultAsync(cancellationToken);
}
