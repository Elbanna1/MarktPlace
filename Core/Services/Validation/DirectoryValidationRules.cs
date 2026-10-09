using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Directory;

namespace Services.Validation;

public static class DirectoryValidationRules
{
    public static void AddDirectoryRules<T>(
        this AbstractValidator<T> validator, DirectoryModule module, bool isCreate)
        where T : CreateDirectoryListingRequest
    {
        validator.RuleFor(x => x.Name)
            .NotEmpty().WithMessage(DirectoryListingRules.NameRequiredMessage(module))
            .MaximumLength(DirectoryListingRules.MaxNameLength);

        validator.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الإعلان مطلوب.")
            .MaximumLength(DirectoryListingRules.MaxTitleLength);

        validator.RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف الإعلان مطلوب.")
            .MaximumLength(DirectoryListingRules.MaxDescriptionLength);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(DirectoryListingRules.MaxAddressLength);

        validator.RuleFor(x => x.GoogleMapsUrl)
            .MaximumLength(DirectoryListingRules.MaxGoogleMapsUrlLength)
            .Must(DirectoryListingRules.IsHttpUrl).WithMessage(DirectoryListingRules.GoogleMapsUrlMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.GoogleMapsUrl));

        validator.RuleFor(x => x.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(DirectoryListingRules.MaxPhoneLength);

        validator.RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(DirectoryListingRules.MaxPhoneLength)
            .When(x => !string.IsNullOrWhiteSpace(x.WhatsApp));

        validator.RuleFor(x => x.Email)
            .EmailAddress().WithMessage(DirectoryListingRules.EmailMessage)
            .MaximumLength(DirectoryListingRules.MaxEmailLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        if (isCreate && module.ImagesRequired)
        {
            validator.RuleFor(x => x.Images)
                .NotEmpty().WithMessage(DirectoryListingRules.ImagesRequiredMessage);
        }

        validator.RuleFor(x => x.Images)
            .Must(images => images.Count <= ImageConstants.MaxImagesPerItem)
            .WithMessage($"مسموح بـ {ImageConstants.MaxImagesPerItem} صور بحد أقصى.");
    }
}

public class DirectoryFilterParamsValidator<T> : AbstractValidator<T>
    where T : DirectoryFilterParams
{
    public DirectoryFilterParamsValidator()
    {
        RuleFor(x => x.Search).MaximumLength(150);
        RuleFor(x => x.Name).MaximumLength(DirectoryListingRules.MaxNameLength);
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("من فضلك اختار ترتيب صح.");
    }
}
