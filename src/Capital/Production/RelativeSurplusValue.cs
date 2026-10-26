using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Relative surplus value (Capital I, ch. 12): surplus value gained by cheapening labour-power —
/// raising productivity in the subsistence industries lowers the value of the subsistence bundle,
/// so with the working day unchanged, the unpaid part of the day grows and s′ rises.
/// </summary>
/// <remarks>
/// The mechanisms Marx traces historically are the carriers of exactly this productivity rise:
/// co-operation (ch. 13), division of labour and manufacture (ch. 14), machinery and modern
/// industry (ch. 15). They are modeled here as the value change they accomplish; the organization
/// of labour itself is not separately represented (fixed-capital depreciation proper belongs to
/// the analysis of machinery and reappears in Volume 2).
/// </remarks>
public sealed record RelativeSurplusValue
{
    public LaborPower Before { get; }
    public LaborPower After { get; }
    public LaborTime WorkingDay { get; }

    public RelativeSurplusValue(LaborPower before, LaborPower after, LaborTime workingDay)
    {
        ArgumentNullException.ThrowIfNull(before);
        ArgumentNullException.ThrowIfNull(after);
        if (after.Value.Hours >= before.Value.Hours)
            throw new ArgumentException(
                $"Cheapening requires a lower value of labour-power: {before.Value} did not fall to {after.Value}.");
        if (workingDay.Hours <= 0m)
            throw new ArgumentOutOfRangeException(nameof(workingDay), workingDay.Hours, "A working day needs labour.");
        if (workingDay < after.Value)
            throw new ArgumentOutOfRangeException(nameof(workingDay), workingDay.Hours,
                "The working day must still reproduce the cheapened value of labour-power.");
        Before = before;
        After = after;
        WorkingDay = workingDay;
    }

    public static RelativeSurplusValue Create(LaborPower before, LaborPower after, LaborTime workingDay) =>
        new(before, after, workingDay);

    public LaborTime NecessaryBefore => Before.Value;
    public LaborTime NecessaryAfter => After.Value;
    public LaborTime SurplusBefore => WorkingDay - Before.Value;
    public LaborTime SurplusAfter => WorkingDay - After.Value;

    /// <summary>The relative surplus value gained: the fall in the value of labour-power.</summary>
    public LaborTime Gained => Before.Value - After.Value;

    public RateOfSurplusValue RateBefore => RateOfSurplusValue.Of(new SurplusValue(SurplusBefore), new VariableCapital(Before.Value));
    public RateOfSurplusValue RateAfter => RateOfSurplusValue.Of(new SurplusValue(SurplusAfter), new VariableCapital(After.Value));

    public override string ToString() => $"v {Before.Value} → {After.Value}: s′ {RateBefore} → {RateAfter}";
}
