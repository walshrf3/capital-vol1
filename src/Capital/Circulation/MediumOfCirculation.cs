using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// The law of the quantity of money in circulation (Capital I, ch. 3 §2a): the money the process
/// needs is the sum of the prices of the commodities in circulation divided by the average number
/// of turns of the same pieces of money. Money streams through circulation; the amount required is
/// derived, not stocked.
/// </summary>
public static class MediumOfCirculation
{
    public static MoneyQuantity Required(IReadOnlyList<MoneyQuantity> salePrices, decimal turnsPerPeriod)
    {
        ArgumentNullException.ThrowIfNull(salePrices);
        if (salePrices.Count == 0)
            throw new ArgumentException("No prices to circulate.", nameof(salePrices));
        if (turnsPerPeriod <= 0m)
            throw new ArgumentOutOfRangeException(nameof(turnsPerPeriod), turnsPerPeriod, "Circulation must turn at least once.");

        var unit = salePrices[0].Unit;
        decimal total = 0m;
        foreach (var price in salePrices)
        {
            if (price.Unit != unit)
                throw new ArgumentException($"All prices must share the unit {unit}; found {price.Unit}.");
            total += price.Amount;
        }
        return MoneyQuantity.From(total / turnsPerPeriod, unit);
    }
}
