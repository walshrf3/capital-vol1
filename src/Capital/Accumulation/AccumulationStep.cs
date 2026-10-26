using Capital.Production;
using Capital.Value;

namespace Capital.Accumulation;

/// <summary>
/// One step of expanded reproduction (Capital I, ch. 23–25): the prior cycle's surplus is
/// capitalized as Δc and Δv, and the composition moves. The movement of the organic composition
/// across the step is the motor of ch. 25's general law: accumulation in a rising composition
/// demands less labour per unit of capital, throwing out a relative surplus population.
/// </summary>
public sealed record AccumulationStep
{
    public ProductionProcess PriorCycle { get; }
    public SurplusDisposition.Capitalized Disposition { get; }

    public AccumulationStep(ProductionProcess priorCycle, SurplusDisposition.Capitalized disposition)
    {
        ArgumentNullException.ThrowIfNull(priorCycle);
        ArgumentNullException.ThrowIfNull(disposition);
        if (priorCycle.Surplus != disposition.Source)
            throw new ArgumentException(
                $"The disposition must act on this cycle's surplus {priorCycle.Surplus}, not {disposition.Source}.");
        PriorCycle = priorCycle;
        Disposition = disposition;
    }

    public static AccumulationStep From(ProductionProcess priorCycle, SurplusDisposition.Capitalized disposition) =>
        new(priorCycle, disposition);

    public ConstantCapital ConstantAfter => new(PriorCycle.Constant.Value + Disposition.AddedConstant.Value);
    public VariableCapital VariableAfter => new(PriorCycle.Variable.Value + Disposition.AddedVariable.Value);

    /// <summary>Total capital after the step: (c + v) + Δc + Δv.</summary>
    public LaborTime CapitalAfter => ConstantAfter.Value + VariableAfter.Value;

    public OrganicComposition CompositionBefore => new(PriorCycle.Constant, PriorCycle.Variable);
    public OrganicComposition CompositionAfter => new(ConstantAfter, VariableAfter);

    /// <summary>
    /// Labour demanded before and after the step at a given wage (ch. 25 §3b: the demand for
    /// labour is determined by the mass of variable capital, not by total capital).
    /// </summary>
    public (decimal Before, decimal After) EmploymentAt(VariableCapital wage)
    {
        if (wage.Value.Hours <= 0m)
            throw new ArgumentOutOfRangeException(nameof(wage), wage.Value.Hours, "A wage needs positive value.");
        return (
            PriorCycle.Variable.Value.Hours / wage.Value.Hours,
            VariableAfter.Value.Hours / wage.Value.Hours);
    }

    public override string ToString() =>
        $"{PriorCycle.Surplus} capitalized; composition {CompositionBefore} → {CompositionAfter}";
}
