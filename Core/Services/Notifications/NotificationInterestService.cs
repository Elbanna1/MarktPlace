using Domain.Entities;
using ServicesAbstraction;
using Shared.DTOs.Lookups;
using Shared.DTOs.Notifications;
using Shared.Exceptions;

namespace Services.Notifications;

public class NotificationInterestService : INotificationInterestService
{
    private readonly INotificationInterestRepository _repository;
    private readonly ILookupService _lookups;

    public NotificationInterestService(
        INotificationInterestRepository repository, ILookupService lookups)
    {
        _repository = repository;
        _lookups = lookups;
    }

    public async Task<NotificationInterestsDto> GetMineAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var interests = await _repository.GetForUserAsync(userId, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        var preference = await _repository.GetPreferenceAsync(userId, cancellationToken);

        return Compose(interests, tree, preference);
    }

    public async Task<NotificationInterestOptionsDto> GetOptionsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var interests = await _repository.GetForUserAsync(userId, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        var preference = await _repository.GetPreferenceAsync(userId, cancellationToken);

        var byCategory = interests
            .Where(i => i.SubCategoryId is null)
            .ToDictionary(i => i.CategoryId);

        var bySubCategory = interests
            .Where(i => i.SubCategoryId is not null)
            .ToDictionary(i => i.SubCategoryId!.Value);

        var options = new NotificationInterestOptionsDto
        {
            NewListingsEnabled = preference?.NewListingsEnabled ?? true
        };

        foreach (var category in tree)
        {
            var wholeCategory = byCategory.GetValueOrDefault(category.Id);

            var option = new NotificationInterestCategoryOptionDto
            {
                CategoryId = category.Id,
                Name = category.Name,
                NameAr = category.NameAr,
                Icon = category.Icon,
                IsSelected = wholeCategory is not null,
                InterestId = wholeCategory?.Id,
                IsEnabled = wholeCategory?.IsEnabled ?? true
            };

            foreach (var subCategory in category.SubCategories)
            {
                var followed = bySubCategory.GetValueOrDefault(subCategory.Id);

                option.SubCategories.Add(new NotificationInterestSubCategoryOptionDto
                {
                    SubCategoryId = subCategory.Id,
                    Name = subCategory.Name,
                    NameAr = subCategory.NameAr,
                    Icon = subCategory.Icon,
                    IsSelected = followed is not null,
                    InterestId = followed?.Id,
                    IsEnabled = followed?.IsEnabled ?? true,

                    CoveredByCategory = wholeCategory is { IsEnabled: true }
                });
            }

            options.Categories.Add(option);
        }

        return options;
    }

