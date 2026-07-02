using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.ValueObjects;

public sealed class SerialNumber
{
    public string Value { get; }

    private SerialNumber(string value)
    {
        Value = value;
    }

    public static Result<SerialNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return SerialNumberErrors.Empty;
        var trimmed = value.Trim();

        if (trimmed.Length > 100)
            return SerialNumberErrors.TooLong;

        return new SerialNumber(trimmed);
    }

    public override bool Equals(object? obj) =>
        obj is SerialNumber other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}

public static class SerialNumberErrors
{
    public static readonly Error Empty =
        new("SerialNumber.Empty", "Serial Number can not be empty");

    public static readonly Error TooLong =
        new("SerialNumber.TooLong", "Serial Number length must be less then 100 characters");
}