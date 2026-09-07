using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Validation;
using Shared.Constants;
using Shared.DTOs.Auth;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Xunit;

namespace MarkatPlace.Tests;

public class GoogleSignInTests
{
    private const string Credential = "google-credential";

    private static GoogleSignInRequest Request(string? referralCode = null, string? center = null) =>
        new() { IdToken = Credential, ReferralCode = referralCode, Center = center };

    [Fact]
    public async Task A_valid_credential_for_an_unknown_account_creates_the_user_and_signs_them_in()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(
            Credential, "google-subject-1", "sara@example.com",
            givenName: "سارة", familyName: "محمد");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        Assert.True(result.AccountCreated);
        Assert.False(string.IsNullOrWhiteSpace(result.Auth.Token));
        Assert.False(string.IsNullOrWhiteSpace(result.Auth.RefreshToken));
        Assert.Equal("sara@example.com", result.Auth.User.Email);
        Assert.Equal("سارة", result.Auth.User.FirstName);
        Assert.Equal("محمد", result.Auth.User.SecondName);
        Assert.Equal(LocationConstants.Governorate, result.Auth.User.Governorate);
        Assert.Contains(result.Auth.User.Center, LocationConstants.Centers);
    }

    [Fact]
    public async Task A_created_account_is_linked_by_the_Google_subject_not_by_its_e_mail()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        await harness.Auth.GoogleSignInAsync(Request());

        var user = await harness.Users.FindByLoginAsync(GoogleAuthCatalog.ProviderName, "google-subject-1");

        Assert.NotNull(user);
        Assert.Equal("sara@example.com", user!.Email);
    }

    [Fact]
    public async Task A_created_account_carries_a_referral_code_of_its_own()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        var user = await harness.Users.FindByIdAsync(result.Auth.User.Id);

        Assert.False(string.IsNullOrWhiteSpace(user!.ReferralCode));
    }

    [Fact]
    public async Task A_created_account_has_no_password_so_the_password_login_cannot_be_guessed_into()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());
        var user = await harness.Users.FindByIdAsync(result.Auth.User.Id);

        Assert.False(await harness.Users.HasPasswordAsync(user!));
    }

    [Fact]
    public async Task Signing_in_again_with_the_same_Google_account_reuses_the_existing_user()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var first = await harness.Auth.GoogleSignInAsync(Request());
        var second = await harness.Auth.GoogleSignInAsync(Request());

        Assert.True(first.AccountCreated);
        Assert.False(second.AccountCreated);
        Assert.Equal(first.Auth.User.Id, second.Auth.User.Id);
        Assert.Equal(1, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task An_unknown_credential_is_refused()
    {
        await using var harness = GoogleSignInHarness.Create();

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(
            () => harness.Auth.GoogleSignInAsync(new GoogleSignInRequest { IdToken = "forged" }));

        Assert.Equal(UserMessages.Auth.GoogleCredentialInvalid, exception.Message);
        Assert.Equal(0, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task An_expired_credential_is_refused_and_creates_nothing()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");
        harness.Google.ExpiredCredentials.Add(Credential);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Equal(UserMessages.Auth.GoogleCredentialInvalid, exception.Message);
        Assert.Equal(0, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task A_credential_whose_e_mail_Google_has_not_verified_cannot_create_an_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(
            Credential, "google-subject-1", "sara@example.com", emailVerified: false);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Equal(UserMessages.Auth.GoogleEmailNotVerified, exception.Message);
        Assert.Equal(0, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task An_unverified_e_mail_can_never_take_over_an_existing_password_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        var victim = await harness.AddPasswordUserAsync("victim", "victim@example.com");

        harness.Google.Register(
            Credential, "attacker-subject", "victim@example.com", emailVerified: false);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        var logins = await harness.Users.GetLoginsAsync(victim);

        Assert.Empty(logins);
    }

    [Fact]
    public async Task A_verified_Google_e_mail_links_to_the_existing_account_instead_of_duplicating_it()
    {
        await using var harness = GoogleSignInHarness.Create();

        var existing = await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com");

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        Assert.False(result.AccountCreated);
        Assert.Equal(existing.Id, result.Auth.User.Id);
        Assert.Equal(1, await harness.Context.Users.CountAsync());

        var logins = await harness.Users.GetLoginsAsync(existing);

        Assert.Contains(logins, login =>
            login.LoginProvider == GoogleAuthCatalog.ProviderName &&
            login.ProviderKey == "google-subject-1");
    }

    [Fact]
    public async Task Linking_can_be_refused_by_configuration_when_an_account_already_owns_the_e_mail()
    {
        await using var harness = GoogleSignInHarness.Create(new GoogleAuthSettings
        {
            ClientId = "test-client-id.apps.googleusercontent.com",
            LinkVerifiedEmailToExistingAccount = false
        });

        await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com");

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Equal(UserMessages.Auth.GoogleEmailAlreadyRegistered, exception.Message);
        Assert.Equal(1, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task The_password_login_still_works_for_an_account_a_Google_login_was_linked_to()
    {
        await using var harness = GoogleSignInHarness.Create();

        await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com", "Passw0rd!");

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        await harness.Auth.GoogleSignInAsync(Request());

        var afterLinking = await harness.Auth.LoginAsync(
            new LoginRequest { Username = "ahmed", Password = "Passw0rd!" });

        Assert.False(string.IsNullOrWhiteSpace(afterLinking.Token));
    }

    [Theory]
    [InlineData(UserAccountStatus.Blocked)]
    [InlineData(UserAccountStatus.Suspended)]
    public async Task A_suspended_or_blocked_account_cannot_sign_in_with_Google(UserAccountStatus status)
    {
        await using var harness = GoogleSignInHarness.Create();

        await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com", status: status);

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        await Assert.ThrowsAsync<ForbiddenException>(() => harness.Auth.GoogleSignInAsync(Request()));
    }

    [Fact]
    public async Task A_blocked_account_is_refused_before_the_Google_login_is_linked_to_it()
    {
        await using var harness = GoogleSignInHarness.Create();

        var blocked = await harness.AddPasswordUserAsync(
            "ahmed", "ahmed@example.com", status: UserAccountStatus.Blocked);

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        await Assert.ThrowsAsync<ForbiddenException>(() => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Empty(await harness.Users.GetLoginsAsync(blocked));
    }

    [Fact]
    public async Task A_locked_out_account_is_refused_with_the_same_message_the_password_login_uses()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com");

        await harness.Users.SetLockoutEnabledAsync(user, true);
        await harness.Users.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15));

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Equal(UserMessages.Auth.AccountLocked, exception.Message);
    }

    [Fact]
    public async Task The_roles_a_user_already_holds_are_carried_into_the_issued_token()
    {
        await using var harness = GoogleSignInHarness.Create();

        var admin = await harness.AddPasswordUserAsync("admin", "admin@example.com");

        harness.Context.Roles.Add(new IdentityRole(AppRoles.Admin)
        {
            NormalizedName = AppRoles.Admin.ToUpperInvariant()
        });

        await harness.Context.SaveChangesAsync();
        await harness.Users.AddToRoleAsync(admin, AppRoles.Admin);

        harness.Google.Register(Credential, "google-subject-1", "admin@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        Assert.False(result.AccountCreated);
        Assert.Contains(AppRoles.Admin, await harness.Users.GetRolesAsync(admin));
        Assert.Contains(harness.Tokens.Issued,
            issued => issued.UserId == admin.Id && issued.Roles.Contains(AppRoles.Admin));
    }

    [Fact]
    public async Task A_referral_code_supplied_with_a_new_Google_account_credits_the_inviter_once()
    {
        await using var harness = GoogleSignInHarness.Create();

        var inviter = await harness.AddPasswordUserAsync(
            "inviter", "inviter@example.com", referralCode: "ABCD2345");

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));

        var referral = await harness.Context.Set<Referral>().SingleAsync();

        Assert.True(result.AccountCreated);
        Assert.Equal(inviter.Id, referral.ReferrerUserId);
        Assert.Equal(result.Auth.User.Id, referral.ReferredUserId);
        Assert.Equal("ABCD2345", referral.ReferralCode);
        Assert.Equal(ReferralStatus.Completed, referral.Status);
    }

    [Fact]
    public async Task A_lower_case_referral_code_from_the_invitation_link_still_credits_the_inviter()
    {
        await using var harness = GoogleSignInHarness.Create();

        var inviter = await harness.AddPasswordUserAsync(
            "inviter", "inviter@example.com", referralCode: "ABCD2345");

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        await harness.Auth.GoogleSignInAsync(Request(referralCode: "abcd2345"));

        Assert.Equal(1, await harness.CountReferralsAsync(inviter.Id));
    }

    [Fact]
    public async Task Repeating_the_Google_login_never_credits_the_inviter_a_second_time()
    {
        await using var harness = GoogleSignInHarness.Create();

        var inviter = await harness.AddPasswordUserAsync(
            "inviter", "inviter@example.com", referralCode: "ABCD2345");

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));
        await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));
        await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));

        Assert.Equal(1, await harness.CountReferralsAsync(inviter.Id));
        Assert.Equal(2, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task Signing_into_an_account_that_already_exists_never_records_a_referral()
    {
        await using var harness = GoogleSignInHarness.Create();

        var inviter = await harness.AddPasswordUserAsync(
            "inviter", "inviter@example.com", referralCode: "ABCD2345");

        await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com");

        harness.Google.Register(Credential, "google-subject-1", "ahmed@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));

        Assert.False(result.AccountCreated);
        Assert.Equal(0, await harness.CountReferralsAsync(inviter.Id));
        Assert.Empty(await harness.Context.Set<Referral>().ToListAsync());
    }

    [Fact]
    public async Task An_unknown_referral_code_still_creates_the_account_but_records_no_referral()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request(referralCode: "ZZZZ9999"));

        Assert.True(result.AccountCreated);
        Assert.Empty(await harness.Context.Set<Referral>().ToListAsync());
    }

    [Fact]
    public async Task A_referral_code_belonging_to_a_blocked_inviter_is_ignored()
    {
        await using var harness = GoogleSignInHarness.Create();

        var inviter = await harness.AddPasswordUserAsync(
            "inviter", "inviter@example.com",
            status: UserAccountStatus.Blocked, referralCode: "ABCD2345");

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request(referralCode: "ABCD2345"));

        Assert.True(result.AccountCreated);
        Assert.Equal(0, await harness.CountReferralsAsync(inviter.Id));
    }

    [Fact]
    public async Task A_new_Google_account_is_told_it_was_created_and_an_existing_one_is_not()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var created = await harness.Auth.GoogleSignInAsync(Request());

        Assert.Contains(harness.Notifications.Created,
            entry => entry.UserId == created.Auth.User.Id &&
                     entry.Type == NotificationType.AccountCreated);

        harness.Notifications.Created.Clear();

        await harness.Auth.GoogleSignInAsync(Request());

        Assert.Contains(harness.Notifications.Created,
            entry => entry.Type == NotificationType.NewLogin);
        Assert.DoesNotContain(harness.Notifications.Created,
            entry => entry.Type == NotificationType.AccountCreated);
    }

    [Fact]
    public async Task Two_Google_accounts_sharing_a_display_name_get_two_distinct_usernames()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register("first", "subject-1", "ahmed@example.com", name: "أحمد على");
        harness.Google.Register("second", "subject-2", "ahmed@other-example.com", name: "أحمد على");

        var first = await harness.Auth.GoogleSignInAsync(new GoogleSignInRequest { IdToken = "first" });
        var second = await harness.Auth.GoogleSignInAsync(new GoogleSignInRequest { IdToken = "second" });

        Assert.NotEqual(first.Auth.User.Username, second.Auth.User.Username);
        Assert.Equal(2, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task A_username_taken_by_a_password_account_is_not_stolen_by_a_Google_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        await harness.AddPasswordUserAsync("ahmed", "someone-else@example.com");

        harness.Google.Register(Credential, "subject-1", "ahmed@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        Assert.NotEqual("ahmed", result.Auth.User.Username);
    }

    [Fact]
    public async Task A_Google_display_name_that_carries_no_usable_characters_still_yields_a_valid_username()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "subject-1", "!!@example.com", name: "***");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        Assert.True(AccountNameRules.IsValidUsername(result.Auth.User.Username));
        Assert.True(result.Auth.User.Username.Length >= GoogleAuthCatalog.MinUsernameLength);
    }

    [Fact]
    public async Task A_supplied_center_is_kept_and_an_invalid_one_falls_back_to_the_default()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register("first", "subject-1", "one@example.com");
        harness.Google.Register("second", "subject-2", "two@example.com");

        var chosen = await harness.Auth.GoogleSignInAsync(
            new GoogleSignInRequest { IdToken = "first", Center = "سنورس" });

        var fallback = await harness.Auth.GoogleSignInAsync(
            new GoogleSignInRequest { IdToken = "second", Center = "القاهرة" });

        Assert.Equal("سنورس", chosen.Auth.User.Center);
        Assert.Equal(GoogleAuthCatalog.DefaultCenter, fallback.Auth.User.Center);
    }

    [Fact]
    public async Task An_unconfigured_client_id_disables_the_flow_instead_of_signing_anyone_in()
    {
        await using var harness = GoogleSignInHarness.Create(new GoogleAuthSettings());

        harness.Google.Register(Credential, "subject-1", "sara@example.com");

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => harness.Auth.GoogleSignInAsync(Request()));

        Assert.Equal(UserMessages.Auth.GoogleNotAvailable, exception.Message);
        Assert.Equal(0, await harness.Context.Users.CountAsync());
    }

    [Fact]
    public async Task The_published_configuration_carries_the_client_id_and_never_the_secret()
    {
        await using var harness = GoogleSignInHarness.Create(new GoogleAuthSettings
        {
            ClientId = "public-client-id.apps.googleusercontent.com",
            ClientSecret = "super-secret-value"
        });

        var config = harness.Auth.GetGoogleConfig();

        var serialized = System.Text.Json.JsonSerializer.Serialize(config);

        Assert.True(config.Enabled);
        Assert.Equal("public-client-id.apps.googleusercontent.com", config.ClientId);
        Assert.DoesNotContain("super-secret-value", serialized, StringComparison.Ordinal);
        Assert.DoesNotContain("secret", serialized, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task The_authentication_response_never_carries_any_Google_credential()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var result = await harness.Auth.GoogleSignInAsync(Request());

        var serialized = System.Text.Json.JsonSerializer.Serialize(result.Auth);

        Assert.DoesNotContain(Credential, serialized, StringComparison.Ordinal);
        Assert.DoesNotContain("google-subject-1", serialized, StringComparison.Ordinal);
    }

    [Fact]
    public async Task The_response_is_the_same_shape_the_password_login_returns()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var google = await harness.Auth.GoogleSignInAsync(Request());

        await harness.AddPasswordUserAsync("ahmed", "ahmed@example.com", "Passw0rd!");

        var password = await harness.Auth.LoginAsync(
            new LoginRequest { Username = "ahmed", Password = "Passw0rd!" });

        Assert.IsType<AuthResponse>(google.Auth);
        Assert.IsType<AuthResponse>(password);
        Assert.True(google.Auth.Expiration > DateTime.UtcNow);
        Assert.True(google.Auth.RefreshTokenExpiration > google.Auth.Expiration);
    }

    [Fact]
    public async Task A_signed_in_Google_user_can_be_refreshed_with_the_ordinary_refresh_token_flow()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register(Credential, "google-subject-1", "sara@example.com");

        var signIn = await harness.Auth.GoogleSignInAsync(Request());

        var refreshed = await harness.Auth.RefreshTokenAsync(
            new RefreshTokenRequest { RefreshToken = signIn.Auth.RefreshToken });

        Assert.Equal(signIn.Auth.User.Id, refreshed.User.Id);
    }
}

