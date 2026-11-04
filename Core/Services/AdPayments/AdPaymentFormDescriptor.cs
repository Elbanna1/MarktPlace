using Shared.Constants;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;

namespace Services.AdPayments;

public static class AdPaymentFormDescriptor
{
    public static CreateAdFormPaymentDto? For(int? subCategoryId)
    {
        if (subCategoryId is not { } id || ListingModuleCatalog.ModuleOf(id) is not { } type)
            return null;

        return For(type);
    }

    public static CreateAdFormPaymentDto For(ListingModuleType type) => new()
    {
        IsSubjectToPayment = AdPaymentCatalog.IsSubjectToPayment(type),
        AdvertisementType = type,
        AdvertisementTypeKey = type.ToString(),
        ConfigEndpoint = AdPaymentCatalog.ConfigRoute,
        SubmitEndpoint = AdPaymentCatalog.SubmitRouteTemplate,
        ResubmitEndpoint = AdPaymentCatalog.ResubmitRouteTemplate,
        StatusEndpoint = AdPaymentCatalog.StatusRouteTemplate,
        ContentType = AdPaymentCatalog.ContentType,
        Section = AdPaymentCatalog.FormSection,
        Fields = Fields()
    };

    public static IReadOnlyList<FormFieldDto> Fields() =>
    [
        new()
        {
            Name = AdPaymentCatalog.Fields.PaymentMethodId,
            Label = "طريقة الدفع",
            LabelEn = "Payment method",
            Type = FormFieldTypes.Radio,
            Required = true,
            Order = 1,
            Section = AdPaymentCatalog.FormSection,
            OptionsSource = "paymentMethods",
            HelpText = "اختار طريقة من طرق الدفع المتاحة، وهتظهر لك بيانات التحويل بتاعتها."
        },
        new()
        {
            Name = AdPaymentCatalog.Fields.TransactionReference,
            Label = "رقم العملية",
            LabelEn = "Transaction reference",
            Type = FormFieldTypes.Text,
            Required = false,
            Order = 2,
            Section = AdPaymentCatalog.FormSection,
            MaxLength = AdPaymentService.MaxTransactionReferenceLength,
            Placeholder = "مثال: TX-123456"
        },
        new()
        {
            Name = AdPaymentCatalog.Fields.Screenshot,
            Label = "صورة إيصال الدفع",
            LabelEn = "Payment proof",
            Type = FormFieldTypes.Image,
            Required = true,
            Order = 3,
            Section = AdPaymentCatalog.FormSection,
            Multiple = false,
            MaxFiles = 1,
            MaxSizeMb = (int)(ImageConstants.MaxFileSizeBytes / (1024 * 1024)),
            AllowedExtensions = ImageConstants.AllowedExtensions
                .Select(extension => extension.TrimStart('.'))
                .ToList(),
            HelpText = "ارفع صورة واضحة لإيصال التحويل أو لقطة شاشة لعملية الدفع."
        },
        new()
        {
            Name = AdPaymentCatalog.Fields.Notes,
            Label = "ملاحظات",
            LabelEn = "Notes",
            Type = FormFieldTypes.TextArea,
            Required = false,
            Order = 4,
            Section = AdPaymentCatalog.FormSection,
            MaxLength = AdPaymentCatalog.MaxNotesLength
        },
        new()
        {
            Name = AdPaymentCatalog.Fields.ExpectedAmount,
            Label = "المبلغ المعروض",
            LabelEn = "Displayed price",
            Type = FormFieldTypes.Number,
            Required = false,
            Visible = false,
            Order = 5,
            Section = AdPaymentCatalog.FormSection,
            HelpText = "حقل مخفي: ابعت قيمة price من إعدادات الدفع اللي اتعرضت للمستخدم."
        },
        new()
        {
            Name = AdPaymentCatalog.Fields.ExpectedCurrency,
            Label = "العملة المعروضة",
            LabelEn = "Displayed currency",
            Type = FormFieldTypes.Text,
            Required = false,
            Visible = false,
            Order = 6,
            Section = AdPaymentCatalog.FormSection,
            MaxLength = 3,
            HelpText = "حقل مخفي: ابعت قيمة currency من إعدادات الدفع اللي اتعرضت للمستخدم."
        }
    ];
}
