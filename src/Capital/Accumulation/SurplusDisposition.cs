using Capital.Production;
using Capital.Value;

namespace Capital.Accumulation;

/// <summary>
/// What becomes of the surplus value (Capital I, ch. 21–24). Consumed as revenue, the process
/// repeats on the same scale — simple reproduction. Converted into capital, the process repeats
/// on an enlarged scale — expanded reproduction.
/// </summary>
public abstract record SurplusDisposition
{
    /// <summary>The total surplus value of the cycle being disposed of.</summary>
    public SurplusValue Source { get; }

    private SurplusDisposition(SurplusValue source) => Source = source;

    /// <summary>
    /// Ch. 22 — simple reproduction: the whole surplus consumed as revenue. Repetition of the
    /// process converts the originally advanced capital, cycle by cycle, into accumulated
    /// surplus value; the personal consumption of the capitalist becomes a deduction from
    /// surplus value, not from capital.
    /// </summary>
    public sealed record ConsumedAsRevenue : SurplusDisposition
    {
        public ConsumedAsRevenue(SurplusValue source) : base(source) { }

        public override string ToString() => $"{Source} consumed as revenue (simple reproduction)";
    }

    /// <summary>
    /// Ch. 23–24 — expanded reproduction: the whole surplus converted into additional constant and
    /// variable capital, split in the proportions of the technical composition. (Partial splits —
    /// revenue plus accumulation — are documented but not modeled.)
    /// </summary>
    public sealed record Capitalized : SurplusDisposition
    {
        public ConstantCapital AddedConstant { get; }
        public VariableCapital AddedVariable { get; }

        public Capitalized(SurplusValue source, ConstantCapital addedConstant, VariableCapital addedVariable)
            : base(source)
        {
            if (addedConstant.Value.Hours + addedVariable.Value.Hours != source.Value.Hours)
                throw new ArgumentException(
                    $"The capitalized surplus must account for the whole surplus {source.Value}: got {addedConstant} + {addedVariable}.");
            AddedConstant = addedConstant;
            AddedVariable = addedVariable;
        }

        public override string ToString() => $"{Source} capitalized as {AddedConstant} + {AddedVariable}";
    }
}
