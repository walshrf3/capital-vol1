using Capital.Value;

namespace Capital.Production;

/// <summary>
/// The rate of surplus value s′ = s/v (Capital I, ch. 9): the degree of exploitation of
/// labour-power. Expressed in labour time it is surplus labour / necessary labour; Marx's
/// convention renders the ratio as a percentage (s′ = 100% when the two halves are equal).
/// </summary>
public readonly record struct RateOfSurplusValue
{
    public decimal Ratio { get; }

    private RateOfSurplusValue(decimal ratio) => Ratio = ratio;

    public static RateOfSurplusValue Of(SurplusValue surplus, VariableCapital variable) =>
        variable.Value.Hours <= 0m
            ? throw new ArgumentException("The rate of surplus value needs a positive variable capital.", nameof(variable))
            : new RateOfSurplusValue(surplus.Value.Hours / variable.Value.Hours);

    public override string ToString() => $"s′ = {Ratio * 100m:0.#}%";
}
