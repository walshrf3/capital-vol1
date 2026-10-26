using Capital.Circulation;
using Capital.Production;
using Capital.Value;

namespace Capital.Tests.Production;

public class RelativeSurplusValueTests
{
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    [Fact]
    public void Surplus_profit_is_the_social_minus_the_individual_value()
    {
        // An innovator spins 100 lb of yarn at 0.8 h/lb while society still spins at 1 h/lb.
        var individualKind = new Commodity("yarn", new UseValue("Spun thread"), "lb", LaborTime.FromHours(0.8m));
        var profit = SurplusProfit.Of(Lot(individualKind, 100m), LaborTime.FromHours(1m));

        Assert.Equal(LaborTime.FromHours(0.2m), profit.PerUnit);
        Assert.Equal(LaborTime.FromHours(20m), profit.Total);
    }

    [Fact]
    public void No_surplus_profit_without_an_individual_advantage()
    {
        var prevailing = new Commodity("yarn", new UseValue("Spun thread"), "lb", LaborTime.FromHours(1m));

        Assert.Throws<ArgumentException>(() => SurplusProfit.Of(Lot(prevailing, 100m), LaborTime.FromHours(1m)));
    }

    [Fact]
    public void Cheapening_labour_power_gains_surplus_with_the_day_fixed()
    {
        // Same physical subsistence, now produced in half the time: corn 10 → 5 h/quarter, tea 1 → 0.5 h/lb.
        var cheapCorn = new Commodity("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(5m));
        var cheapTea = new Commodity("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(0.5m));

        var before = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));
        var after = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(cheapCorn, 0.4m), Lot(cheapTea, 2m) }));

        var relative = RelativeSurplusValue.Create(before, after, LaborTime.FromHours(12m));

        Assert.Equal(LaborTime.FromHours(6m), relative.SurplusBefore);
        Assert.Equal(LaborTime.FromHours(9m), relative.SurplusAfter);
        Assert.Equal(LaborTime.FromHours(3m), relative.Gained);
        Assert.Equal(1.0m, relative.RateBefore.Ratio);
        Assert.Equal(3.0m, relative.RateAfter.Ratio);
    }

    [Fact]
    public void Cheapening_requires_a_lower_value_of_labour_power()
    {
        var before = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));
        var unchanged = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));

        Assert.Throws<ArgumentException>(() => RelativeSurplusValue.Create(before, unchanged, LaborTime.FromHours(12m)));
    }

    [Fact]
    public void The_working_day_must_still_reproduce_the_cheapened_value()
    {
        var cheapCorn = new Commodity("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(5m));
        var cheapTea = new Commodity("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(0.5m));

        var before = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));
        var after = new LaborPower(
            new Owner("the spinner"),
            new SubsistenceBundle(new[] { Lot(cheapCorn, 0.4m), Lot(cheapTea, 2m) }));

        // A day shorter than the cheapened subsistence value cannot reproduce v.
        Assert.Throws<ArgumentOutOfRangeException>(() => RelativeSurplusValue.Create(before, after, LaborTime.FromHours(2m)));
    }
}
