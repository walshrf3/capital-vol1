using Capital.Circulation;
using Capital.Value;

namespace Capital.Production;

/// <summary>
/// The production process (Capital I, ch. 7–8), read as valorization: means of production plus
/// labour-power, with the labour-power actually consumed for <see cref="HoursWorked"/>. The
/// process has a double character — a labour process producing a use-value, and a valorization
/// process producing value — modeled as one record with both readings.
/// </summary>
/// <remarks>
/// Invariants from the text: the labour performed adds its own duration in value
/// (<see cref="NewValue"/>); the product carries the consumed means of production unchanged plus
/// the new value (<see cref="ProductValue"/> = c + new value); and the working day has a necessary
/// minimum — the day must at least reproduce the value of labour-power, below which no
/// valorization is possible (ch. 10 §1).
/// </remarks>
public sealed record ProductionProcess
{
    public IReadOnlyList<CommodityLot> MeansOfProduction { get; }
    public LaborPower Power { get; }
    public LaborTime HoursWorked { get; }

    private readonly LaborTime _constant;

    public ProductionProcess(IReadOnlyList<CommodityLot> meansOfProduction, LaborPower power, LaborTime hoursWorked)
    {
        ArgumentNullException.ThrowIfNull(meansOfProduction);
        ArgumentNullException.ThrowIfNull(power);
        if (meansOfProduction.Count == 0)
            throw new ArgumentException("Production consumes at least one means of production.", nameof(meansOfProduction));
        var array = new CommodityLot[meansOfProduction.Count];
        decimal constantHours = 0m;
        for (var i = 0; i < meansOfProduction.Count; i++)
        {
            var lot = meansOfProduction[i] ?? throw new ArgumentException("Means of production cannot be null.", nameof(meansOfProduction));
            constantHours += lot.Value.Hours;
            array[i] = lot;
        }
        if (hoursWorked.Hours <= 0m)
            throw new ArgumentOutOfRangeException(nameof(hoursWorked), hoursWorked.Hours, "Labour must actually be performed.");
        if (hoursWorked < power.Value)
            throw new ArgumentOutOfRangeException(
                nameof(hoursWorked),
                hoursWorked.Hours,
                "Below the necessary minimum (the value of labour-power) no valorization is possible (ch. 10 §1).");
        MeansOfProduction = array;
        Power = power;
        HoursWorked = hoursWorked;
        _constant = LaborTime.FromHours(constantHours);
    }

    /// <summary>c — the means of production consumed; transferred to the product unchanged.</summary>
    public ConstantCapital Constant => new(_constant);

    /// <summary>v — the value of the labour-power consumed.</summary>
    public VariableCapital Variable => new(Power.Value);

    /// <summary>The new value the labour itself adds — equal to its duration.</summary>
    public LaborTime NewValue => HoursWorked;

    /// <summary>s — the excess of the new value over the value of the labour-power.</summary>
    public SurplusValue Surplus => SurplusValue.Of(NewValue, Variable);

    /// <summary>Necessary labour time — the part of the day reproducing v.</summary>
    public LaborTime NecessaryLabor => Power.Value;

    /// <summary>Surplus labour time — the unpaid part of the day; equal to s.</summary>
    public LaborTime SurplusLabor => HoursWorked - Power.Value;

    /// <summary>s′ = s/v.</summary>
    public RateOfSurplusValue Rate => RateOfSurplusValue.Of(Surplus, Variable);

    /// <summary>The value of the product: the consumed means of production plus the new value.</summary>
    public LaborTime ProductValue => _constant + HoursWorked;

    public override string ToString() => $"{HoursWorked} of labour on {_constant} of means of production";
}
