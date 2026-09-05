using Shared.Enums;

namespace Shared.Constants;

public sealed record NotificationSubject(
    string ReferenceType, string EntityName, string Icon, string? Route);

public sealed record NotificationContent(
    string Title,
    string Message,
    string Icon,
    NotificationType Type,
    string ReferenceType,
    string EntityName,
    string? DeepLink,
    ListingModuleType? ListingType = null,
    int? CategoryId = null,
    int? SubCategoryId = null,
    string? ImageUrl = null,
    string? CategoryName = null,
    string? SubCategoryName = null,
    string? ListingTitle = null,
    string? OwnerId = null,
    string? OwnerName = null)
{
    public NotificationContent WithListing(
        ListingModuleType? listingType = null,
        int? categoryId = null,
        int? subCategoryId = null,
        string? imageUrl = null) =>
        this with
        {
            ListingType = listingType ?? ListingType,
            CategoryId = categoryId ?? CategoryId,
            SubCategoryId = subCategoryId ?? SubCategoryId,
            ImageUrl = imageUrl ?? ImageUrl
        };

    public NotificationContent WithContext(
        string? categoryName = null,
        string? subCategoryName = null,
        string? listingTitle = null,
        string? ownerId = null,
        string? ownerName = null) =>
        this with
        {
            CategoryName = categoryName ?? CategoryName,
            SubCategoryName = subCategoryName ?? SubCategoryName,
            ListingTitle = listingTitle ?? ListingTitle,
            OwnerId = ownerId ?? OwnerId,
            OwnerName = ownerName ?? OwnerName
        };
}

public static class NotificationCatalog
{
    public static readonly NotificationSubject LostItem =
        new(NotificationReferenceTypes.LostFoundPost, "بلاغ المفقودات", "🔍", "/lost-found/lost");

    public static readonly NotificationSubject FoundItem =
        new(NotificationReferenceTypes.LostFoundPost, "بلاغ المعثورات", "🎉", "/lost-found/found");

    public static readonly NotificationSubject Account =
        new(NotificationReferenceTypes.Account, "الحساب", "🛡️", null);

    public static readonly NotificationSubject Profile =
        new(NotificationReferenceTypes.Profile, "بيانات الحساب", "👤", "/profile");

    public static readonly NotificationSubject Payment =
        new(NotificationReferenceTypes.Payment, "الدفع", "💳", "/payments");

    public static readonly NotificationSubject Feedback =
        new(NotificationReferenceTypes.Feedback, "الملاحظة", "📝", "/feedback");

    public static readonly NotificationSubject BannerBooking =
        new(NotificationReferenceTypes.BannerBooking, "حجز الإعلان", "🖼️", "/banner-bookings");

    public static readonly NotificationSubject Referral =
        new(NotificationReferenceTypes.Referral, "الدعوة", "🎉", "/referrals");

