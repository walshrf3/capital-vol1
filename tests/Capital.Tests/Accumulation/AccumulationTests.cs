using Capital.Circulation;
using Capital.Production;
using Capital.Value;
using Capital.Accumulation;

namespace Capital.Tests.Accumulation;

public class AccumulationTests
{
    // The ch. 7-9 spinning series: c = 4 h, v = 6 h, s = 6 h on a 12-hour day.
    private static readonly Commodity Cotton = new("cotton", new UseValue("Raw material for spinning"), "lb", LaborTime.FromHours(0.05m));
    private static readonly Commodity Spindle = new("spindle", new UseValue("An instrument of labour"), "spindle", LaborTime.FromHours(2m));
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));
    private static readonly Commodity Yarn = new("yarn", new UseValue("Spun thread"), "lb", LaborTime.FromHours(1m));
    private static readonly Commodity Gold = new("gold", new UseValue("The money commodity"), "oz", LaborTime.FromHours(5m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    private static LaborPower SpinnerPower { get; } = new(
        new Owner("the spinner"),
        new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));

    private static ProductionProcess Cycle { get; } = new(
        new[] { Lot(Cotton, 40m), Lot(Spindle, 1m) },
        SpinnerPower,
        LaborTime.FromHours(12m));

    private static Owner Capitalist { get; } = new("the capitalist");

    [Fact]
    public void Simple_reproduction_consumes_the_whole_surplus()
    {
        var disposition = new SurplusDisposition.ConsumedAsRevenue(Cycle.Surplus);

        Assert.Equal(Cycle.Surplus, disposition.Source);
        Assert.Equal("s = 6 h consumed as revenue (simple reproduction)", disposition.ToString());
    }

    [Fact]
    public void Expanded_reproduction_splits_the_surplus_in_the_technical_proportions()
    {
        // Capitalize the 6 h of surplus in the prevailing 2:3 composition: Δc = 2.4 h, Δv = 3.6 h.
        var disposition = new SurplusDisposition.Capitalized(
            Cycle.Surplus,
            new ConstantCapital(LaborTime.FromHours(2.4m)),
            new VariableCapital(LaborTime.FromHours(3.6m)));

        Assert.Equal(Cycle.Surplus, disposition.Source);
    }

    [Fact]
    public void Capitalization_must_account_for_the_whole_surplus()
    {
        Assert.Throws<ArgumentException>(() => new SurplusDisposition.Capitalized(
            Cycle.Surplus,
            new ConstantCapital(LaborTime.FromHours(2m)),
            new VariableCapital(LaborTime.FromHours(2m))));
    }

    [Fact]
    public void The_disposition_must_act_on_this_cycles_surplus()
    {
        var otherSurplus = new SurplusValue(LaborTime.FromHours(5m));

        Assert.Throws<ArgumentException>(() => new AccumulationStep(
            Cycle,
            new SurplusDisposition.Capitalized(
                otherSurplus,
                new ConstantCapital(LaborTime.FromHours(3m)),
                new VariableCapital(LaborTime.FromHours(2m)))));
    }

    [Fact]
    public void A_rising_composition_throws_out_relative_surplus_population()
    {
        // Δc = 4 h, Δv = 2 h: total capital grows 10 h → 16 h, but composition rises 2/3 → 1,
        // so labour demanded grows from 1 to 8/6 workers at a 6-hour wage — the general law in numbers.
        var step = new AccumulationStep(
            Cycle,
            new SurplusDisposition.Capitalized(
                Cycle.Surplus,
                new ConstantCapital(LaborTime.FromHours(4m)),
                new VariableCapital(LaborTime.FromHours(2m))));

        Assert.Equal(4m / 6m, step.CompositionBefore.Ratio);
        Assert.Equal(1.0m, step.CompositionAfter.Ratio);
        Assert.Equal(LaborTime.FromHours(16m), step.CapitalAfter);

        var employment = step.EmploymentAt(new VariableCapital(LaborTime.FromHours(6m)));
        Assert.Equal(1m, employment.Before);
        Assert.Equal(8m / 6m, employment.After);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => step.EmploymentAt(new VariableCapital(LaborTime.Zero)));
    }

    [Fact]
    public void Composition_needs_a_positive_variable_capital()
    {
        Assert.Throws<ArgumentException>(() => new OrganicComposition(
            new ConstantCapital(LaborTime.FromHours(4m)),
            new VariableCapital(LaborTime.Zero)));
    }

    [Fact]
    public void The_loot_enters_the_circuit_as_constant_capital()
    {
        // Part 8 → Part 3 back-connection: the spinner's spindle is expropriated and reappears,
        // with bought cotton, as the constant capital of a valorization circuit.
        var expropriation = new Expropriation(
            new Owner("the dispossessed spinner"),
            new[] { Lot(Spindle, 1m) },
            Capitalist);
        var primitive = PrimitiveAccumulation.Of(expropriation);

        Assert.Equal(new[] { Lot(Spindle, 1m) }, primitive.CapitalizedMeansOfProduction);
        Assert.Equal(
            "the capitalist expropriates 1 spindle from the dispossessed spinner",
            expropriation.ToString());

        var process = new ProductionProcess(
            primitive.CapitalizedMeansOfProduction.Append(Lot(Cotton, 40m)).ToArray(),
            SpinnerPower,
            LaborTime.FromHours(12m));
        Assert.Equal(LaborTime.FromHours(4m), process.Constant.Value);

        var disposal = new Sale(Capitalist, Lot(Yarn, 16m), MoneyQuantity.From(3.2m, "oz"));
        var circuit = CapitalistCircuit.Create(process, disposal, Gold);

        Assert.Equal(MoneyQuantity.From(1.2m, "oz"), circuit.SurplusMoney);
    }

    [Fact]
    public void Expropriation_needs_a_third_party_and_real_means()
    {
        Assert.Throws<ArgumentException>(() => new Expropriation(
            Capitalist, new[] { Lot(Spindle, 1m) }, Capitalist));
        Assert.Throws<ArgumentException>(() => new Expropriation(
            new Owner("the dispossessed spinner"), Array.Empty<CommodityLot>(), Capitalist));
    }
}
