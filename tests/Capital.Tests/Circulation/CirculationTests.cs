using Capital.Circulation;
using Capital.Value;

namespace Capital.Tests.Circulation;

public class CirculationTests
{
    private static readonly Commodity Linen = new("linen", new UseValue("Woven cloth"), "yards", LaborTime.FromHours(0.5m));
    private static readonly Commodity Bible = new("Bible", new UseValue("The word of God in print"), "Bible", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    private static readonly Owner Weaver = new("the linen weaver");
    private static readonly Owner Dealer = new("the Bible dealer");

    [Fact]
    public void Blank_owner_name_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new Owner("  "));
    }

    [Fact]
    public void Commodity_circuit_carries_one_money_sum_through_two_metamorphoses()
    {
        // Marx's weaver: 20 yards of linen (10 h) – 2 oz of gold – 1 Bible (10 h).
        var sale = new Sale(Weaver, Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));
        var purchase = new Purchase(Weaver, MoneyQuantity.From(2m, "oz of gold"), Lot(Bible, 1m));

        var circuit = CommodityCircuit.Create(sale, purchase);

        Assert.Equal((Lot(Linen, 20m), Lot(Bible, 1m)), circuit.Commodities);
        Assert.True(circuit.ExchangesEquals);
        Assert.Equal("20 yards of linen – 2 oz of gold – 1 Bible", circuit.ToString());
    }

    [Fact]
    public void Commodity_circuit_rejects_unequal_money_sums()
    {
        var sale = new Sale(Weaver, Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));
        var purchase = new Purchase(Dealer, MoneyQuantity.From(3m, "oz of gold"), Lot(Bible, 1m));

        Assert.Throws<ArgumentException>(() => CommodityCircuit.Create(sale, purchase));
    }

    [Fact]
    public void Commodity_circuit_rejects_a_useless_loop()
    {
        var sale = new Sale(Weaver, Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));
        var purchase = new Purchase(Dealer, MoneyQuantity.From(2m, "oz of gold"), Lot(Linen, 20m));

        Assert.Throws<ArgumentException>(() => CommodityCircuit.Create(sale, purchase));
    }

    [Fact]
    public void Commodity_circuit_can_exchange_unequals()
    {
        // 10 h of linen sold, 5 h of tea bought — circulation tolerates the incongruity.
        var sale = new Sale(Weaver, Lot(Linen, 20m), MoneyQuantity.From(2m, "oz of gold"));
        var purchase = new Purchase(Dealer, MoneyQuantity.From(2m, "oz of gold"), Lot(Tea, 5m));

        var circuit = CommodityCircuit.Create(sale, purchase);

        Assert.False(circuit.ExchangesEquals);
    }

    [Fact]
    public void Hoard_withdraws_money_from_circulation()
    {
        var hoard = new Hoard(Weaver, MoneyQuantity.From(2m, "oz of gold"));

        Assert.Equal("the linen weaver hoards 2 oz of gold", hoard.ToString());
    }

    [Fact]
    public void World_money_is_named_money_without_a_domestic_role()
    {
        var worldMoney = new WorldMoney(MoneyQuantity.From(2m, "oz of gold"));

        Assert.Equal("world money: 2 oz of gold", worldMoney.ToString());
    }

    [Fact]
    public void Payment_discharges_the_debt_exactly()
    {
        var debt = new Debt(Weaver, Dealer, MoneyQuantity.From(2m, "oz of gold"));
        var payment = Payment.Settle(debt, MoneyQuantity.From(2m, "oz of gold"));

        Assert.Equal("2 oz of gold discharges the Bible dealer owes the linen weaver 2 oz of gold", payment.ToString());

        Assert.Throws<ArgumentException>(() => Payment.Settle(debt, MoneyQuantity.From(1m, "oz of gold")));
    }

    [Fact]
    public void Circulation_money_is_price_sum_over_turns()
    {
        var prices = new[]
        {
            MoneyQuantity.From(2m, "oz of gold"),
            MoneyQuantity.From(1m, "oz of gold"),
            MoneyQuantity.From(3m, "oz of gold"),
        };

        Assert.Equal(MoneyQuantity.From(2m, "oz of gold"), MediumOfCirculation.Required(prices, 3m));

        Assert.Throws<ArgumentException>(() => MediumOfCirculation.Required(
            new[] { MoneyQuantity.From(2m, "oz of gold"), MoneyQuantity.From(1m, "£") }, 3m));
        Assert.Throws<ArgumentOutOfRangeException>(() => MediumOfCirculation.Required(prices, 0m));
        Assert.Throws<ArgumentException>(() => MediumOfCirculation.Required(Array.Empty<MoneyQuantity>(), 1m));
    }
}
