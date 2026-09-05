using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Clothing;
using Shared.Enums;

namespace Services.Validation;

public class CreateMenClothingRequestValidator : AbstractValidator<CreateMenClothingRequest>
{
    public CreateMenClothingRequestValidator()
    {
        ClothingValidationRules.StoreName(this, x => x.StoreName);

        RuleFor(x => x.SellingMethod)
            .IsInEnum().WithMessage("من فضلك اختار طريقة البيع صح.");

        RuleFor(x => x.ClothingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الملبس صح.");

        RuleFor(x => x.OtherClothingType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الملبس لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.ClothingType == MenClothingType.Other);

        RuleFor(x => x.Brand)
            .IsInEnum().WithMessage("من فضلك اختار الماركة صح.");

        RuleFor(x => x.OtherBrand)
            .NotEmpty().WithMessage("من فضلك اكتب الماركة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.Brand == MenClothingBrand.Other);

        RuleFor(x => x.Sizes)
            .NotEmpty().WithMessage("لازم تختار مقاس واحد على الأقل.")
            .Must(sizes => sizes.All(size => Enum.IsDefined(typeof(MenClothingSize), size)))
            .WithMessage("فيه مقاس أو أكتر من اللي اخترتهم مش صحيح.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(MenClothingColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RuleFor(x => x.OtherColor)
            .NotEmpty().WithMessage("من فضلك اكتب اللون لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.Colors.Contains(MenClothingColor.Other));

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.");

        ClothingValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        ClothingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp, x => x.Email);

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("لازم ترفع صورة واحدة على الأقل.")
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        ClothingValidationRules.Video(this, x => x.Video!);
        ClothingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}

public class UpdateMenClothingRequestValidator : AbstractValidator<UpdateMenClothingRequest>
{
    public UpdateMenClothingRequestValidator()
    {
        ClothingValidationRules.StoreName(this, x => x.StoreName);

        RuleFor(x => x.SellingMethod)
            .IsInEnum().WithMessage("من فضلك اختار طريقة البيع صح.");

        RuleFor(x => x.ClothingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الملبس صح.");

        RuleFor(x => x.OtherClothingType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الملبس لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.ClothingType == MenClothingType.Other);

        RuleFor(x => x.Brand)
            .IsInEnum().WithMessage("من فضلك اختار الماركة صح.");

        RuleFor(x => x.OtherBrand)
            .NotEmpty().WithMessage("من فضلك اكتب الماركة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.Brand == MenClothingBrand.Other);

        RuleFor(x => x.Sizes)
            .NotEmpty().WithMessage("لازم تختار مقاس واحد على الأقل.")
            .Must(sizes => sizes.All(size => Enum.IsDefined(typeof(MenClothingSize), size)))
            .WithMessage("فيه مقاس أو أكتر من اللي اخترتهم مش صحيح.");

        RuleFor(x => x.Colors)
            .NotEmpty().WithMessage("لازم تختار لون واحد على الأقل.")
            .Must(colors => colors.All(color => Enum.IsDefined(typeof(MenClothingColor), color)))
            .WithMessage("فيه لون أو أكتر من اللي اخترتهم مش صحيح.");

        RuleFor(x => x.OtherColor)
            .NotEmpty().WithMessage("من فضلك اكتب اللون لما تختار «أخرى».")
            .MaximumLength(150)
            .When(x => x.Colors.Contains(MenClothingColor.Other));

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("السعر لازم يكون أكبر من صفر.");

        ClothingValidationRules.Location(this, x => x.Center, x => x.Address, x => x.GoogleMaps);
        ClothingValidationRules.Contact(this, x => x.Phone, x => x.WhatsApp, x => x.Email);

        RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");

        ClothingValidationRules.Video(this, x => x.Video!);
        ClothingValidationRules.Advertisement(this, x => x.Title, x => x.Description);
    }
}
