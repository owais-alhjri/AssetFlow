using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private const string DefaultCurrency = "OMR";

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string? currency = null)
    {
        if (amount < 0)
            return MoneyErrors.NegativeAmount;

        var resolvedCurrency = string.IsNullOrWhiteSpace(currency)
            ? DefaultCurrency
            : currency.Trim().ToUpperInvariant();
        if (resolvedCurrency.Length != 3)
            return MoneyErrors.InvalidCurrency;

        return new Money(amount, resolvedCurrency);
    }

    public static Money Zero(string? currency = null) =>
         new(0, string.IsNullOrWhiteSpace(currency) ? DefaultCurrency : currency.Trim().ToUpperInvariant());

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException(
                $"Cannot add Money with different currencies: {Currency} vs {other.Currency}");
        return new Money(Amount + other.Amount, Currency);
    }

    public override bool Equals(object? obj) =>
        obj is Money other && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount} {Currency}";

}
public static class MoneyErrors
{
    public static readonly Error NegativeAmount =
        new("Money.NegativeAmount", "Amount cannot be negative.");

    public static readonly Error InvalidCurrency =
        new("Money.InvalidCurrency", "Currency must be a valid 3-letter code.");
}