using FluentValidation;
using Shared.DTOs.Notifications;

namespace Services.Validation;

public class AddNotificationInterestRequestValidator : AbstractValidator<AddNotificationInterestRequest>
{
    public AddNotificationInterestRequestValidator() => ApplyRules(this);

    internal static void ApplyRules<T>(AbstractValidator<T> validator, string prefix = "")
        where T : AddNotificationInterestRequest
    {
        validator.RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage($"{prefix}القسم مطلوب.");

        validator.RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage($"{prefix}القسم الفرعي غير صحيح.")
            .When(x => x.SubCategoryId.HasValue);
    }
}

public class ReplaceNotificationInterestsRequestValidator
    : AbstractValidator<ReplaceNotificationInterestsRequest>
{
    private const int MaxInterests = 100;

    public ReplaceNotificationInterestsRequestValidator()
    {
        RuleFor(x => x.Interests)
            .NotNull().WithMessage("قائمة الاهتمامات مطلوبة.")
            .Must(interests => interests.Count <= MaxInterests)
            .WithMessage($"لا يمكن اختيار أكثر من {MaxInterests} اهتمام.");

        RuleForEach(x => x.Interests).ChildRules(item =>
        {
            item.RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("القسم مطلوب.");

            item.RuleFor(x => x.SubCategoryId)
                .GreaterThan(0).WithMessage("القسم الفرعي غير صحيح.")
                .When(x => x.SubCategoryId.HasValue);
        });
    }
}
