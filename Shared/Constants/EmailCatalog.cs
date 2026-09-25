namespace Shared.Constants;

public static class EmailCatalog
{
    public static class Brand
    {
        public const string Name = "Shopiklopik";

        public const string Tagline = "كل احتياجاتك في مكان واحد";

        public const string WebsiteUrl = "https://shopiklopik.com";

        public const string WebsiteLabel = "shopiklopik.com";

        public const string SupportEmail = "shopiklopiksupport@gmail.com";

        public const string Monogram = "S";
    }

    public static class Palette
    {
        public const string Navy = "#0C1E3C";

        public const string NavySoft = "#16325C";

        public const string Gold = "#F7B928";

        public const string GoldTint = "#FFFBF0";

        public const string AmberTint = "#FFF6E0";

        public const string AmberText = "#8A5B00";

        public const string PageBackground = "#EEF2F8";

        public const string CardBackground = "#FFFFFF";

        public const string PanelBackground = "#F5F8FD";

        public const string PanelBorder = "#DCE6F5";

        public const string BodyText = "#25314C";

        public const string MutedText = "#5B6B85";

        public const string FooterBackground = "#F5F8FD";
    }

    public const string FontStack = "Tahoma, 'Segoe UI', Arial, Helvetica, sans-serif";

    public const string CodeFontStack = "'Courier New', Consolas, Menlo, monospace";

    public static class PasswordReset
    {
        public const string Subject = "كود إعادة تعيين كلمة السر - Shopiklopik";

        public const string Preheader = "كود إعادة تعيين كلمة السر بتاعك جوه الرسالة.";

        public const string Heading = "إعادة تعيين كلمة السر";

        public const string Intro = "وصلنا طلب لإعادة تعيين كلمة السر بتاعت حسابك على";

        public const string Instruction = "استخدم الكود ده عشان تكمّل الخطوة:";

        public const string CodeLabel = "كود التحقق";

        public const string SecurityNote = "لو إنت مش اللي طلبت الرسالة دي، تجاهلها وماتشاركش الكود مع أي حد.";

        public const string CallToAction = "زيارة Shopiklopik";
    }

    public static string BuildPasswordResetOtp(string userName, string otp, int expiryMinutes, string? logoUrl = null)
    {
        var safeUserName = System.Net.WebUtility.HtmlEncode(userName);
        var safeOtp = System.Net.WebUtility.HtmlEncode(otp);

        var body = $$"""
            <h1 class="sk-title" style="margin:0 0 14px;font-family:{{FontStack}};font-size:22px;line-height:1.45;font-weight:bold;color:{{Palette.Navy}};">{{PasswordReset.Heading}}</h1>
            <p style="margin:0 0 10px;font-family:{{FontStack}};font-size:16px;line-height:1.85;color:{{Palette.BodyText}};">أهلاً {{safeUserName}} 👋</p>
            <p style="margin:0 0 6px;font-family:{{FontStack}};font-size:15px;line-height:1.9;color:{{Palette.BodyText}};">{{PasswordReset.Intro}} <span dir="ltr" style="font-weight:bold;color:{{Palette.Navy}};">{{Brand.Name}}</span>.</p>
            <p style="margin:0 0 22px;font-family:{{FontStack}};font-size:15px;line-height:1.9;color:{{Palette.MutedText}};">{{PasswordReset.Instruction}}</p>
            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0">
              <tr>
                <td align="center" bgcolor="{{Palette.PanelBackground}}" style="background-color:{{Palette.PanelBackground}};border:1px solid {{Palette.PanelBorder}};border-radius:16px;padding:24px 16px;">
                  <div style="font-family:{{FontStack}};font-size:13px;font-weight:bold;letter-spacing:0.4px;color:{{Palette.MutedText}};">{{PasswordReset.CodeLabel}}</div>
                  <div class="sk-code" dir="ltr" style="margin-top:10px;font-family:{{CodeFontStack}};font-size:36px;line-height:1.2;font-weight:bold;color:{{Palette.Navy}};letter-spacing:10px;text-indent:10px;">{{safeOtp}}</div>
                </td>
              </tr>
              <tr>
                <td align="center" style="padding:18px 0 20px;">
                  <table role="presentation" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr>
                      <td bgcolor="{{Palette.AmberTint}}" style="background-color:{{Palette.AmberTint}};border-radius:999px;padding:10px 20px;font-family:{{FontStack}};font-size:14px;font-weight:bold;color:{{Palette.AmberText}};">الكود ده صالح لمدة {{expiryMinutes}} دقيقة بس.</td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td bgcolor="{{Palette.GoldTint}}" style="background-color:{{Palette.GoldTint}};border-right:4px solid {{Palette.Gold}};border-radius:12px;padding:16px 18px;font-family:{{FontStack}};font-size:14px;line-height:1.85;color:{{Palette.MutedText}};">{{PasswordReset.SecurityNote}}</td>
              </tr>
              <tr>
                <td align="center" style="padding:28px 0 4px;">
                  <table role="presentation" cellpadding="0" cellspacing="0" border="0" align="center" class="sk-btn">
                    <tr>
                      <td align="center" bgcolor="{{Palette.Navy}}" style="background-color:{{Palette.Navy}};border-radius:12px;">
                        <a href="{{Brand.WebsiteUrl}}" target="_blank" style="display:inline-block;padding:15px 40px;font-family:{{FontStack}};font-size:16px;font-weight:bold;color:#FFFFFF;text-decoration:none;border-radius:12px;">{{PasswordReset.CallToAction}}</a>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
            """;

        return BuildLayout(PasswordReset.Subject, PasswordReset.Preheader, body, logoUrl);
    }

