using FluentValidation;
using Shared.DTOs.Lookups;

namespace Services.Validation;

public class AdvertiserLookupParamsValidator : AbstractValidator<AdvertiserLookupParams>
{
    public AdvertiserLookupParamsValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(AdvertiserLookupParams.MaxSearchLength);
    }
}