    private static readonly IReadOnlyDictionary<ListingModuleType, NotificationSubject> Subjects =
        new Dictionary<ListingModuleType, NotificationSubject>
        {
            [ListingModuleType.Advertisement] = new(NotificationReferenceTypes.Advertisement, "الإعلان", "📢", "/ads"),
            [ListingModuleType.Craftsman] = new(NotificationReferenceTypes.Craftsman, "بيانات الحرفي", "🛠️", "/craftsmen"),
            [ListingModuleType.Workshop] = new(NotificationReferenceTypes.Workshop, "بيانات الورشة", "🏭", "/workshops"),

            [ListingModuleType.Factory] = new(NotificationReferenceTypes.Factory, "إعلان المصنع", "🏢", "/factories"),
            [ListingModuleType.Farm] = new(NotificationReferenceTypes.Farm, "إعلان المزرعة", "🌾", "/farms"),
            [ListingModuleType.Company] = new(NotificationReferenceTypes.Company, "إعلان الشركة", "🏢", "/companies"),
            [ListingModuleType.Supplier] = new(NotificationReferenceTypes.Supplier, "إعلان المورد", "📦", "/suppliers"),
            [ListingModuleType.WholesaleTrader] = new(NotificationReferenceTypes.WholesaleTrader, "إعلان تاجر الجملة", "🏬", "/wholesale-traders"),
            [ListingModuleType.FruitVegetableMerchant] = new(NotificationReferenceTypes.FruitVegetableMerchant, "إعلان تاجر الخضر والفاكهة", "🥬", "/fruit-vegetable-merchants"),

            [ListingModuleType.JobRequest] = new(NotificationReferenceTypes.JobRequest, "طلب الوظيفة", "💼", "/job-requests"),
            [ListingModuleType.JobOpportunity] = new(NotificationReferenceTypes.JobOpportunity, "فرصة العمل", "📢", "/job-opportunities"),

            [ListingModuleType.Livestock] = new(NotificationReferenceTypes.Livestock, "إعلان المواشي", "🐄", "/livestock"),
            [ListingModuleType.SheepGoat] = new(NotificationReferenceTypes.SheepGoat, "إعلان الأغنام والماعز", "🐑", "/sheep-goats"),
            [ListingModuleType.Horse] = new(NotificationReferenceTypes.Horse, "إعلان الخيول", "🐎", "/horses"),
            [ListingModuleType.Camel] = new(NotificationReferenceTypes.Camel, "إعلان الإبل", "🐪", "/camels"),
            [ListingModuleType.Bird] = new(NotificationReferenceTypes.Bird, "إعلان الطيور", "🐦", "/birds"),
            [ListingModuleType.Pet] = new(NotificationReferenceTypes.Pet, "إعلان الحيوانات الأليفة", "🐈", "/pets"),
            [ListingModuleType.Fish] = new(NotificationReferenceTypes.Fish, "إعلان الأسماك", "🐟", "/fish"),
            [ListingModuleType.Bee] = new(NotificationReferenceTypes.Bee, "إعلان النحل", "🐝", "/bees"),
            [ListingModuleType.OtherAnimal] = new(NotificationReferenceTypes.OtherAnimal, "إعلان الحيوان", "🐾", "/other-animals"),

            [ListingModuleType.DecorAntique] = new(NotificationReferenceTypes.DecorAntique, "إعلان التحف", "🏺", "/decor-antiques"),
            [ListingModuleType.Antique] = new(NotificationReferenceTypes.Antique, "إعلان الأنتيكات", "🏺", "/antiques"),
            [ListingModuleType.Painting] = new(NotificationReferenceTypes.Painting, "اللوحة الفنية", "🖼️", "/paintings"),
            [ListingModuleType.Handmade] = new(NotificationReferenceTypes.Handmade, "إعلان الأعمال اليدوية", "🧵", "/handmade"),
            [ListingModuleType.CoinStamp] = new(NotificationReferenceTypes.CoinStamp, "إعلان العملات والطوابع", "🪙", "/coins-stamps"),

            [ListingModuleType.MenClothing] = new(NotificationReferenceTypes.MenClothing, "إعلان الملابس الرجالي", "👕", "/men-clothing"),
            [ListingModuleType.WomenClothing] = new(NotificationReferenceTypes.WomenClothing, "إعلان الملابس الحريمي", "👗", "/women-clothing"),
            [ListingModuleType.KidsClothing] = new(NotificationReferenceTypes.KidsClothing, "إعلان ملابس الأطفال", "🧸", "/kids-clothing"),

            [ListingModuleType.Accessory] = new(NotificationReferenceTypes.Accessory, "إعلان الإكسسوارات", "💍", "/accessories"),
            [ListingModuleType.Cosmetic] = new(NotificationReferenceTypes.Cosmetic, "إعلان مستحضرات التجميل", "💄", "/cosmetics"),
            [ListingModuleType.HomeKitchen] = new(NotificationReferenceTypes.HomeKitchen, "إعلان المنزل والمطبخ", "🍽️", "/home-kitchen"),
            [ListingModuleType.ShoppingElectronic] = new(NotificationReferenceTypes.ShoppingElectronic, "إعلان الإلكترونيات", "📱", "/shopping-electronics"),
            [ListingModuleType.GiftToy] = new(NotificationReferenceTypes.GiftToy, "إعلان الهدايا والألعاب", "🎁", "/gifts-toys"),
            [ListingModuleType.HomemadeFood] = new(NotificationReferenceTypes.HomemadeFood, "إعلان الأكل المنزلي", "🍲", "/homemade-food"),

            [ListingModuleType.Furniture] = new(NotificationReferenceTypes.Furniture, "إعلان الأثاث", "🛋️", "/furniture"),
            [ListingModuleType.FurnishingCurtain] = new(NotificationReferenceTypes.FurnishingCurtain, "إعلان المفروشات والستائر", "🪟", "/furnishings-curtains"),
            [ListingModuleType.LightingDecor] = new(NotificationReferenceTypes.LightingDecor, "إعلان الإضاءة والديكور", "💡", "/lighting-decor"),
            [ListingModuleType.KitchenTool] = new(NotificationReferenceTypes.KitchenTool, "إعلان أدوات المطبخ", "🍳", "/kitchen-tools"),
            [ListingModuleType.HomeAppliance] = new(NotificationReferenceTypes.HomeAppliance, "إعلان الأجهزة الكهربائية", "🔌", "/home-appliances"),
            [ListingModuleType.BathroomSupply] = new(NotificationReferenceTypes.BathroomSupply, "إعلان مستلزمات الحمام", "🚿", "/bathroom-supplies"),
            [ListingModuleType.PlantOrnament] = new(NotificationReferenceTypes.PlantOrnament, "إعلان النباتات والزينة", "🪴", "/plants-ornaments"),

            [ListingModuleType.Land] = new(NotificationReferenceTypes.Land, "إعلان الأرض", "🏞️", "/lands"),
            [ListingModuleType.Apartment] = new(NotificationReferenceTypes.Apartment, "إعلان الشقة", "🏢", "/apartments"),
            [ListingModuleType.Shop] = new(NotificationReferenceTypes.Shop, "إعلان المحل", "🏪", "/shops"),

            [ListingModuleType.Rescue] = new(NotificationReferenceTypes.Rescue, "الاستغاثة", "🆘", "/rescues"),
            [ListingModuleType.BloodRequest] = new(NotificationReferenceTypes.BloodRequest, "طلب فصيلة الدم", "🩸", "/blood-requests"),
            [ListingModuleType.AskConsult] = new(NotificationReferenceTypes.AskConsult, "السؤال", "❓", "/ask-consults"),

            [ListingModuleType.LostItem] = LostItem,
            [ListingModuleType.FoundItem] = FoundItem
        };

