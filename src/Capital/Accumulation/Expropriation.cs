using Capital.Circulation;
using Capital.Value;

namespace Capital.Accumulation;

/// <summary>
/// Expropriation (Capital I, Part 8): the producer separated from the means of production by
/// force — the process Marx calls "so-called primitive accumulation", written in letters of blood
/// and fire. Its economic content: the means of production change owners and become capital.
/// </summary>
public sealed record Expropriation
{
    public Owner Producer { get; }
    public IReadOnlyList<CommodityLot> MeansOfProduction { get; }
    public Owner Expropriator { get; }

    public Expropriation(Owner producer, IReadOnlyList<CommodityLot> meansOfProduction, Owner expropriator)
    {
        ArgumentNullException.ThrowIfNull(producer);
        ArgumentNullException.ThrowIfNull(expropriator);
        ArgumentNullException.ThrowIfNull(meansOfProduction);
        if (producer == expropriator)
            throw new ArgumentException("The expropriator must be a different owner than the expropriated producer.");
        if (meansOfProduction.Count == 0)
            throw new ArgumentException("An expropriation divorces the producer from real means of production.", nameof(meansOfProduction));
        var array = new CommodityLot[meansOfProduction.Count];
        for (var i = 0; i < meansOfProduction.Count; i++)
            array[i] = meansOfProduction[i] ?? throw new ArgumentException("Means of production cannot be null.", nameof(meansOfProduction));
        Producer = producer;
        MeansOfProduction = array;
        Expropriator = expropriator;
    }

    public override string ToString() =>
        $"{Expropriator.Name} expropriates {string.Join(", ", MeansOfProduction)} from {Producer.Name}";
}
