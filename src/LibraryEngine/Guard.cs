using System.Net.Mail;

namespace LibraryEngine;

internal static class Guard
{
    public static string NotBlank(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new LibraryException($"{field} is required.");
        }

        return value.Trim();
    }

    public static string Email(string? value)
    {
        var email = NotBlank(value, "Email");

        if (!MailAddress.TryCreate(email, out var parsed) || parsed.Address != email)
        {
            throw new LibraryException("Email is not valid.");
        }

        return email;
    }
}
