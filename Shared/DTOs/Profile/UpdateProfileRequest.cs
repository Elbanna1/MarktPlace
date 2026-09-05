using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Profile;

public class UpdateProfileRequest
{
    public string FirstName { get; set; } = default!;
    public string SecondName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Username { get; set; } = default!;

    public IFormFile? ProfileImage { get; set; }
}