    public static NotificationSubject For(ListingModuleType module) =>
        Subjects.TryGetValue(module, out var subject)
            ? subject
            : throw new ArgumentOutOfRangeException(nameof(module), module,
                "This listing module has no notification subject registered in NotificationCatalog.");

    public static IReadOnlyDictionary<ListingModuleType, NotificationSubject> AllListingSubjects => Subjects;

    public static NotificationContent Build(
        NotificationSubject subject, NotificationAction action, Guid? entityId, string? entityTitle = null)
    {
        var (title, body, type) = Describe(subject, action);

        var message = string.IsNullOrWhiteSpace(entityTitle)
            ? body
            : $"{body} ({entityTitle.Trim()})";

        return new NotificationContent(
            Title: title,
            Message: $"{subject.Icon} {message}",
            Icon: subject.Icon,
            Type: type,
            ReferenceType: subject.ReferenceType,
            EntityName: subject.EntityName,
            DeepLink: BuildDeepLink(subject, entityId));
    }

    private static string? BuildDeepLink(NotificationSubject subject, Guid? entityId) =>
        subject.Route is null
            ? null
            : entityId is { } id
                ? $"{subject.Route}/{id}"
                : subject.Route;

    public static class BannerBookings
    {
        public static NotificationContent Submitted(Guid bookingId, string? adTitle) => Content(
            "استلام حجز الإعلان",
            "تم استلام حجز الإعلان الخاص بك وهو الآن قيد المراجعة.",
            NotificationType.BannerBookingSubmitted, bookingId, adTitle);

