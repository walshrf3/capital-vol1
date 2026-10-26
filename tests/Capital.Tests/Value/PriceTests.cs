using Capital.Value;

namespace Capital.Tests.Value;

public class PriceTests
{
    private static readonly Commodity Linen = new("linen", new UseValue("Woven cloth"), "yards", LaborTime.FromHours(0.5m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    [Fact]
    public void Price_names_the_lot_in_money()
    {
        var price = new Price(Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));

        Assert.Equal("20 yards of linen at 2 oz of gold", price.ToString());
    }

    [Fact]
    public void Price_may_diverge_from_value()
    {
        // The lot embodies 10 hours = 2 oz of gold at parity; the price form permits any expression.
        var atValue = new Price(Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));
        var aboveValue = new Price(Lot(Linen, 20m), MoneyQuantity.From(3m, "oz of gold"));

        Assert.Equal(LaborTime.FromHours(10m), atValue.Lot.Value);
        Assert.NotEqual(atValue.Amount, aboveValue.Amount);
    }

    [Fact]
    public void Null_lot_is_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => new Price(null!, MoneyQuantity.From(1m, "£")));
    }
}
