using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Advertisements;

namespace Services.Validation;

public class AdvertisementFilterParamsValidator : AbstractValidator<AdvertisementFilterParams>
{
    public AdvertisementFilterParamsValidator()
    {
        RuleFor(x => x.FeatureIds)
            .Must(ids => ids == null ||
                         ids.Distinct().Count() <= AdvertisementConstants.MaxFeatureSelection)
            .WithMessage("اخترت مميزات أكتر من المسموح.");
    }
}
