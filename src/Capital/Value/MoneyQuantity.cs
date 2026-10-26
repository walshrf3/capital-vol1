namespace Capital.Value;

/// <summary>
/// A quantity of money in a named monetary unit ("£", "oz of gold"). The money-expression side
/// (Capital I, ch. 3): money names are fixed by custom and conceal the real gold content behind
/// them. Value itself stays measured in <see cref="LaborTime"/> and is never conflated with money.
/// </summary>
public readonly record struct MoneyQuantity : IComparable<MoneyQuantity>
{
    public decimal Amount { get; }
    public string Unit { get; }

    private MoneyQuantity(decimal amount, string unit)
    {
        Amount = amount;
        Unit = unit;
    }

    public static MoneyQuantity From(decimal amount, string unit) =>
        amount < 0m
            ? throw new ArgumentOutOfRangeException(nameof(amount), amount, "A money quantity cannot be negative.")
            : string.IsNullOrWhiteSpace(unit)
                ? throw new ArgumentException("A money quantity needs a monetary unit.", nameof(unit))
                : new MoneyQuantity(amount, unit);

    /// <summary>Zero money in a given unit — the identity element for addition.</summary>
    public static MoneyQuantity Zero(string unit) => new(0m, unit);

    public static MoneyQuantity operator +(MoneyQuantity left, MoneyQuantity right)
    {
        RequireSameUnit(left, right);
        return new MoneyQuantity(left.Amount + right.Amount, left.Unit);
    }

    /// <summary>Difference of two money quantities in the same unit. Throws when the result would be negative.</summary>
    public static MoneyQuantity operator -(MoneyQuantity left, MoneyQuantity right)
    {
        RequireSameUnit(left, right);
        return right.Amount > left.Amount
            ? throw new ArgumentOutOfRangeException(nameof(right), right.Amount, "Money cannot go negative.")
            : new MoneyQuantity(left.Amount - right.Amount, left.Unit);
    }

    /// <summary>Divides money by a positive factor (e.g. the number of turns of circulation).</summary>
    public static MoneyQuantity operator /(MoneyQuantity money, decimal divisor) =>
        divisor <= 0m
            ? throw new ArgumentOutOfRangeException(nameof(divisor), divisor, "Cannot divide money by a non-positive number.")
            : new MoneyQuantity(money.Amount / divisor, money.Unit);

    public static bool operator >(MoneyQuantity left, MoneyQuantity right) { RequireSameUnit(left, right); return left.Amount > right.Amount; }
    public static bool operator >=(MoneyQuantity left, MoneyQuantity right) { RequireSameUnit(left, right); return left.Amount >= right.Amount; }
    public static bool operator <(MoneyQuantity left, MoneyQuantity right) { RequireSameUnit(left, right); return left.Amount < right.Amount; }
    public static bool operator <=(MoneyQuantity left, MoneyQuantity right) { RequireSameUnit(left, right); return left.Amount <= right.Amount; }

    public int CompareTo(MoneyQuantity other)
    {
        RequireSameUnit(this, other);
        return Amount.CompareTo(other.Amount);
    }

    private static void RequireSameUnit(MoneyQuantity left, MoneyQuantity right)
    {
        if (left.Unit != right.Unit)
            throw new ArgumentException($"Cannot combine money in {left.Unit} with money in {right.Unit}.");
    }

    public override string ToString() => $"{Amount:0.###} {Unit}";
}
