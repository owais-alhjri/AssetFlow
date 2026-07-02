using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.ValueObjects;

public sealed class PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static Result<PasswordHash> Create(string hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            return PasswordHashErrors.Empty;

        return new PasswordHash(hashedValue);
    }

    public override bool Equals(object? obj) =>
        obj is PasswordHash other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}

public static class PasswordHashErrors
{
    public static readonly Error Empty =
        new("PasswordHash.Empty", "Password hash cannot be empty.");
}