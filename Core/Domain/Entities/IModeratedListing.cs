using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace Domain.Entities;

public interface IModeratedListing
{
    ModerationStatus ModerationStatus { get; set; }

    DateTime? ModeratedAt { get; set; }

    string? ModeratedBy { get; set; }

    ListingRejectionReason? RejectionReason { get; set; }

    string? ModerationNotes { get; set; }

    [NotMapped]
    string OwnerUserId { get; }

    [NotMapped]
    string ListingTitle { get; }
}
