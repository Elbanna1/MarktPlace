using FluentValidation;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Services.Validation;

public class UpdatePaintingRequestValidator : AbstractValidator<UpdatePaintingRequest>
{
    public UpdatePaintingRequestValidator()
    {
        AntiqueValidationRules.Seller(
            this, x => x.SellerName, x => x.Phone, x => x.WhatsApp,
            sellerNameMessage: "اسم الفنان أو البائع مطلوب.");

        RuleFor(x => x.PaintingName)
            .NotEmpty().WithMessage("اسم اللوحة مطلوب.")
            .MaximumLength(150);

        RuleFor(x => x.PaintingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع اللوحة صح.");

        RuleFor(x => x.OtherType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع اللوحة.")
            .MaximumLength(150)
            .When(x => x.PaintingType == PaintingType.Other);

        RuleFor(x => x.ArtistName)
            .NotEmpty().WithMessage("اسم الفنان مطلوب.")
            .MaximumLength(150);

        AntiqueValidationRules.Year(this, x => x.ExecutionYear, "ExecutionYear");
        AntiqueValidationRules.Measurement(this, x => x.Width, "Width", "العرض");
        AntiqueValidationRules.Measurement(this, x => x.Height, "Height", "الارتفاع");

        RuleFor(x => x.Material)
            .IsInEnum().WithMessage("من فضلك اختار الخامة صح.");

        RuleFor(x => x.OtherMaterial)
            .NotEmpty().WithMessage("من فضلك اكتب الخامة.")
            .MaximumLength(150)
            .When(x => x.Material == PaintingMaterial.Other);

        RuleFor(x => x.Originality)
            .IsInEnum().WithMessage("من فضلك اختار الأصلية صح.");

        AntiqueValidationRules.Price(this, x => x.Price);
        AntiqueValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        AntiqueValidationRules.Advertisement(this, x => x.Title, x => x.Description);
        AntiqueValidationRules.Images(this, x => x.Images, required: false);
        AntiqueValidationRules.Video(this, x => x.Video);
    }
}
