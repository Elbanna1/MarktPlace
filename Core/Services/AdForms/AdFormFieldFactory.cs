using Shared.Constants;
using Shared.DTOs.Lookups.Forms;

namespace Services.AdForms;

internal static class AdFormFieldFactory
{
    public const string SectionAd = "بيانات الإعلان";
    public const string SectionVehicle = "بيانات السيارة";
    public const string SectionEquipment = "بيانات المعدة";
    public const string SectionRent = "تفاصيل الإيجار";
    public const string SectionAccident = "تفاصيل الحادث";
    public const string SectionExchange = "تفاصيل البدل";

    public const string SectionTaxiVehicle = "بيانات المركبة";
    public const string SectionMotorcycle = "بيانات الموتوسيكل";
    public const string SectionLicense = "بيانات الرخصة";
    public const string SectionInsurance = "بيانات التأمين";
    public const string SectionInspection = "الفحص والمعاينة";
    public const string SectionFinance = "التمويل";
    public const string SectionFeatures = "المميزات";
    public const string SectionLocation = "الموقع";
    public const string SectionContact = "بيانات التواصل";
    public const string SectionImages = "الصور";
    public const string SectionWorkshop = "بيانات الورشة";
    public const string SectionCraftsman = "بيانات الحرفي";
    public const string SectionItem = "بيانات المفقودات";

    public const string SectionFactory = "بيانات المصنع";
    public const string SectionFarm = "بيانات المزرعة";
    public const string SectionCompany = "بيانات الشركة";
    public const string SectionSupplier = "بيانات المورد";
    public const string SectionWholesale = "بيانات تاجر الجملة";
    public const string SectionFruitTrader = "بيانات تاجر الخضر والفاكهة";

    public const string SectionApplicant = "بيانات المتقدم";
    public const string SectionRequiredJob = "الوظيفة المطلوبة";
    public const string SectionEmployer = "بيانات صاحب العمل";
    public const string SectionJob = "بيانات الوظيفة";
    public const string SectionUploads = "المرفقات";

    public const string SectionStore = "بيانات المحل";
    public const string SectionMenClothing = "بيانات الملابس الرجالي";
    public const string SectionWomenClothing = "بيانات الملابس الحريمي";
    public const string SectionKidsClothing = "بيانات ملابس الأطفال";

    public const string SectionProject = "بيانات المشروع";
    public const string SectionProduct = "بيانات المنتج";
    public const string SectionProductDetails = "تفاصيل المنتج";

    public const string SectionLand = "بيانات الأرض";
    public const string SectionApartment = "بيانات الشقة";
    public const string SectionShop = "بيانات المحل";
    public const string SectionLicensing = "التراخيص والحالة القانونية";
    public const string SectionOwnershipDocuments = "مستندات الملكية";
    public const string SectionLicensingAndOwnership = "التراخيص والملكية";
    public const string SectionUtilities = "المرافق";
    public const string SectionUtilitiesAndFeatures = "المرافق والمميزات";
    public const string SectionExtraInformation = "معلومات إضافية";
    public const string SectionSale = "بيانات البيع";
    public const string SectionRentDetails = "بيانات الإيجار";
    public const string SectionExchangeDetails = "تفاصيل البدل";
    public const string SectionAgriculturalLand = "بيانات الأرض الزراعية";
    public const string SectionBuilding = "بيانات البناء";
    public const string SectionActivity = "بيانات النشاط";

    public const string SectionLivestock = "بيانات المواشي";
    public const string SectionSheepGoat = "بيانات الأغنام والماعز";
    public const string SectionHorse = "بيانات الخيول";
    public const string SectionCamel = "بيانات الإبل";
    public const string SectionBird = "بيانات الطيور";
    public const string SectionPet = "بيانات الحيوان الأليف";
    public const string SectionFish = "بيانات الأسماك";
    public const string SectionBee = "بيانات النحل";
    public const string SectionOtherAnimal = "بيانات الحيوان";

    public const string SectionFurniture = "بيانات الأثاث";
    public const string SectionFurnishingCurtain = "بيانات المفروشات والستائر";
    public const string SectionLightingDecor = "بيانات الإضاءة والديكور";
    public const string SectionKitchenTool = "بيانات أدوات المطبخ";
    public const string SectionHomeAppliance = "بيانات الجهاز";
    public const string SectionBathroomSupply = "بيانات مستلزمات الحمام";
    public const string SectionPlantOrnament = "بيانات النبات";

    public const string SectionSeller = "بيانات البائع";
    public const string SectionDecorAntique = "بيانات القطعة";
    public const string SectionDimensions = "المقاسات";
    public const string SectionAntique = "بيانات الأنتيك";
    public const string SectionPainting = "بيانات اللوحة";
    public const string SectionHandmade = "بيانات المنتج";
    public const string SectionCoinStamp = "بيانات القطعة";

