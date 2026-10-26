using Capital.Circulation;
using Capital.Value;

namespace Capital.Production;

/// <summary>
/// Labour-power (Capital I, ch. 6): the aggregate of the mental and physical capabilities
/// existing in a human being, which he exercises whenever he produces a use-value of any kind.
/// It becomes a commodity only when its owner sells it for a limited period, in his "double
/// freedom": free owner of his own capacity, and free of means of production.
/// </summary>
/// <remarks>
/// This is the special commodity that resolves the contradiction of M–C–M′: bought at its value
/// (the value of its <see cref="Subsistence"/>), it creates more value than it costs, because its
/// use-value is labour itself. See <see cref="CapitalistCircuit"/>.
/// </remarks>
public sealed record LaborPower
{
    public Owner Worker { get; }
    public SubsistenceBundle Subsistence { get; }

    /// <summary>The value of the daily labour-power — the value of its subsistence bundle.</summary>
    public LaborTime Value => Subsistence.Value;

    public LaborPower(Owner worker, SubsistenceBundle subsistence)
    {
        Worker = worker ?? throw new ArgumentNullException(nameof(worker));
        Subsistence = subsistence ?? throw new ArgumentNullException(nameof(subsistence));
        if (Value.Hours <= 0m)
            throw new ArgumentException("Labour-power cannot be maintained for nothing; subsistence must have positive value.");
    }
}
