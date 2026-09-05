using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Listings;
using Shared.DTOs.Profile;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IUserListingRepository _userListingRepository;
    private readonly IListingInteractionRepository _interactions;
    private readonly ILookupService _lookups;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;

    public ProfileService(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        IUserListingRepository userListingRepository,
        IListingInteractionRepository interactions,
        ILookupService lookups,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _userListingRepository = userListingRepository;
        _interactions = interactions;
        _lookups = lookups;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
    }

    public async Task<ProfileDto> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        var profile = _mapper.Map<ProfileDto>(user);
        profile.Statistics = await BuildStatisticsAsync(userId, cancellationToken);

        return profile;
    }

    public async Task<UserDto> UpdateProfileAsync(
        string userId,
        UpdateProfileRequest request,
        UploadImageModel? profileImage = null,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        var username = AccountNameRules.NormalizeWhitespace(request.Username);
        var byUsername = await _userManager.FindByNameAsync(username);
        if (byUsername is not null && byUsername.Id != user.Id)
            throw new ConflictException("اسم المستخدم ده متاخد قبل كده.");

        var byEmail = await _userManager.FindByEmailAsync(request.Email);
        if (byEmail is not null && byEmail.Id != user.Id)
            throw new ConflictException("البريد الإلكتروني ده مسجل قبل كده.");

        if (await _userRepository.IsPhoneNumberTakenAsync(request.Phone, user.Id))
            throw new ConflictException("رقم الموبايل ده مسجل قبل كده.");

        StoredFile? storedImage = null;
        var previousImagePath = user.ProfileImagePath;

        if (profileImage is not null)
        {
            storedImage = await _fileService.SaveAsync(profileImage, ImageConstants.ProfileFolder, cancellationToken);

            user.ProfileImagePath = storedImage.RelativePath;
            user.ProfileImageUrl = storedImage.Url;
        }

        try
        {
            user.FirstName = AccountNameRules.NormalizeWhitespace(request.FirstName);
            user.SecondName = AccountNameRules.NormalizeWhitespace(request.SecondName);
            user.PhoneNumber = request.Phone.Trim();
            user.Center = request.Center;

            user.Governorate = LocationConstants.Governorate;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.Equals(user.Email, request.Email.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var setEmail = await _userManager.SetEmailAsync(user, request.Email.Trim());
                if (!setEmail.Succeeded)
                    throw new BadRequestException("مش قادرين نغير البريد الإلكتروني.", DescribeErrors(setEmail));
            }

            if (!string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase))
            {
                var setUserName = await _userManager.SetUserNameAsync(user, username);
                if (!setUserName.Succeeded)
                    throw new BadRequestException("مش قادرين نغير اسم المستخدم.", DescribeErrors(setUserName));
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException("مش قادرين نحفظ تعديلات الحساب. حاول تاني.", DescribeErrors(result));
        }
        catch
        {
            if (storedImage is not null)
                _fileService.Delete(storedImage.RelativePath);

            throw;
        }

        if (storedImage is not null)
            DeleteReplacedImage(previousImagePath, storedImage.RelativePath);

        await _notifications.NotifyAsync(
            userId, NotificationCatalog.Profile, NotificationAction.Updated);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateProfileImageAsync(
        string userId,
        UploadImageModel? profileImage,
        CancellationToken cancellationToken = default)
    {
        if (profileImage is null)
            throw new BadRequestException("صورة الحساب مطلوبة.");

        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        var previousImagePath = user.ProfileImagePath;
        var storedImage = await _fileService.SaveAsync(profileImage, ImageConstants.ProfileFolder, cancellationToken);

        user.ProfileImagePath = storedImage.RelativePath;
        user.ProfileImageUrl = storedImage.Url;
        user.UpdatedAt = DateTime.UtcNow;

        IdentityResult result;
        try
        {
            result = await _userManager.UpdateAsync(user);
        }
        catch
        {
            _fileService.Delete(storedImage.RelativePath);
            throw;
        }

        if (!result.Succeeded)
        {
            _fileService.Delete(storedImage.RelativePath);
            throw new BadRequestException("مش قادرين نحفظ صورة الحساب. حاول تاني.", DescribeErrors(result));
        }

        DeleteReplacedImage(previousImagePath, storedImage.RelativePath);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<PaginatedResult<UserListingDto>> GetMyListingsAsync(
        string userId,
        UserListingFilterParams filter,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var (rows, totalCount) = await _userListingRepository.GetPagedAsync(userId, filter, now, cancellationToken);

        var items = await ToListingDtosAsync(rows, now, cancellationToken);

        return new PaginatedResult<UserListingDto>(items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<PaginatedResult<UserListingDto>> GetExpiredListingsAsync(
        string userId,
        UserListingFilterParams filter,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var expiredOnly = new UserListingFilterParams
        {
            PageIndex = filter.PageIndex,
            PageSize = filter.PageSize,
            Type = filter.Type,
            Status = ListingStatus.Expired
        };

        var (rows, totalCount) = await _userListingRepository.GetPagedAsync(
            userId, expiredOnly, now, cancellationToken);

        var items = await ToListingDtosAsync(rows, now, cancellationToken);

        return new PaginatedResult<UserListingDto>(items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("المستخدم مش موجود.");

        if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
            throw new BadRequestException("كلمة السر القديمة غلط.");

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new BadRequestException("مش قادرين نغير كلمة السر. حاول تاني.", DescribeErrors(result));

        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        await AuthService.NotifyPasswordChangedAsync(_notifications, userId);
    }

    private async Task<ProfileStatisticsDto> BuildStatisticsAsync(string userId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var counts = await _userListingRepository.GetStatusCountsAsync(userId, now, cancellationToken);

        var ownedKeys = await _userListingRepository.GetOwnedKeysAsync(userId, now, cancellationToken);
        var (totalViews, totalFavorites) = await _interactions.GetOwnerTotalsAsync(ownedKeys, cancellationToken);

        var byType = counts
            .GroupBy(count => count.Type)
            .Select(group => new ListingTypeStatisticsDto
            {
                Type = group.Key.ToString(),
                Total = group.Sum(count => count.Count),
                Active = CountOf(group, ListingStatus.Active),
                Expired = CountOf(group, ListingStatus.Expired),
                Pending = CountOf(group, ListingStatus.Pending),
                Rejected = CountOf(group, ListingStatus.Rejected)
            })
            .OrderBy(statistics => statistics.Type)
            .ToList();

        return new ProfileStatisticsDto
        {
            TotalListings = byType.Sum(statistics => statistics.Total),
            ActiveListings = byType.Sum(statistics => statistics.Active),
            ExpiredListings = byType.Sum(statistics => statistics.Expired),
            PendingListings = byType.Sum(statistics => statistics.Pending),
            RejectedListings = byType.Sum(statistics => statistics.Rejected),
            TotalViews = totalViews,
            TotalFavorites = totalFavorites,
            ByType = byType
        };
    }

    private static int CountOf(IEnumerable<UserListingStatusCount> counts, ListingStatus status) =>
        counts.Where(count => count.Status == status).Sum(count => count.Count);

    private async Task<List<UserListingDto>> ToListingDtosAsync(
        IReadOnlyList<UserListingRow> rows, DateTime utcNow, CancellationToken cancellationToken)
    {
        if (rows.Count == 0)
            return new List<UserListingDto>();

        var keys = rows.Select(row => (row.Type, row.Id)).ToList();

        var counters = await _interactions.GetCountersAsync(keys, userId: null, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return rows.Select(row =>
        {
            var counter = counters.GetValueOrDefault((row.Type, row.Id));
            var (categoryId, subCategoryId) = Services.Listings.ListingInteractionService.ResolveCategory(row);
            var (categoryName, subCategoryName) =
                Services.Listings.ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

            return ToListingDto(
                row, utcNow, categoryId, categoryName, subCategoryId, subCategoryName, counter);
        }).ToList();
    }

    private static UserListingDto ToListingDto(
        UserListingRow row,
        DateTime utcNow,
        int categoryId,
        string categoryName,
        int subCategoryId,
        string subCategoryName,
        ListingCountersDto? counters)
    {
        var isExpired = row.Status == ListingStatus.Expired;

        var remainingDays = isExpired ? 0 : ListingLifecycle.RemainingDays(row.ExpireAt, utcNow);

        ExpiredSinceDto? expiredSince = null;
        if (isExpired && row.ExpireAt is { } expiredAt)
        {
            var days = (int)Math.Floor((utcNow - expiredAt).TotalDays);
            days = days < 0 ? 0 : days;

            expiredSince = new ExpiredSinceDto
            {
                Days = days,
                ExpiredAt = expiredAt,
                Text = ListingInteractionCatalog.ElapsedText(days)
            };
        }

        return new UserListingDto
        {
            Id = row.Id,
            Type = row.Type.ToString(),
            TypeId = row.Type,
            Route = ListingModuleCatalog.RouteOf(row.Type),
            Title = row.Title,
            CategoryId = categoryId,
            CategoryName = categoryName,
            SubCategoryId = subCategoryId,
            SubCategoryName = subCategoryName,
            MainImageUrl = row.MainImageUrl,
            Price = row.Price,
            Status = row.Status.ToString(),
            CreatedAt = row.CreatedAt,
            StartDate = row.PublishedAt,
            EndDate = row.ExpireAt,

            ExpireAt = row.ExpireAt,
            RemainingDays = remainingDays,
            IsExpired = isExpired,
            ExpiredSince = expiredSince,

            CanRepublish = row.SupportsRepublish && isExpired,
            Views = counters?.Views ?? 0,
            FavoriteCount = counters?.FavoriteCount ?? 0,
            AverageRating = counters?.AverageRating,
            RatingsCount = counters?.RatingsCount ?? 0,

            Moderation = ListingModerationDto.ForOwner(row)
        };
    }

    private void DeleteReplacedImage(string? previousImagePath, string newImagePath)
    {
        if (!string.IsNullOrWhiteSpace(previousImagePath) &&
            !string.Equals(previousImagePath, newImagePath, StringComparison.OrdinalIgnoreCase))
        {
            _fileService.Delete(previousImagePath);
        }
    }

    private static IReadOnlyList<string> DescribeErrors(IdentityResult result) =>
        result.Errors.Select(e => e.Description).ToList();
}
