using Capital.Production;
using Capital.Value;

namespace Capital.Wages;

/// <summary>
/// The time-wage (Capital I, ch. 18): the value (or price) of labour-power converted into the
/// price of a definite quantity of labour. The daily wage divided by the nominal length of the
/// working day yields the apparent "price of one hour of labour" — and lengthening the day at an
/// unchanged daily wage cheapens the hour, the quiet lever of exploitation.
/// </summary>
/// <remarks>
/// Labour itself has no value and can have none: it is the substance and measure of value (ch. 16).
/// That the money actually paid can stand above or below the value of the labour-power (ch. 17's
/// market fluctuations) is what makes the irrational "price of labour" appear to be a real thing;
/// see <see cref="PaidBelowValue"/>.
/// </remarks>
public sealed record TimeWage
{
    public LaborPower Power { get; }
    public MoneyQuantity DailyWage { get; }
    public LaborTime NominalDay { get; }
    public Commodity MoneyCommodity { get; }

    public TimeWage(LaborPower power, MoneyQuantity dailyWage, LaborTime nominalDay, Commodity moneyCommodity)
    {
        ArgumentNullException.ThrowIfNull(power);
        ArgumentNullException.ThrowIfNull(moneyCommodity);
        if (moneyCommodity.UnitValue.Hours <= 0m)
            throw new ArgumentException("The money commodity must itself have value.", nameof(moneyCommodity));
        if (nominalDay.Hours <= 0m)
            throw new ArgumentOutOfRangeException(nameof(nominalDay), nominalDay.Hours, "A working day needs labour.");
        Power = power;
        DailyWage = dailyWage;
        NominalDay = nominalDay;
        MoneyCommodity = moneyCommodity;
    }

    /// <summary>The apparent "price of one hour of labour" — daily wage over the nominal day (ch. 18 §2).</summary>
    public MoneyQuantity PriceOfOneHour => DailyWage / NominalDay.Hours;

    /// <summary>The value of the labour-power, expressed in the money commodity's unit — what the wage form disguises.</summary>
    public MoneyQuantity MoneyValueOfLaborPower =>
        MoneyQuantity.From(Power.Value.Hours / MoneyCommodity.UnitValue.Hours, MoneyCommodity.Unit);

    /// <summary>True when the money paid stands below the value of the labour-power (ch. 17's fluctuation).</summary>
    public bool PaidBelowValue => DailyWage < MoneyValueOfLaborPower;

    public override string ToString() => $"{DailyWage} per {NominalDay}";
}
