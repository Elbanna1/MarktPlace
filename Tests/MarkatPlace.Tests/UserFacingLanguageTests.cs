using System.Reflection;
using Shared.Constants;
using Xunit;

namespace MarkatPlace.Tests;

public class UserFacingLanguageTests
{
    public static TheoryData<string, string> AllUserMessages()
    {
        var data = new TheoryData<string, string>();

        void Collect(Type type, string prefix)
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.IsLiteral && field.FieldType == typeof(string))
                    data.Add($"{prefix}{field.Name}", (string)field.GetRawConstantValue()!);
            }

            foreach (var nested in type.GetNestedTypes(BindingFlags.Public))
                Collect(nested, $"{prefix}{nested.Name}.");
        }

        Collect(typeof(UserMessages), "UserMessages.");
        return data;
    }

    [Theory]
    [MemberData(nameof(AllUserMessages))]
    public void Every_user_message_is_written_in_Arabic(string name, string message)
    {
        Assert.False(string.IsNullOrWhiteSpace(message), $"{name} is empty.");
        Assert.True(ArabicText.IsArabic(message), $"{name} is not Arabic: \"{message}\"");
    }

    [Theory]
    [MemberData(nameof(AllUserMessages))]
    public void No_user_message_contains_English_prose_or_an_internal_name(string name, string message)
    {
        var withoutBorrowings = message.Replace("Spam", string.Empty);

        Assert.False(ArabicText.ContainsEnglishProse(withoutBorrowings),
            $"{name} contains Latin words a customer should not be shown: \"{message}\"");
    }

    [Fact]
    public void The_catalogue_is_not_empty_and_covers_the_operations_the_API_performs()
    {
        var all = AllUserMessages().Select(row => (string)row[0]!).ToList();

        Assert.True(all.Count >= 40, $"only {all.Count} messages are defined.");
        Assert.Contains("UserMessages.Listings.Created", all);
        Assert.Contains("UserMessages.Listings.Updated", all);
        Assert.Contains("UserMessages.Listings.Deleted", all);
        Assert.Contains("UserMessages.Auth.LoggedIn", all);
        Assert.Contains("UserMessages.Errors.TooManyRequests", all);
        Assert.Contains("UserMessages.Errors.Unexpected", all);
    }

    [Fact]
    public void The_create_and_update_messages_tell_the_seller_a_review_is_coming()
    {
        Assert.Contains("راجع", UserMessages.Listings.Created);
        Assert.Contains("راجع", UserMessages.Listings.Updated);
    }

    [Fact]
    public void The_password_reset_message_does_not_confirm_whether_the_account_exists()
    {
        Assert.Contains("لو", UserMessages.Auth.PasswordResetCodeSent);
    }

    [Fact]
    public void The_pipeline_failure_messages_say_what_happened_rather_than_naming_a_status_code()
    {
        foreach (var message in new[]
                 {
                     UserMessages.Errors.Unexpected, UserMessages.Errors.SignInRequired,
                     UserMessages.Errors.NotAllowed, UserMessages.Errors.NotFound,
                     UserMessages.Errors.InvalidData, UserMessages.Errors.TooManyRequests,
                 })
        {
            Assert.True(ArabicText.IsArabic(message));
            foreach (var forbidden in new[]
                     {
                         "Unauthorized", "Forbidden", "Not Found", "Bad Request", "Error",
                         "Exception", "401", "403", "404", "500",
                     })
            {
                Assert.DoesNotContain(forbidden, message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
