using Capital.Value;

namespace Capital.Tests.Value;

public class CommodityTests
{
    public static readonly Commodity Linen = new("linen", new UseValue("Woven cloth"), "yards", LaborTime.FromHours(0.5m));
    public static readonly Commodity Coat = new("coat", new UseValue("A garment"), "coat", LaborTime.FromHours(10m));

    [Fact]
    public void Lot_value_is_quantity_times_per_unit_labour()
    {
        var lot = new CommodityLot(Linen, new Quantity(20m, "yards"));
        Assert.Equal(LaborTime.FromHours(10m), lot.Value);
    }

    [Fact]
    public void Commodities_with_equal_components_are_equal()
    {
        var twin = new Commodity("linen", new UseValue("Woven cloth"), "yards", LaborTime.FromHours(0.5m));
        Assert.Equal(Linen, twin);
    }

    [Fact]
    public void Blank_name_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new Commodity(" ", new UseValue("Woven cloth"), "yards", LaborTime.Zero));
    }

    [Fact]
    public void Blank_unit_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new Commodity("linen", new UseValue("Woven cloth"), " ", LaborTime.Zero));
    }

    [Fact]
    public void Blank_use_value_description_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new UseValue("  "));
    }

    [Fact]
    public void Lot_rejects_defaulted_quantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommodityLot(Linen, default));
    }

    [Fact]
    public void Lot_renders_the_way_Marx_writes_it()
    {
        Assert.Equal("20 yards of linen", new CommodityLot(Linen, new Quantity(20m, "yards")).ToString());
        Assert.Equal("1 coat", new CommodityLot(Coat, new Quantity(1m, "coat")).ToString());
    }
}
