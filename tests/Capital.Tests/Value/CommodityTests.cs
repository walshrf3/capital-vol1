using Capital.Value;
using Xunit;

namespace Capital.Tests.Value;

public class CommodityTests
{
    private static readonly UseValue LinenUse = new("Cloth, 20 yards of linen");
    private static readonly UseValue CoatUse = new("A coat");

    [Fact]
    public void Commodities_with_equal_components_are_equal()
    {
        var a = new Commodity("Linen", LinenUse, LaborTime.FromHours(10m));
        var b = new Commodity("Linen", LinenUse, LaborTime.FromHours(10m));

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Commodities_with_different_values_are_not_equal()
    {
        var a = new Commodity("Linen", LinenUse, LaborTime.FromHours(10m));
        var b = new Commodity("Linen", LinenUse, LaborTime.FromHours(12m));

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Blank_name_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new Commodity(" ", LinenUse, LaborTime.Zero));
    }

    [Fact]
    public void Blank_use_value_description_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new UseValue("  "));
    }

    [Fact]
    public void Simple_value_form_equates_commodities_of_equal_labour()
    {
        var linen = new Commodity("20 yards of linen", LinenUse, LaborTime.FromHours(10m));
        var coat = new Commodity("1 coat", CoatUse, LaborTime.FromHours(10m));

        var form = SimpleValueForm.Create(linen, coat);

        Assert.Equal(linen, form.Relative);
        Assert.Equal(coat, form.Equivalent);
        Assert.Equal("20 yards of linen = 1 coat", form.ToString());
    }

    [Fact]
    public void Simple_value_form_rejects_unequal_values()
    {
        var linen = new Commodity("20 yards of linen", LinenUse, LaborTime.FromHours(10m));
        var coat = new Commodity("1 coat", CoatUse, LaborTime.FromHours(12m));

        Assert.Throws<ArgumentException>(() => SimpleValueForm.Create(linen, coat));
    }

    [Fact]
    public void A_commodity_cannot_be_its_own_equivalent()
    {
        var linen = new Commodity("20 yards of linen", LinenUse, LaborTime.FromHours(10m));

        Assert.Throws<ArgumentException>(() => SimpleValueForm.Create(linen, linen));
    }
}
