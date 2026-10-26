using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Constant capital c (Capital I, ch. 8 §1): the value of the means of production consumed in the
/// process. It re-appears in the product's value unchanged in magnitude, whatever service the
/// means render in the process.
/// </summary>
public readonly record struct ConstantCapital
{
    public LaborTime Value { get; }

    public ConstantCapital(LaborTime value) => Value = value;

    public override string ToString() => $"c = {Value}";
}