    public static FormFieldDto Text(string name, string label, string? labelEn, string section,
        bool required, int? maxLength = null, string? placeholder = null) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Text,
            Required = required,
            Section = section,
            MaxLength = maxLength,
            MinLength = required ? 1 : null,
            Placeholder = placeholder
        };

    public static FormFieldDto TextArea(string name, string label, string? labelEn, string section,
        bool required, int? maxLength = null, string? placeholder = null) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.TextArea,
            Required = required,
            Section = section,
            MaxLength = maxLength,
            MinLength = required ? 1 : null,
            Placeholder = placeholder
        };

    public static FormFieldDto Number(string name, string label, string? labelEn, string section,
        bool required, decimal? min = null, decimal? max = null, string? placeholder = null) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Number,
            Required = required,
            Section = section,
            MinValue = min,
            MaxValue = max,
            Placeholder = placeholder
        };

    public static FormFieldDto Checkbox(string name, string label, string? labelEn, string section,
        bool defaultValue = false) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Checkbox,
            Required = false,
            Section = section,
            DefaultValue = defaultValue
        };

    public static FormFieldDto DatePicker(string name, string label, string? labelEn, string section,
        bool required) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Date,
            Required = required,
            Section = section,
            HelpText = "التاريخ بصيغة yyyy-MM-dd"
        };

    public static FormFieldDto LookupSelect(string name, string label, string? labelEn, string section,
        string optionsSource, bool required, string type = FormFieldTypes.Select) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = type,
            Required = required,
            Section = section,
            OptionsSource = optionsSource
        };

    public static FormFieldDto AsSearchable(this FormFieldDto field, bool grouped = false)
    {
        field.Searchable = true;

        if (grouped)
            field.Grouped = true;

        return field;
    }

    public static FormFieldDto EnumSelect<TEnum>(string name, string label, string? labelEn, string section,
        IReadOnlyDictionary<TEnum, string> names, bool required, string type = FormFieldTypes.Select)
        where TEnum : struct, Enum =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = type,
            Required = required,
            Section = section,
            Options = names
                .Select(entry => new FormFieldOptionDto(
                    Convert.ToInt32(entry.Key), entry.Value, entry.Key.ToString()))
                .ToList()
        };

    public static FormFieldDto VisibleOnlyWhen(this FormFieldDto field, string otherField, params object[] values)
    {
        field.Visible = false;
        field.VisibleWhen = new FormFieldConditionDto(otherField, values);
        return field;
    }

    public static FormFieldDto MandatoryWhen(this FormFieldDto field, string otherField, params object[] values)
    {
        field.Required = false;
        field.RequiredWhen = new FormFieldConditionDto(otherField, values);
        return field;
    }

    public static FormFieldDto Title(string name = "Title", string label = "عنوان الإعلان",
        string? labelEn = "Advertisement title") =>
        Text(name, label, labelEn, SectionAd, required: true, maxLength: 150,
            placeholder: "اكتب عنوانًا واضحًا ومختصرًا");

    public static FormFieldDto Description(string name = "Description", string label = "الوصف",
        string? labelEn = "Description") =>
        TextArea(name, label, labelEn, SectionAd, required: true, maxLength: 4000,
            placeholder: "اذكر كل التفاصيل المهمة");

    public static FormFieldDto Price() =>
        Number("Price", "السعر (جنيه)", "Price", SectionAd, required: true, min: 0.01m);

    public static FormFieldDto Negotiable() =>
        Checkbox("Negotiable", "السعر قابل للتفاوض", "Negotiable", SectionAd);

    public static IEnumerable<FormFieldDto> SelectionFields(int categoryId, int subCategoryId)
    {
        yield return new FormFieldDto
        {
            Name = "CategoryId",
            Label = "القسم",
            LabelEn = "Category",
            Type = FormFieldTypes.Number,
            Required = true,
            Visible = false,
            ReadOnly = true,
            Section = SectionAd,
            DefaultValue = categoryId,
            HelpText = "يُرسل كما هو — القسم المختار."
        };

        yield return new FormFieldDto
        {
            Name = "SubCategoryId",
            Label = "القسم الفرعي",
            LabelEn = "Sub category",
            Type = FormFieldTypes.Number,
            Required = true,
            Visible = false,
            ReadOnly = true,
            Section = SectionAd,
            DefaultValue = subCategoryId,
            HelpText = "يُرسل كما هو — القسم الفرعي المختار."
        };
    }

    public static FormFieldDto Governorate() =>
        new()
        {
            Name = "Governorate",
            Label = "المحافظة",
            LabelEn = "Governorate",
            Type = FormFieldTypes.Select,
            Required = false,
            Section = SectionLocation,
            DefaultValue = LocationConstants.Governorate,
            ReadOnly = true,
            HelpText = "الخدمة متاحة داخل محافظة الفيوم فقط.",
            Options = new List<FormFieldOptionDto>
            {
                new(LocationConstants.Governorate, LocationConstants.Governorate, "Fayoum")
            }
        };

    public static FormFieldDto Center(
        string label = "المركز", string? labelEn = "Center", bool required = true) =>
        new()
        {
            Name = "Center",
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Select,
            Required = required,
            Section = SectionLocation,
            Options = LocationConstants.Centers
                .Select(center => new FormFieldOptionDto(center, center))
                .ToList()
        };

    public static FormFieldDto Address() =>
        Text("Address", "العنوان", "Address", SectionLocation, required: true, maxLength: 300,
            placeholder: "الشارع، العلامة المميزة…");

    public static FormFieldDto GoogleMapsUrl() =>
        new()
        {
            Name = "GoogleMapsUrl",
            Label = "رابط الموقع على خرائط جوجل",
            LabelEn = "Google Maps link",
            Type = FormFieldTypes.Text,
            Required = false,
            Section = SectionLocation,
            MaxLength = 1000,
            Placeholder = "https://maps.google.com/…"
        };

    public static FormFieldDto PhoneNumber(string name = "PhoneNumber", string label = "رقم الهاتف",
        string? labelEn = "Phone number", bool required = true) =>
        new()
        {
            Name = name,
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Text,
            Required = required,
            Section = SectionContact,
            MaxLength = 20,
            Pattern = AdvertisementCatalog.EgyptianPhonePattern,
            PatternMessage = AdvertisementCatalog.EgyptianPhoneMessage,
            Placeholder = "01012345678"
        };

    public static FormFieldDto WhatsApp() =>
        PhoneNumber("WhatsApp", "رقم الواتساب", "WhatsApp number");

    public static FormFieldDto Email() =>
        new()
        {
            Name = "Email",
            Label = "البريد الإلكتروني",
            LabelEn = "Email",
            Type = FormFieldTypes.Text,
            Required = false,
            Section = SectionContact,
            MaxLength = 256,
            Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            PatternMessage = "من فضلك اكتب بريد إلكتروني صحيح.",
            Placeholder = "name@example.com"
        };

    public static FormFieldDto Images() =>
        new()
        {
            Name = "Images",
            Label = "الصور",
            LabelEn = "Images",
            Type = FormFieldTypes.Image,
            Required = false,
            Section = SectionImages,
            Multiple = true,
            MaxFiles = ImageConstants.MaxImagesPerItem,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ImageConstants.AllowedExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText =
                $"حتى {ImageConstants.MaxImagesPerItem} صور، بحد أقصى " +
                $"{ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت للصورة."
        };

    public static FormFieldDto ProfileImageUpload() =>
        new()
        {
            Name = "ProfileImage",
            Label = "صورة شخصية",
            LabelEn = "Profile image",
            Type = FormFieldTypes.Image,
            Required = false,
            Section = SectionUploads,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ["jpg", "jpeg", "png", "webp"],
            HelpText =
                "اختياري — JPG أو PNG أو WEBP، بحد أقصى " +
                $"{ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت."
        };

    public static FormFieldDto CvUpload() =>
        new()
        {
            Name = "CvFile",
            Label = "السيرة الذاتية",
            LabelEn = "CV",
            Type = FormFieldTypes.File,
            Required = false,
            Section = SectionUploads,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(FileUploadConstants.MaxDocumentSizeBytes / (1024 * 1024)),
            AllowedExtensions = DocumentFormatCatalog.AllExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText =
                $"اختياري — {DocumentFormatCatalog.DisplayNames}، بحد أقصى " +
                $"{FileUploadConstants.MaxDocumentSizeBytes / (1024 * 1024)} ميجابايت."
        };

    public static FormFieldDto IntroVideoUpload() =>
        new()
        {
            Name = "IntroVideo",
            Label = "فيديو تعريفي",
            LabelEn = "Intro video",
            Type = FormFieldTypes.Video,
            Required = false,
            Section = SectionUploads,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)),
            AllowedExtensions = VideoFormatCatalog.AllExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText =
                $"اختياري — {VideoFormatCatalog.DisplayNames}، بحد أقصى " +
                $"{FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت."
        };

    public static FormFieldDto VideoUpload() =>
        new()
        {
            Name = "Video",
            Label = "فيديو الإعلان",
            LabelEn = "Advertisement video",
            Type = FormFieldTypes.Video,
            Required = false,
            Section = SectionImages,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)),
            AllowedExtensions = VideoFormatCatalog.AllExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText =
                $"اختياري — {VideoFormatCatalog.DisplayNames}، بحد أقصى " +
                $"{FileUploadConstants.MaxVideoSizeBytes / (1024 * 1024)} ميجابايت."
        };

    public static FormFieldDto RequiredImages()
    {
        var field = Images();
        field.Required = true;
        field.MinItems = 1;
        field.HelpText =
            $"مطلوبة — حتى {ImageConstants.MaxImagesPerItem} صور، بحد أقصى " +
            $"{ImageConstants.MaxFileSizeBytes / (1024 * 1024)} ميجابايت للصورة.";
        return field;
    }

    public static FormFieldDto LogoUpload(string label = "شعار الشركة", string? labelEn = "Company logo") =>
        new()
        {
            Name = "Logo",
            Label = label,
            LabelEn = labelEn,
            Type = FormFieldTypes.Image,
            Required = false,
            Section = SectionImages,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ImageConstants.AllowedExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText = "اختياري — يُرفع منفصلًا عن صور المعرض."
        };
}
