using Capital.Value;

namespace Capital.Tests.Value;

public class QuantityTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Non_positive_amount_is_rejected(decimal amount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Quantity(amount, "yards"));
    }

    [Fact]
    public void Blank_unit_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new Quantity(1m, "  "));
    }

    [Fact]
    public void Equal_quantities_are_equal()
    {
        Assert.True(new Quantity(20m, "yards") == new Quantity(20m, "yards"));
        Assert.False(new Quantity(20m, "yards") == new Quantity(20m, "meters"));
    }

    [Fact]
    public void ToString_renders_amount_and_unit()
    {
        Assert.Equal("20 yards", new Quantity(20m, "yards").ToString());
    }
}