        public static NotificationContent PaymentApproved(Guid bookingId, string? adTitle) => Content(
            "تم تأكيد الدفع",
            "تم تأكيد استلام قيمة الإعلان. جاري الآن مراجعة محتوى الإعلان قبل النشر.",
            NotificationType.BannerPaymentApproved, bookingId, adTitle, icon: "💳");

        public static NotificationContent PaymentRejected(Guid bookingId, string? adTitle, string? notes) => Content(
            "تعذر تأكيد الدفع",
            Append("لم نتمكن من تأكيد عملية التحويل الخاصة بحجز الإعلان.", notes),
            NotificationType.BannerPaymentRejected, bookingId, adTitle, icon: "💳");

        public static NotificationContent Approved(Guid bookingId, string? adTitle, DateTime startDate) => Content(
            "تمت الموافقة على الإعلان",
            $"تمت الموافقة على إعلانك، وسيبدأ ظهوره في {startDate:yyyy/MM/dd}.",
            NotificationType.BannerBookingApproved, bookingId, adTitle, icon: "✅");

        public static NotificationContent Published(
            Guid bookingId, string? adTitle, string placementName, DateTime endDate) => Content(
            "تم نشر الإعلان",
            $"إعلانك يظهر الآن في {placementName}، وينتهي في {endDate:yyyy/MM/dd}.",
            NotificationType.BannerBookingPublished, bookingId, adTitle, icon: "🚀");

        public static NotificationContent Rejected(
            Guid bookingId, string? adTitle, string reasonName, string? notes) => Content(
            "تم رفض الإعلان",
            Append($"تم رفض حجز الإعلان. السبب: {reasonName}.", notes),
            NotificationType.BannerBookingRejected, bookingId, adTitle, icon: "❌");

        public static NotificationContent Expired(Guid bookingId, string? adTitle, int durationDays) => Content(
            "انتهت مدة الإعلان",
            $"انتهت مدة ظهور إعلانك ({BannerBookingCatalog.FormatDuration(durationDays)}). " +
            "يمكنك حجز مساحة جديدة في أي وقت.",
            NotificationType.BannerBookingExpired, bookingId, adTitle, icon: "⌛");

        private static NotificationContent Content(
            string title, string body, NotificationType type, Guid bookingId, string? adTitle, string? icon = null)
        {
            var resolvedIcon = icon ?? BannerBooking.Icon;

            var message = string.IsNullOrWhiteSpace(adTitle)
                ? body
                : $"{body} ({adTitle.Trim()})";

            return new NotificationContent(
                Title: title,
                Message: $"{resolvedIcon} {message}",
                Icon: resolvedIcon,
                Type: type,
                ReferenceType: BannerBooking.ReferenceType,
                EntityName: BannerBooking.EntityName,
                DeepLink: $"{BannerBooking.Route}/{bookingId}");
        }

        private static string Append(string body, string? notes) =>
            string.IsNullOrWhiteSpace(notes) ? body : $"{body} {notes.Trim()}";
    }

