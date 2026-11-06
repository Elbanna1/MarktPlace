using Shared.Enums;

namespace Domain.Entities;

public class CatalogConfiguration
{
    public int Id { get; set; }

    public CatalogConfigurationKind Kind { get; set; }

    public CatalogNodeType NodeType { get; set; }

    public int NodeId { get; set; }

    public CatalogArea Area { get; set; }

    public bool IsEnabled { get; set; }

    public bool? IsVisible { get; set; }

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public DateTime? ActiveFrom { get; set; }

    public DateTime? ActiveUntil { get; set; }

    public DateTime? EnabledAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}
