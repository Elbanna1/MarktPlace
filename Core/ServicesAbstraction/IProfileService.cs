using Shared.DTOs.Advertisements;
using Shared.DTOs.Listings;
using Shared.DTOs.Profile;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IProfileService
{
    Task<ProfileDto> GetProfileAsync(string userId, CancellationToken cancellationToken = default);

    Task<UserDto> UpdateProfileAsync(
        string userId,
        UpdateProfileRequest request,
        UploadImageModel? profileImage = null,
        CancellationToken cancellationToken = default);

    Task<UserDto> UpdateProfileImageAsync(
        string userId,
        UploadImageModel? profileImage,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<UserListingDto>> GetMyListingsAsync(
        string userId,
        UserListingFilterParams filter,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<UserListingDto>> GetExpiredListingsAsync(
        string userId,
        UserListingFilterParams filter,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
