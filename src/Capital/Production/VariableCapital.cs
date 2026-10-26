using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Variable capital v (Capital I, ch. 8 §2): the value of the labour-power bought. In the process
/// the capitalist consumes the labour-power, and the worker replaces the variable capital with a
/// new creation — if the labour is long enough, with a surplus besides.
/// </summary>
public readonly record struct VariableCapital
{
    public LaborTime Value { get; }

    public VariableCapital(LaborTime value) => Value = value;

    public override string ToString() => $"v = {Value}";
}
