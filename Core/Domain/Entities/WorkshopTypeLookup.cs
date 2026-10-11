namespace Domain.Entities;

public class WorkshopTypeLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}