public class GoogleSignInRequestValidatorTests
{
    private static readonly GoogleSignInRequestValidator Validator = new();

    [Fact]
    public void A_request_carrying_neither_a_credential_nor_a_code_is_rejected()
    {
        var result = Validator.Validate(new GoogleSignInRequest());

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            error => error.ErrorMessage == UserMessages.Auth.GoogleCredentialRequired);
    }

    [Fact]
    public void A_request_carrying_an_identity_token_is_accepted()
    {
        Assert.True(Validator.Validate(new GoogleSignInRequest { IdToken = "token" }).IsValid);
    }

    [Fact]
    public void A_request_carrying_an_authorization_code_is_accepted()
    {
        Assert.True(Validator.Validate(new GoogleSignInRequest { Code = "code" }).IsValid);
    }

    [Fact]
    public void A_referral_code_longer_than_the_catalogue_allows_is_rejected()
    {
        var result = Validator.Validate(new GoogleSignInRequest
        {
            IdToken = "token",
            ReferralCode = new string('A', ReferralCatalog.MaxCodeLength + 1)
        });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void A_referral_code_carrying_punctuation_is_rejected()
    {
        var result = Validator.Validate(new GoogleSignInRequest
        {
            IdToken = "token",
            ReferralCode = "ABC-123"
        });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void A_center_outside_the_governorate_is_rejected()
    {
        var result = Validator.Validate(new GoogleSignInRequest
        {
            IdToken = "token",
            Center = "القاهرة"
        });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Every_rejection_message_is_written_in_Arabic()
    {
        var result = Validator.Validate(new GoogleSignInRequest
        {
            Center = "القاهرة",
            ReferralCode = "ABC-123"
        });

        Assert.All(result.Errors, error => Assert.True(ArabicText.IsArabic(error.ErrorMessage)));
    }
}
