using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Listings;

namespace Services.Validation;

public class RateListingRequestValidator : AbstractValidator<RateListingRequest>
{
    public RateListingRequestValidator()
    {
        RuleFor(x => x.Rating)
            .NotNull().WithMessage("التقييم مطلوب.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(ListingInteractionCatalog.MinRating, ListingInteractionCatalog.MaxRating)
            .WithMessage(ListingInteractionCatalog.RatingRangeMessage)
            .When(x => x.Rating.HasValue);
    }
}
