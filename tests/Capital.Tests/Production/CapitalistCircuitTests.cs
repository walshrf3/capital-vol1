using Capital.Circulation;
using Capital.Production;
using Capital.Value;

namespace Capital.Tests.Production;

public class CapitalistCircuitTests
{
    // The ch. 7-9 spinning example, closed into a circuit: cotton and spindle (c = 4 h), a spinner
    // whose daily labour-power is worth 6 h, a 12-hour day, yarn sold at its value (16 h).
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

    private static ProductionProcess Process { get; } = new(
        new[] { Lot(Cotton, 40m), Lot(Spindle, 1m) },
        SpinnerPower,
        LaborTime.FromHours(12m));

    [Fact]
    public void The_full_circuit_resolves_the_contradiction_of_the_general_formula()
    {
        // M = c + v = 10 h = 2 oz of gold; C′ = 16 h of yarn = 3.2 oz of gold; s = 6 h = 1.2 oz.
        var disposal = new Sale(new Owner("the capitalist"), Lot(Yarn, 16m), MoneyQuantity.From(3.2m, "oz"));
        var circuit = CapitalistCircuit.Create(Process, disposal, Gold);

        Assert.Equal(MoneyQuantity.From(2m, "oz"), circuit.Start);
        Assert.Equal(MoneyQuantity.From(3.2m, "oz"), circuit.End);
        Assert.Equal(MoneyQuantity.From(1.2m, "oz"), circuit.SurplusMoney);
        Assert.Equal(new SurplusValue(LaborTime.FromHours(6m)), circuit.Surplus);
        Assert.Equal(1.0m, circuit.Rate.Ratio);
        Assert.Equal("M–C…P…C′–M′: 2 oz – 12 h of labour – 3.2 oz", circuit.ToString());
    }

    [Fact]
    public void A_disposal_off_value_is_rejected()
    {
        // 4 oz of gold expresses 20 h, not the 16 h the product carries.
        var disposal = new Sale(new Owner("the capitalist"), Lot(Yarn, 16m), MoneyQuantity.From(4m, "oz"));

        Assert.Throws<ArgumentException>(() => CapitalistCircuit.Create(Process, disposal, Gold));
    }

    [Fact]
    public void A_disposal_of_a_foreign_product_is_rejected()
    {
        // 12 lb of yarn embody 12 h, not the 16 h the process produced.
        var disposal = new Sale(new Owner("the capitalist"), Lot(Yarn, 12m), MoneyQuantity.From(2.4m, "oz"));

        Assert.Throws<ArgumentException>(() => CapitalistCircuit.Create(Process, disposal, Gold));
    }

    [Fact]
    public void The_degenerate_circuit_reproduces_the_advance()
    {
        var process = new ProductionProcess(
            new[] { Lot(Cotton, 40m), Lot(Spindle, 1m) },
            SpinnerPower,
            LaborTime.FromHours(6m));
        var disposal = new Sale(new Owner("the capitalist"), Lot(Yarn, 10m), MoneyQuantity.From(2m, "oz"));

        var circuit = CapitalistCircuit.Create(process, disposal, Gold);

        Assert.Equal(MoneyQuantity.From(2m, "oz"), circuit.Start);
        Assert.Equal(MoneyQuantity.From(2m, "oz"), circuit.End);
        Assert.Equal(MoneyQuantity.Zero("oz"), circuit.SurplusMoney);
    }

    [Fact]
    public void The_mass_of_surplus_value_grows_with_variable_capital()
    {
        // Two spinners: total v = 12 h, each day s = 6 h at rate 1.0; S = v × s′ = 12 h.
        var mass = MassOfSurplusValue.Of(new VariableCapital(LaborTime.FromHours(12m)), Process.Rate);

        Assert.Equal(new SurplusValue(LaborTime.FromHours(12m)), mass);
    }
}