    public static class Interests
    {
        public static NotificationContent NewListing(
            ListingModuleType module,
            string? categoryName,
            string? subCategoryName,
            Guid listingId,
            string? listingTitle,
            int categoryId,
            int subCategoryId,
            string? imageUrl,
            string? ownerId = null,
            string? ownerName = null)
        {
            var subject = For(module);

            var category = string.IsNullOrWhiteSpace(categoryName)
                ? subject.EntityName
                : categoryName.Trim();

            var sub = string.IsNullOrWhiteSpace(subCategoryName) ? null : subCategoryName.Trim();

            var place = sub is null ? category : $"{category} ← {sub}";

            var publisher = string.IsNullOrWhiteSpace(ownerName) ? null : ownerName.Trim();
            var listing = string.IsNullOrWhiteSpace(listingTitle) ? null : listingTitle.Trim();

            var subjectPart = publisher is null ? "تم نشر" : $"{publisher} نشر";
            var objectPart = listing is null ? "إعلانًا جديدًا" : $"إعلان {listing}";

            var message = $"{subjectPart} {objectPart} في قسم {place}";

            return new NotificationContent(
                Title: $"إعلان جديد في {place}",
                Message: $"{subject.Icon} {message}",
                Icon: subject.Icon,
                Type: NotificationType.NewListingForInterest,
                ReferenceType: subject.ReferenceType,
                EntityName: subject.EntityName,
                DeepLink: BuildDeepLink(subject, listingId),
                ListingType: module,
                CategoryId: categoryId,
                SubCategoryId: subCategoryId,
                ImageUrl: imageUrl,
                CategoryName: category,
                SubCategoryName: sub,
                ListingTitle: listing,
                OwnerId: ownerId,
                OwnerName: publisher);
        }
    }

    public static class Referrals
    {
        public static NotificationContent Joined(Guid referralId, int totalReferrals)
        {
            var body = totalReferrals > 1
                ? $"انضم مستخدم جديد عن طريق رابط الدعوة الخاص بك. إجمالي دعواتك الآن {totalReferrals}."
                : "انضم مستخدم جديد عن طريق رابط الدعوة الخاص بك.";

            return new NotificationContent(
                Title: "دعوة ناجحة",
                Message: $"{Referral.Icon} {body}",
                Icon: Referral.Icon,
                Type: NotificationType.ReferralCompleted,
                ReferenceType: Referral.ReferenceType,
                EntityName: Referral.EntityName,
                DeepLink: $"{Referral.Route}/{referralId}");
        }
    }

    public static class Moderation
    {
        public static NotificationContent Approved(
            ListingModuleType module, Guid listingId, string? listingTitle) =>
            Content(module, listingId, listingTitle,
                title: "تمت الموافقة على الإعلان",
                body: $"تمت مراجعة {For(module).EntityName} والموافقة عليه، وهو الآن منشور.",
                type: NotificationType.AdvertisementApproved,
                icon: "✅");

        public static NotificationContent Rejected(
            ListingModuleType module, Guid listingId, string? listingTitle,
            ListingRejectionReason reason, string? notes) =>
            Content(module, listingId, listingTitle,
                title: "تم رفض الإعلان",
                body: Append(
                    $"تم رفض {For(module).EntityName}. السبب: {ModerationCatalog.NameOf(reason)}.",
                    notes),
                type: NotificationType.AdvertisementRejected,
                icon: "❌");

        public static NotificationContent Suspended(
            ListingModuleType module, Guid listingId, string? listingTitle, string? notes) =>
            Content(module, listingId, listingTitle,
                title: "تم إيقاف الإعلان",
                body: Append(
                    $"تم إيقاف {For(module).EntityName} مؤقتًا بواسطة الإدارة.",
                    notes),
                type: NotificationType.ListingSuspended,
                icon: "⏸️");

        private static NotificationContent Content(
            ListingModuleType module, Guid listingId, string? listingTitle,
            string title, string body, NotificationType type, string icon)
        {
            var subject = For(module);

            var message = string.IsNullOrWhiteSpace(listingTitle)
                ? body
                : $"{body} ({listingTitle.Trim()})";

            return new NotificationContent(
                Title: title,
                Message: $"{icon} {message}",
                Icon: icon,
                Type: type,
                ReferenceType: subject.ReferenceType,
                EntityName: subject.EntityName,
                DeepLink: BuildDeepLink(subject, listingId));
        }

