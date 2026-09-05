namespace Shared.DTOs.Profile;

public class UserDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string SecondName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string? ProfileImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}
