using Capital.Value;

namespace Capital.Accumulation;

/// <summary>
/// Primitive accumulation (Capital I, ch. 26): "so-called primitive accumulation is nothing else
/// than the historical process of divorcing the producer from the means of production" — a series
/// of expropriations. Its product is capital: the divorced means of production, concentrated in
/// one owner's hands, enter production as constant capital (ch. 27–31: enclosures, national debt,
/// taxation, colonies).
/// </summary>
public sealed record PrimitiveAccumulation
{
    public IReadOnlyList<Expropriation> Expropriations { get; }

    /// <summary>The divorced means of production, concentrated — the raw material of future constant capital.</summary>
    public IReadOnlyList<CommodityLot> CapitalizedMeansOfProduction { get; }

    public PrimitiveAccumulation(IReadOnlyList<Expropriation> expropriations)
    {
        ArgumentNullException.ThrowIfNull(expropriations);
        if (expropriations.Count == 0)
            throw new ArgumentException("Primitive accumulation is a series of expropriations; none given.", nameof(expropriations));
        var expropriationArray = new Expropriation[expropriations.Count];
        var looted = new List<CommodityLot>();
        for (var i = 0; i < expropriations.Count; i++)
        {
            var expropriation = expropriations[i] ?? throw new ArgumentException("Expropriations cannot be null.", nameof(expropriations));
            looted.AddRange(expropriation.MeansOfProduction);
            expropriationArray[i] = expropriation;
        }
        Expropriations = expropriationArray;
        CapitalizedMeansOfProduction = looted;
    }

    public static PrimitiveAccumulation Of(params Expropriation[] expropriations) => new(expropriations);

    public override string ToString() =>
        $"primitive accumulation: {Expropriations.Count} expropriation(s), {CapitalizedMeansOfProduction.Count} lot(s) capitalized";
}
