namespace Capital.Value;

/// <summary>
/// A definite positive amount measured in a named unit of use-value ("20 yards", "1 coat").
/// Quantities are the carriers of exchange equations; value is never measured in them.
/// </summary>
public readonly record struct Quantity
{
    public decimal Amount { get; }
    public string Unit { get; }

    public Quantity(decimal amount, string unit)
    {
        if (amount <= 0m)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "A quantity must be positive.");
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("A quantity needs a unit of measure.", nameof(unit));
        Amount = amount;
        Unit = unit;
    }

    public override string ToString() => $"{Amount:0.###} {Unit}";
}
