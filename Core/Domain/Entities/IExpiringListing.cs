namespace Domain.Entities;

public interface IExpiringListing
{
    DateTime? PublishedAt { get; set; }

    DateTime? ExpireAt { get; set; }

    DateTime? FirstPublishedAt { get; set; }

    int RepublishCount { get; set; }
}