        private static string Append(string body, string? notes) =>
            string.IsNullOrWhiteSpace(notes) ? body : $"{body} {notes.Trim()}";
    }

    public static class AdminAlerts
    {
        public static NotificationContent PendingListing(
            ListingModuleType module,
            Guid listingId,
            string? title,
            string? ownerId = null,
            string? ownerName = null,
            int? categoryId = null,
            string? categoryName = null,
            int? subCategoryId = null,
            string? subCategoryName = null,
            string? imageUrl = null)
        {
            var subject = For(module);

            var category = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName.Trim();
            var sub = string.IsNullOrWhiteSpace(subCategoryName) ? null : subCategoryName.Trim();
            var owner = string.IsNullOrWhiteSpace(ownerName) ? null : ownerName.Trim();

            var place = category is null ? null : sub is null ? category : $"{category} ← {sub}";

            var body = owner is null
                ? $"تم إرسال {subject.EntityName} وهو بانتظار المراجعة."
                : $"قام {owner} بإرسال {subject.EntityName} وهو بانتظار المراجعة.";

            if (place is not null)
                body = $"{body} القسم: {place}.";

            return Content(
                title: "إعلان جديد بانتظار المراجعة",
                body: Describe(body, title),
                type: NotificationType.AdminPendingListing,
                icon: "📢",
                referenceType: subject.ReferenceType,
                entityName: subject.EntityName,
                deepLink: $"/admin/ads/{module}/{listingId}",
                listingType: module) with
            {
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                CategoryName = category,
                SubCategoryName = sub,
                ListingTitle = string.IsNullOrWhiteSpace(title) ? null : title.Trim(),
                OwnerId = ownerId,
                OwnerName = owner,
                ImageUrl = imageUrl
            };
        }

        public static NotificationContent NewReport(
            Guid reportId, ListingModuleType module, Guid listingId, string? listingTitle, string reasonName) =>
            Content(
                title: "بلاغ جديد",
                body: Describe($"تم الإبلاغ عن {For(module).EntityName}. سبب البلاغ: {reasonName}.", listingTitle),
                type: NotificationType.AdminNewReport,
                icon: "🚨",
                referenceType: NotificationReferenceTypes.Advertisement,
                entityName: "بلاغ",
                deepLink: $"/admin/reports/{reportId}",
                listingType: module);

        public static NotificationContent NewBannerRequest(
            Guid bookingId, string? title, string advertiserName, decimal price) =>
            Content(
                title: "طلب حجز بانر جديد",
                body: Describe(
                    $"طلب حجز بانر جديد من {advertiserName} بقيمة {price:N0} {PaymentCatalog.DefaultCurrency}.",
                    title),
                type: NotificationType.AdminNewBannerRequest,
                icon: "🖼️",
                referenceType: NotificationReferenceTypes.BannerBooking,
                entityName: "حجز إعلان",
                deepLink: $"/admin/banner-requests/{bookingId}");

        public static NotificationContent NewPayment(
            Guid paymentId, decimal amount, string currency, string? payerName) =>
            Content(
                title: "إثبات دفع جديد",
                body: Describe($"تم رفع إثبات دفع بقيمة {amount:N0} {currency} بانتظار المراجعة.", payerName),
                type: NotificationType.AdminNewPayment,
                icon: "💳",
                referenceType: NotificationReferenceTypes.Payment,
                entityName: "عملية دفع",
                deepLink: $"/admin/payments/{paymentId}");

        public static NotificationContent UserModeration(
            string targetUserId, string? targetUserName, UserAccountStatus status, string? reason)
        {
            var verb = status == UserAccountStatus.Blocked ? "حظر" : "إيقاف";

            var body = $"تم {verb} حساب المستخدم.";

            if (!string.IsNullOrWhiteSpace(reason))
                body = $"{body} السبب: {reason.Trim()}";

            return Content(
                title: $"{verb} حساب مستخدم",
                body: Describe(body, targetUserName),
                type: NotificationType.AdminUserModeration,
                icon: status == UserAccountStatus.Blocked ? "🚫" : "⏸️",
                referenceType: NotificationReferenceTypes.Account,
                entityName: "مستخدم",
                deepLink: $"/admin/users/{targetUserId}");
        }

