using Capital.Circulation;
using Capital.Production;
using Capital.Value;

namespace Capital.Tests.Production;

public class ProductionProcessTests
{
    // Marx's ch. 7-9 spinning example: a 12-hour day, necessary and surplus labour 6 hours each.
    private static readonly Commodity Cotton = new("cotton", new UseValue("Raw material for spinning"), "lb", LaborTime.FromHours(0.05m));
    private static readonly Commodity Spindle = new("spindle", new UseValue("An instrument of labour"), "spindle", LaborTime.FromHours(2m));
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    private static LaborPower SpinnerPower { get; } = new(
        new Owner("the spinner"),
        new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));

    private static IReadOnlyList<CommodityLot> Means { get; } = new[] { Lot(Cotton, 40m), Lot(Spindle, 1m) };
    // c = 2 h + 2 h = 4 h

    [Fact]
    public void Process_computes_c_v_s_and_the_product_value()
    {
        var process = new ProductionProcess(Means, SpinnerPower, LaborTime.FromHours(12m));

        Assert.Equal(new ConstantCapital(LaborTime.FromHours(4m)), process.Constant);
        Assert.Equal(new VariableCapital(LaborTime.FromHours(6m)), process.Variable);
        Assert.Equal(new SurplusValue(LaborTime.FromHours(6m)), process.Surplus);
        Assert.Equal(1.0m, process.Rate.Ratio);
        Assert.Equal("s′ = 100%", process.Rate.ToString());
        Assert.Equal(LaborTime.FromHours(6m), process.NecessaryLabor);
        Assert.Equal(LaborTime.FromHours(6m), process.SurplusLabor);
        Assert.Equal(LaborTime.FromHours(16m), process.ProductValue);
    }

    [Fact]
    public void Working_day_below_the_necessary_minimum_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ProductionProcess(Means, SpinnerPower, LaborTime.FromHours(5m)));
    }

    [Fact]
    public void Degenerate_day_reproduces_value_only()
    {
        var process = new ProductionProcess(Means, SpinnerPower, LaborTime.FromHours(6m));

        Assert.Equal(new SurplusValue(LaborTime.Zero), process.Surplus);
        Assert.Equal(0m, process.Rate.Ratio);
        Assert.Equal(LaborTime.FromHours(10m), process.ProductValue);
    }

    [Fact]
    public void Empty_means_of_production_are_rejected()
    {
        Assert.Throws<ArgumentException>(
            () => new ProductionProcess(Array.Empty<CommodityLot>(), SpinnerPower, LaborTime.FromHours(12m)));
    }

    [Fact]
    public void Zero_hours_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ProductionProcess(Means, SpinnerPower, LaborTime.Zero));
    }
}
