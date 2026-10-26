using Capital.Value;

namespace Capital.Tests.Value;

public class MoneyQuantityTests
{
    [Fact]
    public void Negative_amount_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MoneyQuantity.From(-1m, "£"));
    }

    [Fact]
    public void Blank_unit_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => MoneyQuantity.From(1m, " "));
    }

    [Fact]
    public void Addition_requires_the_same_unit()
    {
        Assert.Equal(MoneyQuantity.From(5m, "£"), MoneyQuantity.From(2m, "£") + MoneyQuantity.From(3m, "£"));
        Assert.Throws<ArgumentException>(() => MoneyQuantity.From(2m, "£") + MoneyQuantity.From(3m, "oz of gold"));
    }

    [Fact]
    public void Subtraction_cannot_go_negative_or_cross_units()
    {
        Assert.Equal(MoneyQuantity.From(1m, "£"), MoneyQuantity.From(3m, "£") - MoneyQuantity.From(2m, "£"));
        Assert.Throws<ArgumentOutOfRangeException>(() => MoneyQuantity.From(2m, "£") - MoneyQuantity.From(3m, "£"));
        Assert.Throws<ArgumentException>(() => MoneyQuantity.From(2m, "£") - MoneyQuantity.From(2m, "oz of gold"));
    }

    [Fact]
    public void Division_splits_money_by_turns()
    {
        Assert.Equal(MoneyQuantity.From(2m, "£"), MoneyQuantity.From(6m, "£") / 3m);
        Assert.Throws<ArgumentOutOfRangeException>(() => MoneyQuantity.From(6m, "£") / 0m);
    }

    [Fact]
    public void Ordering_follows_amounts_within_a_unit()
    {
        Assert.True(MoneyQuantity.From(3m, "£") > MoneyQuantity.From(2m, "£"));
        Assert.True(MoneyQuantity.From(2m, "£") <= MoneyQuantity.From(2m, "£"));
        Assert.Throws<ArgumentException>(() => MoneyQuantity.From(3m, "£").CompareTo(MoneyQuantity.From(2m, "oz of gold")));
    }

    [Fact]
    public void Zero_is_the_additive_identity()
    {
        Assert.Equal(MoneyQuantity.From(2m, "£"), MoneyQuantity.Zero("£") + MoneyQuantity.From(2m, "£"));
    }

    [Fact]
    public void ToString_renders_amount_and_unit()
    {
        Assert.Equal("100 £", MoneyQuantity.From(100m, "£").ToString());
    }
}
