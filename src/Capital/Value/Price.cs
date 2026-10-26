namespace Capital.Value;

/// <summary>
/// The money-expression of a commodity lot's value (Capital I, ch. 3 §1a) — money's first function,
/// the measure of values. Price is ideal: no real money needs to be at hand to name a price. The
/// form deliberately allows quantitative incongruity: a price can diverge from the value of the
/// lot (a possibility circulation constantly exploits); the value itself remains the SNLT in the
/// lot and is never an alias for money.
/// </summary>
public sealed record Price
{
    public CommodityLot Lot { get; }
    public MoneyQuantity Amount { get; }

    public Price(CommodityLot lot, MoneyQuantity amount)
    {
        Lot = lot ?? throw new ArgumentNullException(nameof(lot));
        Amount = amount;
    }

    public static Price Create(CommodityLot lot, MoneyQuantity amount) => new(lot, amount);

    public override string ToString() => $"{Lot} at {Amount}";
}
