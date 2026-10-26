namespace Capital.Value;

/// <summary>
/// A magnitude of abstract human labour, measured in hours of socially necessary labour time (SNLT).
/// Capital I, ch. 1 §1–2: value is congealed social labour; only the quantity of labour time counts,
/// never the particular concrete kind of labour that produced it. Exact decimal arithmetic, kept
/// strictly separate from money.
/// </summary>
/// <remarks>
/// Allocation-free value object, immutably non-negative: no quantity of congealed labour can be negative.
/// </remarks>
public readonly record struct LaborTime : IComparable<LaborTime>
{
    public decimal Hours { get; }

    private LaborTime(decimal hours) => Hours = hours;

    /// <summary>Zero labour time — the identity element for addition.</summary>
    public static LaborTime Zero { get; } = new(0m);

    /// <summary>Creates a magnitude of SNLT from an hour count; rejects negative values.</summary>
    public static LaborTime FromHours(decimal hours) =>
        hours < 0m
            ? throw new ArgumentOutOfRangeException(nameof(hours), hours, "Labour time cannot be negative.")
            : new LaborTime(hours);

    public static LaborTime operator +(LaborTime left, LaborTime right) => new(left.Hours + right.Hours);

    /// <summary>
    /// Difference of two labour-time magnitudes. Throws when the result would be negative.
    /// </summary>
    public static LaborTime operator -(LaborTime left, LaborTime right) =>
        right.Hours > left.Hours
            ? throw new ArgumentOutOfRangeException(nameof(right), right.Hours, "Labour time cannot go negative.")
            : new LaborTime(left.Hours - right.Hours);

    /// <summary>Scales a labour-time magnitude by a non-negative factor (e.g. a quantity of output).</summary>
    public static LaborTime operator *(LaborTime time, decimal factor) =>
        factor < 0m
            ? throw new ArgumentOutOfRangeException(nameof(factor), factor, "Cannot scale labour time by a negative factor.")
            : new LaborTime(time.Hours * factor);

    /// <summary>Commutative form of scaling: a quantity of units times the per-unit labour time.</summary>
    public static LaborTime operator *(decimal factor, LaborTime time) => time * factor;

    /// <summary>Divides labour time by a positive number of units — the per-unit value under a given productivity.</summary>
    public static LaborTime operator /(LaborTime time, decimal divisor) =>
        divisor <= 0m
            ? throw new ArgumentOutOfRangeException(nameof(divisor), divisor, "Cannot divide labour time by a non-positive number.")
            : new LaborTime(time.Hours / divisor);

    public static bool operator >(LaborTime left, LaborTime right) => left.Hours > right.Hours;
    public static bool operator >=(LaborTime left, LaborTime right) => left.Hours >= right.Hours;
    public static bool operator <(LaborTime left, LaborTime right) => left.Hours < right.Hours;
    public static bool operator <=(LaborTime left, LaborTime right) => left.Hours <= right.Hours;

    public int CompareTo(LaborTime other) => Hours.CompareTo(other.Hours);

    public override string ToString() => $"{Hours:0.###} h";
}
