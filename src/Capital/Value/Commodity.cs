namespace Capital.Value;

/// <summary>
/// The elementary form of bourgeois wealth (Capital I, ch. 1): a thing with a use-value whose value
/// is the quantity of abstract labour congealed in it. <see cref="Value"/> is expressed for the
/// definite quantity named by the use-value (e.g. "20 yards of linen").
/// </summary>
public sealed record Commodity
{
    public string Name { get; }
    public UseValue UseValue { get; }
    public LaborTime Value { get; }

    public Commodity(string name, UseValue useValue, LaborTime value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A commodity needs a name.", nameof(name));
        Name = name;
        UseValue = useValue ?? throw new ArgumentNullException(nameof(useValue));
        Value = value;
    }
}
