namespace Capital.Value;

/// <summary>
/// The qualitative, material side of the commodity — the utility that makes a thing a use-value
/// (Capital I, ch. 1 §1). Use-values differ in kind; they are compared only as bearers of value,
/// never as use-values.
/// </summary>
public sealed record UseValue
{
    public string Description { get; }

    public UseValue(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A use-value needs a concrete description.", nameof(description));
        Description = description;
    }
}
