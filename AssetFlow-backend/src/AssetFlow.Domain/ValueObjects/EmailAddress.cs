using System.Text.RegularExpressions;
using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.ValueObjects;

public sealed class EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static Result<EmailAddress> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return EmailAddressErrors.Empty;
        var trimmed = value.Trim();
        var isValid = Regex.IsMatch(
            trimmed,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        );
        if (!isValid)
            return EmailAddressErrors.InvalidFormat;

        return new EmailAddress(trimmed.ToLowerInvariant());
    }


    public override bool Equals(object? obj) =>
        obj is EmailAddress other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}

public static class EmailAddressErrors
{
    public static readonly Error Empty =
        new("EmailAddress.Empty", "Email Address can not be empty");

    public static readonly Error InvalidFormat =
        new("EmailAddress.InvalidFormat", "Invalid format for email address");
}