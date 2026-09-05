using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public class JobOpportunity : IModeratedListing, IExpiringListing
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string EmployerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string WhatsApp { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public JobField JobField { get; set; }

    public string? OtherJobField { get; set; }

    public JobExperienceLevel RequiredExperience { get; set; }

    public WorkType WorkType { get; set; }

    public SalaryType SalaryType { get; set; }

    public decimal? Salary { get; set; }

    public string Governorate { get; set; } = default!;

    public string? Center { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string? LogoPath { get; set; }

    public string? LogoUrl { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<JobOpportunityImage> Images { get; set; } = new List<JobOpportunityImage>();

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime? FirstPublishedAt { get; set; }

    public int RepublishCount { get; set; }

    public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;

    public DateTime? ModeratedAt { get; set; }

    public string? ModeratedBy { get; set; }

    public ListingRejectionReason? RejectionReason { get; set; }

    public string? ModerationNotes { get; set; }

    [NotMapped]
    public string OwnerUserId => UserId;

    [NotMapped]
    public string ListingTitle => Title;
}

public class JobOpportunityImage : IListingImage
{
    public Guid Id { get; set; }

    public Guid JobOpportunityId { get; set; }
    public JobOpportunity JobOpportunity { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
