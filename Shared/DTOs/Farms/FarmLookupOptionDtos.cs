namespace Shared.DTOs.Farms;

public class FarmTypeOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Group { get; set; } = default!;

    public string GroupAr { get; set; } = default!;
}

public class FarmingMethodOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}

public class AvailabilitySeasonOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
