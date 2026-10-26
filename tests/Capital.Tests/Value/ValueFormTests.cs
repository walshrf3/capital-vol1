using Capital.Value;

namespace Capital.Tests.Value;

public class ValueFormTests
{
    // Marx's own series from Capital I, ch. 1 §3: every lot embodies 10 hours of SNLT.
    private static readonly Commodity Linen = new("linen", new UseValue("Woven cloth"), "yards", LaborTime.FromHours(0.5m));
    private static readonly Commodity Coat = new("coat", new UseValue("A garment"), "coat", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));
    private static readonly Commodity Coffee = new("coffee", new UseValue("A beverage"), "lb", LaborTime.FromHours(0.25m));
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Gold = new("gold", new UseValue("The money commodity"), "oz", LaborTime.FromHours(5m));
    private static readonly Commodity Iron = new("iron", new UseValue("Metal"), "ton", LaborTime.FromHours(20m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    private static CommodityLot LinenLot => Lot(Linen, 20m);

    [Fact]
    public void Simple_form_equates_lots_of_equal_labour()
    {
        var form = ValueForm.Simple.Create(LinenLot, Lot(Coat, 1m));

        Assert.Equal(LinenLot, form.Relative);
        Assert.Equal(Lot(Coat, 1m), form.Equivalent);
        Assert.Null(form.GeneralEquivalent);
        Assert.Equal("20 yards of linen = 1 coat", form.ToString());
    }

    [Fact]
    public void Simple_form_rejects_unequal_labour()
    {
        // 5 lb of tea embodies 5 hours, not 10.
        Assert.Throws<ArgumentException>(() => ValueForm.Simple.Create(LinenLot, Lot(Tea, 5m)));
    }

    [Fact]
    public void Simple_form_rejects_a_lot_expressing_itself()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.Simple.Create(LinenLot, Lot(Linen, 20m)));
    }

    [Fact]
    public void Expanded_form_carries_the_whole_series()
    {
        var form = ValueForm.Expanded.Create(
            LinenLot,
            new[] { Lot(Coat, 1m), Lot(Tea, 10m), Lot(Coffee, 40m), Lot(Corn, 1m), Lot(Gold, 2m), Lot(Iron, 0.5m) });

        Assert.Single(form.Relatives);
        Assert.Equal(6, form.Equivalents.Count);
        Assert.Null(form.GeneralEquivalent);
        Assert.StartsWith("20 yards of linen = 1 coat", form.ToString());
        Assert.Contains("or = 10 lb of tea", form.ToString());
    }

    [Fact]
    public void Expanded_form_rejects_a_single_equivalent()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.Expanded.Create(LinenLot, new[] { Lot(Coat, 1m) }));
    }

    [Fact]
    public void Expanded_form_rejects_repeated_kinds()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.Expanded.Create(
            LinenLot,
            new[] { Lot(Coat, 1m), Lot(Tea, 10m), Lot(Tea, 10m) }));
    }

    [Fact]
    public void Expanded_form_rejects_unequal_labour()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.Expanded.Create(
            LinenLot,
            new[] { Lot(Coat, 1m), Lot(Tea, 4m) }));
    }

    [Fact]
    public void Expanded_form_rejects_the_relative_as_its_own_equivalent()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.Expanded.Create(
            LinenLot,
            new[] { Lot(Coat, 1m), Lot(Linen, 20m) }));
    }

    [Fact]
    public void General_form_pours_all_commodities_into_one_equivalent()
    {
        var generalEquivalent = LinenLot;
        var form = ValueForm.General.Create(
            new[] { Lot(Coat, 1m), Lot(Tea, 10m), Lot(Coffee, 40m), Lot(Corn, 1m), Lot(Gold, 2m), Lot(Iron, 0.5m) },
            generalEquivalent);

        Assert.Equal(generalEquivalent, form.GeneralEquivalent);
        Assert.Equal(6, form.Relatives.Count);
        Assert.Single(form.Equivalents);
        Assert.Contains("1 coat = 20 yards of linen", form.ToString());
        Assert.Contains("0.5 ton of iron = 20 yards of linen", form.ToString());
    }

    [Fact]
    public void General_form_excludes_the_equivalent_from_the_relative_world()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.General.Create(
            new[] { Lot(Coat, 1m), Lot(Linen, 20m) },
            LinenLot));
    }

    [Fact]
    public void General_form_rejects_a_single_relative()
    {
        Assert.Throws<ArgumentException>(() => ValueForm.General.Create(new[] { Lot(Coat, 1m) }, LinenLot));
    }

    [Fact]
    public void Money_form_fixes_the_general_equivalent_by_custom()
    {
        var general = ValueForm.General.Create(
            new[] { Lot(Coat, 1m), Lot(Tea, 10m), Lot(Coffee, 40m) },
            Lot(Gold, 2m));

        var money = ValueForm.Money.FromGeneral(general);

        Assert.Equal(Lot(Gold, 2m), money.GeneralEquivalent);
        Assert.Equal(3, money.Relatives.Count);
        Assert.Single(money.Equivalents);
        Assert.Contains("1 coat = 2 oz of gold", money.ToString());
        Assert.Contains("10 lb of tea = 2 oz of gold", money.ToString());
    }

    [Fact]
    public void Forms_are_consumed_through_switch_expressions()
    {
        ValueForm form = ValueForm.Simple.Create(LinenLot, Lot(Coat, 1m));

        var label = form switch
        {
            ValueForm.Simple s => "simple",
            ValueForm.Expanded e => "expanded",
            ValueForm.General g => "general",
            ValueForm.Money m => "money",
            _ => throw new InvalidOperationException("Unknown value form"),
        };

        Assert.Equal("simple", label);
    }
}
