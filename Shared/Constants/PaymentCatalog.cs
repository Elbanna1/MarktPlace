using Shared.Enums;

namespace Shared.Constants;

public static class PaymentCatalog
{
    public const decimal MinAmount = 1m;

    public const decimal MaxAmount = 1_000_000m;

    public const string DefaultCurrency = "EGP";

    public const int MaxNotesLength = 1000;

    public const int MaxRejectReasonLength = 500;

    public static readonly IReadOnlyDictionary<PaymentMethodType, string> MethodTypeNames =
        new Dictionary<PaymentMethodType, string>
        {
            [PaymentMethodType.MobileWallet] = "محفظة إلكترونية",
            [PaymentMethodType.InstaPay] = "إنستا باي",
            [PaymentMethodType.BankAccount] = "حساب بنكي",
            [PaymentMethodType.Other] = "أخرى"
        };

    public static readonly IReadOnlyDictionary<PaymentStatus, string> StatusNames =
        new Dictionary<PaymentStatus, string>
        {
            [PaymentStatus.Pending] = "قيد المراجعة",
            [PaymentStatus.Approved] = "مقبول",
            [PaymentStatus.Rejected] = "مرفوض",
            [PaymentStatus.Refunded] = "مسترد"
        };

    public static string GetMethodTypeName(PaymentMethodType type) =>
        MethodTypeNames.TryGetValue(type, out var name) ? name : type.ToString();

    public static string GetStatusName(PaymentStatus status) =>
        StatusNames.TryGetValue(status, out var name) ? name : status.ToString();

    public static string? ResolvePaymentInfo(
        PaymentMethodType type,
        string? phoneNumber,
        string? instaPayIdentifier,
        string? accountNumber) =>
        type switch
        {
            PaymentMethodType.MobileWallet => phoneNumber,
            PaymentMethodType.InstaPay => instaPayIdentifier,
            PaymentMethodType.BankAccount => accountNumber,
            _ => null
        };

    public static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static class SeedIds
    {
        public const int VodafoneCash = 1;
        public const int InstaPay = 2;
        public const int BankAccount = 3;
    }

    public static readonly IReadOnlyList<SeedPaymentMethod> SeedMethods = new List<SeedPaymentMethod>
    {
        new()
        {
            Id = SeedIds.VodafoneCash,
            Name = "Vodafone Cash",
            ArabicName = "فودافون كاش",
            Type = PaymentMethodType.MobileWallet,
            PhoneNumber = "01026568617",
            DisplayOrder = 1,
            Instructions = "حوّل المبلغ إلى رقم المحفظة ثم ارفع صورة إيصال التحويل."
        },
        new()
        {
            Id = SeedIds.InstaPay,
            Name = "InstaPay",
            ArabicName = "إنستا باي",
            Type = PaymentMethodType.InstaPay,
            InstaPayIdentifier = "01026568617",
            DisplayOrder = 2,
            Instructions = "حوّل المبلغ عبر إنستا باي إلى المعرّف الموضح ثم ارفع صورة الإيصال."
        },
        new()
        {
            Id = SeedIds.BankAccount,
            Name = "Bank Transfer",
            ArabicName = "تحويل بنكي",
            Type = PaymentMethodType.BankAccount,
            BankName = "بنك مصر",
            AccountHolderName = "MarkatPlace",
            AccountNumber = "0000000000000000",
            Iban = null,
            DisplayOrder = 3,
            Instructions = "حوّل المبلغ إلى الحساب البنكي الموضح ثم ارفع صورة إيصال التحويل."
        }
    };

    public sealed class SeedPaymentMethod
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string? ArabicName { get; init; }
        public PaymentMethodType Type { get; init; }
        public string? PhoneNumber { get; init; }
        public string? InstaPayIdentifier { get; init; }
        public string? BankName { get; init; }
        public string? AccountHolderName { get; init; }
        public string? AccountNumber { get; init; }
        public string? Iban { get; init; }
        public string? Instructions { get; init; }
        public int DisplayOrder { get; init; }
    }
}
