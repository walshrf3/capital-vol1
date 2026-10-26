namespace Capital.Value;

/// <summary>
/// A definite quantity of a commodity — the bearer Marx actually exchanges ("20 yards of linen",
/// "1 coat"). Its <see cref="Value"/> is the SNLT congealed in the whole lot, computed as
/// quantity times the commodity's per-unit value; derived quantities are never stored.
/// </summary>
public sealed record CommodityLot
{
    public Commodity Commodity { get; }
    public Quantity Quantity { get; }

    public LaborTime Value => Quantity.Amount * Commodity.UnitValue;

    public CommodityLot(Commodity commodity, Quantity quantity)
    {
        Commodity = commodity ?? throw new ArgumentNullException(nameof(commodity));
        if (quantity.Amount <= 0m || string.IsNullOrWhiteSpace(quantity.Unit))
            throw new ArgumentOutOfRangeException(nameof(quantity), "A lot needs a positive, well-formed quantity.");
        Quantity = quantity;
    }

    /// <summary>Renders the way Marx writes lots: "20 yards of linen", "1 coat".</summary>
    public override string ToString()
    {
        var amount = Quantity.Amount;
        return Commodity.Unit == Commodity.Name
            ? $"{amount:0.###} {Commodity.Unit}"
            : $"{amount:0.###} {Commodity.Unit} of {Commodity.Name}";
    }
}
