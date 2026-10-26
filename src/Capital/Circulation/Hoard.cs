using Capital.Value;

namespace Capital.Circulation;

/// <summary>
/// Money as hoard (Capital I, ch. 3 §3a): money withdrawn from circulation, the metamorphosis
/// interrupted on purpose. Circulation forges its own hoards — money piles up wherever the two
/// phases of a metamorphosis are separated in time.
/// </summary>
public sealed record Hoard
{
    public Owner Owner { get; }
    public MoneyQuantity Amount { get; }

    public Hoard(Owner owner, MoneyQuantity amount)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        Amount = amount;
    }

    public override string ToString() => $"{Owner.Name} hoards {Amount}";
}
