using FluentValidation;
using Shared.Constants;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Services.Validation;

public class CreateBuildingRequestValidator : AbstractValidator<CreateBuildingRequest>
{
    public CreateBuildingRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddBuildingRules();
    }
}

public class UpdateBuildingRequestValidator : AbstractValidator<UpdateBuildingRequest>
{
    public UpdateBuildingRequestValidator()
    {
        this.AddRealEstateSharedRules();
        this.AddBuildingRules();
    }
}

internal static class BuildingValidationRules
{
    public static void AddBuildingRules<T>(this AbstractValidator<T> validator)
        where T : CreateBuildingRequest
    {
        validator.RuleFor(x => x.BuildingType)
            .NotNull().WithMessage("نوع المبنى مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع مبنى صحيح (فيلا / بيت / عمارة).");

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

        validator.RuleFor(x => x.FloorsCount)
            .NotNull().WithMessage("عدد الطوابق مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxFloors)
            .WithMessage($"عدد الطوابق يجب أن يكون من 1 إلى {BuildingCatalog.MaxFloors}.")
            .When(x => x.BuildingType is BuildingType.Villa or BuildingType.House);

        validator.RuleFor(x => x.FloorsCount)
            .NotNull().WithMessage("عدد الأدوار مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxFloors)
            .WithMessage($"عدد الأدوار يجب أن يكون من 1 إلى {BuildingCatalog.MaxFloors}.")
            .When(x => x.BuildingType == BuildingType.ApartmentBuilding);

        validator.RuleFor(x => x.GardenArea)
            .GreaterThanOrEqualTo(0).WithMessage("مساحة الحديقة لا يمكن أن تكون سالبة.")
            .When(x => IsVilla(x) && x.GardenArea.HasValue);

        validator.RuleFor(x => x.BathroomsCount)
            .NotNull().WithMessage("عدد الحمامات مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxRooms)
            .WithMessage($"عدد الحمامات يجب أن يكون من 1 إلى {BuildingCatalog.MaxRooms}.")
            .When(IsVilla);

        validator.RuleFor(x => x.ReceptionPieces)
            .NotNull().WithMessage("عدد قطع الريسبشن مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار عدد قطع ريسبشن صحيح.")
            .When(IsVilla);

        validator.RuleFor(x => x.RoomsCount)
            .NotNull().WithMessage("عدد الغرف مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxRooms)
            .WithMessage($"عدد الغرف يجب أن يكون من 1 إلى {BuildingCatalog.MaxRooms}.")
            .When(IsVilla);

        validator.RuleFor(x => x.ResidentialUnitsCount)
            .NotNull().WithMessage("عدد الوحدات السكنية مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxUnits)
            .WithMessage($"عدد الوحدات السكنية يجب أن يكون من 1 إلى {BuildingCatalog.MaxUnits}.")
            .When(IsHouse);

        validator.RuleFor(x => x.ApartmentsCount)
            .NotNull().WithMessage("عدد الشقق مطلوب.")
            .InclusiveBetween(1, BuildingCatalog.MaxUnits)
            .WithMessage($"عدد الشقق يجب أن يكون من 1 إلى {BuildingCatalog.MaxUnits}.")
            .When(IsApartmentBuilding);

        validator.RuleFor(x => x.ShopsCount)
            .NotNull().WithMessage("عدد المحلات التجارية مطلوب.")
            .InclusiveBetween(0, BuildingCatalog.MaxUnits)
            .WithMessage($"عدد المحلات التجارية يجب أن يكون من 0 إلى {BuildingCatalog.MaxUnits}.")
            .When(IsApartmentBuilding);

        validator.RuleFor(x => x.LegalStatus)
            .NotNull().WithMessage("حالة المبنى مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة مبنى صحيحة (مرخصة / تصالح / غير مرخصة).");

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

        validator.RuleFor(x => x.MaintenanceDepositAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة وديعة الصيانة.")
            .GreaterThan(0).WithMessage("قيمة وديعة الصيانة يجب أن تكون أكبر من صفر.")
            .When(x => IsSale(x) && x.HasMaintenanceDeposit == true);

        validator.RuleFor(x => x.MonthlyFees)
            .GreaterThanOrEqualTo(0).WithMessage("الرسوم الشهرية لا يمكن أن تكون سالبة.")
            .When(x => IsSale(x) && x.MonthlyFees.HasValue);

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
            .NotNull().WithMessage("تاريخ متاح من مطلوب.")
            .When(IsRent);

        validator.RuleFor(x => x.AvailableTo)
            .NotNull().WithMessage("تاريخ متاح حتى مطلوب.")
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

        validator.RuleFor(x => x.DifferenceAmount)
            .NotNull().WithMessage("من فضلك اكتب قيمة الفرق.")
            .GreaterThan(0).WithMessage("قيمة الفرق يجب أن تكون أكبر من صفر.")
            .When(x => IsExchange(x) && x.AcceptsDifferencePayment == true);

        validator.RuleFor(x => x.ExchangeDetails)
            .NotEmpty().WithMessage("تفاصيل البدل مطلوبة.")
            .MaximumLength(4000)
            .When(IsExchange);
    }

    private static bool IsVilla(CreateBuildingRequest request) => BuildingCatalog.IsVilla(request.BuildingType);

    private static bool IsHouse(CreateBuildingRequest request) => BuildingCatalog.IsHouse(request.BuildingType);

    private static bool IsApartmentBuilding(CreateBuildingRequest request) =>
        BuildingCatalog.IsApartmentBuilding(request.BuildingType);

    private static bool IsSale(CreateBuildingRequest request) =>
        request.ListingType == RealEstateListingType.Sale;

    private static bool IsRent(CreateBuildingRequest request) =>
        request.ListingType == RealEstateListingType.Rent;

    private static bool IsExchange(CreateBuildingRequest request) =>
        request.ListingType == RealEstateListingType.Exchange;

    private static bool IsInstallments(CreateBuildingRequest request) =>
        IsSale(request) && request.PaymentMethod == ApartmentPaymentMethod.Installments;
}
