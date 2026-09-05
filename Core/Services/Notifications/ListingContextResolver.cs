using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;

namespace Services.Notifications;

public sealed record ListingNotificationContext(
    Guid ListingId,
    ListingModuleType ListingType,
    string? Title,
    string? ImageUrl,
    int CategoryId,
    string? CategoryName,
    int SubCategoryId,
    string? SubCategoryName,
    string OwnerId,
    string? OwnerName,
    ListingStatus Status);

public static class ListingContextResolver
{
    public static async Task<ListingNotificationContext?> ResolveAsync(
        IUserListingRepository listings,
        ILookupService lookups,
        IUserRepository users,
        ListingModuleType listingType,
        Guid listingId,
        CancellationToken cancellationToken = default)
    {
        var row = await listings.GetByKeyAsync(
            listingType, listingId, DateTime.UtcNow, cancellationToken, includeUnmoderated: true);

        if (row is null)
            return null;

        return await FromRowAsync(lookups, users, row, cancellationToken);
    }

    public static async Task<ListingNotificationContext> FromRowAsync(
        ILookupService lookups,
        IUserRepository users,
        Shared.DTOs.Listings.UserListingRow row,
        CancellationToken cancellationToken = default)
    {
        var (categoryId, subCategoryId) = Listings.ListingInteractionService.ResolveCategory(row);

        var tree = await lookups.GetCategoriesTreeAsync(cancellationToken);
        var (categoryName, subCategoryName) =
            Listings.ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

        return new ListingNotificationContext(
            ListingId: row.Id,
            ListingType: row.Type,
            Title: row.Title,
            ImageUrl: row.MainImageUrl,
            CategoryId: categoryId,
            CategoryName: string.IsNullOrWhiteSpace(categoryName) ? null : categoryName,
            SubCategoryId: subCategoryId,
            SubCategoryName: string.IsNullOrWhiteSpace(subCategoryName) ? null : subCategoryName,
            OwnerId: row.OwnerId,
            OwnerName: await DisplayNameAsync(users, row.OwnerId, cancellationToken),
            Status: row.Status);
    }

    public static async Task<string?> DisplayNameAsync(
        IUserRepository users, string? userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var user = await users.GetDisplayNameAsync(userId, cancellationToken);

        if (user is null)
            return null;

        var full = string.Join(' ', new[] { user.FirstName, user.SecondName }
            .Where(part => !string.IsNullOrWhiteSpace(part)));

        return string.IsNullOrWhiteSpace(full) ? user.UserName : full;
    }
}
