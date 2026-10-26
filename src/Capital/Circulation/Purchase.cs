using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// The purchase M–C (Capital I, ch. 3 §2): a commodity's second metamorphosis — money re-assumes
/// the commodity form and vanishes from circulation into consumption.
/// </summary>
public sealed record Purchase
{
    public Owner Buyer { get; }
    public MoneyQuantity Money { get; }
    public CommodityLot Commodity { get; }

    public Purchase(Owner buyer, MoneyQuantity money, CommodityLot commodity)
    {
        Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
        Money = money;
        Commodity = commodity ?? throw new ArgumentNullException(nameof(commodity));
    }

    public override string ToString() => $"{Money} – {Commodity}";
}
