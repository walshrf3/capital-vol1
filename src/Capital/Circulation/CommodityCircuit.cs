using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// C–M–C, the complete metamorphosis of a commodity (Capital I, ch. 3 §2): sale then purchase,
/// the same money sum passing through both phases unchanged. The circuit begins and ends in
/// use-values — its goal is consumption, not value — and money functions here as mere medium of
/// circulation. Requires two different use-values: a circuit that ends where it began does nothing.
/// </summary>
public sealed record CommodityCircuit
{
    public Sale Sale { get; }
    public Purchase Purchase { get; }

    public CommodityCircuit(Sale sale, Purchase purchase)
    {
        ArgumentNullException.ThrowIfNull(sale);
        ArgumentNullException.ThrowIfNull(purchase);
        if (sale.Money != purchase.Money)
            throw new ArgumentException(
                $"C–M–C passes the same money through both phases: the sale brings {sale.Money}, the purchase spends {purchase.Money}.");
        if (sale.Commodity.Commodity == purchase.Commodity.Commodity)
            throw new ArgumentException(
                $"C–M–C is pointless without a change of use-value: {sale.Commodity} and {purchase.Commodity} are the same commodity.");
        Sale = sale;
        Purchase = purchase;
    }

    public static CommodityCircuit Create(Sale sale, Purchase purchase) => new(sale, purchase);

    /// <summary>The two use-value poles of the circuit — C′ – M – C′′.</summary>
    public (CommodityLot First, CommodityLot Second) Commodities => (Sale.Commodity, Purchase.Commodity);

    /// <summary>True when the circuit exchanges equals: the bought use-value embodies the same labour as the sold one.</summary>
    public bool ExchangesEquals => Sale.Commodity.Value == Purchase.Commodity.Value;

    public override string ToString() => $"{Sale.Commodity} – {Sale.Money} – {Purchase.Commodity}";
}
