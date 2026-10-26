using Capital.Value;
using Xunit;

namespace Capital.Tests.Value;

public class LaborTimeTests
{
    [Fact]
    public void Addition_sums_labour_time()
    {
        Assert.Equal(LaborTime.FromHours(15m), LaborTime.FromHours(10m) + LaborTime.FromHours(5m));
    }

    [Fact]
    public void Addition_leaves_operand_unchanged()
    {
        var tenHours = LaborTime.FromHours(10m);
        _ = tenHours + LaborTime.FromHours(5m);
        Assert.Equal(LaborTime.FromHours(10m), tenHours);
    }

    [Fact]
    public void Scaling_multiplies_by_a_positive_factor()
    {
        Assert.Equal(LaborTime.FromHours(5m), LaborTime.FromHours(10m) * 0.5m);
    }

    [Fact]
    public void Ordering_follows_hours()
    {
        Assert.True(LaborTime.FromHours(12m) > LaborTime.FromHours(10m));
        Assert.True(LaborTime.FromHours(10m) >= LaborTime.FromHours(10m));
        Assert.True(LaborTime.Zero < LaborTime.FromHours(0.25m));
        Assert.Equal(0, LaborTime.FromHours(7m).CompareTo(LaborTime.FromHours(7m)));
    }

    [Fact]
    public void Negative_hours_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LaborTime.FromHours(-1m));
    }

    [Fact]
    public void Subtraction_beyond_zero_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LaborTime.FromHours(2m) - LaborTime.FromHours(3m));
    }

    [Fact]
    public void Negative_scale_factor_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LaborTime.FromHours(2m) * -1m);
    }

    [Fact]
    public void ToString_shows_hours()
    {
        Assert.Equal("7.5 h", LaborTime.FromHours(7.5m).ToString());
    }
}
