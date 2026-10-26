namespace Capital.Value;

/// <summary>
/// A kind of commodity (Capital I, ch. 1 §1): a use-value with a unit of measure and a definite
/// value per unit — the socially necessary labour time congealed in one unit. Concrete traded
/// quantities are <see cref="CommodityLot"/>s.
/// </summary>
public sealed record Commodity
{
    public string Name { get; }
    public UseValue UseValue { get; }
    public string Unit { get; }
    public LaborTime UnitValue { get; }

    public Commodity(string name, UseValue useValue, string unit, LaborTime unitValue)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A commodity needs a name.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("A commodity needs a unit of measure.", nameof(unit));
        Name = name;
        UseValue = useValue ?? throw new ArgumentNullException(nameof(useValue));
        Unit = unit;
        UnitValue = unitValue;
    }
}
