namespace Capital.Circulation;

/// <summary>
/// A commodity owner — the personified bearer of commodities (Capital I, ch. 2). Owners enter the
/// exchange process only as private owners, each recognizing the other as such; their will is
/// what resides in the commodities. Agents are names until accumulation demands more.
/// </summary>
public sealed record Owner
{
    public string Name { get; }

    public Owner(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("An owner needs a name.", nameof(name));
        Name = name;
    }
}
