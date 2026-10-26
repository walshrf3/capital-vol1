using Capital.Circulation;
using Capital.Value;

namespace Capital.Production;

/// <summary>
/// M–C…P…C′–M′: the concrete circuit of capital (Capital I, ch. 4–7). Money is thrown into
/// circulation to buy means of production and labour-power (at their values); the labour-power is
/// consumed in production; the product is sold at its value. Because the labour performed exceeds
/// the value of the labour-power, M′ exceeds M — surplus value arises within an exchange of
/// equivalents, resolving the contradiction of the general formula.
/// </summary>
/// <remarks>
/// Composed entirely of already-modeled parts: <see cref="ProductionProcess"/> (c, v, s, product
/// value), the <see cref="Sale"/> of the product, and the money commodity's own value for the
/// conversion between hours and money. The product is sold at its value in the money commodity's
/// own unit of measure; money names (coins, £) arrive with token money and are not modeled here.
/// </remarks>
public sealed record CapitalistCircuit
{
    public ProductionProcess Process { get; }
    public Sale Disposal { get; }
    public Commodity MoneyCommodity { get; }

    public CapitalistCircuit(ProductionProcess process, Sale disposal, Commodity moneyCommodity)
    {
        ArgumentNullException.ThrowIfNull(process);
        ArgumentNullException.ThrowIfNull(disposal);
        ArgumentNullException.ThrowIfNull(moneyCommodity);
        if (moneyCommodity.UnitValue.Hours <= 0m)
            throw new ArgumentException("The money commodity must itself have value.", nameof(moneyCommodity));
        if (disposal.Commodity.Value != process.ProductValue)
            throw new ArgumentException(
                $"The disposal must sell this process's product: the process yields value {process.ProductValue}, the disposal sells {disposal.Commodity} embodying {disposal.Commodity.Value}.");
        if (disposal.Money.Amount * moneyCommodity.UnitValue.Hours != disposal.Commodity.Value.Hours)
            throw new ArgumentException(
                $"The valorization circuit sells at value: {disposal.Money} does not express {disposal.Commodity.Value} in {moneyCommodity.Unit}.");
        Process = process;
        Disposal = disposal;
        MoneyCommodity = moneyCommodity;
    }

    public static CapitalistCircuit Create(ProductionProcess process, Sale disposal, Commodity moneyCommodity) =>
        new(process, disposal, moneyCommodity);

    /// <summary>M — the value advanced (c + v), thrown into circulation in the money commodity's unit.</summary>
    public MoneyQuantity Start =>
        MoneyQuantity.From((Process.Constant.Value + Process.Variable.Value).Hours / MoneyCommodity.UnitValue.Hours, MoneyCommodity.Unit);

    /// <summary>M′ — the money thrown out of circulation again.</summary>
    public MoneyQuantity End => Disposal.Money;

    /// <summary>ΔM as money (M′ − M).</summary>
    public MoneyQuantity SurplusMoney => End - Start;

    /// <summary>ΔM as value — the surplus value s.</summary>
    public SurplusValue Surplus => Process.Surplus;

    /// <summary>s′ = s/v — the degree of exploitation.</summary>
    public RateOfSurplusValue Rate => Process.Rate;

    public override string ToString() => $"M–C…P…C′–M′: {Start} – {Process.HoursWorked} of labour – {End}";
}
