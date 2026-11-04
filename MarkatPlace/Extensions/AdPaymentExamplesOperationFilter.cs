using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Presentation.Controllers;
using Shared.Constants;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkatPlace.Extensions;

public class AdPaymentExamplesOperationFilter : IOperationFilter
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    private static readonly Guid AdId = Guid.Parse("3c1d2e4f-0000-4000-8000-000000000050");

    private static readonly Guid PaymentId = Guid.Parse("8a4f1b2c-0000-4000-8000-000000000001");

    private static readonly Guid RejectedPaymentId = Guid.Parse("8a4f1b2c-0000-4000-8000-000000000000");

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var type = context.MethodInfo.DeclaringType;
        var name = context.MethodInfo.Name;

        if (type == typeof(AdPaymentConfigController) || type == typeof(PublicAdPaymentController))
        {
            if (name == nameof(AdPaymentConfigController.GetConfig))
            {
                Add(operation, "200", "enabled", "Payments enabled", Config(enabled: true), UserMessages.AdPayments.ConfigLoaded);
                Add(operation, "200", "disabled", "Payments disabled — hide the Payment section", Config(enabled: false), UserMessages.AdPayments.ConfigLoaded);
            }
            else if (name == nameof(AdPaymentConfigController.GetModules))
            {
                Add(operation, "200", "modules", "Applicability map (first rows)", new[]
                {
                    Module(ListingModuleType.Advertisement, 1, null),
                    Module(ListingModuleType.Furniture, 10, 40)
                }, UserMessages.AdPayments.ModulesLoaded);
            }

            return;
        }

        if (type != typeof(AdPaymentsController))
            return;

        switch (name)
        {
            case nameof(AdPaymentsController.Submit):
                Add(operation, "201", "submitted", "Proof received, under review", Submitted(resubmission: false), UserMessages.AdPayments.Submitted);
                Error(operation, "409", "priceChanged", "ExpectedAmount no longer matches the settings",
                    "رسوم نشر الإعلان اتغيرت. الرسوم الحالية 200 EGP، حدّث الصفحة وراجع المبلغ قبل ما تدفع.");
                Error(operation, "409", "alreadyPending", "A submission is already under review",
                    "فيه عملية دفع للإعلان ده قيد المراجعة بالفعل. استنى نتيجة المراجعة.");
                Error(operation, "400", "disabled", "Payments are switched off",
                    "الدفع للإعلانات مش مفعل حالياً، ومش محتاج تدفع حاجة.");
                break;

            case nameof(AdPaymentsController.Resubmit):
                Add(operation, "201", "resubmitted", "New proof after a rejection", Submitted(resubmission: true), UserMessages.AdPayments.Resubmitted);
                Error(operation, "409", "notRejected", "Latest submission is still pending",
                    "إثبات الدفع اللي رفعته لسه قيد المراجعة، ومينفعش تعيد رفعه دلوقتي.");
                Error(operation, "400", "nothingToResubmit", "No submission exists yet",
                    "مفيش عملية دفع مرفوضة للإعلان ده عشان تعيد رفعها. استخدم رفع الدفع العادي.");
                break;

            case nameof(AdPaymentsController.GetStatus):
                Add(operation, "200", "awaitingPayment", "Created, nothing paid yet", Status(AdPaymentCatalog.Stages.AwaitingPayment), UserMessages.AdPayments.StatusLoaded);
                Add(operation, "200", "rejected", "Proof rejected — resubmission allowed", Status(AdPaymentCatalog.Stages.PaymentRejected), UserMessages.AdPayments.StatusLoaded);
                Add(operation, "200", "paid", "Payment approved, waiting for ad moderation", Status(AdPaymentCatalog.Stages.Paid), UserMessages.AdPayments.StatusLoaded);
                break;
        }
    }

    private static PublicAdPaymentConfigDto Config(bool enabled) => new()
    {
        IsEnabled = enabled,
        Price = 150m,
        Currency = PaymentCatalog.DefaultCurrency,
        Instructions = "حوّل المبلغ وارفع صورة الإيصال.",
        UpdatedAt = new DateTime(2026, 11, 4, 8, 0, 0, DateTimeKind.Utc),
        PaymentMethods = enabled
            ? new[]
            {
                new AdPaymentMethodDto
                {
                    Id = 1, Name = "Vodafone Cash", ArabicName = "فودافون كاش", Type = PaymentMethodType.MobileWallet,
                    PhoneNumber = "01000000000", AccountHolderName = "Shopiklopik",
                    PaymentInfo = "01000000000", Instructions = "حوّل على الرقم ده"
                },
                new AdPaymentMethodDto
                {
                    Id = 3, Name = "Bank transfer", ArabicName = "تحويل بنكي", Type = PaymentMethodType.BankAccount,
                    BankName = "البنك الأهلي", AccountHolderName = "Shopiklopik", AccountNumber = "1234567890",
                    Iban = "EG380019000500000000263180002", PaymentInfo = "1234567890"
                }
            }
            : Array.Empty<AdPaymentMethodDto>()
    };

    private static AdPaymentModuleDto Module(ListingModuleType type, int? category, int? subCategory) => new()
    {
        AdvertisementType = type,
        AdvertisementTypeKey = type.ToString(),
        AdvertisementTypeName = ListingModuleCatalog.NameOf(type),
        CategoryId = category,
        SubCategoryId = subCategory,
        ApiRoute = "/" + ListingModuleCatalog.RouteOf(type),
        IsSubjectToPayment = true
    };

    private static AdPaymentSubmissionResultDto Submitted(bool resubmission) => new()
    {
        PaymentSubmissionId = PaymentId,
        AdvertisementId = AdId,
        AdvertisementType = ListingModuleType.Furniture,
        Amount = resubmission ? 200m : 150m,
        Currency = PaymentCatalog.DefaultCurrency,
        PaymentStatus = PaymentStatus.Pending,
        PaymentStatusName = PaymentCatalog.GetStatusName(PaymentStatus.Pending),
        SubmittedAt = new DateTime(2026, 11, 4, 8, 1, 0, DateTimeKind.Utc),
        PaymentMethodId = 1,
        PaymentMethodName = "فودافون كاش",
        TransactionReference = "TX-123",
        IsResubmission = resubmission,
        PreviousPaymentSubmissionId = resubmission ? RejectedPaymentId : null,
        Stage = AdPaymentCatalog.Stages.PaymentUnderReview,
        StageName = AdPaymentCatalog.Stages.NameOf(AdPaymentCatalog.Stages.PaymentUnderReview),
        StatusEndpoint = AdPaymentCatalog.SubmitRouteOf(ListingModuleType.Furniture, AdId)
    };

    private static AdPaymentStatusDto Status(string stage)
    {
        var latest = stage switch
        {
            AdPaymentCatalog.Stages.PaymentRejected => PaymentStatus.Rejected,
            AdPaymentCatalog.Stages.Paid => PaymentStatus.Approved,
            _ => (PaymentStatus?)null
        };

        var required = stage is AdPaymentCatalog.Stages.AwaitingPayment or AdPaymentCatalog.Stages.PaymentRejected;

        return new AdPaymentStatusDto
        {
            AdvertisementId = AdId,
            AdvertisementType = ListingModuleType.Furniture,
            AdvertisementTitle = "كنبة 3 مقاعد",
            AdvertisementModerationStatus = nameof(ModerationStatus.Pending),
            AdvertisementModerationStatusName = ModerationCatalog.NameOf(ModerationStatus.Pending),
            PaymentsEnabled = true,
            IsPaymentRequired = required,
            Stage = stage,
            StageName = AdPaymentCatalog.Stages.NameOf(stage),
            CanSubmit = required,
            CanResubmit = latest == PaymentStatus.Rejected,
            HasPayment = latest is not null,
            PaymentSubmissionId = latest is null ? null : PaymentId,
            PaymentStatus = latest,
            PaymentStatusName = latest is { } status ? PaymentCatalog.GetStatusName(status) : null,
            Amount = latest is null ? null : 150m,
            Currency = latest is null ? null : PaymentCatalog.DefaultCurrency,
            PaymentMethodId = latest is null ? null : 1,
            PaymentMethodName = latest is null ? null : "فودافون كاش",
            TransactionReference = latest is null ? null : "TX-123",
            SubmittedAt = latest is null ? null : new DateTime(2026, 11, 4, 8, 1, 0, DateTimeKind.Utc),
            ReviewedAt = latest is null ? null : new DateTime(2026, 11, 4, 9, 0, 0, DateTimeKind.Utc),
            RejectionReason = latest == PaymentStatus.Rejected ? "المبلغ ناقص" : null,
            CurrentPrice = 200m,
            CurrentCurrency = PaymentCatalog.DefaultCurrency,
            SubmitEndpoint = AdPaymentCatalog.SubmitRouteOf(ListingModuleType.Furniture, AdId),
            ResubmitEndpoint = AdPaymentCatalog.ResubmitRouteOf(ListingModuleType.Furniture, AdId)
        };
    }

    private static void Add<T>(OpenApiOperation operation, string code, string key, string summary, T data, string message) =>
        Set(operation, code, key, summary, ApiResponse<T>.Ok(data, message));

    private static void Error(OpenApiOperation operation, string code, string key, string summary, string message) =>
        Set(operation, code, key, summary, new { success = false, message, data = (object?)null, errors = (object?)null });

    private static void Set(OpenApiOperation operation, string code, string key, string summary, object envelope)
    {
        if (!operation.Responses.TryGetValue(code, out var response))
            return;

        if (!response.Content.TryGetValue("application/json", out var mediaType))
        {
            mediaType = new OpenApiMediaType();
            response.Content["application/json"] = mediaType;
        }

        mediaType.Examples[key] = new OpenApiExample
        {
            Summary = summary,
            Value = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(envelope, SerializerOptions))
        };
    }
}
