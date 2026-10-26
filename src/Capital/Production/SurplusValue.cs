using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Surplus value s (Capital I, ch. 7–8): the excess of the new value created by the labour over
/// the value of the labour-power consumed — the value of the unpaid part of the working day.
/// </summary>
public readonly record struct SurplusValue
{
    public LaborTime Value { get; }

    public SurplusValue(LaborTime value) => Value = value;

    /// <summary>s = new value − v; refuses a negative surplus.</summary>
    public static SurplusValue Of(LaborTime newValue, VariableCapital variable) =>
        new(newValue - variable.Value);

    public override string ToString() => $"s = {Value}";
}
