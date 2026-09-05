namespace Shared.Constants;

public static class BannerCatalog
{
    public const int MaxTitleLength = 200;

    public const int MaxDescriptionLength = 1000;

    public const int MaxRedirectUrlLength = 2000;

    public const int MinDisplayOrder = 0;

    public const int MaxDisplayOrder = 10_000;

    public static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public const string AllowedImageFormatNames = "JPG, JPEG, PNG, WebP, GIF";
}
