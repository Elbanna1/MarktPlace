using FluentValidation;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Validation;

public class CreateShopRequestValidator : AbstractValidator<CreateShopRequest>
{
    public CreateShopRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddShopRules();
    }
}

public class UpdateShopRequestValidator : AbstractValidator<UpdateShopRequest>
{
    public UpdateShopRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddShopRules();
    }
}

internal static class ShopValidationRules
{
    public static void AddShopRules<T>(this AbstractValidator<T> validator)
        where T : CreateShopRequest
    {
        validator.RuleFor(x => x.SuitableActivity)
            .IsInEnum().WithMessage("من فضلك اختار نشاط مناسب صحيح.")
            .When(x => x.SuitableActivity.HasValue);

        validator.RuleFor(x => x.OtherSuitableActivity)
            .NotEmpty().WithMessage("من فضلك اكتب اسم النشاط عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.SuitableActivity == ShopSuitableActivity.Other);

        validator.RuleFor(x => x.Price)
            .NotNull().WithMessage("السعر مطلوب.")
            .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر.")
            .When(IsSale);

        validator.RuleFor(x => x.Area)
            .NotNull().WithMessage("المساحة مطلوبة.")
            .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر.");

        validator.RuleFor(x => x.FloorType)
            .NotNull().WithMessage("الدور مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار دور صحيح.");

        validator.RuleFor(x => x.CeilingHeight)
            .GreaterThan(0).WithMessage("ارتفاع السقف يجب أن يكون أكبر من صفر.")
            .When(x => x.CeilingHeight.HasValue);

        validator.RuleFor(x => x.FacadeWidth)
            .GreaterThan(0).WithMessage("عرض الواجهة يجب أن يكون أكبر من صفر.")
            .When(x => x.FacadeWidth.HasValue);

        validator.RuleFor(x => x.FacadesCount)
            .IsInEnum().WithMessage("من فضلك اختار عدد واجهات صحيح.")
            .When(x => x.FacadesCount.HasValue);

        validator.RuleFor(x => x.FacadeDirection)
            .IsInEnum().WithMessage("من فضلك اختار اتجاه واجهة صحيح.")
            .When(x => x.FacadeDirection.HasValue);

        validator.RuleFor(x => x.FinishingType)
            .IsInEnum().WithMessage("من فضلك اختار نوع تشطيب صحيح.")
            .When(x => x.FinishingType.HasValue);

        validator.RuleFor(x => x.PropertyAge)
            .IsInEnum().WithMessage("من فضلك اختار عمر عقار صحيح.")
            .When(x => x.PropertyAge.HasValue);

        validator.RuleFor(x => x.BathroomsCount)
            .NotNull().WithMessage("من فضلك اكتب عدد الحمامات.")
            .GreaterThan(0).WithMessage("عدد الحمامات يجب أن يكون أكبر من صفر.")
            .When(x => x.HasBathroom == true);

        validator.RuleFor(x => x.StorageArea)
            .NotNull().WithMessage("من فضلك اكتب مساحة المخزن.")
            .GreaterThan(0).WithMessage("مساحة المخزن يجب أن تكون أكبر من صفر.")
            .When(x => x.HasStorage == true);

        validator.RuleFor(x => x.EntrancesCount)
            .IsInEnum().WithMessage("من فضلك اختار عدد مداخل صحيح.")
            .When(x => x.EntrancesCount.HasValue);

        validator.RuleFor(x => x.LegalStatus)
            .NotNull().WithMessage("حالة المحل مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة محل صحيحة.");

        validator.RuleFor(x => x.LicenseNumber)
            .MaximumLength(100)
            .When(IsLicensed);

        validator.RuleFor(x => x.LicenseType)
            .NotNull().WithMessage("نوع الرخصة مطلوب عند اختيار 'مرخص'.")
            .IsInEnum().WithMessage("من فضلك اختار نوع رخصة صحيح.")
            .When(IsLicensed);

        validator.RuleFor(x => x.LicenseIssuer)
            .MaximumLength(150)
            .When(IsLicensed);

        validator.AddLicenceWindowRule(x => x.LicenseIssueDate, x => x.LicenseExpiryDate);

        validator.RuleFor(x => x.ReconciliationForm)
            .NotNull().WithMessage("نموذج التصالح مطلوب عند اختيار 'تصالح'.")
            .IsInEnum().WithMessage("من فضلك اختار نموذج تصالح صحيح.")
            .When(IsReconciliation);

        validator.RuleFor(x => x.OtherReconciliationForm)
            .NotEmpty().WithMessage("من فضلك اكتب اسم النموذج عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => IsReconciliation(x) && x.ReconciliationForm == ShopReconciliationForm.Other);

        validator.RuleFor(x => x.OwnershipDocument)
            .NotNull().WithMessage("مستند الملكية مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار مستند ملكية صحيح.");

        validator.RuleFor(x => x.OtherOwnershipDocument)
            .NotEmpty().WithMessage("من فضلك اكتب اسم المستند عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.OwnershipDocument == ShopOwnershipDocument.Other);

        validator.RuleForEach(x => x.Utilities)
            .IsInEnum().WithMessage("من فضلك اختار مرافق صحيحة.");

        validator.RuleFor(x => x.Utilities)
            .Must(utilities => utilities.Distinct().Count() == utilities.Count)
            .WithMessage("لا يمكن تكرار نفس المرفق.");

        validator.RuleFor(x => x.PreviousActivity)
            .NotEmpty().WithMessage("من فضلك اكتب النشاط السابق.")
            .MaximumLength(150)
            .When(x => x.WasPreviouslyOperating == true);

        validator.RuleFor(x => x.PreviousOperatingPeriod)
            .MaximumLength(100)
            .When(x => x.WasPreviouslyOperating == true);

        validator.RuleFor(x => x.VacancyReason)
            .MaximumLength(1000)
            .When(x => x.WasPreviouslyOperating == true);

        validator.RuleFor(x => x.PaymentMethod)
            .NotNull().WithMessage("طريقة السداد مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار طريقة سداد صحيحة.")
            .When(IsSale);

        validator.RuleFor(x => x.DownPayment)
            .NotNull().WithMessage("قيمة المقدم مطلوبة عند اختيار التقسيط.")
            .GreaterThan(0).WithMessage("قيمة المقدم يجب أن تكون أكبر من صفر.")
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentPeriod)
            .NotEmpty().WithMessage("مدة التقسيط مطلوبة عند اختيار التقسيط.")
            .MaximumLength(100)
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentAmount)
            .NotNull().WithMessage("قيمة القسط مطلوبة عند اختيار التقسيط.")
            .GreaterThan(0).WithMessage("قيمة القسط يجب أن تكون أكبر من صفر.")
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentProvider)
            .NotNull().WithMessage("جهة التقسيط مطلوبة عند اختيار التقسيط.")
            .IsInEnum().WithMessage("من فضلك اختار جهة تقسيط صحيحة.")
            .When(IsInstallments);

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

        validator.RuleFor(x => x.RentDownPayment)
            .GreaterThanOrEqualTo(0).WithMessage("المقدم لا يمكن أن يكون سالبًا.")
            .When(x => IsRent(x) && x.RentDownPayment.HasValue);

        validator.RuleFor(x => x.MinimumRentPeriod)
            .GreaterThan(0).WithMessage("الحد الأدنى لمدة الإيجار يجب أن يكون أكبر من صفر.")
            .When(x => IsRent(x) && x.MinimumRentPeriod.HasValue);

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

        validator.RuleFor(x => x.DifferenceAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة الفرق.")
            .GreaterThan(0).WithMessage("قيمة الفرق يجب أن تكون أكبر من صفر.")
            .When(x => IsExchange(x) && x.AcceptsDifferencePayment == true);

        validator.RuleFor(x => x.ExchangeDetails)
            .MaximumLength(4000)
            .When(IsExchange);
    }

    private static bool IsSale(CreateShopRequest request) =>
        request.ListingType == RealEstateListingType.Sale;

    private static bool IsRent(CreateShopRequest request) =>
        request.ListingType == RealEstateListingType.Rent;

    private static bool IsExchange(CreateShopRequest request) =>
        request.ListingType == RealEstateListingType.Exchange;

    private static bool IsInstallments(CreateShopRequest request) =>
        IsSale(request) && request.PaymentMethod == ShopPaymentMethod.Installments;

    private static bool IsLicensed(CreateShopRequest request) =>
        request.LegalStatus == ShopLegalStatus.Licensed;

    private static bool IsReconciliation(CreateShopRequest request) =>
        request.LegalStatus == ShopLegalStatus.Reconciliation;
}
