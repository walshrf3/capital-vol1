using Capital.Value;

namespace Capital.Production;

/// <summary>
/// The subsistence bundle (Capital I, ch. 6 §2): the definite quantity of commodities required to
/// keep the owner of labour-power alive and able to work. Labour-power's value is the value of
/// exactly these commodities — congealed in <see cref="LaborTime"/> like any other value.
/// </summary>
public sealed record SubsistenceBundle
{
    public IReadOnlyList<CommodityLot> Items { get; }

    /// <summary>The value of the bundle — the value of the labour-power it maintains.</summary>
    public LaborTime Value { get; }

    public SubsistenceBundle(IReadOnlyList<CommodityLot> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (items.Count == 0)
            throw new ArgumentException("A subsistence bundle cannot be empty.", nameof(items));
        var array = new CommodityLot[items.Count];
        decimal totalHours = 0m;
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i] ?? throw new ArgumentException("Subsistence items cannot be null.", nameof(items));
            totalHours += item.Value.Hours;
            array[i] = item;
        }
        Items = array;
        Value = LaborTime.FromHours(totalHours);
    }

    public override string ToString() => string.Join(", ", Items);
}
