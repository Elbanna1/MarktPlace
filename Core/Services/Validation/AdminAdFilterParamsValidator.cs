using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Admin;

namespace Services.Validation;

public class AdminAdFilterParamsValidator : AbstractValidator<AdminAdFilterParams>
{
    public AdminAdFilterParamsValidator()
    {
        RuleFor(x => x.ExpiringWithinDays)
            .InclusiveBetween(1, ListingLifecycle.ActiveDurationDays)
            .WithMessage($"عدد الأيام لازم يكون من 1 لـ {ListingLifecycle.ActiveDurationDays}.")
            .When(x => x.ExpiringWithinDays.HasValue);
    }
}
