namespace Shared.Constants;

public static class UserMessages
{
    public static class Listings
    {
        public const string Created = "تم إرسال إعلانك بنجاح، وهيظهر للناس بعد ما الإدارة تراجعه.";

        public const string Updated = "تم حفظ التعديلات، والإعلان هيتراجع تاني قبل ما يظهر.";

        public const string Deleted = "تم حذف الإعلان.";

        public const string Loaded = "تم تحميل الإعلانات.";

        public const string DetailsLoaded = "تم تحميل تفاصيل الإعلان.";

        public const string MineLoaded = "تم تحميل إعلاناتك.";

        public const string SimilarLoaded = "تم تحميل إعلانات مشابهة.";

        public const string RelatedLoaded = "تم تحميل إعلانات ليها علاقة.";

        public const string RecentlyAddedLoaded = "تم تحميل أحدث الإعلانات.";

        public const string PriceStatisticsLoaded = "تم تحميل إحصائيات الأسعار.";

        public const string SearchSuggestionsLoaded = "تم تحميل اقتراحات البحث.";

        public const string ImagesUploaded = "تم رفع الصور.";

        public const string ImageDeleted = "تم حذف الصورة.";

        public const string ImagesReordered = "تم ترتيب صور الإعلان.";

        public const string PromotionUpdated = "تم تحديث تمييز الإعلان.";

        public const string Republished = "تم إعادة نشر الإعلان، وهيظهر بعد مراجعة الإدارة.";

        public const string StatisticsLoaded = "تم تحميل الإحصائيات.";
    }

    public static class Lookups
    {
        public const string Loaded = "تم تحميل قائمة الاختيارات.";

        public const string FormLoaded = "تم تحميل نموذج إضافة الإعلان.";

        public const string ReadConfigLoaded = "تم تحميل إعدادات العرض.";
    }

    public static class Auth
    {
        public const string Registered = "تم إنشاء حسابك بنجاح. أهلاً بيك في ماركت بليس.";

        public const string LoggedIn = "تم تسجيل الدخول بنجاح.";

        public const string LoggedOut = "تم تسجيل الخروج.";

        public const string TokenRefreshed = "تم تجديد الجلسة.";

        public const string PasswordResetCodeSent =
            "لو البريد ده مسجّل عندنا، هيوصله كود إعادة تعيين كلمة السر. راجع بريدك (وبص في الـ Spam كمان).";

        public const string PasswordResetCodeValid = "الكود صح. تقدر تحط كلمة السر الجديدة دلوقتي.";

        public const string PasswordReset = "تم تغيير كلمة السر. تقدر تسجل دخولك بيها دلوقتي.";

        public const string PasswordChanged = "تم تغيير كلمة السر.";

        public const string AccountLocked =
            "قفلنا الحساب مؤقتاً بعد محاولات دخول كتير غلط. استنى شوية وجرّب تاني.";

        public const string ResetSessionExpired =
            "لازم تتأكد من الكود الأول، أو إن الوقت خلص. ابدأ خطوات نسيت كلمة السر من الأول.";

        public const string GoogleSignedIn = "تم تسجيل الدخول بحساب جوجل بنجاح.";

        public const string GoogleRegistered =
            "تم إنشاء حسابك بحساب جوجل. أهلاً بيك في ماركت بليس.";

        public const string GoogleConfigLoaded = "تم تحميل إعدادات الدخول بحساب جوجل.";

        public const string GoogleCredentialInvalid =
            "بيانات الدخول بحساب جوجل مش صحيحة أو انتهت صلاحيتها. جرب تاني.";

        public const string GoogleCredentialRequired =
            "لازم تبعت بيانات الدخول اللي جاية من جوجل.";

        public const string GoogleEmailNotVerified =
            "لازم تفعّل البريد الإلكتروني بتاع حساب جوجل الأول عشان تقدر تسجل بيه.";

        public const string GoogleNotAvailable = "الدخول بحساب جوجل مش متاح دلوقتي.";

