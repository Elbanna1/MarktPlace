using FluentValidation;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.Enums;

namespace Services.Validation;

public static class AdvertisementValidationRules
{
    public static bool IsVehicle(int subCategoryId) => CarSubCategories.Contains(subCategoryId);

    public static bool PricesBySalePrice(IVehicleAdvertisementFields request) =>
        IsVehicle(request.SubCategoryId) &&
        request.ListingType is ListingType.Sale or ListingType.Accident;

    public static void AddVehicleRules<T>(this AbstractValidator<T> validator)
        where T : IVehicleAdvertisementFields
    {
        static bool IsCar(T request) => CarSubCategories.Contains(request.SubCategoryId);
        static bool Is(T request, SubCategoryType type) => request.SubCategoryId == (int)type;
        static SubCategoryType SubOf(T request) => (SubCategoryType)request.SubCategoryId;

        validator.RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("الماركة مطلوبة.")
            .MaximumLength(100)
            .When(IsCar);

        validator.RuleFor(x => x.OtherBrand)
            .NotEmpty().WithMessage("من فضلك اكتب الماركة لما تختار «أخرى».")
            .MaximumLength(100)
            .When(request => PicksBrandFromCatalog(request) && request.Brand == CarCatalog.OtherOptionValue);

        validator.RuleFor(x => x.Model)
            .NotEmpty().WithMessage("الموديل مطلوب.")
            .MaximumLength(100)
            .When(IsCar);

        validator.RuleFor(x => x.ManufacturingYear)
            .InclusiveBetween(CarCatalog.MinManufacturingYear, CarCatalog.MaxManufacturingYear)
            .WithMessage(
                $"سنة الصنع لازم تكون بين {CarCatalog.MinManufacturingYear} و " +
                $"{CarCatalog.MaxManufacturingYear}.")
            .When(IsCar);

        validator.RuleFor(x => x.Color)
            .NotEmpty().WithMessage("اللون مطلوب.")
            .MaximumLength(50)
            .When(IsCar);

        validator.RuleFor(x => x.Color).MaximumLength(50).When(request => !IsCar(request));

        validator.RuleFor(x => x.OtherColor)
            .NotEmpty().WithMessage("من فضلك اكتب اللون لما تختار «أخرى».")
            .MaximumLength(50)
            .When(request => IsCar(request) && request.Color == CarCatalog.OtherOptionValue);

        validator.RuleFor(x => x.OtherColor)
            .MaximumLength(50)
            .When(request => request.Color != CarCatalog.OtherOptionValue);

        validator.RuleFor(x => x.Kilometers)
            .GreaterThanOrEqualTo(0).WithMessage("عدد الكيلومترات ما ينفعش يكون بالسالب.")
            .When(request => request.Kilometers.HasValue);

        validator.RuleFor(x => x.EngineCC)
            .GreaterThan(0).WithMessage("سعة المحرك لازم تكون أكبر من صفر.")
            .When(request => request.EngineCC.HasValue);

        validator.RuleFor(x => x.Condition)
            .NotNull().WithMessage("الحالة مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار الحالة صح.")
            .When(IsCar);

        validator.RuleFor(x => x.TechnicalCondition)
            .NotNull().WithMessage("الحالة الفنية مطلوبة.")
            .When(IsCar);

        validator.RuleFor(x => x.TechnicalCondition)
            .IsInEnum().WithMessage("من فضلك اختار الحالة الفنية صح.")
            .When(request => request.TechnicalCondition.HasValue);

        validator.RuleFor(x => x.Transmission)
            .IsInEnum().WithMessage("من فضلك اختار ناقل الحركة صح.")
            .Must((request, transmission) =>
                transmission != TransmissionType.SemiAutomatic || Is(request, SubCategoryType.Motorcycles))
            .WithMessage("نصف أوتوماتيك is available for موتوسيكلات only.")
            .When(request => request.Transmission.HasValue);

        validator.RuleFor(x => x.DetailedAddress)
            .NotEmpty().WithMessage("العنوان بالتفصيل مطلوب.")
            .MaximumLength(300)
            .When(IsCar);

        validator.RuleFor(x => x.LicenseStatus)
            .NotNull().WithMessage("حالة الرخصة مطلوبة.")
            .IsInEnum().WithMessage("من فضلك اختار حالة الرخصة صح.")
            .Must((request, status) =>
                status != LicenseStatus.NotRequired || Is(request, SubCategoryType.HeavyEquipment))
            .WithMessage("'لا تحتاج رخصة' is available for لوادر ومعدات ثقيلة only.")
            .When(IsCar);

        validator.RuleFor(x => x.LicenseExpiryDate)
            .NotNull().WithMessage("تاريخ انتهاء الرخصة مطلوب لما تكون الرخصة سارية.")
            .When(request => IsCar(request) && request.LicenseStatus == LicenseStatus.Valid);

        validator.RuleFor(x => x.Transmission)
            .NotNull().WithMessage("ناقل الحركة مطلوب.")
            .When(request =>
                Is(request, SubCategoryType.Private) ||
                Is(request, SubCategoryType.Taxi) ||
                Is(request, SubCategoryType.Motorcycles));

        validator.RuleFor(x => x.FuelType)
            .NotNull().WithMessage("نوع الوقود مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع الوقود صح.")
            .When(request =>
                Is(request, SubCategoryType.Private) ||
                Is(request, SubCategoryType.Taxi) ||
                Is(request, SubCategoryType.Motorcycles));

        validator.RuleFor(x => x.EngineCC)
            .NotNull().WithMessage("سعة المحرك مطلوبة.")
            .When(request => Is(request, SubCategoryType.Private) || Is(request, SubCategoryType.Taxi));

        validator.RuleFor(x => x.BodyType)
            .NotNull().WithMessage("نوع الهيكل مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع الهيكل صح.")
            .When(request => Is(request, SubCategoryType.Private));

        validator.RuleFor(x => x.DoorsCount)
            .NotNull().WithMessage("عدد الأبواب مطلوب.")
            .When(request => Is(request, SubCategoryType.Private));

        validator.RuleFor(x => x.DoorsCount)
            .InclusiveBetween(1, 10).WithMessage("عدد الأبواب لازم يكون من 1 لـ 10.")
            .When(request => request.DoorsCount.HasValue);

        validator.RuleFor(x => x.SeatsCount)
            .InclusiveBetween(1, 20).WithMessage("عدد الكراسي لازم يكون من 1 لـ 20.")
            .When(request => request.SeatsCount.HasValue);

        validator.RuleFor(x => x.InsuranceType)
            .NotNull().WithMessage("نوع التأمين مطلوب لما تكون العربية مؤمّنة.")
            .IsInEnum().WithMessage("من فضلك اختار نوع التأمين صح.")
            .When(request => request.IsInsured == true);

        validator.RuleFor(x => x.DownPayment)
            .NotNull().WithMessage("المقدم مطلوب لما تقبل التقسيط.")
            .GreaterThanOrEqualTo(0)
            .When(request => request.InstallmentsAccepted == true);

        validator.RuleFor(x => x.InstallmentMonths)
            .NotNull().WithMessage("عدد شهور التقسيط مطلوب لما تقبل التقسيط.")
            .InclusiveBetween(1, 120)
            .When(request => request.InstallmentsAccepted == true);

        validator.RuleFor(x => x.ChangedParts)
            .NotEmpty().WithMessage("من فضلك حدد الأجزاء اللي اتغيرت.")
            .When(request => IsCar(request) && request.PartsChanged == true);

        validator.RuleFor(x => x.ChangedParts)
            .Must((request, parts) => parts!.All(part => ChangedPartsOf(SubOf(request)).ContainsKey(part)))
            .WithMessage("واحد من الأجزاء اللي اخترتها مش متاح في القسم ده.")
            .When(request => IsCar(request) && request.ChangedParts is { Count: > 0 });

        validator.RuleFor(x => x.VehicleType)
            .NotNull().WithMessage("نوع المركبة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع المركبة صح.")
            .When(request => Is(request, SubCategoryType.Taxi));

        validator.RuleFor(x => x.OtherVehicleType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المركبة لما تختار «أخرى».")
            .MaximumLength(100)
            .When(request => request.VehicleType == TaxiVehicleType.Other);

        validator.RuleFor(x => x.PassengersCount)
            .InclusiveBetween(1, 60).WithMessage("عدد الركاب لازم يكون من 1 لـ 60.")
            .When(request => request.PassengersCount.HasValue);

        validator.RuleFor(x => x.Route).MaximumLength(300);

        validator.RuleFor(x => x.OperatingLicenseExpiryDate)
            .NotNull()
            .WithMessage("تاريخ انتهاء رخصة التشغيل مطلوب لما تكون الرخصة سارية.")
            .When(request => request.OperatingLicenseStatus == OperatingLicenseStatus.Valid);

        validator.RuleFor(x => x.MotorcycleType)
            .NotNull().WithMessage("نوع الموتوسيكل مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع الموتوسيكل صح.")
            .When(request => Is(request, SubCategoryType.Motorcycles));

        validator.RuleFor(x => x.EngineCC)
            .NotNull().WithMessage("سعة المحرك مطلوبة.")
            .When(request => Is(request, SubCategoryType.Motorcycles));

        validator.RuleFor(x => x.CoolingType)
            .NotNull().WithMessage("نظام التبريد مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نظام التبريد صح.")
            .When(request => Is(request, SubCategoryType.Motorcycles));

        validator.RuleFor(x => x.MachineType)
            .NotNull().WithMessage("نوع المعدة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع المعدة صح.")
            .When(request => Is(request, SubCategoryType.HeavyEquipment));

        validator.RuleFor(x => x.OtherMachineType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المعدة لما تختار «أخرى».")
            .MaximumLength(100)
            .When(request => request.MachineType == EquipmentMachineType.Other);

        validator.RuleFor(x => x.WorkingHours)
            .NotNull().WithMessage("ساعات العمل مطلوبة.")
            .GreaterThanOrEqualTo(0)
            .When(request => Is(request, SubCategoryType.HeavyEquipment));

        validator.RuleFor(x => x.PowerUnit)
            .NotNull().WithMessage("وحدة القدرة مطلوبة لما تكتب قيمة القدرة.")
            .IsInEnum().WithMessage("من فضلك اختار وحدة القدرة صح.")
            .When(request => request.PowerValue.HasValue);

        validator.RuleFor(x => x.BucketCapacity).MaximumLength(100);

        validator.RuleFor(x => x.UsageFields)
            .Must(fields => fields!.All(field => CarCatalog.UsageFieldNames.ContainsKey(field)))
            .WithMessage("واحد من مجالات الاستخدام اللي اخترتها مش صحيح.")
            .When(request => request.UsageFields is { Count: > 0 });

        validator.RuleFor(x => x.RentSystems)
            .NotEmpty().WithMessage("لازم تختار نظام إيجار واحد على الأقل في إعلان الإيجار.")
            .When(request => IsCar(request) && request.ListingType == ListingType.Rent);

        validator.RuleFor(x => x.RentSystems)
            .Must((request, systems) =>
                systems!.All(system => RentSystemsOf(SubOf(request)).ContainsKey(system)))
            .WithMessage("واحد من أنظمة الإيجار اللي اخترتها مش متاح في القسم ده.")
            .When(request => IsCar(request) && request.RentSystems is { Count: > 0 });

        validator.RuleFor(x => x.RentSystems)
            .Must(HasPriceForEverySelectedSystem)
            .WithMessage("كل نظام إيجار اخترته لازم يكون له سعر.")
            .When(request =>
                IsCar(request) &&
                request.ListingType == ListingType.Rent &&
                request.RentSystems is { Count: > 0 });

        foreach (var price in new Func<T, decimal?>[]
                 {
                     x => x.HourlyPrice, x => x.DailyPrice, x => x.WeeklyPrice,
                     x => x.MonthlyPrice, x => x.DepositAmount
                 })
        {
            validator.RuleFor(x => price(x))
                .GreaterThanOrEqualTo(0).WithMessage("السعر ما ينفعش يكون بالسالب.")
                .When(request => price(request).HasValue);
        }

        validator.RuleFor(x => x.MinimumRentPeriod)
            .GreaterThan(0).WithMessage("أقل مدة إيجار لازم تكون أكبر من صفر.")
            .When(request => request.MinimumRentPeriod.HasValue);

        validator.RuleFor(x => x.MaximumRentPeriod)
            .GreaterThanOrEqualTo(request => request.MinimumRentPeriod ?? 1)
            .WithMessage("أقصى مدة إيجار ما ينفعش تكون أقل من الحد الأدنى.")
            .When(request => request.MaximumRentPeriod.HasValue);

        validator.RuleFor(x => x.DamageLevel)
            .NotNull().WithMessage("مستوى الضرر مطلوب في إعلان الحوادث.")
            .IsInEnum().WithMessage("من فضلك اختار مستوى الضرر صح.")
            .When(request => IsCar(request) && request.ListingType == ListingType.Accident);

        validator.RuleFor(x => x.ConditionReport).MaximumLength(4000);

        validator.RuleFor(x => x.InterestedIn)
            .NotNull().WithMessage("من فضلك اختار الحاجة اللي عايز تبدل بيها.")
            .When(request => IsCar(request) && request.ListingType == ListingType.Exchange);

        validator.RuleFor(x => x.InterestedIn)
            .Must((request, target) => ExchangeTargetsOf(SubOf(request)).ContainsKey(target!.Value))
            .WithMessage("خيار البدل ده مش متاح في القسم ده.")
            .When(request => IsCar(request) && request.InterestedIn.HasValue);

        validator.RuleFor(x => x.DifferenceAmount)
            .GreaterThanOrEqualTo(0).WithMessage("قيمة الفرق ما ينفعش تكون بالسالب.")
            .When(request => request.DifferenceAmount.HasValue);

        validator.RuleFor(x => x.ExchangeDetails).MaximumLength(4000);
    }

    private static bool PicksBrandFromCatalog<T>(T request)
        where T : IVehicleAdvertisementFields =>
        request.SubCategoryId is (int)SubCategoryType.Motorcycles or (int)SubCategoryType.HeavyEquipment;

    private static bool HasPriceForEverySelectedSystem<T>(T request, IReadOnlyList<RentSystem>? systems)
        where T : IVehicleAdvertisementFields =>
        systems!.All(system => system switch
        {
            RentSystem.Hourly => request.HourlyPrice.HasValue,
            RentSystem.Daily => request.DailyPrice.HasValue,
            RentSystem.Weekly => request.WeeklyPrice.HasValue,
            RentSystem.Monthly => request.MonthlyPrice.HasValue,
            _ => true
        });

    private static IReadOnlyDictionary<ChangedVehiclePart, string> ChangedPartsOf(
        SubCategoryType subCategory) =>
        subCategory is SubCategoryType.Taxi
            ? CarCatalog.TaxiChangedPartNames
            : CarCatalog.ChangedPartNames;

    private static IReadOnlyDictionary<RentSystem, string> RentSystemsOf(SubCategoryType subCategory) =>
        subCategory is SubCategoryType.HeavyEquipment
            ? CarCatalog.EquipmentRentSystemNames
            : CarCatalog.RentSystemNames;

    private static IReadOnlyDictionary<InterestedIn, string> ExchangeTargetsOf(
        SubCategoryType subCategory) =>
        subCategory switch
        {
            SubCategoryType.Taxi => CarCatalog.TaxiExchangeTargetNames,
            SubCategoryType.Motorcycles => CarCatalog.MotorcycleExchangeTargetNames,
            SubCategoryType.HeavyEquipment => CarCatalog.EquipmentExchangeTargetNames,
            _ => CarCatalog.PrivateExchangeTargetNames
        };

    public static void AddBusinessRules<T>(this AbstractValidator<T> validator)
        where T : IBusinessAdvertisementFields
    {
        bool IsBusiness(T request) => BusinessSubCategories.Contains(request.SubCategoryId);
        static bool Is(T request, SubCategoryType type) => request.SubCategoryId == (int)type;

        validator.RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("اسم النشاط مطلوب.")
            .MaximumLength(150)
            .When(IsBusiness);

        validator.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("العنوان مطلوب.")
            .MaximumLength(300)
            .When(IsBusiness);

        validator.RuleFor(x => x.GoogleMapsUrl)
            .MaximumLength(1000)
            .Must(BeAnHttpUrl)
            .WithMessage("لينك خرائط جوجل لازم يبدأ بـ http أو https.")
            .When(request => !string.IsNullOrWhiteSpace(request.GoogleMapsUrl));

        validator.RuleFor(x => x.GoogleMapsUrl)
            .NotEmpty().WithMessage("رابط الموقع على خرائط جوجل مطلوب.")
            .When(request =>
                Is(request, SubCategoryType.Factories) ||
                Is(request, SubCategoryType.Farms) ||
                Is(request, SubCategoryType.Companies));

        validator.RuleFor(x => x.WhatsApp)
            .Matches(AdvertisementCatalog.EgyptianPhonePattern)
            .WithMessage(AdvertisementCatalog.EgyptianPhoneMessage)
            .MaximumLength(20)
            .When(request => !string.IsNullOrWhiteSpace(request.WhatsApp));

        validator.RuleFor(x => x.Email)
            .EmailAddress().WithMessage("من فضلك اكتب بريد إلكتروني صحيح.")
            .MaximumLength(256)
            .When(request => !string.IsNullOrWhiteSpace(request.Email));

        validator.RuleFor(x => x.ProductionSpecialty)
            .NotNull().WithMessage("تخصص الإنتاج مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار تخصص الإنتاج صح.")
            .When(request => Is(request, SubCategoryType.Factories));

        validator.RuleFor(x => x.OtherProductionSpecialty)
            .NotEmpty().WithMessage("من فضلك اكتب تخصص الإنتاج لما تختار «أخرى».")
            .MaximumLength(150)
            .When(request => request.ProductionSpecialty == Shared.Enums.ProductionSpecialty.Other);

        validator.RuleFor(x => x.FarmType)
            .NotNull().WithMessage("نوع المزرعة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع المزرعة صح.")
            .When(request => Is(request, SubCategoryType.Farms));

        validator.RuleFor(x => x.OtherFarmType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المزرعة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(request => request.FarmType == Shared.Enums.FarmType.Other);

        validator.RuleFor(x => x.FarmingMethod)
            .IsInEnum().WithMessage("من فضلك اختار طريقة الزراعة صح.")
            .When(request => request.FarmingMethod.HasValue);

        validator.RuleFor(x => x.AvailabilitySeason)
            .IsInEnum().WithMessage("من فضلك اختار موسم التوافر صح.")
            .When(request => request.AvailabilitySeason.HasValue);

        validator.RuleFor(x => x.CompanyField)
            .NotNull().WithMessage("مجال الشركة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار مجال الشركة صح.")
            .When(request => Is(request, SubCategoryType.Companies));

        validator.RuleFor(x => x.OtherCompanyField)
            .NotEmpty().WithMessage("من فضلك اكتب مجال الشركة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(request => request.CompanyField == Shared.Enums.CompanyField.Other);

        validator.RuleFor(x => x.SupplierType)
            .NotNull().WithMessage("نوع المورد مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع المورد صح.")
            .When(request => Is(request, SubCategoryType.Suppliers));

        validator.RuleFor(x => x.OtherSupplierType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع المورد لما تختار «أخرى».")
            .MaximumLength(150)
            .When(request => request.SupplierType == Shared.Enums.SupplierType.Other);

        validator.RuleFor(x => x.TradeType)
            .NotNull().WithMessage("نوع التجارة مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع التجارة صح.")
            .When(request => Is(request, SubCategoryType.WholesaleTraders));

        validator.RuleFor(x => x.OtherTradeType)
            .NotEmpty().WithMessage("من فضلك اكتب نوع التجارة لما تختار «أخرى».")
            .MaximumLength(150)
            .When(request => request.TradeType == Shared.Enums.TradeType.Other);

        validator.RuleFor(x => x.SaleType)
            .NotNull().WithMessage("نوع البيع مطلوب.")
            .IsInEnum().WithMessage("من فضلك اختار نوع البيع صح.")
            .When(request => Is(request, SubCategoryType.FruitAndVegetableTraders));
    }

    private static bool BeAnHttpUrl(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        (Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}