        public static NotificationContent BannerExpiring(
            Guid bookingId, string? title, DateTime endDate, int daysLeft) =>
            Content(
                title: "بانر يقترب من الانتهاء",
                body: Describe(
                    $"ينتهي هذا البانر خلال {daysLeft} يوم (في {endDate:yyyy/MM/dd}).", title),
                type: NotificationType.AdminBannerExpiring,
                icon: "⌛",
                referenceType: NotificationReferenceTypes.BannerBooking,
                entityName: "حجز إعلان",
                deepLink: $"/admin/banner-requests/{bookingId}");

        private static string Describe(string body, string? name) =>
            string.IsNullOrWhiteSpace(name) ? body : $"{body} ({name.Trim()})";

        private static NotificationContent Content(
            string title, string body, NotificationType type, string icon,
            string referenceType, string entityName, string? deepLink,
            ListingModuleType? listingType = null) =>
            new(
                Title: title,
                Message: $"{icon} {body}",
                Icon: icon,
                Type: type,
                ReferenceType: referenceType,
                EntityName: entityName,
                DeepLink: deepLink,
                ListingType: listingType);
    }

    public static class AdminAccounts
    {
        private const string Icon = "🛡️";

        private const int MaxMessageLength = 1000;

        public static NotificationContent Confirmed(
            IReadOnlyList<(string PageKey, AdminPermission Permissions)> pages)
        {
            var granted = pages
                .Select(entry => (Page: AdminPageCatalog.Find(entry.PageKey), entry.Permissions))
                .Where(entry => entry.Page is not null && entry.Permissions != AdminPermission.None)
                .Select(entry => $"- {entry.Page!.NameAr}: {Describe(entry.Permissions)}")
                .ToList();

            var body = granted.Count == 0
                ? "تم اعتماد حسابك كأدمن بنجاح. لسه مفيش صفحات متحددة ليك، والإدارة هتحدد صلاحياتك."
                : "تم اعتماد حسابك كأدمن بنجاح، وتم منحك الصلاحيات المحددة من الإدارة." +
                  "\n\nالصفحات والصلاحيات الممنوحة لك:\n" + string.Join("\n", granted);

            return new NotificationContent(
                Title: "تم اعتماد حسابك كأدمن",
                Message: Fit($"{Icon} {body}"),
                Icon: Icon,
                Type: NotificationType.AdminRoleGranted,
                ReferenceType: NotificationReferenceTypes.Account,
                EntityName: "صلاحيات الأدمن",
                DeepLink: "/admin");
        }

        private static string Describe(AdminPermission mask) =>
            string.Join(" و", AdminPageCatalog.Explode(mask).Select(AdminPageCatalog.ArabicNameOf));

        private static string Fit(string message) =>
            message.Length <= MaxMessageLength
                ? message
                : message[..(MaxMessageLength - 1)] + "…";
    }