    public static string BuildPasswordResetOtpText(string userName, string otp, int expiryMinutes) =>
        $"""
        أهلاً {userName}

        {PasswordReset.Intro} {Brand.Name}.

        {PasswordReset.CodeLabel}: {otp}

        الكود ده صالح لمدة {expiryMinutes} دقيقة بس.

        {PasswordReset.SecurityNote}

        {Brand.Name} - {Brand.Tagline}
        {Brand.WebsiteUrl}
        {Brand.SupportEmail}
        """;

    public static string BuildLayout(string title, string preheader, string bodyHtml, string? logoUrl = null)
    {
        var safeTitle = System.Net.WebUtility.HtmlEncode(title);
        var safePreheader = System.Net.WebUtility.HtmlEncode(preheader);

        return $$"""
            <!DOCTYPE html>
            <html dir="rtl" lang="ar" xmlns="http://www.w3.org/1999/xhtml">
            <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
            <meta http-equiv="X-UA-Compatible" content="IE=edge" />
            <meta name="x-apple-disable-message-reformatting" />
            <meta name="color-scheme" content="light only" />
            <meta name="supported-color-schemes" content="light only" />
            <title>{{safeTitle}}</title>
            <style type="text/css">
            body { margin:0 !important; padding:0 !important; width:100% !important; background-color:{{Palette.PageBackground}}; }
            table { border-collapse:collapse; }
            img { border:0; outline:none; text-decoration:none; -ms-interpolation-mode:bicubic; }
            a { text-decoration:none; }
            @media only screen and (max-width:620px) {
              .sk-pad { padding-left:22px !important; padding-right:22px !important; }
              .sk-title { font-size:20px !important; }
              .sk-code { font-size:30px !important; letter-spacing:8px !important; text-indent:8px !important; }
              .sk-btn a { display:block !important; padding-left:22px !important; padding-right:22px !important; }
            }
            </style>
            </head>
            <body style="margin:0;padding:0;background-color:{{Palette.PageBackground}};">
            <div style="display:none;font-size:1px;color:{{Palette.PageBackground}};line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden;">{{safePreheader}}</div>
            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" bgcolor="{{Palette.PageBackground}}" style="background-color:{{Palette.PageBackground}};">
              <tr>
                <td align="center" style="padding:28px 12px;">
                  <!--[if mso]><table role="presentation" width="600" cellpadding="0" cellspacing="0" border="0"><tr><td><![endif]-->
                  <div style="max-width:600px;margin:0 auto;">
                  <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="width:100%;max-width:600px;background-color:{{Palette.CardBackground}};border-radius:18px;">
                    <tr>
                      <td align="center" bgcolor="{{Palette.Navy}}" style="background-color:{{Palette.Navy}};padding:32px 24px 26px;border-radius:18px 18px 0 0;">
                        {{BuildLogo(logoUrl)}}
                        <div dir="ltr" style="margin-top:14px;font-family:{{FontStack}};font-size:24px;font-weight:bold;letter-spacing:0.5px;color:#FFFFFF;">{{Brand.Name}}</div>
                        <div style="margin-top:6px;font-family:{{FontStack}};font-size:13px;color:{{Palette.Gold}};">{{Brand.Tagline}}</div>
                      </td>
                    </tr>
                    <tr>
                      <td height="4" bgcolor="{{Palette.Gold}}" style="height:4px;line-height:4px;font-size:0;background-color:{{Palette.Gold}};">&nbsp;</td>
                    </tr>
                    <tr>
                      <td class="sk-pad" align="right" style="padding:34px 40px 36px;text-align:right;background-color:{{Palette.CardBackground}};">
                        {{bodyHtml}}
                      </td>
                    </tr>
                    <tr>
                      <td class="sk-pad" align="center" bgcolor="{{Palette.FooterBackground}}" style="background-color:{{Palette.FooterBackground}};border-top:1px solid {{Palette.PanelBorder}};padding:26px 40px 28px;text-align:center;border-radius:0 0 18px 18px;">
                        <div dir="ltr" style="font-family:{{FontStack}};font-size:16px;font-weight:bold;color:{{Palette.Navy}};">{{Brand.Name}}</div>
                        <div style="margin-top:6px;font-family:{{FontStack}};font-size:13px;color:{{Palette.MutedText}};">{{Brand.Tagline}}</div>
                        <div style="margin-top:14px;font-family:{{FontStack}};font-size:13px;line-height:1.9;color:{{Palette.MutedText}};">
                          <a href="{{Brand.WebsiteUrl}}" target="_blank" style="color:{{Palette.NavySoft}};font-weight:bold;text-decoration:none;">{{Brand.WebsiteLabel}}</a>
                          <span style="color:{{Palette.PanelBorder}};padding:0 8px;">|</span>
                          <a href="mailto:{{Brand.SupportEmail}}" style="color:{{Palette.NavySoft}};font-weight:bold;text-decoration:none;">{{Brand.SupportEmail}}</a>
                        </div>
                      </td>
                    </tr>
                  </table>
                  <div style="margin:16px auto 0;font-family:{{FontStack}};font-size:12px;line-height:1.8;color:#8B98AD;text-align:center;">رسالة تلقائية من {{Brand.Name}}، مش محتاجة رد.</div>
                  </div>
                  <!--[if mso]></td></tr></table><![endif]-->
                </td>
              </tr>
            </table>
            </body>
            </html>
            """;
    }

    private static string BuildLogo(string? logoUrl)
    {
        if (!string.IsNullOrWhiteSpace(logoUrl)
            && Uri.TryCreate(logoUrl, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            var safeLogoUrl = System.Net.WebUtility.HtmlEncode(uri.AbsoluteUri);

            return $"""<img src="{safeLogoUrl}" width="64" height="64" alt="{Brand.Name}" style="display:block;margin:0 auto;width:64px;height:64px;border-radius:18px;" />""";
        }

        return $$"""
            <table role="presentation" cellpadding="0" cellspacing="0" border="0" align="center" style="margin:0 auto;">
                          <tr>
                            <td align="center" valign="middle" width="64" height="64" bgcolor="{{Palette.Gold}}" style="width:64px;height:64px;background-color:{{Palette.Gold}};border-radius:18px;font-family:{{FontStack}};font-size:30px;line-height:64px;font-weight:bold;color:{{Palette.Navy}};text-align:center;">{{Brand.Monogram}}</td>
                          </tr>
                        </table>
            """;
    }
}
