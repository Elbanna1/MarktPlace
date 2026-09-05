using FluentValidation;
using Shared.Constants;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Validation;

public class CreateLandRequestValidator : AbstractValidator<CreateLandRequest>
{
    public CreateLandRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddLandRules();
    }
}

public class UpdateLandRequestValidator : AbstractValidator<UpdateLandRequest>
{
    public UpdateLandRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddLandRules();
    }
}

internal static class LandValidationRules
{
    public static void AddLandRules<T>(this AbstractValidator<T> validator)
        where T : CreateLandRequest
    {
        validator.RuleFor(x => x.LandType)
            .IsInEnum().WithMessage("من فضلك اختار نوع الأرض.");

        validator.RuleFor(x => x.OtherLandType)
            .NotEmpty().WithMessage("من فضلك اكتب اسم نوع الأرض عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.LandType == LandType.Other);

        validator.RuleFor(x => x.AreaUnit)
            .NotNull().WithMessage("وحدة المساحة مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار وحدة مساحة صحيحة.");

        validator.RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("المساحة مطلوبة ويجب أن تكون أكبر من صفر.");

        validator.RuleFor(x => x.TotalPrice)
            .NotNull().WithMessage("إجمالي السعر مطلوب.")
            .GreaterThan(0).WithMessage("إجمالي السعر يجب أن يكون أكبر من صفر.")
            .When(IsSale);

        validator.RuleFor(x => x.PricePerMeter)
            .NotNull().WithMessage("سعر المتر مطلوب.")
            .GreaterThan(0).WithMessage("سعر المتر يجب أن يكون أكبر من صفر.")
            .When(IsSale);

        validator.RuleFor(x => x.Length)
            .NotNull().WithMessage("طول الأرض مطلوب.")
            .GreaterThan(0).WithMessage("الطول يجب أن يكون أكبر من صفر.");

        validator.RuleFor(x => x.Width)
            .NotNull().WithMessage("عرض الأرض مطلوب.")
            .GreaterThan(0).WithMessage("العرض يجب أن يكون أكبر من صفر.");

        validator.RuleFor(x => x.FacadeLength)
            .NotNull().WithMessage("طول الواجهة مطلوب.")
            .GreaterThan(0).WithMessage("طول الواجهة يجب أن يكون أكبر من صفر.");

        validator.RuleFor(x => x.StreetWidth)
            .GreaterThan(0).WithMessage("عرض الشارع يجب أن يكون أكبر من صفر.")
            .When(x => x.StreetWidth.HasValue);

        validator.RuleFor(x => x.FacadesCount)
            .IsInEnum().WithMessage("من فضلك اختار عدد واجهات صحيح.")
            .When(x => x.FacadesCount.HasValue);

        validator.RuleFor(x => x.Direction)
            .NotNull().WithMessage("اتجاه الأرض مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار اتجاه صحيح.");

        validator.RuleFor(x => x.RoadType)
            .IsInEnum().WithMessage("من فضلك اختار نوع طريق صحيح.")
            .When(x => x.RoadType.HasValue);

        validator.RuleFor(x => x.AllowedBuildingRatio)
            .InclusiveBetween(0, 100).WithMessage("نسبة البناء المسموح بها يجب أن تكون بين 0 و 100.")
            .When(x => x.IsBuildable == true && x.AllowedBuildingRatio.HasValue);

        validator.RuleFor(x => x.AllowedFloorsCount)
            .GreaterThan(0).WithMessage("عدد الأدوار المسموح بها يجب أن يكون أكبر من صفر.")
            .When(x => x.IsBuildable == true && x.AllowedFloorsCount.HasValue);

        validator.RuleFor(x => x.LegalStatus)
            .NotNull().WithMessage("حالة الأرض مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة أرض صحيحة.");

        validator.RuleFor(x => x.LicenseNumber)
            .MaximumLength(100)
            .When(x => x.LegalStatus == LandLegalStatus.Licensed);

        validator.RuleFor(x => x.LicenseIssuer)
            .MaximumLength(150)
            .When(x => x.LegalStatus == LandLegalStatus.Licensed);

        validator.AddLicenceWindowRule(x => x.LicenseIssueDate, x => x.LicenseExpiryDate);

        validator.RuleFor(x => x.ReconciliationForm)
            .NotNull().WithMessage("نموذج التصالح مطلوب عند اختيار 'تصالح'.")
            .IsInEnum().WithMessage("من فضلك اختار نموذج تصالح صحيح.")
            .When(x => x.LegalStatus == LandLegalStatus.Reconciliation);

        validator.RuleFor(x => x.OtherReconciliationForm)
            .NotEmpty().WithMessage("من فضلك اكتب رقم أو اسم النموذج عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.LegalStatus == LandLegalStatus.Reconciliation &&
                       x.ReconciliationForm == LandReconciliationForm.Other);

        validator.RuleFor(x => x.OwnershipDocument)
            .NotNull().WithMessage("نوع مستند الملكية مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع مستند ملكية صحيح.");

        validator.RuleFor(x => x.OtherOwnershipDocument)
            .NotEmpty().WithMessage("من فضلك اكتب اسم المستند عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.OwnershipDocument == LandOwnershipDocument.Other);

        validator.RuleFor(x => x.ViolationDetails)
            .NotEmpty().WithMessage("من فضلك اكتب تفاصيل المخالفات.")
            .MaximumLength(4000)
            .When(x => x.HasViolations == true);

        validator.RuleForEach(x => x.Utilities)
            .IsInEnum().WithMessage("من فضلك اختار مرافق صحيحة.");

        validator.RuleFor(x => x.Utilities)
            .Must(utilities => utilities.Distinct().Count() == utilities.Count)
            .WithMessage("لا يمكن تكرار نفس المرفق.");

        validator.RuleFor(x => x.RentType)
            .NotNull().WithMessage("نوع الإيجار مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع إيجار صحيح.")
            .When(IsRent);

        validator.RuleFor(x => x.RentValue)
            .NotNull().WithMessage("قيمة الإيجار مطلوبة.")
            .GreaterThan(0).WithMessage("قيمة الإيجار يجب أن تكون أكبر من صفر.")
            .When(IsRent);

        validator.RuleFor(x => x.SecurityDeposit)
            .GreaterThanOrEqualTo(0).WithMessage("قيمة التأمين لا يمكن أن تكون سالبة.")
            .When(x => IsRent(x) && x.SecurityDeposit.HasValue);

        validator.RuleFor(x => x.DownPayment)
            .GreaterThanOrEqualTo(0).WithMessage("المقدم لا يمكن أن يكون سالبًا.")
            .When(x => IsRent(x) && x.DownPayment.HasValue);

        validator.RuleFor(x => x.MinimumRentPeriod)
            .IsInEnum().WithMessage("من فضلك اختار حد أدنى صحيح لمدة الإيجار.")
            .When(x => IsRent(x) && x.MinimumRentPeriod.HasValue);

        validator.RuleFor(x => x.ContractDuration)
            .IsInEnum().WithMessage("من فضلك اختار مدة عقد صحيحة.")
            .When(x => IsRent(x) && x.ContractDuration.HasValue);

        validator.RuleFor(x => x.OwnerConditions)
            .MaximumLength(4000)
            .When(IsRent);

        validator.RuleForEach(x => x.RentInclusions)
            .IsInEnum().WithMessage("من فضلك اختار بنود إيجار صحيحة.")
            .When(IsRent);

        validator.AddAvailabilityWindowRule(x => x.AvailableFrom, x => x.AvailableTo);

        validator.RuleFor(x => x.ExchangeWith)
            .NotNull().WithMessage("من فضلك اختار ما ترغب بالبدل معه.")
            .IsInEnum().WithMessage("من فضلك اختار خيار بدل صحيح.")
            .When(IsExchange);

        validator.RuleFor(x => x.OtherExchangeWith)
            .NotEmpty().WithMessage("من فضلك اكتب المطلوب عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => IsExchange(x) && x.ExchangeWith == LandExchangeWith.Other);

        validator.RuleFor(x => x.DifferenceAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة الفرق التقريبية.")
            .GreaterThan(0).WithMessage("قيمة الفرق يجب أن تكون أكبر من صفر.")
            .When(x => IsExchange(x) && x.AcceptsDifferencePayment == true);

        validator.RuleFor(x => x.ExchangeDetails)
            .MaximumLength(4000)
            .When(IsExchange);

        validator.RuleFor(x => x.CurrentCropType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المحصول الحالي.")
            .MaximumLength(150)
            .When(x => IsAgricultural(x) && x.IsCurrentlyCultivated == true);

        validator.RuleFor(x => x.CultivatedFeddans)
            .GreaterThan(0).WithMessage("عدد الأفدنة المزروعة يجب أن يكون أكبر من صفر.")
            .When(x => IsAgricultural(x) && x.IsCurrentlyCultivated == true && x.CultivatedFeddans.HasValue);

        validator.RuleFor(x => x.HarvestSeason)
            .IsInEnum().WithMessage("من فضلك اختار موسم حصاد صحيح.")
            .When(x => IsAgricultural(x) && x.HarvestSeason.HasValue);

        validator.RuleFor(x => x.SoilType)
            .IsInEnum().WithMessage("من فضلك اختار نوع تربة صحيح.")
            .When(x => IsAgricultural(x) && x.SoilType.HasValue);

        validator.RuleFor(x => x.IrrigationSource)
            .IsInEnum().WithMessage("من فضلك اختار مصدر ري صحيح.")
            .When(x => IsAgricultural(x) && x.IrrigationSource.HasValue);

        validator.RuleFor(x => x.OtherIrrigationSource)
            .NotEmpty().WithMessage("من فضلك اكتب مصدر الري عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => IsAgricultural(x) && x.IrrigationSource == LandIrrigationSource.Other);

        validator.RuleFor(x => x.TreeType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع الأشجار.")
            .MaximumLength(150)
            .When(x => IsAgricultural(x) && x.HasTrees == true);

        validator.RuleFor(x => x.TreesCount)
            .GreaterThan(0).WithMessage("عدد الأشجار يجب أن يكون أكبر من صفر.")
            .When(x => IsAgricultural(x) && x.HasTrees == true && x.TreesCount.HasValue);

        validator.RuleFor(x => x.TreesAge)
            .MaximumLength(100)
            .When(x => IsAgricultural(x) && x.HasTrees == true);

        validator.RuleFor(x => x.QualityCertificate)
            .NotNull().WithMessage("من فضلك اختار نوع الشهادة.")
            .IsInEnum().WithMessage("من فضلك اختار نوع شهادة صحيح.")
            .When(x => IsAgricultural(x) && x.HasQualityCertificate == true);

        validator.RuleFor(x => x.ExistingBuildingType)
            .NotNull().WithMessage("من فضلك اختار نوع المبنى.")
            .IsInEnum().WithMessage("من فضلك اختار نوع مبنى صحيح.")
            .When(x => IsBuildingLand(x) && x.HasExistingBuilding == true);

        validator.RuleFor(x => x.BuildingCompletionRatio)
            .NotNull().WithMessage("من فضلك اختار نسبة تنفيذ المبنى.")
            .IsInEnum().WithMessage("من فضلك اختار نسبة تنفيذ صحيحة.")
            .When(x => IsBuildingLand(x) && x.HasExistingBuilding == true);

        validator.RuleFor(x => x.CurrentFloorsCount)
            .GreaterThan(0).WithMessage("عدد الأدوار الحالية يجب أن يكون أكبر من صفر.")
            .When(x => IsBuildingLand(x) && x.HasExistingBuilding == true && x.CurrentFloorsCount.HasValue);
    }

    private static bool IsSale(CreateLandRequest request) =>
        request.ListingType == RealEstateListingType.Sale;

    private static bool IsRent(CreateLandRequest request) =>
        request.ListingType == RealEstateListingType.Rent;

    private static bool IsExchange(CreateLandRequest request) =>
        request.ListingType == RealEstateListingType.Exchange;

    private static bool IsAgricultural(CreateLandRequest request) =>
        LandTypeGroups.IsAgricultural(request.LandType);

    private static bool IsBuildingLand(CreateLandRequest request) =>
        LandTypeGroups.IsBuilding(request.LandType);
}
