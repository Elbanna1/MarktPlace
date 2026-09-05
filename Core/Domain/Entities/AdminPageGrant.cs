namespace Domain.Entities;

public class AdminPageGrant
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string AdminUserId { get; set; } = default!;

    public ApplicationUser? AdminUser { get; set; }

    public string PageKey { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? GrantedBy { get; set; }

    public ICollection<AdminPagePermission> Permissions { get; set; } = new List<AdminPagePermission>();
}

public class AdminPagePermission
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AdminPageGrantId { get; set; }

    public AdminPageGrant? Grant { get; set; }

    public int Permission { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
