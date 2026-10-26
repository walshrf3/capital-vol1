using Capital.Circulation;
using Capital.Value;

namespace Capital.Tests.Circulation;

public class MoneyCircuitTests
{
    private static readonly Commodity Cotton = new("cotton", new UseValue("Spinning material"), "lb", LaborTime.FromHours(0.05m));
    private static readonly Owner Merchant = new("the cotton merchant");

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    [Fact]
    public void The_circuit_resells_the_same_lot_for_more_money()
    {
        // Marx's example: £100 buys 2000 lb of cotton; the very same lot sells for £110.
        var lot = Lot(Cotton, 2000m);
        var purchase = new Purchase(Merchant, MoneyQuantity.From(100m, "£"), lot);
        var sale = new Sale(Merchant, lot, MoneyQuantity.From(110m, "£"));

        var circuit = MoneyCircuit.Create(purchase, sale);

        Assert.Equal(MoneyQuantity.From(100m, "£"), circuit.Start);
        Assert.Equal(MoneyQuantity.From(110m, "£"), circuit.End);
        Assert.Equal(MoneyQuantity.From(10m, "£"), circuit.SurplusValue);
        Assert.Equal("100 £ – 2000 lb of cotton – 110 £", circuit.ToString());
    }

    [Fact]
    public void The_degenerate_circuit_valorizes_nothing()
    {
        var lot = Lot(Cotton, 2000m);
        var purchase = new Purchase(Merchant, MoneyQuantity.From(100m, "£"), lot);
        var sale = new Sale(Merchant, lot, MoneyQuantity.From(100m, "£"));

        var circuit = MoneyCircuit.Create(purchase, sale);

        Assert.Equal(MoneyQuantity.Zero("£"), circuit.SurplusValue);
    }

    [Fact]
    public void The_circuit_cannot_close_below_its_start()
    {
        var lot = Lot(Cotton, 2000m);
        var purchase = new Purchase(Merchant, MoneyQuantity.From(100m, "£"), lot);
        var sale = new Sale(Merchant, lot, MoneyQuantity.From(90m, "£"));

        Assert.Throws<ArgumentException>(() => MoneyCircuit.Create(purchase, sale));
    }

    [Fact]
    public void The_circuit_cannot_sell_a_different_lot()
    {
        var purchase = new Purchase(Merchant, MoneyQuantity.From(100m, "£"), Lot(Cotton, 2000m));
        var sale = new Sale(Merchant, Lot(Cotton, 2500m), MoneyQuantity.From(110m, "£"));

        Assert.Throws<ArgumentException>(() => MoneyCircuit.Create(purchase, sale));
    }
}