    private static (string Title, string Message, NotificationType Type) Describe(
        NotificationSubject subject, NotificationAction action)
    {
        var name = subject.EntityName;

        return action switch
        {
            NotificationAction.Created =>
                ($"تم إرسال {name}",
                    $"تم إرسال {name} بنجاح، وهو الآن قيد المراجعة من الإدارة قبل النشر.",
                    PublishedType(subject)),

            NotificationAction.Updated =>
                ($"تم تحديث {name}", $"تم تحديث {name} بنجاح.", UpdatedType(subject)),

            NotificationAction.EditedPendingReview =>
                ($"{name} قيد المراجعة بعد التعديل",
                    $"تم حفظ تعديلاتك على {name}، وهو الآن قيد المراجعة من الإدارة ولن يظهر للجمهور حتى تتم الموافقة عليه. مدة النشر لم تتغير.",
                    UpdatedType(subject)),

            NotificationAction.Deleted =>
                ($"تم حذف {name}", $"تم حذف {name}.", DeletedType(subject)),

            NotificationAction.Approved =>
                ($"تمت الموافقة على {name}", $"تمت الموافقة على {name} بواسطة الإدارة.",
                    NotificationType.AdvertisementApproved),

            NotificationAction.Rejected =>
                ($"تم رفض {name}", $"تم رفض {name} بواسطة الإدارة.",
                    NotificationType.AdvertisementRejected),

            NotificationAction.Republished =>
                ($"تمت إعادة نشر {name}", $"تمت إعادة نشر {name} بنجاح.",
                    NotificationType.AdvertisementRepublished),

            NotificationAction.Expired =>
                ($"انتهت مدة {name}", $"انتهت مدة {name}.", NotificationType.AdvertisementExpired),

            NotificationAction.AdminUpdated =>
                ($"تم تعديل {name}", $"قام المشرف بتعديل {name}.", NotificationType.AdminUpdatedListing),

            NotificationAction.Reported =>
                ($"تم الإبلاغ عن {name}",
                    $"تلقينا بلاغًا بشأن {name}، وهو الآن قيد المراجعة.",
                    NotificationType.ListingReported),

            NotificationAction.ReportUnderReview =>
                ($"جاري فحص البلاغ", $"البلاغ الخاص بـ {name} قيد الفحص الآن.",
                    NotificationType.ListingReportReviewed),

            NotificationAction.ReportActionTaken =>
                ($"تم اتخاذ إجراء", $"تم اتخاذ إجراء بشأن {name} بعد مراجعة البلاغ.",
                    NotificationType.ListingReportReviewed),

            NotificationAction.ReportDismissed =>
                ($"تم إغلاق البلاغ", $"تمت مراجعة البلاغ بشأن {name} ولم يتم رصد مخالفة.",
                    NotificationType.ListingReportReviewed),

            NotificationAction.AdminDeleted =>
                ($"تم حذف {name}", $"قام المشرف بحذف {name}.", NotificationType.AdminDeletedListing),

            NotificationAction.Suspended =>
                ($"تم إيقاف {name}", $"تم إيقاف {name} بواسطة الإدارة.", NotificationType.ListingSuspended),

            NotificationAction.Rated =>
                ($"تقييم جديد على {name}", $"حصل {name} على تقييم جديد من أحد المستخدمين.",
                    NotificationType.ListingRated),

            NotificationAction.Liked =>
                ($"إعجاب جديد", $"أعجب أحد المستخدمين بـ{name}.", NotificationType.NewLike),

            NotificationAction.Commented =>
                ($"تعليق جديد", $"قام أحد المستخدمين بالتعليق على {name}.", NotificationType.NewComment),

            _ => ($"تحديث على {name}", $"طرأ تحديث على {name}.", NotificationType.General)
        };
    }

    private static NotificationType PublishedType(NotificationSubject subject) =>
        subject.ReferenceType switch
        {
            NotificationReferenceTypes.Advertisement => NotificationType.AdvertisementPublished,
            NotificationReferenceTypes.LostFoundPost => subject == FoundItem
                ? NotificationType.FoundItemPublished
                : NotificationType.LostItemPublished,
            _ => NotificationType.ListingPublished
        };

    private static NotificationType UpdatedType(NotificationSubject subject) =>
        subject.ReferenceType == NotificationReferenceTypes.Profile
            ? NotificationType.ProfileUpdated
            : NotificationType.ListingUpdated;

    private static NotificationType DeletedType(NotificationSubject subject) =>
        subject.ReferenceType == NotificationReferenceTypes.Advertisement
            ? NotificationType.AdvertisementDeleted
            : NotificationType.ListingDeleted;
}
