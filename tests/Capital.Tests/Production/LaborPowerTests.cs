using Capital.Circulation;
using Capital.Production;
using Capital.Value;

namespace Capital.Tests.Production;

public class LaborPowerTests
{
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    private static Owner Spinner => new("the spinner");

    [Fact]
    public void Value_is_the_labour_in_the_subsistence_bundle()
    {
        // 0.4 quarter of corn (4 h) + 2 lb of tea (2 h) = 6 h of daily labour-power.
        var power = new LaborPower(Spinner, new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));

        Assert.Equal(LaborTime.FromHours(6m), power.Value);
        Assert.Equal("0.4 quarter of corn, 2 lb of tea", power.Subsistence.ToString());
    }

    [Fact]
    public void Empty_subsistence_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new SubsistenceBundle(Array.Empty<CommodityLot>()));
    }

    [Fact]
    public void Null_items_are_rejected()
    {
        Assert.Throws<ArgumentException>(() => new SubsistenceBundle(new CommodityLot[] { null! }));
    }

    [Fact]
    public void Valueless_labour_power_is_rejected()
    {
        var gift = new Commodity("gift", new UseValue("Freely available"), "item", LaborTime.Zero);
        var bundle = new SubsistenceBundle(new[] { new CommodityLot(gift, new Quantity(1m, "item")) });

        Assert.Throws<ArgumentException>(() => new LaborPower(Spinner, bundle));
    }
}
