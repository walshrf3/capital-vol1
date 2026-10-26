using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// The sale C–M (Capital I, ch. 3 §2): a commodity's first metamorphosis — the lot leaves
/// circulation as use-value and its value re-appears in money.
/// </summary>
public sealed record Sale
{
    public Owner Seller { get; }
    public CommodityLot Commodity { get; }
    public MoneyQuantity Money { get; }

    public Sale(Owner seller, CommodityLot commodity, MoneyQuantity money)
    {
        Seller = seller ?? throw new ArgumentNullException(nameof(seller));
        Commodity = commodity ?? throw new ArgumentNullException(nameof(commodity));
        Money = money;
    }

    public override string ToString() => $"{Commodity} – {Money}";
}
