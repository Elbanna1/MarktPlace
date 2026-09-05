using FluentValidation;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Validation;

public class CreateApartmentRequestValidator : AbstractValidator<CreateApartmentRequest>
{
    public CreateApartmentRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddApartmentRules();
    }
}

public class UpdateApartmentRequestValidator : AbstractValidator<UpdateApartmentRequest>
{
    public UpdateApartmentRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddApartmentRules();
    }
}

internal static class ApartmentValidationRules
{
    public static void AddApartmentRules<T>(this AbstractValidator<T> validator)
        where T : CreateApartmentRequest
    {
        validator.RuleFor(x => x.ApartmentType)
            .NotNull().WithMessage("نوع الشقة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع شقة صحيح.");

        validator.RuleFor(x => x.OtherApartmentType)
            .NotEmpty().WithMessage("من فضلك اكتب اسم النوع عند اختيار 'أخرى'.")
            .MaximumLength(150)
            .When(x => x.ApartmentType == ApartmentType.Other);

        validator.RuleFor(x => x.OwnershipType)
            .NotNull().WithMessage("نوع الملكية مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع ملكية صحيح.");

        validator.RuleFor(x => x.Price)
            .NotNull().WithMessage("السعر مطلوب.")
            .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر.")
            .When(IsSale);

        validator.RuleFor(x => x.PricePerMeter)
            .NotNull().WithMessage("سعر المتر مطلوب.")
            .GreaterThan(0).WithMessage("سعر المتر يجب أن يكون أكبر من صفر.")
            .When(IsSale);

        validator.RuleFor(x => x.Area)
            .NotNull().WithMessage("المساحة مطلوبة.")
            .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر.");

        validator.RuleFor(x => x.RoomsCount)
            .NotNull().WithMessage("عدد الغرف مطلوب.")
            .GreaterThan(0).WithMessage("عدد الغرف يجب أن يكون أكبر من صفر.");

        validator.RuleFor(x => x.BathroomsCount)
            .NotNull().WithMessage("عدد الحمامات مطلوب.")
            .GreaterThan(0).WithMessage("عدد الحمامات يجب أن يكون أكبر من صفر.");

        validator.RuleFor(x => x.ReceptionPieces)
            .NotNull().WithMessage("عدد قطع الريسبشن مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار عدد قطع ريسبشن صحيح.");

        validator.RuleFor(x => x.FloorType)
            .NotNull().WithMessage("الدور مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار دور صحيح.");

        validator.RuleFor(x => x.FloorNumber)
            .NotNull().WithMessage("رقم الدور مطلوب.")
            .GreaterThanOrEqualTo(0).WithMessage("رقم الدور لا يمكن أن يكون سالبًا.")
            .When(HasFloorDetails);

        validator.RuleFor(x => x.TotalFloors)
            .NotNull().WithMessage("إجمالي عدد الأدوار مطلوب.")
            .GreaterThan(0).WithMessage("إجمالي عدد الأدوار يجب أن يكون أكبر من صفر.")
            .When(HasFloorDetails);

        validator.RuleFor(x => x.ApartmentsPerFloor)
            .NotNull().WithMessage("عدد الشقق في الدور مطلوب.")
            .GreaterThan(0).WithMessage("عدد الشقق في الدور يجب أن يكون أكبر من صفر.")
            .When(HasFloorDetails);

        validator.RuleFor(x => x.FloorNumber)
            .LessThanOrEqualTo(x => x.TotalFloors)
            .WithMessage("رقم الدور لا يمكن أن يتجاوز إجمالي عدد الأدوار.")
            .When(x => x.FloorNumber.HasValue && x.TotalFloors.HasValue);

        validator.RuleFor(x => x.HasElevator)
            .NotNull().WithMessage("من فضلك حدد وجود أسانسير.");

        validator.RuleFor(x => x.FurnishedStatus)
            .NotNull().WithMessage("حالة الفرش مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة فرش صحيحة.");

        validator.RuleFor(x => x.FinishingType)
            .NotNull().WithMessage("التشطيب مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع تشطيب صحيح.");

        validator.RuleFor(x => x.PropertyAge)
            .NotNull().WithMessage("عمر العقار مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار عمر عقار صحيح.");

        validator.RuleFor(x => x.Direction)
            .NotNull().WithMessage("الاتجاه مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار اتجاه صحيح.");

        validator.RuleFor(x => x.ViewType)
            .IsInEnum().WithMessage("من فضلك اختار إطلالة صحيحة.")
            .When(x => x.ViewType.HasValue);

        validator.RuleFor(x => x.LegalStatus)
            .NotNull().WithMessage("حالة الشقة مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة شقة صحيحة.");

        validator.RuleFor(x => x.LicenseNumber)
            .MaximumLength(100);

        validator.RuleFor(x => x.LicenseIssuer)
            .MaximumLength(150);

        validator.AddLicenceWindowRule(x => x.LicenseIssueDate, x => x.LicenseExpiryDate);

        validator.RuleFor(x => x.ReconciliationForm)
            .NotNull().WithMessage("نموذج التصالح مطلوب عند اختيار 'تصالح'.")
            .IsInEnum().WithMessage("من فضلك اختار نموذج تصالح صحيح.")
            .When(x => x.LegalStatus == ApartmentLegalStatus.Reconciliation);

        validator.RuleFor(x => x.OwnershipDocument)
            .NotNull().WithMessage("مستند الملكية مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار مستند ملكية صحيح.");

        validator.RuleFor(x => x.ViolationDetails)
            .NotEmpty().WithMessage("من فضلك اكتب تفاصيل المخالفات.")
            .MaximumLength(4000)
            .When(x => x.HasViolations == true);

        validator.RuleForEach(x => x.Features)
            .IsInEnum().WithMessage("من فضلك اختار مرافق ومميزات صحيحة.");

        validator.RuleFor(x => x.Features)
            .Must(features => features.Distinct().Count() == features.Count)
            .WithMessage("لا يمكن تكرار نفس الميزة.");

        validator.RuleFor(x => x.PaymentMethod)
            .NotNull().WithMessage("طريقة السداد مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار طريقة سداد صحيحة.")
            .When(IsSale);

        validator.RuleFor(x => x.DownPayment)
            .NotNull().WithMessage("المقدم مطلوب عند اختيار التقسيط.")
            .GreaterThan(0).WithMessage("المقدم يجب أن يكون أكبر من صفر.")
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentAmount)
            .NotNull().WithMessage("قيمة القسط مطلوبة عند اختيار التقسيط.")
            .GreaterThan(0).WithMessage("قيمة القسط يجب أن تكون أكبر من صفر.")
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentPeriod)
            .NotEmpty().WithMessage("مدة التقسيط مطلوبة عند اختيار التقسيط.")
            .MaximumLength(100)
            .When(IsInstallments);

        validator.RuleFor(x => x.InstallmentProvider)
            .NotNull().WithMessage("جهة التقسيط مطلوبة عند اختيار التقسيط.")
            .IsInEnum().WithMessage("من فضلك اختار جهة تقسيط صحيحة.")
            .When(IsInstallments);

        validator.RuleFor(x => x.HasMaintenanceDeposit)
            .NotNull().WithMessage("من فضلك حدد وجود وديعة صيانة.")
            .When(IsSale);

        validator.RuleFor(x => x.MaintenanceDepositAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة وديعة الصيانة.")
            .GreaterThan(0).WithMessage("قيمة وديعة الصيانة يجب أن تكون أكبر من صفر.")
            .When(x => IsSale(x) && x.HasMaintenanceDeposit == true);

        validator.RuleFor(x => x.MonthlyFees)
            .NotNull().WithMessage("الرسوم الشهرية مطلوبة.")
            .GreaterThanOrEqualTo(0).WithMessage("الرسوم الشهرية لا يمكن أن تكون سالبة.")
            .When(IsSale);

        validator.RuleFor(x => x.RentType)
            .NotNull().WithMessage("نوع الإيجار مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع إيجار صحيح.")
            .When(IsRent);

        validator.RuleFor(x => x.RentValue)
            .NotNull().WithMessage("قيمة الإيجار مطلوبة.")
            .GreaterThan(0).WithMessage("قيمة الإيجار يجب أن تكون أكبر من صفر.")
            .When(IsRent);

        validator.RuleFor(x => x.SecurityDeposit)
            .NotNull().WithMessage("قيمة التأمين مطلوبة.")
            .GreaterThanOrEqualTo(0).WithMessage("قيمة التأمين لا يمكن أن تكون سالبة.")
            .When(IsRent);

        validator.RuleFor(x => x.RentDownPayment)
            .NotNull().WithMessage("المقدم مطلوب.")
            .GreaterThanOrEqualTo(0).WithMessage("المقدم لا يمكن أن يكون سالبًا.")
            .When(IsRent);

        validator.RuleFor(x => x.MinimumRentPeriod)
            .NotNull().WithMessage("الحد الأدنى لمدة الإيجار مطلوب.")
            .GreaterThan(0).WithMessage("الحد الأدنى لمدة الإيجار يجب أن يكون أكبر من صفر.")
            .When(IsRent);

        validator.RuleFor(x => x.AvailableFrom)
            .NotNull().WithMessage("تاريخ متاحة من مطلوب.")
            .When(IsRent);

        validator.RuleFor(x => x.AvailableTo)
            .NotNull().WithMessage("تاريخ متاحة حتى مطلوب.")
            .When(IsRent);

        validator.RuleFor(x => x.OwnerConditions)
            .NotEmpty().WithMessage("شروط المالك مطلوبة.")
            .MaximumLength(4000)
            .When(IsRent);

        validator.RuleFor(x => x.RentInclusions)
            .NotEmpty().WithMessage("من فضلك اختار بند إيجار واحد على الأقل.")
            .When(IsRent);

        validator.RuleForEach(x => x.RentInclusions)
            .IsInEnum().WithMessage("من فضلك اختار بنود إيجار صحيحة.")
            .When(IsRent);

        validator.AddAvailabilityWindowRule(x => x.AvailableFrom, x => x.AvailableTo);

        validator.RuleFor(x => x.ExchangeWith)
            .NotNull().WithMessage("من فضلك اختار ما ترغب بالبدل معه.")
            .IsInEnum().WithMessage("من فضلك اختار خيار بدل صحيح.")
            .When(IsExchange);

        validator.RuleFor(x => x.AcceptsDifferencePayment)
            .NotNull().WithMessage("من فضلك حدد قبول دفع فرق.")
            .When(IsExchange);

        validator.RuleFor(x => x.DifferenceAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة الفرق.")
            .GreaterThan(0).WithMessage("قيمة الفرق يجب أن تكون أكبر من صفر.")
            .When(x => IsExchange(x) && x.AcceptsDifferencePayment == true);

        validator.RuleFor(x => x.ExchangeDetails)
            .NotEmpty().WithMessage("تفاصيل البدل مطلوبة.")
            .MaximumLength(4000)
            .When(IsExchange);
    }

    private static bool HasFloorDetails(CreateApartmentRequest request) =>
        request.FloorType is not null and not ApartmentFloorType.Basement;

    private static bool IsSale(CreateApartmentRequest request) =>
        request.ListingType == RealEstateListingType.Sale;

    private static bool IsRent(CreateApartmentRequest request) =>
        request.ListingType == RealEstateListingType.Rent;

    private static bool IsExchange(CreateApartmentRequest request) =>
        request.ListingType == RealEstateListingType.Exchange;

    private static bool IsInstallments(CreateApartmentRequest request) =>
        IsSale(request) && request.PaymentMethod == ApartmentPaymentMethod.Installments;
}
