using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// M–C–M′, the general formula of capital (Capital I, ch. 4): money buys a commodity lot, and the
/// very same lot is sold again for more money. M′ = M + ΔM; ΔM is the surplus value. The circuit
/// is the inversion of C–M–C — driven by value itself, not by use-value — and is composed from the
/// same <see cref="Purchase"/> and <see cref="Sale"/> primitives, so the two circuit forms are two
/// readings of one circulation apparatus.
/// </summary>
/// <remarks>
/// The contradiction of the formula (ch. 4 §2): surplus value cannot originate in circulation
/// itself — an exchange of equivalents leaves the total value unchanged — nor outside it. The
/// formula's own resolution, the consumption of labour-power in production, arrives with Part 3
/// (ch. 6): <see cref="Capital.Production.CapitalistCircuit"/> — labour-power is bought at its
/// value yet creates more value in consumption. Until then, <see cref="SurplusValue"/> names a
/// result whose source is not yet represented.
/// </remarks>
public sealed record MoneyCircuit
{
    public Purchase Purchase { get; }
    public Sale Sale { get; }

    public MoneyCircuit(Purchase purchase, Sale sale)
    {
        ArgumentNullException.ThrowIfNull(purchase);
        ArgumentNullException.ThrowIfNull(sale);
        if (purchase.Commodity != sale.Commodity)
            throw new ArgumentException(
                $"M–C–M′ resells the very lot it bought: purchased {purchase.Commodity}, sold {sale.Commodity}.");
        if (sale.Money < purchase.Money)
            throw new ArgumentException(
                $"The circuit cannot close below its start: bought for {purchase.Money}, sold for {sale.Money}.");
        Purchase = purchase;
        Sale = sale;
    }

    public static MoneyCircuit Create(Purchase purchase, Sale sale) => new(purchase, sale);

    /// <summary>M — the money advanced.</summary>
    public MoneyQuantity Start => Purchase.Money;

    /// <summary>M′ — the money thrown out of circulation again.</summary>
    public MoneyQuantity End => Sale.Money;

    /// <summary>ΔM — the surplus value (M′ = M + ΔM); zero marks the degenerate circuit that valorizes nothing.</summary>
    public MoneyQuantity SurplusValue => End - Start;

    public override string ToString() => $"{Start} – {Purchase.Commodity} – {End}";
}
