using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.ValueObjects;

public sealed class AssetTag
{
    public string Value { get; }

    private AssetTag(string value)
    {
        Value = value;
    }

    public static Result<AssetTag> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return AssetTagErrors.Empty;
        var trimmed = value.Trim();

        if (trimmed.Length > 50)
            return AssetTagErrors.TooLong;

        return new AssetTag(trimmed.ToUpperInvariant());
    }

    public override bool Equals(object? obj) =>
        obj is AssetTag other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

}

public static class AssetTagErrors
{
    public static readonly Error Empty =
        new("AssetTag.Empty", "Asset Tag cannot be empty");

    public static readonly Error TooLong =
        new("AssetTag.TooLong", "Asset Tag length must be less then 50 characters");
}
