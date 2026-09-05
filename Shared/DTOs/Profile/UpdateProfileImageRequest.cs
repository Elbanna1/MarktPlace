using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Profile;

public class UpdateProfileImageRequest
{
    public IFormFile? ProfileImage { get; set; }
}
