using Shared.Constants;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class EmailTemplateTests
{
    private const string UserName = "عبدالله";

    private const string Otp = "482913";

    private const int ExpiryMinutes = 10;

    private static string PasswordResetHtml(string? logoUrl = null) =>
        EmailCatalog.BuildPasswordResetOtp(UserName, Otp, ExpiryMinutes, logoUrl);

    [Fact]
    public void The_password_reset_email_carries_no_retired_branding()
    {
        var html = PasswordResetHtml();

        Assert.DoesNotContain("MarkatPlace", html);
        Assert.DoesNotContain("ماركت بليس", html);
        Assert.Contains("Shopiklopik", html);
        Assert.DoesNotContain("MarkatPlace", EmailCatalog.PasswordReset.Subject);
        Assert.Contains("Shopiklopik", EmailCatalog.PasswordReset.Subject);
        Assert.Equal("Shopiklopik", new EmailSettings().DisplayName);
    }

    [Fact]
    public void The_verification_code_is_rendered_from_the_argument_and_never_hard_coded()
    {
        Assert.Contains(">482913<", PasswordResetHtml());
        Assert.Contains(">190347<", EmailCatalog.BuildPasswordResetOtp(UserName, "190347", ExpiryMinutes));
        Assert.DoesNotContain("482913", EmailCatalog.BuildPasswordResetOtp(UserName, "190347", ExpiryMinutes));
    }

    [Fact]
    public void The_expiry_shown_is_the_expiry_the_backend_passed_in()
    {
        Assert.Contains("صالح لمدة 10 دقيقة بس.", PasswordResetHtml());
        Assert.Contains("صالح لمدة 5 دقيقة بس.", EmailCatalog.BuildPasswordResetOtp(UserName, Otp, 5));
    }

    [Fact]
    public void The_call_to_action_and_footer_point_at_the_public_brand_addresses()
    {
        var html = PasswordResetHtml();

        Assert.Equal("https://shopiklopik.com", EmailCatalog.Brand.WebsiteUrl);
        Assert.Contains("href=\"https://shopiklopik.com\"", html);
        Assert.Contains("href=\"mailto:shopiklopiksupport@gmail.com\"", html);
        Assert.Contains(EmailCatalog.Brand.Tagline, html);
    }

    [Fact]
    public void The_markup_is_right_to_left_and_declares_its_encoding()
    {
        var html = PasswordResetHtml();

        Assert.Contains("<html dir=\"rtl\" lang=\"ar\"", html);
        Assert.Contains("<meta charset=\"utf-8\"", html);
        Assert.Contains("dir=\"ltr\"", html);
    }

    [Fact]
    public void The_markup_stays_within_what_mail_clients_render()
    {
        var html = PasswordResetHtml();

        Assert.DoesNotContain("<script", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("javascript:", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<link", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("position:absolute", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("role=\"presentation\"", html);
        Assert.Contains("@media only screen and (max-width:620px)", html);
        Assert.Contains("<meta name=\"viewport\"", html);
        Assert.True(System.Text.Encoding.UTF8.GetByteCount(html) < 102_400);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("wwwroot/uploads/logo.png")]
    [InlineData("/images/logo.png")]
    [InlineData("file:///C:/logo.png")]
    public void An_unusable_logo_setting_falls_back_to_the_drawn_monogram_instead_of_a_broken_image(string? logoUrl)
    {
        var html = PasswordResetHtml(logoUrl);

        Assert.DoesNotContain("<img", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(EmailCatalog.Brand.Monogram, html);
    }

    [Fact]
    public void A_public_logo_url_is_used_as_the_header_image()
    {
        var html = PasswordResetHtml("https://shopiklopik.com/assets/logo.png");

        Assert.Contains("<img src=\"https://shopiklopik.com/assets/logo.png\"", html);
        Assert.Contains("alt=\"Shopiklopik\"", html);
    }

    [Fact]
    public void The_user_name_is_encoded_so_it_cannot_break_out_of_the_markup()
    {
        var html = EmailCatalog.BuildPasswordResetOtp("<b>خالد</b>", Otp, ExpiryMinutes);

        Assert.DoesNotContain("<b>خالد</b>", html);
        Assert.Contains("&lt;b&gt;خالد&lt;/b&gt;", html);
    }

    [Fact]
    public void A_plain_text_alternative_is_offered_alongside_the_html()
    {
        var text = EmailCatalog.BuildPasswordResetOtpText(UserName, Otp, ExpiryMinutes);

        Assert.Contains(Otp, text);
        Assert.Contains("https://shopiklopik.com", text);
        Assert.Contains("Shopiklopik", text);
        Assert.DoesNotContain("<", text);
    }

    [Fact]
    public void No_credential_is_written_into_the_template_source()
    {
        var source = RepositoryRoot.ReadFile("Shared/Constants/EmailCatalog.cs");

        Assert.DoesNotContain("Password =", source);
        Assert.DoesNotContain("smtp.", source, StringComparison.OrdinalIgnoreCase);
    }
}
