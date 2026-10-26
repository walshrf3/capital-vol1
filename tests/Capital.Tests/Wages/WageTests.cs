using Capital.Circulation;
using Capital.Production;
using Capital.Value;
using Capital.Wages;

namespace Capital.Tests.Wages;

public class WageTests
{
    private static readonly Commodity Corn = new("corn", new UseValue("Grain"), "quarter", LaborTime.FromHours(10m));
    private static readonly Commodity Tea = new("tea", new UseValue("A beverage"), "lb", LaborTime.FromHours(1m));
    private static readonly Commodity Gold = new("gold", new UseValue("The money commodity"), "oz", LaborTime.FromHours(5m));

    private static CommodityLot Lot(Commodity commodity, decimal amount) =>
        new(commodity, new Quantity(amount, commodity.Unit));

    // The spinner of the ch. 7-9 series: daily labour-power worth 6 h = 1.2 oz of gold.
    private static LaborPower SpinnerPower { get; } = new(
        new Owner("the spinner"),
        new SubsistenceBundle(new[] { Lot(Corn, 0.4m), Lot(Tea, 2m) }));

    [Fact]
    public void The_hourly_price_falls_as_the_nominal_day_stretches()
    {
        var twelveHourDay = new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(12m), Gold);
        var stretchedDay = new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(15m), Gold);

        Assert.Equal(MoneyQuantity.From(0.1m, "oz"), twelveHourDay.PriceOfOneHour);
        Assert.Equal(MoneyQuantity.From(0.08m, "oz"), stretchedDay.PriceOfOneHour);
        Assert.False(stretchedDay.PaidBelowValue);
    }

    [Fact]
    public void The_market_price_of_labour_can_stand_below_the_value_of_labour_power()
    {
        var atValue = new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(12m), Gold);
        var belowValue = new TimeWage(SpinnerPower, MoneyQuantity.From(1.0m, "oz"), LaborTime.FromHours(12m), Gold);
        var aboveValue = new TimeWage(SpinnerPower, MoneyQuantity.From(1.5m, "oz"), LaborTime.FromHours(12m), Gold);

        Assert.False(atValue.PaidBelowValue);
        Assert.True(belowValue.PaidBelowValue);
        Assert.False(aboveValue.PaidBelowValue);
    }

    [Fact]
    public void The_piece_wage_is_the_converted_time_wage()
    {
        var timeWage = new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(12m), Gold);
        var pieceWage = PieceWage.ConvertedFrom(timeWage, 12m);   // 12 lb of yarn per average day

        Assert.Equal(MoneyQuantity.From(0.1m, "oz"), pieceWage.PricePerPiece);
        Assert.Equal(MoneyQuantity.From(1.2m, "oz"), pieceWage.ForPieces(12m));
        Assert.Equal(MoneyQuantity.From(1.5m, "oz"), pieceWage.ForPieces(15m));   // intensification pays — and hides
        Assert.Equal("piece price 0.1 oz (12 pieces/day)", pieceWage.ToString());
    }

    [Fact]
    public void Piece_wage_rejects_impossible_output()
    {
        var pieceWage = PieceWage.ConvertedFrom(
            new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(12m), Gold), 12m);

        Assert.Throws<ArgumentOutOfRangeException>(() => pieceWage.ForPieces(0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => PieceWage.ConvertedFrom(
            new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.FromHours(12m), Gold), 0m));
    }

    [Fact]
    public void Zero_length_days_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TimeWage(SpinnerPower, MoneyQuantity.From(1.2m, "oz"), LaborTime.Zero, Gold));
    }
}
