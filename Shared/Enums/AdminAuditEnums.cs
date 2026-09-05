namespace Shared.Enums;

public enum AdminAuditAction
{
    ApproveAd = 1,

    RejectAd = 2,

    SuspendAd = 3,

    DeleteAd = 4,

    ApproveBannerPayment = 10,

    RejectBannerPayment = 11,

    ApproveBanner = 12,

    RejectBanner = 13,

    ExpireBanner = 14,

    DeleteBanner = 15,

    UpdateBannerPrice = 16,

    ApprovePayment = 20,

    RejectPayment = 21,

    RefundPayment = 22,

    CreatePaymentMethod = 23,

    UpdatePaymentMethod = 24,

    DeletePaymentMethod = 25,

    CreateCategory = 30,

    UpdateCategory = 31,

    DeleteCategory = 32,

    SetCategoryStatus = 33,

    CreateSubCategory = 34,

    UpdateSubCategory = 35,

    DeleteSubCategory = 36,

    SetSubCategoryStatus = 37,

    ActivateUser = 40,

    SuspendUser = 41,

    BlockUser = 42,

    UpdateSettings = 50,

    UpdateLogo = 51,

    UpdateHomeSection = 52,

    ReorderHomeSections = 53,

    CreateGovernorate = 60,

    UpdateGovernorate = 61,

    DeleteGovernorate = 62,

    CreateCenter = 63,

    UpdateCenter = 64,

    DeleteCenter = 65,

    CreateProject = 66,

    UpdateProject = 67,

    DeleteProject = 68,

    DismissReport = 70,

    ResolveReport = 71,

    CreateFormField = 80,

    UpdateFormField = 81,

    DeleteFormField = 82,

    CreateAdmin = 90,

    UpdateAdmin = 91,

    ActivateAdmin = 92,

    DeactivateAdmin = 93,

    UpdateAdminPermissions = 94,

    RevokeAdmin = 95
}
