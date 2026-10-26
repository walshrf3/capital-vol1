using Capital.Production;
using Capital.Value;

namespace Capital.Accumulation;

/// <summary>
/// The organic composition of capital (Capital I, ch. 23 §1, ch. 25 §1): the value composition
/// c/v insofar as determined by the technical composition — the mass of means of production set
/// in motion by a given quantity of labour-power. A rising composition means more constant
/// capital relative to labour.
/// </summary>
public readonly record struct OrganicComposition
{
    public decimal Ratio { get; }

    public OrganicComposition(ConstantCapital constant, VariableCapital variable)
    {
        if (variable.Value.Hours <= 0m)
            throw new ArgumentException("Composition needs a positive variable capital.", nameof(variable));
        Ratio = constant.Value.Hours / variable.Value.Hours;
    }

    public override string ToString() => $"c/v = {Ratio:0.###}";
}
