namespace Shared.DTOs.Profile;

public class ProfileDto : UserDto
{
    public ProfileStatisticsDto Statistics { get; set; } = new();
}