        public const string GoogleEmailAlreadyRegistered =
            "البريد ده مسجل عندنا بحساب بكلمة سر. سجل دخولك بكلمة السر الأول.";

        public const string GoogleSignInRetry =
            "مش قادرين نكمل الدخول بحساب جوجل دلوقتي. جرب تاني.";
    }

    public static class Profile
    {
        public const string Loaded = "تم تحميل بياناتك.";

        public const string Updated = "تم حفظ بياناتك.";

        public const string PictureUpdated = "تم تحديث صورتك الشخصية.";
    }

    public static class Notifications
    {
        public const string Loaded = "تم تحميل الإشعارات.";

        public const string UnreadCountLoaded = "تم تحميل عدد الإشعارات الجديدة.";

        public const string MarkedRead = "تم تعليم الإشعار كمقروء.";

        public const string AllMarkedRead = "تم تعليم كل الإشعارات كمقروءة.";

        public const string Deleted = "تم حذف الإشعار.";

        public const string AllDeleted = "تم حذف كل الإشعارات.";

        public const string PreferencesLoaded = "تم تحميل إعدادات الإشعارات.";

        public const string PreferencesUpdated = "تم حفظ إعدادات الإشعارات.";

        public const string InterestsLoaded = "تم تحميل اهتماماتك.";

        public const string InterestOptionsLoaded = "تم تحميل الأقسام اللي تقدر تتابعها.";

        public const string InterestSaved = "تم إضافة الاهتمام.";

        public const string InterestUpdated = "تم تحديث الاهتمام.";

        public const string InterestsUpdated = "تم حفظ اهتماماتك.";

        public const string InterestRemoved = "تم حذف الاهتمام.";
    }

    public static class Comments
    {
        public const string Loaded = "تم تحميل التعليقات.";

        public const string Added = "تم إضافة تعليقك.";

        public const string Updated = "تم تعديل تعليقك.";

        public const string Deleted = "تم حذف التعليق.";
    }

    public static class Posts
    {
        public const string Created = "تم نشر المنشور، وهيظهر بعد مراجعة الإدارة.";

        public const string Updated = "تم حفظ تعديلات المنشور، وهيتراجع تاني قبل ما يظهر.";

        public const string Deleted = "تم حذف المنشور.";

        public const string Loaded = "تم تحميل المنشورات.";

        public const string DetailsLoaded = "تم تحميل تفاصيل المنشور.";

        public const string MarkedReturned = "تم تعليم المنشور إنه اتسلّم لصاحبه. ألف مبروك.";
    }

    public static class Payments
    {
        public const string Loaded = "تم تحميل المدفوعات.";

        public const string DetailsLoaded = "تم تحميل تفاصيل الدفع.";

        public const string MethodsLoaded = "تم تحميل طرق الدفع المتاحة.";

        public const string Submitted = "استلمنا بيانات الدفع، والإدارة هتراجعها وهتوصلك النتيجة.";
    }

    public static class Generic
    {
        public const string Loaded = "تم تحميل البيانات.";

        public const string Created = "تمت الإضافة.";

        public const string Updated = "تم حفظ التعديلات.";

        public const string Deleted = "تم الحذف.";

        public const string Saved = "تم الحفظ.";
    }

    public static class Errors
    {
        public const string Unexpected = "حصلت مشكلة مؤقتة عندنا. جرّب تاني بعد شوية.";

        public const string SignInRequired = "لازم تسجل دخولك الأول عشان تعمل العملية دي.";

        public const string NotAllowed = "مش مسموح ليك تعمل العملية دي.";

        public const string NotFound = "الحاجة اللي بتدور عليها مش موجودة أو مش متاحة دلوقتي.";

        public const string InvalidData = "فيه بيانات ناقصة أو مش صحيحة. راجع الحقول وجرّب تاني.";

        public const string MissingBody = "البيانات المرسلة ناقصة أو مش صحيحة.";

        public const string TooManyRequests =
            "بعتّ طلبات كتير في وقت قصير. استنى شوية وجرّب تاني.";
    }
}
