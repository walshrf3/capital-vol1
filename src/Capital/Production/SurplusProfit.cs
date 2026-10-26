using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Surplus profit (Capital I, ch. 12 §2): a producer whose individual value for a commodity kind
/// stands below the social value sells at the social value and pockets the difference. This is the
/// motive of every productivity innovation — and it is transient: as the method spreads, the
/// social value itself falls, and the surplus profit evaporates.
/// </summary>
public sealed record SurplusProfit
{
    /// <summary>The lot produced at its individual value (below the social value).</summary>
    public CommodityLot Lot { get; }

    /// <summary>The prevailing social value of one unit of the kind.</summary>
    public LaborTime SocialUnitValue { get; }

    public SurplusProfit(CommodityLot lot, LaborTime socialUnitValue)
    {
        ArgumentNullException.ThrowIfNull(lot);
        if (socialUnitValue.Hours <= lot.Commodity.UnitValue.Hours)
            throw new ArgumentException(
                $"No surplus profit: the individual value {lot.Commodity.UnitValue} does not stand below the social value {socialUnitValue}.");
        Lot = lot;
        SocialUnitValue = socialUnitValue;
    }

    public static SurplusProfit Of(CommodityLot lot, LaborTime socialUnitValue) => new(lot, socialUnitValue);

    /// <summary>Surplus profit per unit — social value minus individual value.</summary>
    public LaborTime PerUnit => SocialUnitValue - Lot.Commodity.UnitValue;

    /// <summary>Surplus profit over the whole lot.</summary>
    public LaborTime Total => PerUnit * Lot.Quantity.Amount;

    public override string ToString() => $"{Lot}: surplus profit {Total} against a social value of {SocialUnitValue}/unit";
}
