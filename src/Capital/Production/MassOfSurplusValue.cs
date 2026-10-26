using Capital.Production;
using Capital.Value;

namespace Capital.Production;

/// <summary>
/// The mass of surplus value (Capital I, ch. 11): S = v × s′. With the rate given, the mass grows
/// with the total variable capital — the number of simultaneously exploited workers — while the
/// absolute limits of the working day bound how far s′ itself can rise.
/// </summary>
public static class MassOfSurplusValue
{
    public static SurplusValue Of(VariableCapital variable, RateOfSurplusValue rate) =>
        new(variable.Value * rate.Ratio);
}
