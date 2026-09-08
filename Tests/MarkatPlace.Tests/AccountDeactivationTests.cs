using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs.Account;
using Shared.DTOs.Auth;
using Shared.Enums;
using Shared.Exceptions;
using Xunit;

namespace MarkatPlace.Tests;

public class AccountDeactivationTests
{
    private static DeactivateAccountRequest Confirmed(string? password = "Passw0rd!") =>
        new() { Confirm = true, Password = password };

    [Fact]
    public async Task A_user_can_close_their_own_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("محمد", "owner@example.com");

        var result = await harness.Account.DeactivateAsync(user.Id, Confirmed());

        Assert.Equal(user.Id, result.UserId);
        Assert.Equal((int)UserAccountStatus.Deactivated, result.Status);

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Equal(UserAccountStatus.Deactivated, stored!.Status);
        Assert.Equal(user.Id, stored.StatusChangedBy);
        Assert.NotNull(stored.StatusChangedAt);
    }

    [Fact]
    public async Task Closing_an_account_is_only_ever_applied_to_the_caller()
    {
        await using var harness = GoogleSignInHarness.Create();

        var attacker = await harness.AddPasswordUserAsync("مهاجم", "a@example.com");
        var victim = await harness.AddPasswordUserAsync("ضحية", "b@example.com");

        await harness.Account.DeactivateAsync(attacker.Id, Confirmed());

        var untouched = await harness.Users.FindByIdAsync(victim.Id);

        Assert.Equal(UserAccountStatus.Active, untouched!.Status);
        Assert.DoesNotContain(victim.Id, harness.Closure.Closed);
    }

    [Fact]
    public async Task An_unknown_caller_is_refused_rather_than_creating_anything()
    {
        await using var harness = GoogleSignInHarness.Create();

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            harness.Account.DeactivateAsync(Guid.NewGuid().ToString(), Confirmed()));
    }

    [Fact]
    public async Task A_second_close_of_the_same_account_is_a_conflict()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("مكرر", "dup@example.com");

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        await Assert.ThrowsAsync<ConflictException>(() =>
            harness.Account.DeactivateAsync(user.Id, Confirmed()));

        Assert.Single(harness.Closure.Closed);
    }

    [Fact]
    public async Task Repeated_requests_close_the_account_exactly_once()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("متزامن", "race@example.com");

        var succeeded = 0;

        foreach (var _ in Enumerable.Range(0, 4))
        {
            try
            {
                await harness.Account.DeactivateAsync(user.Id, Confirmed());
                succeeded++;
            }
            catch (ConflictException)
            {
            }
        }

        Assert.Equal(1, succeeded);
        Assert.Single(harness.Closure.Closed);

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Equal(UserAccountStatus.Deactivated, stored!.Status);
    }

    [Fact]
    public async Task A_wrong_password_does_not_close_the_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("كلمةالسر", "pwd@example.com");

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            harness.Account.DeactivateAsync(user.Id, Confirmed("Wrong1!aa")));

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Equal(UserAccountStatus.Active, stored!.Status);
        Assert.Empty(harness.Closure.Closed);
    }

    [Fact]
    public async Task An_account_that_has_a_password_must_supply_it()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("بدون", "nopwd@example.com");

        await Assert.ThrowsAsync<BadRequestException>(() =>
            harness.Account.DeactivateAsync(user.Id, Confirmed(password: null)));

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Equal(UserAccountStatus.Active, stored!.Status);
    }

    [Fact]
    public async Task A_Google_only_account_closes_without_a_password()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register("credential", "google-subject", "google@example.com");

        var signIn = await harness.Auth.GoogleSignInAsync(
            new GoogleSignInRequest { IdToken = "credential" });

        Assert.True(signIn.AccountCreated);

        var created = await harness.Users.FindByEmailAsync("google@example.com");

        Assert.False(await harness.Users.HasPasswordAsync(created!));

        await harness.Account.DeactivateAsync(created!.Id, Confirmed(password: null));

        var stored = await harness.Users.FindByIdAsync(created.Id);

        Assert.Equal(UserAccountStatus.Deactivated, stored!.Status);
    }

    [Fact]
    public async Task A_closed_Google_account_cannot_sign_in_again_with_the_same_identity()
    {
        await using var harness = GoogleSignInHarness.Create();

        harness.Google.Register("credential", "google-subject", "google@example.com");

        await harness.Auth.GoogleSignInAsync(new GoogleSignInRequest { IdToken = "credential" });

        var created = await harness.Users.FindByEmailAsync("google@example.com");

        await harness.Account.DeactivateAsync(created!.Id, Confirmed(password: null));

        var refusal = await Assert.ThrowsAsync<ForbiddenException>(() =>
            harness.Auth.GoogleSignInAsync(new GoogleSignInRequest { IdToken = "credential" }));

        Assert.Equal(UserMessages.Account.DeactivatedAccess, refusal.Message);
    }

    [Fact]
    public async Task A_closed_account_cannot_log_in_with_its_password()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("مقفول", "closed@example.com");

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        var refusal = await Assert.ThrowsAsync<ForbiddenException>(() =>
            harness.Auth.LoginAsync(new LoginRequest
            {
                Username = "مقفول",
                Password = "Passw0rd!"
            }));

        Assert.Equal(UserMessages.Account.DeactivatedAccess, refusal.Message);
    }

    [Fact]
    public async Task Closing_an_account_drops_its_refresh_token_and_reset_session()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("جلسة", "session@example.com");

        user.RefreshToken = "a-live-refresh-token";
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(60);
        user.PasswordResetTokenHash = "a-live-reset-token";
        user.PasswordResetVerified = true;

        await harness.Users.UpdateAsync(user);

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Null(stored!.RefreshToken);
        Assert.Null(stored.RefreshTokenExpiryTime);
        Assert.Null(stored.PasswordResetTokenHash);
        Assert.False(stored.PasswordResetVerified);
    }

    [Fact]
    public async Task A_closed_account_cannot_refresh_its_way_back_in()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("تجديد", "refresh@example.com");

        user.RefreshToken = "still-held-by-the-client";
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(60);

        await harness.Users.UpdateAsync(user);

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        await Assert.ThrowsAnyAsync<Exception>(() =>
            harness.Auth.RefreshTokenAsync(new RefreshTokenRequest
            {
                RefreshToken = "still-held-by-the-client"
            }));
    }

    [Fact]
    public async Task Closing_an_account_hides_the_listings_and_interests_it_owns()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("صاحب", "listings@example.com");

        harness.Closure.ListingsPerAccount = 7;

        var result = await harness.Account.DeactivateAsync(user.Id, Confirmed());

        Assert.Equal([user.Id], harness.Closure.Closed);
        Assert.Equal(7, result.ListingsHidden);
    }

    [Fact]
    public async Task An_administrator_cannot_close_their_own_account_from_the_public_endpoint()
    {
        await using var harness = GoogleSignInHarness.Create();

        var admin = await harness.AddPasswordUserAsync("مسؤول", "admin@example.com");

        await harness.PutInRoleAsync(admin, AppRoles.Admin);

        var refusal = await Assert.ThrowsAsync<ForbiddenException>(() =>
            harness.Account.DeactivateAsync(admin.Id, Confirmed()));

        Assert.Equal(UserMessages.Account.AdminCannotSelfDeactivate, refusal.Message);

        var stored = await harness.Users.FindByIdAsync(admin.Id);

        Assert.Equal(UserAccountStatus.Active, stored!.Status);
    }

    [Fact]
    public async Task The_last_super_administrator_is_protected_by_its_own_rule()
    {
        await using var harness = GoogleSignInHarness.Create();

        var superAdmin = await harness.AddPasswordUserAsync("الاعلى", "super@example.com");

        await harness.PutInRoleAsync(superAdmin, AppRoles.Admin);
        await harness.PutInRoleAsync(superAdmin, AppRoles.SuperAdmin);

        var refusal = await Assert.ThrowsAsync<ForbiddenException>(() =>
            harness.Account.DeactivateAsync(superAdmin.Id, Confirmed()));

        Assert.Equal(UserMessages.Account.LastSuperAdminProtected, refusal.Message);
    }

    [Fact]
    public async Task A_super_administrator_with_an_active_peer_still_cannot_close_itself_here()
    {
        await using var harness = GoogleSignInHarness.Create();

        var first = await harness.AddPasswordUserAsync("الاول", "super1@example.com");
        var second = await harness.AddPasswordUserAsync("التاني", "super2@example.com");

        await harness.PutInRoleAsync(first, AppRoles.SuperAdmin);
        await harness.PutInRoleAsync(second, AppRoles.SuperAdmin);

        var refusal = await Assert.ThrowsAsync<ForbiddenException>(() =>
            harness.Account.DeactivateAsync(first.Id, Confirmed()));

        Assert.Equal(UserMessages.Account.AdminCannotSelfDeactivate, refusal.Message);
    }

    [Fact]
    public async Task A_suspended_owner_can_still_close_their_own_account()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync(
            "موقوف", "suspended@example.com", status: UserAccountStatus.Suspended);

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        var stored = await harness.Users.FindByIdAsync(user.Id);

        Assert.Equal(UserAccountStatus.Deactivated, stored!.Status);
    }

    [Fact]
    public async Task A_closed_account_stops_earning_referrals()
    {
        await using var harness = GoogleSignInHarness.Create();

        var referrer = await harness.AddPasswordUserAsync(
            "داعي", "ref@example.com", referralCode: "ABCD1234");

        await harness.Account.DeactivateAsync(referrer.Id, Confirmed());

        var resolved = await harness.Referrals.ResolveReferrerForRegistrationAsync("ABCD1234");

        Assert.Null(resolved);
        Assert.Equal(0, await harness.CountReferralsAsync(referrer.Id));
    }

    [Fact]
    public async Task Closing_an_account_keeps_the_rows_other_records_depend_on()
    {
        await using var harness = GoogleSignInHarness.Create();

        var user = await harness.AddPasswordUserAsync("محفوظ", "kept@example.com");

        harness.Context.Notifications.Add(new Notification
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Title = "عنوان",
            Message = "رسالة",
            CreatedAt = DateTime.UtcNow
        });

        await harness.Context.SaveChangesAsync();

        await harness.Account.DeactivateAsync(user.Id, Confirmed());

        Assert.NotNull(await harness.Users.FindByIdAsync(user.Id));
        Assert.True(await harness.Context.Notifications.AnyAsync(row => row.UserId == user.Id));
    }
}
