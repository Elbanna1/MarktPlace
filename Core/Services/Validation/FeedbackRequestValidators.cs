using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Feedback;

namespace Services.Validation;

public class CreateFeedbackRequestValidator : AbstractValidator<CreateFeedbackRequest>
{
    public CreateFeedbackRequestValidator()
    {
        RuleFor(x => x.Rating)
            .NotNull()
            .WithMessage($"التقييم مطلوب ويجب أن يكون رقمًا بين {FeedbackCatalog.MinRating} و {FeedbackCatalog.MaxRating}.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(FeedbackCatalog.MinRating, FeedbackCatalog.MaxRating)
            .WithMessage($"التقييم يجب أن يكون بين {FeedbackCatalog.MinRating} و {FeedbackCatalog.MaxRating}.")
            .When(x => x.Rating.HasValue);

        RuleFor(x => x.Title)
            .MaximumLength(FeedbackCatalog.MaxTitleLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(FeedbackCatalog.MaxDescriptionLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("نوع الملاحظة غير صحيح.");

        RuleFor(x => x.Images)
            .Must(images => images == null || images.Count <= FeedbackCatalog.MaxImages)
            .WithMessage($"عدد الصور يجب ألا يزيد عن {FeedbackCatalog.MaxImages}.");
    }
}

public class UpdateFeedbackStatusRequestValidator : AbstractValidator<UpdateFeedbackStatusRequest>
{
    public UpdateFeedbackStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("حالة الملاحظة غير صحيحة.");

        RuleFor(x => x.AdminReply)
            .MaximumLength(FeedbackCatalog.MaxAdminReplyLength)
            .When(x => !string.IsNullOrWhiteSpace(x.AdminReply));
    }
}
