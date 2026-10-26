using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// World money (Capital I, ch. 3 §3c): money leaving domestic circulation. Only gold and silver
/// serve here — the universally recognized form; the national tokens that can replace money at
/// home (coins, notes) cannot replace it between nations.
/// </summary>
public sealed record WorldMoney
{
    public MoneyQuantity Amount { get; }

    public WorldMoney(MoneyQuantity amount) => Amount = amount;

    public override string ToString() => $"world money: {Amount}";
}