    public async Task<NotificationInterestDto> AddAsync(
        string userId, AddNotificationInterestRequest request,
        CancellationToken cancellationToken = default)
    {
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        Validate(tree, request.CategoryId, request.SubCategoryId);

        var existing = await _repository.FindByKeyAsync(
            userId, request.CategoryId, request.SubCategoryId, cancellationToken);

        if (existing is not null)
        {
            if (!existing.IsEnabled)
            {
                existing.IsEnabled = true;
                existing.UpdatedAt = DateTime.UtcNow;
                await _repository.SaveChangesAsync(cancellationToken);
            }

            return ToDto(existing, tree);
        }

        var interest = new UserNotificationInterest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = request.CategoryId,
            SubCategoryId = request.SubCategoryId,
            IsEnabled = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(interest, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ToDto(interest, tree);
    }

    public async Task<NotificationInterestsDto> ReplaceAsync(
        string userId, ReplaceNotificationInterestsRequest request,
        CancellationToken cancellationToken = default)
    {
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var wanted = request.Interests
            .Select(item => (item.CategoryId, item.SubCategoryId))
            .Distinct()
            .ToList();

        foreach (var (categoryId, subCategoryId) in wanted)
            Validate(tree, categoryId, subCategoryId);

        var existing = await _repository.GetForUserAsync(userId, cancellationToken);

        var wantedKeys = wanted.ToHashSet();
        var existingKeys = existing
            .Select(i => (i.CategoryId, i.SubCategoryId))
            .ToHashSet();

        var removed = existing
            .Where(i => !wantedKeys.Contains((i.CategoryId, i.SubCategoryId)))
            .ToList();

        if (removed.Count > 0)
            _repository.RemoveRange(removed);

        var now = DateTime.UtcNow;

        foreach (var survivor in existing.Except(removed).Where(i => !i.IsEnabled))
        {
            survivor.IsEnabled = true;
            survivor.UpdatedAt = now;
        }

        foreach (var (categoryId, subCategoryId) in wanted.Where(key => !existingKeys.Contains(key)))
        {
            await _repository.AddAsync(new UserNotificationInterest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                IsEnabled = true,
                CreatedAt = now
            }, cancellationToken);
        }

        if (request.NewListingsEnabled is { } enabled)
            await ApplyPreferenceAsync(userId, enabled, now, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return await GetMineAsync(userId, cancellationToken);
    }

    public async Task<NotificationInterestDto> SetEnabledAsync(
        string userId, Guid interestId, bool isEnabled, CancellationToken cancellationToken = default)
    {
        var interest = await _repository.FindAsync(userId, interestId, cancellationToken)
            ?? throw new NotFoundException("الاهتمام غير موجود.");

        if (interest.IsEnabled != isEnabled)
        {
            interest.IsEnabled = isEnabled;
            interest.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync(cancellationToken);
        }

        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        return ToDto(interest, tree);
    }

    public async Task RemoveAsync(
        string userId, Guid interestId, CancellationToken cancellationToken = default)
    {
        var interest = await _repository.FindAsync(userId, interestId, cancellationToken)
            ?? throw new NotFoundException("الاهتمام غير موجود.");

        _repository.Remove(interest);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationPreferencesDto> GetPreferencesAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var preference = await _repository.GetPreferenceAsync(userId, cancellationToken);

        return new NotificationPreferencesDto
        {
            NewListingsEnabled = preference?.NewListingsEnabled ?? true
        };
    }

    public async Task<NotificationPreferencesDto> SetPreferencesAsync(
        string userId, UpdateNotificationPreferencesRequest request,
        CancellationToken cancellationToken = default)
    {
        await ApplyPreferenceAsync(userId, request.NewListingsEnabled, DateTime.UtcNow, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new NotificationPreferencesDto { NewListingsEnabled = request.NewListingsEnabled };
    }

    private async Task ApplyPreferenceAsync(
        string userId, bool newListingsEnabled, DateTime now, CancellationToken cancellationToken)
    {
        var preference = await _repository.GetPreferenceAsync(userId, cancellationToken);

        if (preference is null)
        {
            await _repository.AddPreferenceAsync(new UserNotificationPreference
            {
                UserId = userId,
                NewListingsEnabled = newListingsEnabled,
                CreatedAt = now
            }, cancellationToken);

            return;
        }

        preference.NewListingsEnabled = newListingsEnabled;
        preference.UpdatedAt = now;
    }

    private static void Validate(
        IReadOnlyList<CategoryTreeDto> tree, int categoryId, int? subCategoryId)
    {
        var category = tree.FirstOrDefault(c => c.Id == categoryId)
            ?? throw new BadRequestException("القسم المختار غير موجود.");

        if (subCategoryId is not { } subId)
            return;

        if (category.SubCategories.All(sub => sub.Id != subId))
            throw new BadRequestException("القسم الفرعي المختار لا ينتمي إلى هذا القسم.");
    }

    private static NotificationInterestsDto Compose(
        IReadOnlyList<UserNotificationInterest> interests,
        IReadOnlyList<CategoryTreeDto> tree,
        UserNotificationPreference? preference) => new()
    {
        NewListingsEnabled = preference?.NewListingsEnabled ?? true,
        Interests = interests.Select(interest => ToDto(interest, tree)).ToList()
    };

    private static NotificationInterestDto ToDto(
        UserNotificationInterest interest, IReadOnlyList<CategoryTreeDto> tree)
    {
        var category = tree.FirstOrDefault(c => c.Id == interest.CategoryId);

        var subCategory = interest.SubCategoryId is { } subId
            ? category?.SubCategories.FirstOrDefault(sub => sub.Id == subId)
            : null;

        return new NotificationInterestDto
        {
            Id = interest.Id,
            CategoryId = interest.CategoryId,

            CategoryName = category?.NameAr ?? string.Empty,
            SubCategoryId = interest.SubCategoryId,
            SubCategoryName = subCategory?.NameAr,
            IsEnabled = interest.IsEnabled,
            CreatedAt = interest.CreatedAt,
            UpdatedAt = interest.UpdatedAt
        };
    }
}
