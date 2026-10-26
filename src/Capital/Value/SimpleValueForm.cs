namespace Capital.Value;

/// <summary>
/// The simple or accidental form of value (Capital I, ch. 1 §3a): one commodity's value is expressed
/// in the natural form of another, which serves as its equivalent — e.g. "20 yards of linen = 1 coat".
/// The equation is only meaningful between commodities embodying equal magnitudes of labour; that
/// equality is what makes qualitatively different use-values commensurable.
/// </summary>
public sealed record SimpleValueForm
{
    /// <summary>The commodity whose value is being expressed (relative form).</summary>
    public Commodity Relative { get; }

    /// <summary>The commodity serving as the equivalent of the other's value (equivalent form).</summary>
    public Commodity Equivalent { get; }

    private SimpleValueForm(Commodity relative, Commodity equivalent)
    {
        Relative = relative;
        Equivalent = equivalent;
    }

    public static SimpleValueForm Create(Commodity relative, Commodity equivalent)
    {
        ArgumentNullException.ThrowIfNull(relative);
        ArgumentNullException.ThrowIfNull(equivalent);
        if (relative == equivalent)
            throw new ArgumentException(
                "A commodity cannot express its value in itself; the equivalent must be a different commodity.");
        if (relative.Value != equivalent.Value)
            throw new ArgumentException(
                $"Commodities in a value equation must embody equal labour: {relative.Name} is {relative.Value}, {equivalent.Name} is {equivalent.Value}.");
        return new SimpleValueForm(relative, equivalent);
    }

    public override string ToString() => $"{Relative.Name} = {Equivalent.Name}";
}
